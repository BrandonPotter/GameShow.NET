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
using GameShow.WpfApp.Activities;

namespace GameShow.WpfApp.Controls
{
    /// <summary>
    /// Interaction logic for ActivityListControl.xaml
    /// </summary>
    public partial class ActivityListControl : UserControl
    {
        public ActivityListControl()
        {
            InitializeComponent();
            this.DataContext = ShowContext.Current;
            lbActivities.ItemsSource = ShowContext.Current.Activities;
            ShowContext.Current.CurrentSelectedActivity.ValueChanged += OnCurrentActivityValueChanged;
            SyncSelectionToCurrentActivity();
        }

        private void OnCurrentActivityValueChanged(object sender, EventArgs e)
        {
            SyncSelectionToCurrentActivity();
        }

        private void SyncSelectionToCurrentActivity()
        {
            if (ShowContext.Current.CurrentSelectedActivity.Value == null)
            {
                this.lbActivities.SelectedValue = null;
                this.btnMoveActivityDown.IsEnabled = false;
                this.btnMoveActivityUp.IsEnabled = false;
                this.btnRemove.IsEnabled = false;
                return;
            }

            this.btnMoveActivityDown.IsEnabled = true;
            this.btnMoveActivityUp.IsEnabled = true;
            this.btnRemove.IsEnabled = true;

            if (ShowContext.Current.CurrentSelectedActivity.Value == lbActivities.SelectedItem) { return; }
            _suppressSelectionChanged = true;
            lbActivities.SelectedItem = ShowContext.Current.CurrentSelectedActivity.Value;
            _suppressSelectionChanged = false;

            
        }

        private bool _suppressSelectionChanged = false;
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_suppressSelectionChanged) { return; }
            if (lbActivities.SelectedItem != null)
            {
                var sItem = lbActivities.SelectedItem as ActivityBase;
                ShowContext.Current.CurrentSelectedActivity.Value = sItem;
            }
        }

        private void btnAddActivity_Click(object sender, RoutedEventArgs e)
        {
            ShowContext.Current.MainWindow.ShowDialog(new Dialogs.NewActivityTypeDialog());
        }
    }
}
