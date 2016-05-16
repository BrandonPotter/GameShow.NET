using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GameShow.GameModel;

namespace GameShow.WpfApp.Activities
{
    public abstract class ActivityBase
    {
        public abstract UserControl GetShowDisplayControl();
        public abstract string GetActivityType();
        public abstract string GetTitle();

        public string Title => GetTitle();

        public virtual void NotifyActive()
        {
            
        }

        public virtual void NotifyEvent(string eventId, CloudGameStateController controller)
        {
            
        }
    }
}
