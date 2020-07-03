using LibraryManagement.Models;
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
            // Set source binding to BookImageCover
            Binding imgBinding = new Binding("BookImageCover");
            BindingOperations.SetBinding(imgCover, Image.SourceProperty, imgBinding);

            SaveBook.Visibility = Visibility.Visible;
            UpdateBook.Visibility = Visibility.Hidden;
            NameBook.IsEnabled = true;
            Category.IsEnabled = true;
            ListAuthor.IsEnabled = true;
            Author.Visibility = Visibility.Visible;
            Publisher.IsEnabled = true;
            DateManufacture.IsEnabled = true;
            Price.IsEnabled = true;
            btnSelectImage.Visibility = Visibility.Visible;

            //Tắt hết các tính năng không liên quan
            AddBook.Visibility = Visibility.Hidden;
            DeleteBook.Visibility = Visibility.Hidden;
            StopBorrowBook.Visibility = Visibility.Hidden;
            ListDisplayBook.IsEnabled = false;
            NextPage.Visibility = Visibility.Hidden;
            PrePage.Visibility = Visibility.Hidden;
            SearchBook.IsEnabled = false;
        }

        private void SaveBook_Click(object sender, RoutedEventArgs e)
        {
            // Set source binding back to SelectedItem.image
            Binding imgBinding = new Binding("SelectedItem.image");
            imgBinding.Converter = new ImageToSource();
            BindingOperations.SetBinding(imgCover, Image.SourceProperty, imgBinding);

            SaveBook.Visibility = Visibility.Hidden;
            UpdateBook.Visibility = Visibility.Visible;
            NameBook.IsEnabled = false;
            Category.IsEnabled = false;
            ListAuthor.IsEnabled = false;
            Publisher.IsEnabled = false;
            DateManufacture.IsEnabled = false;
            Price.IsEnabled = false;
            Author.Visibility = Visibility.Hidden;
            btnSelectImage.Visibility = Visibility.Hidden;

            //Mở lại các tính năng đã khóa
            AddBook.Visibility = Visibility.Visible;
            DeleteBook.Visibility = Visibility.Visible;
            StopBorrowBook.Visibility = Visibility.Visible;
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
    }
}
