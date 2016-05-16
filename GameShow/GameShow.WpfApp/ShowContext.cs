using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShow.CloudClient;
using GameShow.GameModel;
using GameShow.WpfApp.Activities;

namespace GameShow.WpfApp
{
    public class ShowContext
    {
        private ShowContext()
        {
            Game = new Game();
        }

        private static ShowContext _current = null;
        public static ShowContext Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new ShowContext();
                }
                return _current;
            }
        }
        
        public int ScreenCount { get; set; }
        public int? ShowScreenIndex { get; set; }

        public string GameName
        {
            get
            {
                if (Game == null) { return string.Empty; }
                return Game.Name ?? string.Empty;
            }
            set
            {
                if (Game == null) { return; }
                Game.Name = value;
            }
        }

        public MainWindow MainWindow { get; set; }
        public ShowWindow ShowWindow { get; set; }
        public Game Game { get; set; }
        private Observable<ActivityBase> _selActivity = new Observable<ActivityBase>();
        public Observable<ActivityBase> CurrentSelectedActivity => _selActivity;

        private Observable<ActivityBase> _liveActivity = new Observable<ActivityBase>();
        public Observable<ActivityBase> CurrentLiveActivity => _liveActivity;

        private ObservableCollection<ActivityBase> _activityList = null;
        public ObservableCollection<ActivityBase> Activities => _activityList ?? (_activityList = new ObservableCollection<ActivityBase>());

        private CloudSession _cloud = null;
        public CloudSession Cloud
        {
            get
            {
                if (_cloud == null)
                {
                    _cloud = new CloudSession();
                    _cloud.ControllerEvent += OnControllerEvent;
                }
                return _cloud;
            }
        }

        private void OnControllerEvent(CloudSession session, string controllerToken, string eventType, string eventValue)
        {
            var controllerObj =
                Cloud.CloudState.Controllers.FirstOrDefault(
                    c => c.ControllerToken.ToUpperInvariant() == controllerToken.ToUpperInvariant());
            if (controllerObj == null) { return; }

            if (CurrentLiveActivity.Value != null)
            {
                try
                {
                    CurrentLiveActivity.Value.NotifyControllerEvent(controllerObj, eventType, eventValue);
                }
                catch (Exception ex)
                {
                    Logging.LogMessage("ShowContext",
                        "Exception delivering controller event to live activity: " + ex.Message);
                }
            }

            if ((eventType ?? string.Empty).ToUpperInvariant() == "ANSWER")
            {
                _dictControllerAnswers[controllerToken] = eventValue;
                var targetPrompt = Game.ControllerPrompts.FirstOrDefault(cp => cp.ControllerToken == controllerToken);
                if (targetPrompt != null)
                {
                    if (targetPrompt.PromptButtons != null)
                    {
                        var targetButton =
                            targetPrompt.PromptButtons.FirstOrDefault(
                                b => b.EventType == eventType && b.EventValue == eventValue);
                        if (targetButton != null)
                        {
                            targetButton.Color = "green";
                            Cloud.PushGameStateAsync(Game);
                        }
                    }
                }
            }
        }

        private Dictionary<string, string> _dictControllerAnswers =
            new Dictionary<string, string>();

        public void SetQuestionPromptsAndAnswers(string question, IEnumerable<PromptButton> answerButtons)
        {
            _dictControllerAnswers.Clear();
            List<GameModel.ControllerPrompt> cPrompts = new List<ControllerPrompt>();

            foreach (var controller in Cloud.CloudState.Controllers)
            {
                GameModel.ControllerPrompt cPrompt = new ControllerPrompt()
                {
                    ControllerToken = controller.ControllerToken,
                    Text = question,
                    PromptButtons = answerButtons.ToList()
                };

                foreach (var a in cPrompt.PromptButtons)
                {
                    if (a.EventType.ToUpperInvariant() == "ANSWER")
                    {
                        a.Color = "darkgray";

                        if (_dictControllerAnswers.ContainsKey(controller.ControllerToken) &&
                            _dictControllerAnswers[controller.ControllerToken] == a.EventValue)
                        {
                            a.Color = "green";
                        }
                    }
                }

                cPrompts.Add(cPrompt);
            }

            Game.ControllerPrompts = cPrompts;
            Cloud.PushGameStateAsync(Game);
        }
    }
}
