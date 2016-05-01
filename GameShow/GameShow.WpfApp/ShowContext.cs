using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShow.WpfApp
{
    public class ShowContext
    {
        private ShowContext()
        {
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
        public string GameName { get; set; }
        public MainWindow MainWindow { get; set; }
        public ShowWindow ShowWindow { get; set; }
    }
}
