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

        public bool HostConnected
        {
            get
            {
                if (LastPush == null) { return false; }
                if (DateTime.Now.Subtract(LastPush.Value).TotalSeconds < 60) { return true; }
                return false;
            }
        }
    }
}