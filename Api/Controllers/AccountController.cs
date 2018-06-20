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
        [AllowConnection]
        [Route("/")]
        public IActionResult Index()
        {
            if (Users.IsConnected())
                return Redirect(_configuration.GetValue<string>("RedirectFront"));
            return View("Login");
        }

        [HttpGet]
        [AllowConnection]
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
            userDB = (accessToken != null) ? DataAccess.User.GetFromAccessToken(accessToken).Result
                                           : DataAccess.User.Get(id.Value).Result;
            if (userDB != null && userDB.Id != 0)
            {
                Helper.AppHttpContext.HttpContext.Session.SetObject<long>("currentUserId", userDB.Id);
            }
            return Json(userDB);
        }

        [HttpPost]
        [AllowConnection]
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
                Phone = "",
                Password = "",
                Private = false
            };
            return user;
        }

        [HttpPost]
        [AllowConnection]
        [Route("/sign_in")]
        public async Task<IActionResult> SignIn(string EmailSign, string PasswordSign)
        {
            if (ModelState.IsValid)
            {
                if (EmailSign == null || EmailSign == "" || PasswordSign == null || PasswordSign == "")
                {
                    ViewData["Error"] = "Field missing";
                    return RedirectToAction("Index");
                }
                Core.Models.User user = new Core.Models.User
                {
                    Email = EmailSign,
                    Password = Users.HashPassword(PasswordSign)
                };
                var currentUser = await DataAccess.User.SignInUser(user);
                if (currentUser == null || currentUser.Id == 0)
                    return RedirectToLocal("/");
                Helper.AppHttpContext.HttpContext.Session.SetObject<long>("currentUserId", currentUser.Id);
                return Redirect(_configuration.GetValue<string>("RedirectFront"));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [AllowConnection]
        [Route("/register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                if (model.Email == null || model.Email == "" || model.Firstname == null
                    || model.Firstname == "" || model.Lastname == null || model.Lastname == "" || model.Password == null || model.Password == "")
                {
                    ViewData["Error"] = "Field missing";
                    return RedirectToAction("Index");
                }
                try
                {
                    Core.Models.User user = new Core.Models.User
                    {
                        Email = model.Email,
                        Password = Users.HashPassword(model.Password),
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        Nickname = $"{model.Firstname.ToLower()}.{model.Lastname.ToLower()}",
                        Description = "",
                        Gender = "",
                        Phone = "",
                        Picture = "",
                        AccessToken = "",
                        Private = false
                    };
                    var userInDb = DataAccess.User.Add(user);
                    if (userInDb == null)
                    {
                        ViewData["Error"] = "Impossible to save the current user";
                        return RedirectToAction("Index");
                    }
                    userInDb.objectID = user.Id.ToString();
                    AlgoliaClient client = new AlgoliaClient(_configuration.GetValue<string>("Algolia:AppId"),
                    _configuration.GetValue<string>("Algolia:AppSecret"));
                    Index usersIndex = client.InitIndex(_configuration.GetValue<string>("Algolia:IndexUser"));
                    usersIndex.AddObject(userInDb);
                    DataAccess.User.Update(userInDb);
                    Helper.AppHttpContext.HttpContext.Session.SetObject<long>("currentUserId", userInDb.Id);
                    return Redirect(_configuration.GetValue<string>("RedirectFront"));
                }
                catch (Exception e)
                {
                    ViewData["Error"] = "Impossible to save the current user\n" + e.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [AllowConnection]
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
                var userInDb = await DataAccess.User.GetFromEmail(userDB.Email);
                if (userDB == null)
                {
                    //First connection
                    userDB = DataAccess.User.Add(userDB);
                    AlgoliaClient algolia = new AlgoliaClient(_configuration.GetValue<string>("Algolia:AppId"),
                                                             _configuration.GetValue<string>("Algolia:AppSecret"));
                    Index usersIndex = algolia.InitIndex(_configuration.GetValue<string>("Algolia:IndexUser"));
                    userDB.objectID = userDB.Id.ToString();
                    usersIndex.AddObject(userDB);
                    DataAccess.User.Update(userDB);
                }
                else
                {
                    userDB = userInDb;
                    userDB.Password = Users.HashPassword(accessToken);
                    userDB.AccessToken = accessToken;
                    DataAccess.User.Update(userDB);
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

            return Redirect(_configuration.GetValue<string>("RedirectFront"));
        }

        [HttpGet]
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
        [AllowConnectionAttribute]
        [Route("/users/lockout")]
        public IActionResult Lockout()
        {
            return View();
        }

        //
        // GET /Account/AccessDenied
        [HttpGet]
        [AllowConnectionAttribute]
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

        #endregion Helpers
    }
}