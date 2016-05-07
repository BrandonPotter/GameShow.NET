using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameShow.GameModel;

namespace GameShow.WpfApp.Controls
{
    /// <summary>
    /// Interaction logic for ControllerList.xaml
    /// </summary>
    public partial class ControllerList : UserControl
    {
        public ControllerList()
        {
            InitializeComponent();
        }

        public void UpdateControllerList(List<CloudGameStateController> controllers)
        {
            var listItems = this.stackControllers.Children.Cast<ControllerListItem>().ToList();

            foreach (var existingLi in listItems)
            {
                if (!controllers.Any(c => c.ControllerToken == existingLi.ControllerToken))
                {
                    stackControllers.Children.Remove(existingLi);
                }
            }

            foreach (var c in controllers)
            {
                var liTarget = listItems.FirstOrDefault(li => li.ControllerToken == c.ControllerToken);
                if (liTarget == null)
                {
                    liTarget = new ControllerListItem();
                    liTarget.UpdateControllerInfo(c);
                    stackControllers.Children.Add(liTarget);
                }
            }
        }
    }
}
