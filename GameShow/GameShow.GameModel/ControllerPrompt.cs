using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShow.GameModel
{
    public class ControllerPrompt
    {
        public string ControllerToken { get; set; }
        public string Text { get; set; }
        public List<PromptButton> PromptButtons { get; set; }
    }
}
