using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShow.CloudClient;
using GameShow.GameModel;

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

        private CloudSession _cloud = null;
        public CloudSession Cloud
        {
            get
            {
                if (_cloud == null)
                {
                    _cloud = new CloudSession();
                }

                return _cloud;
            }
        }
    }
}
