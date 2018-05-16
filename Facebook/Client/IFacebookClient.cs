using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Client
{
    public interface IFacebookClient
    {
        Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null);
        Task PostAsync(string accessToken, string endpoint, object data, string args = null);
    }
}
