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
    /// Interaction logic for BookScreen.xaml
    /// </summary>
    public partial class BookScreen : UserControl
    {
        public BookScreen()
        {
            InitializeComponent();
        }

        private void UpdateBook_Click(object sender, RoutedEventArgs e)
        {
            SaveBook.Visibility = Visibility.Visible;
            UpdateBook.Visibility = Visibility.Hidden;
            NameBook.IsEnabled = true;
            Category.IsEnabled = true;
            ListAuthor.IsReadOnly = false;
            Author.Visibility = Visibility.Visible;
            Publisher.IsEnabled = true;
            DateManufacture.IsEnabled = true;
            Price.IsEnabled = true;
        }

        private void SaveBook_Click(object sender, RoutedEventArgs e)
        {
            SaveBook.Visibility = Visibility.Hidden;
            UpdateBook.Visibility = Visibility.Visible;
            NameBook.IsReadOnly = true;
            Category.IsEnabled = false;
            ListAuthor.IsReadOnly = true;
            Author.IsEnabled = false;
            Publisher.IsEnabled = false;
            DateManufacture.IsEnabled = false;
            Price.IsEnabled = false;
        }
    }
}
