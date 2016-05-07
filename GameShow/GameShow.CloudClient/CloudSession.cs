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
        public async Task PushGameStateAsync(Game game)
        {
            _lastGamePushed = game;
            _lastPush = DateTime.Now;

            if (string.IsNullOrEmpty(game.CloudId))
            {
                game.CloudId = await new HttpClient().GetStringAsync(Endpoint("/game/newid"));
            }

            if (_autoPushThread == null)
            {
                _autoPushThread = new Thread(AutoPushThread);
                _autoPushThread.IsBackground = true;
                _autoPushThread.Start();
            }

            var gameStateJson = Newtonsoft.Json.JsonConvert.SerializeObject(game);
            HttpClient hc = new HttpClient();
            var postOp = hc.PostAsync(this.Endpoint("/game/push"),
                new StringContent(gameStateJson, Encoding.UTF8, "application/json"));

            var result = await postOp;
            if (result.IsSuccessStatusCode)
            {
                var cState =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<CloudGameState>(
                        result.Content.ReadAsStringAsync().Result);
                _lastCloudState = cState;
                SessionUpdated?.Invoke(this);
            }
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
    }
}
