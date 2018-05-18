using Facebook.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Service
{
    public interface IFacebookService
    {
        Task<Account> GetAccountAsync(string accessToken);
        Task PostOnWallAsync(string accessToken, string message);
    }
}
