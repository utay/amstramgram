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
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("auth/facebook")]
        public async Task<IActionResult> FacebookLogin(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
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
       
            return Redirect("http://google.com?email=" + account.Email);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
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