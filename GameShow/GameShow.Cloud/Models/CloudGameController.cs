using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameShow.Cloud.Models
{
    public class CloudGameController
    {
        public string ConnectionID { get; set; }
        public string GameID { get; set; }
        public string ControllerToken { get; set; }
        public DateTime LastHeartbeat { get; set; }
        public string Nickname { get; set; }

        public bool IsConnected
        {
            get { return DateTime.Now.Subtract(LastHeartbeat).TotalSeconds > 10; }
        }

        public string CurrentFrame { get; set; }
    }
}