using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameShow.GameModel;

namespace GameShow.Cloud.Models
{
    public class CloudGame
    {
        public string GameID { get; set; }
        public DateTime? LastPush { get; set; }
        public Game Game { get; set; }
        public DateTime? LastHostSignalRHeartbeat { get; set; }
        public string HostConnectionID { get; set; }

        public bool HostConnected
        {
            get
            {
                if (LastPush == null) { return false; }
                if (DateTime.Now.Subtract(LastPush.Value).TotalSeconds < 60) { return true; }
                return false;
            }
        }

        public CloudGameState ToCloudGameState()
        {
            CloudGameState cgs = new CloudGameState();
            cgs.GameID = this.GameID;
            cgs.Controllers = new List<CloudGameStateController>();
            cgs.JoinGameUrl = "http://gshow.azurewebsites.net/" + this.GameID;

            foreach (var c in GameContext.Current.ControllersByGame(this.GameID))
            {
                cgs.Controllers.Add(new CloudGameStateController()
                {
                    ControllerToken = c.ControllerToken,
                    IsOnline = c.IsConnected,
                    Nickname = c.Nickname ?? "Unknown"
                });
            }

            return cgs;
        }
    }
}