using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AntMe.Online.Client
{
    public sealed class Connection
    {
        private readonly string configPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "AntMe";
        private readonly string pluginGuid = "DB565C88-0E2C-450A-B5C7-3F7E6C363E39";
#if DEBUG
        private readonly Uri identityUri = new Uri("https://localhost:44303/");
        private readonly Uri apiUri = new Uri("http://localhost:13733/");
#else
        private readonly Uri identityUri = new Uri("https://service.antme.net/");
        private readonly Uri apiUri = new Uri("http://api.antme.net/");
#endif
        private readonly string clientVersion = "1.7";
        private readonly int timeout = 5000;

        #region Singleton

        private static Connection instance = null;

        /// <summary>
        /// Gibt die zentrale Instanz des Connectors zurück.
        /// </summary>
        public static Connection Instance
        {
            get
            {
                if (instance == null)
                    instance = new Connection();
                return instance;
            }
        }

        #endregion

        private Configuration configuration = null;

        /// <summary>
        /// Der aktuelle Status des Connectors.
        /// </summary>
        public ConnectionState State { get; private set; }

        /// <summary>
        /// Gibt an, ob der Connector gerade beschäftigt ist.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return State == ConnectionState.LoggingIn;
            }
        }

        /// <summary>
        /// Gibt an, ob der Connector einen gültigen Login hat.
        /// </summary>
        public bool IsLoggedIn
        {
            get
            {
                return !string.IsNullOrEmpty(configuration.AccessToken);
            }
        }

        /// <summary>
        /// Gibt die User-Id des aktuell angemeldeten Users zurück.
        /// </summary>
        public Guid UserId
        {
            get
            {
                if (configuration != null)
                    return configuration.UserId;
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Gibt den Usernamen des aktuellen Users zurück.
        /// </summary>
        public string Username
        {
            get
            {
                if (configuration != null && !string.IsNullOrEmpty(configuration.Email))
                    return configuration.Email;
                return string.Empty;
            }
        }

        /// <summary>
        /// Liste der Rollen, in denen sich der aktuelle User befindet.
        /// </summary>
        public IEnumerable<string> Roles
        {
            get
            {
                return configuration.Roles.AsEnumerable();
            }
        }

        private Connection()
        {
            State = ConnectionState.Disconnected;

            // Load Config
            LoadSettings();
            if (!string.IsNullOrEmpty(configuration.AccessToken))
                State = ConnectionState.Connected;
        }

        /// <summary>
        /// Öffnet eine Verbindung, falls notwendig.
        /// </summary>
        /// <param name="owner">Window Handle des aufrufenden Fensters.</param>
        public void Open(IWin32Window owner)
        {
            if (!IsBusy && !IsLoggedIn)
                PromptLogin(owner);
        }

        /// <summary>
        /// Beendet eine Verbindung und löscht vorhandene User-Infos.
        /// </summary>
        public void Close()
        {
            if (!IsBusy)
            {
                State = ConnectionState.Disconnected;
                configuration.Reset();
                SaveSettings();
            }
        }

        /// <summary>
        /// Prüft nach eventuellen Versions-Updates.
        /// </summary>
        /// <param name="version">Aktuelle AntMe!-Version</param>
        /// <returns>URL zu einem Download oder null, falls die aktuelle Version installiert ist.</returns>
        public Uri CheckForUpdates(Version version)
        {
            UpdateRequestModel request = new UpdateRequestModel()
            {
                ClientId = configuration.ClientId,
                UserId = configuration.UserId,
                AntMeVersion = version.ToString(),
                OsVersion = Environment.OSVersion.Version.ToString()
            };

            using (var client = CreateClient())
            {
                client.BaseAddress = identityUri;
                StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                Task<HttpResponseMessage> t = client.PostAsync("/Application/Update", content);

                string result = Handle<string>(t);
                if (!string.IsNullOrEmpty(result))
                    return new Uri(result);
                return null;
            }
        }

        /// <summary>
        /// Führt einen Get-Call zum API Server durch.
        /// </summary>
        /// <typeparam name="T">Rückgabe-Datentyp</typeparam>
        /// <param name="path">Relativer Pfad zur Methode</param>
        /// <returns>Rückgabe der Methode</returns>
        internal T Get<T>(string path)
        {
            using (var client = CreateClient())
            {
                return Handle<T>(client.GetAsync(path));
            }
        }

        /// <summary>
        /// Führt einen Post-Call zum API Server durch.
        /// </summary>
        /// <typeparam name="T">Rückgabe-Datentyp</typeparam>
        /// <typeparam name="R">Request-Datentyp</typeparam>
        /// <param name="path">Relativer Pfad </param>
        /// <param name="request">Request Inhalt</param>
        /// <returns>Antwort der Methode</returns>
        internal T Post<T, R>(string path, R request)
        {
            using (var client = CreateClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(request));
                Task<HttpResponseMessage> t = client.PostAsync(path, content);
                return Handle<T>(t);
            }
        }

        #region Helper

        /// <summary>
        /// Erstellt einen neuen HttpClient mit allen notwendigen Einstellungen. Muss disposed werden!
        /// </summary>
        /// <returns>Client Instanz</returns>
        private HttpClient CreateClient()
        {
            var client = new HttpClient();

            client.BaseAddress = apiUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AntMeClient", clientVersion));
            client.DefaultRequestHeaders.From = configuration.ClientId.ToString() + "@antme.net";

            if (!string.IsNullOrEmpty(configuration.AccessToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration.AccessToken);

            return client;
        }

        /// <summary>
        /// Behandelt den erstellten Request-Task und wirft eventuelle Exceptions, setzt aber gleichzeitig den richtigen Connection-Status
        /// </summary>
        /// <typeparam name="T">Rückgabe-Dtentyp</typeparam>
        /// <param name="call">Request-Task</param>
        /// <returns>Methoden-Antwort</returns>
        private T Handle<T>(Task<HttpResponseMessage> call)
        {
            try
            {
                call.Wait(timeout);
                if (call.IsCompleted)
                {
                    var result = call.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        // Erfolg
                        var t2 = result.Content.ReadAsStringAsync();
                        t2.Wait(timeout);
                        if (t2.IsCompleted)
                        {
                            return JsonConvert.DeserializeObject<T>(t2.Result);
                        }

                        State = ConnectionState.NoConnection;
                        throw new TimeoutException();
                    }
                    else if (
                        result.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                        result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        State = ConnectionState.TokenInvalid;
                        throw new UnauthorizedAccessException();
                    }
                    else
                    {
                        State = ConnectionState.NoConnection;
                        throw new HttpRequestException();
                    }
                }
            }
            catch (Exception ex)
            {
                State = ConnectionState.NoConnection;
                throw;
            }

            State = ConnectionState.NoConnection;
            throw new TimeoutException();
        }

        /// <summary>
        /// Zeigt den Login-Screen an und verarbeitet alle Rückgabe-Möglichkeiten
        /// </summary>
        /// <param name="owner">Window-Handle des aufrufenden Fensters</param>
        private void PromptLogin(IWin32Window owner)
        {
            State = ConnectionState.LoggingIn;
            using (LoginForm form = new LoginForm())
            {
                switch (form.ShowDialog(owner))
                {
                    case DialogResult.OK:
                        configuration.AccessToken = form.Response.AccessToken;
                        configuration.UserId = form.Response.UserId;
                        configuration.Email = form.Response.Email;
                        configuration.Roles.Clear();
                        configuration.Roles.AddRange(form.Response.Roles);
                        SaveSettings();
                        State = ConnectionState.Connected;
                        break;
                    case DialogResult.Abort:
                        MessageBox.Show(owner, Properties.Resources.ErrorBody, Properties.Resources.ErrorTitle);
                        State = ConnectionState.NoConnection;
                        break;
                    case DialogResult.Cancel:
                        configuration.AccessToken = string.Empty;
                        SaveSettings();
                        State = ConnectionState.Disconnected;
                        break;
                }
            }
        }

        /// <summary>
        /// Lädt die Connector Settings.
        /// </summary>
        private void LoadSettings()
        {
            string path = configPath + pluginGuid + ".conf";
            try
            {
                configuration = new Configuration();
                using (Stream file = File.OpenRead(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                    Configuration config = serializer.Deserialize(file) as Configuration;
                    if (config != null)
                        configuration = config;
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Speichert die Connector-Settings.
        /// </summary>
        private void SaveSettings()
        {
            string path = configPath + pluginGuid + ".conf";
            using (Stream file = File.Open(path, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                serializer.Serialize(file, configuration);
            }
        }

        #endregion
    }
}
