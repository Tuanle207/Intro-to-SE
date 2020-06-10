using LibraryManagement.ViewModels;
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
    /// Interaction logic for ReportScreen.xaml
    /// </summary>
    public partial class ReportScreen : UserControl
    {
        public ReportScreen()
        {
            InitializeComponent();
        }



        private void year_Loaded(object sender, RoutedEventArgs e)
        {
            year.Items.Clear();
            for (int i = 2010; i <= DateTime.Now.Year; i++)
            {
                year.Items.Add(i);
            }
        }

        private void month_Loaded(object sender, RoutedEventArgs e)
        {
            month.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                month.Items.Add(i);
            }
        }

        private void day_Loaded(object sender, RoutedEventArgs e)
        {
            Day.SelectedDate = DateTime.Today;
        }
    }
}
