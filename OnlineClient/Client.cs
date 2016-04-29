using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AntMe.Online.Client
{
    /// <summary>
    /// Connection Client for the AntMe! Online Legacy Controller.
    /// </summary>
    public sealed class Client
    {
        /// <summary>
        /// Indicates a logged in user.
        /// </summary>
        public bool LoggedIn { get; private set; }

        /// <summary>
        /// Returns the Login Name of the current user.
        /// </summary>
        public string Username { get; private set; }

        public Client()
        {

        }

        /// <summary>
        /// Tries to log in with some saved tokens.
        /// </summary>
        public void Login()
        {
            
        }

        /// <summary>
        /// Uses the given Token to log in.
        /// </summary>
        /// <param name="token">Token</param>
        public void Login(string token)
        {

        }

        /// <summary>
        /// Uses the given Credentials to log in.
        /// </summary>
        /// <param name="email">Email Address</param>
        /// <param name="password">Password</param>
        public void Login(string email, string password)
        {
            using (var client = HttpConnection.CreateAuthClient())
            {
                HttpContent requestContent = new StringContent(
                    "grant_type=password&scope=api&username=" + Uri.EscapeUriString(email) + "&password=" + Uri.EscapeUriString(password),
                    Encoding.UTF8, "application/x-www-form-urlencoded");
                Task<HttpResponseMessage> requestTask = client.PostAsync("/identity/connect/token", requestContent);

                HttpConnection.HandleTask(requestTask);

                if (requestTask.Result.IsSuccessStatusCode)
                {

                }
                else
                {
                }
            }
        }

        /// <summary>
        /// Checks for newer Versions.
        /// </summary>
        public void CheckForUpdates()
        {

        }

        /// <summary>
        /// Logs the current user out.
        /// </summary>
        public void Logout()
        {

        }
    }
}
