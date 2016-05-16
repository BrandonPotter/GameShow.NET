using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GameShow.WpfApp.Activities;

namespace GameShow.WpfApp.Dialogs
{
    /// <summary>
    /// Interaction logic for NewActivityTypeDialog.xaml
    /// </summary>
    public partial class NewActivityTypeDialog : Window
    {
        public NewActivityTypeDialog()
        {
            InitializeComponent();
            var allTypes = FindDerivedTypes(Assembly.GetExecutingAssembly(), typeof (Activities.ActivityBase));

            foreach (var t in allTypes)
            {
                lbActivities.Items.Add(new NewTypeItem() {Type = t});
            }
        }

        public IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(t => t != baseType &&
                                                  baseType.IsAssignableFrom(t));
        }

        private class NewTypeItem
        {
            public Type Type { get; set; }
            public override string ToString()
            {
                var dna = Type.GetCustomAttribute<DisplayNameAttribute>();
                if (dna != null)
                {
                    return dna.DisplayName;
                }
                return Type.ToString();
            }
        }

        private void lbActivities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnOK.Visibility = (lbActivities.SelectedItem != null ? Visibility.Visible : Visibility.Hidden);
        }

        private void AddAndClose()
        {
            var nti = lbActivities.SelectedItem as NewTypeItem;
            if (nti == null) { return; }
            ActivityBase aB = Activator.CreateInstance(nti.Type) as ActivityBase;
            ShowContext.Current.Activities.Add(aB);
            ShowContext.Current.CurrentSelectedActivity.Value = aB;
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            AddAndClose();
        }
    }
}
