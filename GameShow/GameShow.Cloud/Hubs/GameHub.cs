using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace GameShow.Cloud.Hubs
{
    public class GameHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<GameHub>();

        public void NotifyUpdate(string entityType, string id)
        {
            SendNotifyUpdateSignal(entityType, id);
        }

        public static void SendNotifyUpdateSignal(string entityType, string id)
        {
            hubContext.Clients.All.updateNotification(entityType, id);
        }
    }
}