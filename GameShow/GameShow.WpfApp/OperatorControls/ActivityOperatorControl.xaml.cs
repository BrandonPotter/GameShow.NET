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

namespace GameShow.WpfApp.OperatorControls
{
    /// <summary>
    /// Interaction logic for ActivityOperatorControl.xaml
    /// </summary>
    public partial class ActivityOperatorControl : UserControl
    {
        public ActivityOperatorControl()
        {
            InitializeComponent();
        }

        private Activities.ActivityBase _currentActivity = null;
        public void LoadActivity(Activities.ActivityBase activity)
        {
            _currentActivity = activity;
            pGrid.SelectedObject = activity;
        }

        private void runActivity_Click(object sender, RoutedEventArgs e)
        {
            if (_currentActivity == null) { return; }
            if (ShowContext.Current.ShowWindow == null)
            {
                MessageBox.Show("No show window");
                return;
            }

            ShowContext.Current.ShowWindow.SlideToContent(_currentActivity.GetShowDisplayControl(),
                TimeSpan.FromSeconds(1));
        }
    }
}
