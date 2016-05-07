using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameShow.Cloud.Models;
using Microsoft.AspNet.SignalR;

namespace GameShow.Cloud.Hubs
{
    public class GameHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<GameHub>();

        public void ControllerHeartbeat(string gameId, string controllerToken)
        {
            hubContext.Groups.Add(Context.ConnectionId, gameId);
            if (string.IsNullOrEmpty(controllerToken))
            {
                controllerToken = Guid.NewGuid().ToString();
                hubContext.Clients.Client(Context.ConnectionId).assignControllerToken(controllerToken);
            }
            GameContext.Current.SetControllerHeartbeat(controllerToken, gameId, Context.ConnectionId);

            if (!string.IsNullOrEmpty(gameId))
            {
                var g = GameContext.Current.GameByID(gameId);
                if (!g.HostConnected)
                {
                    ChangeControllerFrame(GameContext.Current.ControllerByToken(controllerToken),
                        $"/{gameId}/disconnected");
                }
            }
        }

        public static void ChangeControllerFrame(CloudGameController controller, string targetFrame)
        {
            hubContext.Clients.Client(controller.ConnectionID).changeFrame(targetFrame);
            controller.CurrentFrame = targetFrame;
        }

        public static void BlinkController(CloudGameController controller)
        {
            hubContext.Clients.Client(controller.ConnectionID).blink();
        }
    }
}