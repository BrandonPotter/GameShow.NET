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
    /// Interaction logic for CloudPanelControl.xaml
    /// </summary>
    public partial class CloudPanelControl : UserControl
    {
        public CloudPanelControl()
        {
            InitializeComponent();
            ShowContext.Current.Cloud.SessionUpdated += OnCloudSessionUpdated;
        }

        private void OnCloudSessionUpdated(CloudClient.CloudSession session)
        {
            if (session.CloudState == null) { return; }

            this.Dispatcher.Invoke(() =>
            {
                this.txtGameID.Text = session.CloudState.GameID;
                this.txtJoinUrl.Text = session.CloudState.JoinGameUrl ?? string.Empty;
                controllerList.UpdateControllerList(session.CloudState.Controllers);
            });
            
        }
    }
}
