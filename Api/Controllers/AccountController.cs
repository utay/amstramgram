using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.IO;
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using Data;
using Microsoft.AspNetCore.Http;
using Api.Helper;
using Algolia.Search;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    [Authorize]
    [Route("")]
    public class AccountController : Controller
    {
        private readonly UserManager<Data.ApplicationUser> _userManager;
        private readonly SignInManager<Data.ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public string ErrorMessage { get; private set; }

        public AccountController(
            UserManager<Data.ApplicationUser> userManager,
            SignInManager<Data.ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (Users.IsConnected())
                return RedirectToLocal("/feed");
            return View("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("me")]
        public IActionResult Me()
        {
            Core.Models.User userDB = null;
            string accessToken = null;
            long? id = null;
            if (!Helper.AppHttpContext.HttpContext.Request.Cookies.TryGetValue(".Amstramgram.Cookie", out accessToken))
            {
                id = Helper.AppHttpContext.HttpContext.Session.GetObject<long>("currentUserId");
            }
            if (accessToken == null && (id == null || id == 0))
                return Json(null);
            using (var ctx = new AmstramgramContext())
            {
               var userRepo = new Data.Repositories.UserRepository(ctx, null);
               userDB = (accessToken != null) ? userRepo.GetFromAccessToken(accessToken).Result : userRepo.Get(id.Value).Result;
               if (userDB != null)
               {
                   Helper.AppHttpContext.HttpContext.Session.SetObject<long>("currentUserId", userDB.Id);
                   Helper.AppHttpContext.HttpContext.Response.Cookies.Delete(".Amstramgram.Cookie");
               }
            }
            return Json(userDB);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("auth/facebook")]
        public async Task<IActionResult> FacebookLogin(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            var authProperties = new AuthenticationProperties
            {
                RedirectUri = _configuration.GetValue<string>("Facebook:redirectUri"),
                Items = { new KeyValuePair<string, string>("LoginProvider", "Facebook") }
            };
            return Challenge(authProperties, "Facebook");
        }

        private async Task<string> GetAccessToken(string url)
        {
            var _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/v3.0/")
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadAsStringAsync();
            var data = (JObject)JsonConvert.DeserializeObject(result);
            string access_token = data["access_token"].Value<string>();
            return access_token;
        }

        private Data.ApplicationUser GetApplicationUser(Facebook.Data.Account account, String accessToken)
        {
            Data.ApplicationUser user = new Data.ApplicationUser
            {
                Email = account.Email,
                FacebookId = Convert.ToInt64(account.Id),
                LastName = account.LastName,
                UserName = account.UserName,
                FirstName = account.FirstName,
                AccessToken = accessToken,
                Gender = account.Gender,
                PictureUrl = account.PictureUrl
            };
            return user;
        }

        private Core.Models.User GetTypeUser(Data.ApplicationUser currentUser)
        {
            Core.Models.User user = new Core.Models.User
            {
                Email = currentUser.Email,
                Description = "",
                Firstname = currentUser.FirstName,
                Lastname = currentUser.LastName,
                Gender = currentUser.Gender ?? "",
                Nickname = currentUser.UserName ?? $"{currentUser.FirstName.ToLower()}.{currentUser.LastName.ToLower()}",
                Picture = currentUser.PictureUrl ?? "",
                Password = "",
                Private = true
            };
            return user;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("signin-facebook")]
        public async Task<IActionResult> FacebookLoginCallback(string returnUrl = null, string remoteError = null)
        {
            string codeString = HttpContext.Request.Query["code"].ToString();
            string stateString = HttpContext.Request.Query["state"].ToString();
            if (codeString == null || stateString == null || codeString == "" || stateString == "")
            {
                _logger?.LogInformation($"Impossible to connect. Code: { codeString } and state { stateString } are invalid");
                return RedirectToAction("Index");
            }
            string url = "https://graph.facebook.com/v3.0/oauth/access_token?client_id=";
            url += _configuration.GetValue<string>("Facebook:AppId");
            url += "&redirect_uri=";
            url += _configuration.GetValue<string>("Facebook:redirectUri") + "&client_secret=";
            url += _configuration.GetValue<string>("Facebook:AppSecret");
            url += "&code=" + codeString;

            string accessToken = await GetAccessToken(url);

            if (accessToken == null || accessToken == "")
            {
                _logger?.LogInformation($"Impossible to connect. Access Token: { accessToken } is invalid");
                return RedirectToAction("Index");
            }

            var client = new Facebook.Client.FacebookClient();
            var service = new Facebook.Service.FacebookService(client);
            var account = await service.GetAccountAsync(accessToken);

            if (account == null || account.Email == null || account.Email == "")
            {
                _logger?.LogInformation($"Impossible to connect. Impossible to get account information");
                return RedirectToAction("Index");
            }

            var currentUser = GetApplicationUser(account, accessToken);            
            var userDB = GetTypeUser(currentUser);
            userDB.Password = Users.HashPassword(accessToken);
            userDB.AccessToken = accessToken;
            try
            {
                using (var db = new AmstramgramContext())
                {
                    Data.Repositories.UserRepository userRepo = new Data.Repositories.UserRepository(db, null);
                    var userInDb = await userRepo.GetFromEmail(userDB.Email);
                    if (userInDb == null)
                    {
                        userDB = userRepo.Add(userDB);                        
                        userRepo.SaveChanges();
                        AlgoliaClient algolia = new AlgoliaClient("A71NP8C36C", "ac1a68327b713553e3d21307968adab7");
                        Index usersIndex = algolia.InitIndex("Amstramgram_users");
                        userDB.objectID = userDB.Id.ToString();
                        usersIndex.AddObject(userDB);
                        userRepo.Update(userDB);
                        userRepo.SaveChanges();
                    }
                    else
                    {            
                        userDB = userInDb;
                        userDB.Password = Users.HashPassword(accessToken);
                        userRepo.Update(userDB);
                        userRepo.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                _logger?.LogInformation($"Impossible to connect. Impossible to save account information\n" + e.Message);
                return RedirectToAction("Index");
            }

            _logger?.LogInformation("User connected");

            _signInManager.Context.Session.SetObject("currentToken", accessToken);
            await _signInManager.Context.Session.CommitAsync();

            Response.Cookies.Append(".Amstramgram.Cookie", userDB.AccessToken);

            return RedirectToLocal("/feed");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("users/logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("currentToken");
            HttpContext.Session.Remove("currentUserId");
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/users/lockout")]
        public IActionResult Lockout()
        {
            return View();
        }

        //
        // GET /Account/AccessDenied
        [HttpGet]
        [Route("/users/denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        #endregion
    }
}
