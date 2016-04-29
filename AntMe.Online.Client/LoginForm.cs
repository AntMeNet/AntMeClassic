using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AntMe.Online.Client
{
    internal partial class LoginForm : Form
    {
        private string clientId = string.Empty;

        private string baseUrl = string.Empty;

        private string redirectUrl = string.Empty;

        public TokenResponse Response { get; private set; }

        public string ErrorMessage { get; private set; }

        public LoginForm()
#if DEBUG
            : this("https://localhost:44303/identity",
            "AntMeClient", "https://localhost:44303/Application/LoggedIn")
#else
            : this("https://service.antme.net/identity",
            "AntMeClient", "https://service.antme.net/Application/LoggedIn")
#endif
        {
            InitializeComponent();
            DialogResult = System.Windows.Forms.DialogResult.Cancel;

            // Thinktecture.IdentityModel.Clients.OAuth2Client x = new Thinktecture.IdentityModel.Clients.OAuth2Client(new Uri(baseUrl));

            webBrowser.Navigate(baseUrl + "/connect/authorize?" +
                "client_id=" + clientId +
                "&response_type=id_token token" +
                "&scope=openid email roles profile api" +
                "&nonce=random_nonce" +
                "&state=random_state" +
                "&redirect_uri=" + redirectUrl);
        }

        public LoginForm(string baseUrl, string clientId, string redirectUrl)
        {
            this.baseUrl = baseUrl;
            this.clientId = clientId;
            this.redirectUrl = redirectUrl;
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.ToString().StartsWith(redirectUrl))
            {
                var parts = e.Url.Fragment.Substring(1).Split('&');

                var errorPart = parts.FirstOrDefault(p => p.StartsWith("error="));
                if (errorPart != null)
                {
                    // Identity Server Error -> Close
                    DialogResult = System.Windows.Forms.DialogResult.Abort;
                    ErrorMessage = errorPart.Substring("error=".Length);
                    Close();
                    return;
                }

                // Success
                var idtokenPart = parts.FirstOrDefault(p => p.StartsWith("id_token="));
                if (idtokenPart != null)
                {
                    var accesstokenPart = parts.FirstOrDefault(p => p.StartsWith("access_token="));

                    Response = new TokenResponse();
                    Response.IdToken = idtokenPart.Substring("id_token=".Length);
                    Response.AccessToken = accesstokenPart.Substring("access_token=".Length);

                    var part = Encoding.UTF8.GetString(Decode(Response.IdToken.Split('.')[1]));
                    var jwt = JObject.Parse(part);

                    Response.Email = (string)jwt["email"];
                    Response.UserId = Guid.Parse((string)jwt["sub"]);
                    Response.Name = (string)jwt["name"];
                    if (jwt["role"] is JArray)
                    {
                        foreach (var item in jwt["role"])
                        {
                            Response.Roles.Add(item.ToString());
                        }
                    }
                    else
                    {
                        string role = (string)jwt["role"];
                        if (!string.IsNullOrEmpty(role))
                            Response.Roles.Add(role);
                    }

                    CloseTimer.Enabled = true;
                    return;
                }

                // Fallback (Kein Token bekommen)
                ErrorMessage = "No Access Token";
                DialogResult = System.Windows.Forms.DialogResult.Abort;
                return;
            }
        }

        public static byte[] Decode(string arg)
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding

            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default: throw new Exception("Illegal base64url string!");
            }

            return Convert.FromBase64String(s); // Standard base64 decoder
        }

        private void webBrowser_NavigateError(object sender, WebBrowserNavigateErrorEventArgs e)
        {
            // Navigationsfehler -> Fenster mit fehler schließen
            ErrorMessage = "Loading Error";
            DialogResult = System.Windows.Forms.DialogResult.Abort;
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            webBrowser.Stop();
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            CloseTimer.Enabled = false;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
