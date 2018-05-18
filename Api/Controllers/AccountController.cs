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

namespace Api.Controllers
{
    [Authorize]
    [Route("")]
    public class AccountController : Controller
    {
        private readonly UserManager<Data.ApplicationUser> _userManager;
        private readonly SignInManager<Data.ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public string ErrorMessage { get; private set; }

        public AccountController(
            UserManager<Data.ApplicationUser> userManager,
            SignInManager<Data.ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("users/sign_in")]
        public async Task<IActionResult> Index()
        {
            await Helper.AppHttpContext.HttpContext.Session.LoadAsync();
            await Helper.AppHttpContext.HttpContext.Session.CommitAsync();
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
            //HttpContext.Session.Remove("currentUser");
            string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            System.Diagnostics.Debug.WriteLine(baseUrl + (Url.Action("FacebookLoginCallback", "Account")));
            // Request a redirect to the external login provider.
            var authProperties = new AuthenticationProperties
            {
                RedirectUri = baseUrl + (Url.Action("FacebookLoginCallback", "Account")),
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
                Gender = account.Gender
            };
            return user;
        }

        private Core.Models.User GetTypeUser(Data.ApplicationUser currentUser)
        {
            Core.Models.User user = new Core.Models.User
            {
                Email = currentUser.Email,
                Firstname = currentUser.FirstName,
                Lastname = currentUser.LastName,
                Gender = currentUser.Gender,
                Nickname = currentUser.UserName,
                Picture = currentUser.PictureUrl,
                Password = ""
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
            url += "373455309833356&redirect_uri=https://localhost:44307/signin-facebook&client_secret=2e0e737ad1a89d0f251ebfaebfd3f76c";
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
            userDB.Password = accessToken;

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
                    }
                    else
                    {                        
                        userDB = userInDb;
                        userDB.Password = accessToken;
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

            
            if (currentUser.UserName == null)
                currentUser.UserName = currentUser.Email;

            _logger?.LogInformation("User connected");

            _signInManager.Context.Session.SetObject("currentToken", accessToken);
            await _signInManager.Context.Session.CommitAsync();

            Response.Cookies.Append(".Amstramgram.Cookie", accessToken);

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
        public IActionResult Lockout()
        {
            return View();
        }

        //
        // GET /Account/AccessDenied
        [HttpGet]
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
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}