using Facebook.Exception;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Facebook.Client
{
    //Taken From http://piotrgankiewicz.com/2017/02/06/accessing-facebook-api-using-c/
    public class FacebookClient : IFacebookClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public FacebookClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/v3.0/")
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public FacebookClient(ILogger logger)
            : this()
        {
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task PostAsync(string accessToken, string endpoint, object data, string args = null)
        {
            var payload = GetPayload(data);
            await _httpClient.PostAsync($"{endpoint}?access_token={accessToken}&{args}", payload);
        }

        private static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        //This function init Chilkat trial of 30 days for Oauth2 connection
        private void InitChilKat()
        {
            Chilkat.Global glob = new Chilkat.Global();
            bool success = glob.UnlockBundle("Anything for 30-day trial");
            if (success != true)
            {
                _logger?.LogInformation("Error: " + glob.LastErrorText);
                throw new InvalidFacebookConnectionException("Impossible to enable initialate Oauth2 connection");                
            }
            _logger?.LogInformation("Error: " + glob.LastErrorText);
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        public string GetAccessToken()
        {
            InitChilKat();
            Chilkat.OAuth2 oauth2 = new Chilkat.OAuth2
            {

                //  This should match the Site URL configured for your Facebook APP, such as "http://localhost:5000/"
                ListenPort = 5000,

                AuthorizationEndpoint = "https://www.facebook.com/v3.0/dialog/oauth",
                TokenEndpoint = "https://graph.facebook.com/v3.0/oauth/access_token",

                RedirectAllowHtml = "http://localhost:5000/users/callback",
                RedirectDenyHtml = "http://localhost:5000/users/deny",

                //  Replace these with actual values.
                ClientId = "373455309833356",
                ClientSecret = "2e0e737ad1a89d0f251ebfaebfd3f76c",

                //  Set the Scope to a comma-separated list of permissions the app wishes to request.
                //  See https://developers.facebook.com/docs/facebook-login/permissions/ for a full list of permissions.
                Scope = "public_profile,email",

                CodeChallenge = true,
                CodeChallengeMethod = "S256"
            };

            //  Begin the OAuth2 three-legged flow.  This returns a URL that should be loaded in a browser.
            string url = oauth2.StartAuth();
            _logger?.LogInformation(url);
            if (oauth2.LastMethodSuccess != true)
            {
                throw new InvalidFacebookConnectionException(oauth2.LastErrorText);
            }

            OpenUrl(url);

            int numMsWaited = 0;
            while ((numMsWaited < 30000) && (oauth2.AuthFlowState < 3))
            {
                oauth2.SleepMs(100);
                numMsWaited = numMsWaited + 100;
            }

            if (oauth2.AuthFlowState < 3)
            {
                oauth2.Cancel();
                throw new InvalidFacebookConnectionException("No response from the browser!");
            }

            if (oauth2.AuthFlowState == 5)
            {
                throw new InvalidFacebookConnectionException("OAuth2 failed to complete. " + oauth2.FailureInfo);
            }

            if (oauth2.AuthFlowState == 4)
            {
                throw new InvalidFacebookConnectionException("OAuth2 authorization was denied." + oauth2.AccessTokenResponse);
            }

            if (oauth2.AuthFlowState != 3)
            {
                throw new InvalidFacebookConnectionException("Unexpected AuthFlowState:" + Convert.ToString(oauth2.AuthFlowState));
            }
            return oauth2.AccessToken;
        }
    }
}
