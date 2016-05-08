using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GameShow.WpfApp.Activities
{
    public abstract class ActivityBase
    {
        public abstract UserControl GetShowDisplayControl();
        public abstract string GetActivityType();
        public abstract string GetTitle();

        public virtual void NotifyActive()
        {
            
        }
    }
}
