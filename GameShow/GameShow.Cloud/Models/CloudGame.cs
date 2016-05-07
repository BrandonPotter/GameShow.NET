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
    }
}