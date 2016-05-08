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
            _hubConn.Start();
            InitProxy();
        }

        private void InitProxy()
        {
            _proxy.On<string>("NotifyGameStateChanged", (json) =>
            {
                _parentSession.NotifyGameStateChangeFromSignalR(
                    Newtonsoft.Json.JsonConvert.DeserializeObject<CloudGameState>(json));
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
