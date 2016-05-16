﻿using System;
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

        public void ControllerHeartbeat(string gameId, string controllerToken, string nickname)
        {
            hubContext.Groups.Add(Context.ConnectionId, gameId);
            if (string.IsNullOrEmpty(controllerToken))
            {
                controllerToken = Guid.NewGuid().ToString();
                hubContext.Clients.Client(Context.ConnectionId).assignControllerToken(controllerToken);

                if (!string.IsNullOrEmpty(gameId))
                {
                    NotifyHostGameStateChanged(GameContext.Current.GameByID(gameId));
                }
            }
            GameContext.Current.SetControllerHeartbeat(controllerToken, gameId, Context.ConnectionId, nickname);

            if (!string.IsNullOrEmpty(gameId))
            {
                var g = GameContext.Current.GameByID(gameId);
                if (!g.HostConnected)
                {
                    string disconnectedFrame = $"/{gameId}/disconnected";
                    var controller = GameContext.Current.ControllerByToken(controllerToken);
                    ChangeControllerFrame(controller, disconnectedFrame);
                }
            }
        }

        public void HostHeartbeat(string gameId)
        {
            GameContext.Current.SetGameHostHeartbeat(gameId, Context.ConnectionId);
            NotifyHostDebugMessage(GameContext.Current.GameByID(gameId),
                "RX host heartbeat from connection " + Context.ConnectionId);
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

        public static void NotifyHostGameStateChanged(CloudGame game)
        {
            if (string.IsNullOrEmpty(game.HostConnectionID)) { return; }
            hubContext.Clients.Client(game.HostConnectionID).GameStateChanged(
                Newtonsoft.Json.JsonConvert.SerializeObject(game.ToCloudGameState()));
        }

        public static void NotifyHostDebugMessage(CloudGame game, string message)
        {
            if (string.IsNullOrEmpty(game.HostConnectionID))
            {
                hubContext.Clients.All.ServerDebugMessage("Could not find host for game " + (game.GameID ?? "unknown game ID"));
            }

            hubContext.Clients.Client(game.HostConnectionID).ServerDebugMessage(message);
        }
    }
}