using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShow.GameModel;
using Microsoft.AspNet.SignalR.Client;

namespace GameShow.CloudClient
{
    internal class CloudSignalRClient
    {
        private HubConnection _hubConn = null;
        private IHubProxy _proxy = null;
        private CloudSession _parentSession = null;
        internal CloudSignalRClient(CloudSession session, string signalRUrl)
        {
            _parentSession = session;
            _hubConn = new HubConnection(signalRUrl);
            _proxy = _hubConn.CreateHubProxy("GameHub");
            _hubConn.Start().Wait();
            Logging.LogMessage("Cloud", "Connected");
            InitProxy();
        }

        private void InitProxy()
        {
            _proxy.On<string>("NotifyGameStateChanged", (json) =>
            {
                Logging.LogMessage("Cloud", "NotifyGameStateChanged");
                _parentSession.NotifyGameStateChangeFromSignalR(
                    Newtonsoft.Json.JsonConvert.DeserializeObject<CloudGameState>(json));
            });

            _proxy.On<string>("ServerDebugMessage", (message) =>
            {
                Logging.LogMessage("Cloud Debug", message ?? string.Empty);
            });
        }

        internal void SendHostHeartbeat()
        {
            if (_parentSession.CloudState != null)
            {
                _proxy.Invoke<string>("HostHeartbeat", _parentSession.CloudState.GameID);
            }
        }
    }
}
