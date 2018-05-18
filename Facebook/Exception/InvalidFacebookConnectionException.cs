using System;
using System.Collections.Generic;
using System.Text;

namespace Facebook.Exception
{
    public class InvalidFacebookConnectionException : System.Exception
    {
        public InvalidFacebookConnectionException(string message)
            : base (message)
        {

        }
    }
}
