using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameShow.GameModel;

namespace GameShow.CloudClient
{
    public delegate void CloudSessionUpdatedEvent(CloudSession session);

    public class CloudSession
    {
        public event CloudSessionUpdatedEvent SessionUpdated;

        private Thread _autoPushThread = null;

        private string Endpoint(string relativePath)
        {
            if (!relativePath.StartsWith("/"))
            {
                relativePath = $"/{relativePath}";
            }
            return "http://gshow.azurewebsites.net" + relativePath;
        }

        public CloudGameState CloudState
        {
            get
            {
                return _lastCloudState;
            }
        }

        private DateTime _lastPush = DateTime.Now;
        private Game _lastGamePushed = null;
        private CloudGameState _lastCloudState = null;
        private CloudSignalRClient _srClient = null;
        public async Task PushGameStateAsync(Game game)
        {
            _lastGamePushed = game;
            _lastPush = DateTime.Now;

            if (string.IsNullOrEmpty(game.CloudId))
            {
                game.CloudId = Newtonsoft.Json.JsonConvert.DeserializeObject<NewGameId>(
                    await new HttpClient().GetStringAsync(Endpoint("game/newid"))).GameID;
            }

            if (_autoPushThread == null)
            {
                _autoPushThread = new Thread(AutoPushThread);
                _autoPushThread.IsBackground = true;
                _autoPushThread.Start();
            }

            if (_srClient == null)
            {
                _srClient = new CloudSignalRClient(this, Endpoint("/signalr"));
            }

            var gameStateJson = Newtonsoft.Json.JsonConvert.SerializeObject(game);
            var cState = await PostJsonGetResponse<CloudGameState>(this.Endpoint("/game/push"), gameStateJson);

            _srClient.SendHostHeartbeat();

            if (cState != null)
            {
                _lastCloudState = cState;
                SessionUpdated?.Invoke(this);
            }
        }

        internal void NotifyGameStateChangeFromSignalR(CloudGameState gameState)
        {
            _lastCloudState = gameState;
            SessionUpdated?.Invoke(this);
            Logging.LogMessage("CloudSession", "Game state changed");
        }

        private async Task<T> PostJsonGetResponse<T>(string url, object payload)
        {
            return await PostJsonGetResponse<T>(url, Newtonsoft.Json.JsonConvert.SerializeObject(payload));
        }

        private async Task<T> PostJsonGetResponse<T>(string url, string jsonPayload)
        {
            HttpClient hc = new HttpClient();
            var postOp = hc.PostAsync(url,
                new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

            var result = await postOp;
            if (result.IsSuccessStatusCode)
            {
                var deserializedObj =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<T>(
                        result.Content.ReadAsStringAsync().Result);
                return deserializedObj;
            }

            Logging.LogMessage("CloudSession", "Error retrieving " + url);
            return default(T);
        }

        private void AutoPushThread()
        {
            while (true)
            {
                try
                {
                    AutoPushIfNecessary();
                }
                catch (Exception ex)
                {
                    Logging.LogMessage("CloudSession", "Exception auto-push: " + ex.Message);
                }

                System.Threading.Thread.Sleep(500);
            }
        }

        private void AutoPushIfNecessary()
        {
            if (_lastGamePushed == null) { return; }
            if (DateTime.Now.Subtract(_lastPush).TotalSeconds < 10)
            {
                return;
            }

#pragma warning disable 4014
            PushGameStateAsync(_lastGamePushed);
#pragma warning restore 4014
        }

        public void BlinkController(CloudGameStateController controller)
        {
            PostJsonGetResponse<string>(Endpoint("/controller/blink"), controller);
        }
    }
}
