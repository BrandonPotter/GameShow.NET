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

namespace GameShow.WpfApp.Activities.GenericText
{
    /// <summary>
    /// Interaction logic for GenericTextUserControl.xaml
    /// </summary>
    public partial class GenericTextUserControl : UserControl
    {
        public GenericTextUserControl()
        {
            InitializeComponent();
        }

        public GenericTextUserControl(GenericTextActivity activity)
        {
            InitializeComponent();
        }
    }
}
