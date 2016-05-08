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

namespace GameShow.WpfApp.Controls
{
    /// <summary>
    /// Interaction logic for ControllerListItem.xaml
    /// </summary>
    public partial class ControllerListItem : UserControl
    {
        public ControllerListItem()
        {
            InitializeComponent();
        }

        public string ControllerToken { get; set; }
        public GameModel.CloudGameStateController ControllerObject { get; set; }

        public void UpdateControllerInfo(GameModel.CloudGameStateController controller)
        {
            if (controller.IsOnline)
            {
                orbStatus.Fill = Brushes.Lime;
            }
            else
            {
                orbStatus.Fill = Brushes.Red;
            }

            ControllerToken = controller.ControllerToken;
            ControllerObject = controller;
            txtName.Text = controller.ControllerToken;
        }

        private void btnBlink_Click(object sender, RoutedEventArgs e)
        {
            ShowContext.Current.Cloud.BlinkController(ControllerObject);
        }
    }
}
