using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Api.Helper
{
    public class Users
    {
        public static bool IsConnected()
        {
            return AppHttpContext.HttpContext.Request.Cookies.TryGetValue(".Amstramgram.Cookie", out string accessToken)
                || AppHttpContext.HttpContext.Session.TryGetValue("currentUserId", out byte[] id);
        }

        public static string HashPassword(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
