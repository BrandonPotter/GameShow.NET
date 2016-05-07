using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShow.GameModel
{
    public class CloudGameState
    {
        public string GameID { get; set; }
        public List<CloudGameStateController> Controllers { get; set; }
    }
}