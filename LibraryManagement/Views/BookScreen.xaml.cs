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
            CancelUpdate.Visibility = Visibility.Visible;
            UpdateBook.Visibility = Visibility.Hidden;
            NameBook.IsEnabled = true;
            Category.IsEnabled = true;
            ListAuthor.IsEnabled = true;
            Author.Visibility = Visibility.Visible;
            Publisher.IsEnabled = true;
            DateManufacture.IsEnabled = true;
            Price.IsEnabled = true;
            Add.Visibility = Visibility.Visible;
            Add.IsEnabled = false;
            btnSelectImage.Visibility = Visibility.Visible;
            //Tắt hết các tính năng không liên quan

            DeleteBook.Visibility = Visibility.Hidden;
            //StopBorrowBook.Visibility = Visibility.Hidden;
            ListDisplayBook.IsEnabled = false;
            NextPage.Visibility = Visibility.Hidden;
            PrePage.Visibility = Visibility.Hidden;
            SearchBook.IsEnabled = false;
        }

        private void SaveBook_Click(object sender, RoutedEventArgs e)
        {
            SaveBook.Visibility = Visibility.Hidden;
            UpdateBook.Visibility = Visibility.Visible;
            NameBook.IsEnabled = false;
            Category.IsEnabled = false;
            ListAuthor.IsEnabled = false;
            Publisher.IsEnabled = false;
            DateManufacture.IsEnabled = false;
            Price.IsEnabled = false;
            Author.Visibility = Visibility.Hidden;

            //Mở lại các tính năng đã khóa
            AddBook.Visibility = Visibility.Visible;
            DeleteBook.Visibility = Visibility.Visible;
        
            ListDisplayBook.IsEnabled = true;
            NextPage.Visibility = Visibility.Visible;
            PrePage.Visibility = Visibility.Visible;
            SearchBook.IsEnabled = true;
        }
        private void ListDisplayBook_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                ListDisplayBook.UnselectAll();
        }
        private void ButtonAuthor_Click(object sender, RoutedEventArgs e)
        {
            Window window = new AuthorScreen();
            window.ShowDialog();
        }

        private void ButtonCategory_Click(object sender, RoutedEventArgs e)
        {
            Window window = new CategoryScreen();
            window.ShowDialog();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddBook.Visibility = Visibility.Visible;
            AddBooks.Visibility = Visibility.Visible;
            ButtonAuthor.Visibility = Visibility.Visible;
            ButtonCategory.Visibility = Visibility.Visible;
         
            CancelAdd.Visibility = Visibility.Visible;
            UpdateBook.IsEnabled = false;
            DeleteBook.IsEnabled = false;
        }

        private void CancelAdd_Click(object sender, RoutedEventArgs e)
        {
            Add.Visibility = Visibility.Visible;
            CancelAdd.Visibility = Visibility.Hidden;
            AddBook.Visibility = Visibility.Hidden;
            AddBooks.Visibility = Visibility.Hidden;
            ButtonAuthor.Visibility = Visibility.Hidden;
            ButtonCategory.Visibility = Visibility.Hidden;
           
            UpdateBook.Visibility = Visibility.Visible;
            UpdateBook.IsEnabled = true;
            DeleteBook.Visibility = Visibility.Visible;
            DeleteBook.IsEnabled = true;
            ListDisplayBook.IsEnabled = true;


        }

        private void CancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            Add.Visibility = Visibility.Visible;
            CancelUpdate.Visibility = Visibility.Hidden;
            CancelAdd.Visibility = Visibility.Hidden;
            UpdateBook.Visibility = Visibility.Hidden;
            SaveBook.Visibility = Visibility.Hidden;
            Add.IsEnabled = true;
            AddBook.Visibility = Visibility.Hidden;
            AddBooks.Visibility = Visibility.Hidden;
            ButtonAuthor.Visibility = Visibility.Hidden;
            ButtonCategory.Visibility = Visibility.Hidden;
          
            UpdateBook.Visibility = Visibility.Visible;
          //  DeleteBook.Visibility = Visibility.Visible;
          //  DeleteBook.IsEnabled = false;
            ListDisplayBook.IsEnabled = true;

            SaveBook.Visibility = Visibility.Hidden;
   
            NameBook.IsEnabled = false;
            Category.IsEnabled = false;
            ListAuthor.IsEnabled = false;
            Author.Visibility = Visibility.Hidden;
            Publisher.IsEnabled = false;
            DateManufacture.IsEnabled = false;
            Price.IsEnabled = false;
            btnSelectImage.Visibility = Visibility.Hidden;
            //Tắt hết các tính năng không liên quan
            DeleteBook.Visibility = Visibility.Visible;
            NextPage.Visibility = Visibility.Visible;
            PrePage.Visibility = Visibility.Visible;
            SearchBook.IsEnabled = true;
        }
    }
}
