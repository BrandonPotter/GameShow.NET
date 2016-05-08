using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShow.GameModel
{
    public class Game
    {
        public string CloudId { get; set; }
        public string Name { get; set; }
        public List<ControllerPrompt> ControllerPrompts { get; set; }
    }
}
