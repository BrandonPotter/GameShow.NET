using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using GameShow.GameModel;

namespace GameShow.WpfApp.Activities.GenericText
{
    public class GenericTextActivity : ActivityBase
    {
        private string _ttd = string.Empty;
        public string TextToDisplay
        {
            get { return _ttd; }
            set
            {
                _ttd = value;
                if (_gtuc != null)
                {
                    _gtuc.txtText.Text = value;
                }
            }
        }

        private GenericTextUserControl _gtuc = null;
        public override UserControl GetShowDisplayControl()
        {
            GenericTextUserControl gtuc = new GenericTextUserControl();
            gtuc.txtText.Text = TextToDisplay ?? string.Empty;
            _gtuc = gtuc;
            return gtuc;
        }

        public override string GetActivityType()
        {
            return "Generic Text Display";
        }

        public override string GetTitle()
        {
            return $"Display Text {TextToDisplay ?? string.Empty}";
        }

        public override void NotifyActive()
        {
            List<ControllerPrompt> cPrompts = new List<ControllerPrompt>();

            foreach (var c in ShowContext.Current.Cloud.CloudState.Controllers)
            {
                cPrompts.Add(new ControllerPrompt()
                {
                    Text = TextToDisplay ?? string.Empty,
                    ControllerToken = c.ControllerToken
                });
            }

            ShowContext.Current.Game.ControllerPrompts = cPrompts;
            ShowContext.Current.Cloud.PushGameStateAsync(ShowContext.Current.Game);
        }
    }
}
