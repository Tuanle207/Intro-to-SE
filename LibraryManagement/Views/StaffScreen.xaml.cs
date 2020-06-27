using LibraryManagement.Models;
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

namespace LibraryManagement.Views
{
    /// <summary>
    /// Interaction logic for StaffSc.xaml
    /// </summary>
    public partial class StaffScreen : UserControl
    {
        public StaffScreen()
        {
            InitializeComponent();
        }

        private void btnAddStaff_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Command.CanExecute(null))
            {
                button.Command.Execute(null);
            }
            AddStaff wd = new AddStaff();
            wd.ShowDialog();
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            btnSua.Visibility = Visibility.Hidden;
            btnLuu.Visibility = Visibility.Visible;
            NameStaff.IsReadOnly = false;
            Address.IsReadOnly = false;
            PhoneNumber.IsReadOnly = false;
            DobStaff.IsEnabled = true;
            UserName.IsReadOnly = false;
            cbPermission.IsEnabled = true;
            btnCancel.IsEnabled = true;
            btnAddStaff.IsEnabled = false;
            btnDeleStaff.IsEnabled = false;
            btnResetPassword.IsEnabled = false;
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            btnLuu.Visibility = Visibility.Hidden;
            btnSua.Visibility = Visibility.Visible;
            NameStaff.IsReadOnly = true;
            Address.IsReadOnly = true;
            PhoneNumber.IsReadOnly = true;
            DobStaff.IsEnabled = false;
            UserName.IsReadOnly = true;
            cbPermission.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnAddStaff.IsEnabled = true;
            btnDeleStaff.IsEnabled = true;
            btnResetPassword.IsEnabled = true;
        }

        private void cbPermission_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPermission.SelectedIndex == 0)
            {
                setPermissionView(1);
            }
            else setPermissionView(2);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            btnLuu.Visibility = Visibility.Hidden;
            btnSua.Visibility = Visibility.Visible;
            NameStaff.IsReadOnly = true;
            Address.IsReadOnly = true;
            PhoneNumber.IsReadOnly = true;
            DobStaff.IsEnabled = false;
            UserName.IsReadOnly = true;
            cbPermission.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnAddStaff.IsEnabled = true;
            btnDeleStaff.IsEnabled = true;
            btnResetPassword.IsEnabled = true;
        }
    }
}
