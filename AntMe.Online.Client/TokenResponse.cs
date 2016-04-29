using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Online.Client
{
    internal class TokenResponse
    {
        public TokenResponse()
        {
            Roles = new List<string>();
        }

        public string IdToken { get; set; }

        public string AccessToken { get; set; }

        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public List<string> Roles { get; set; }
    }
}
