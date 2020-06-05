using LibraryManagement.Views;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void hideNavigation()
        {
            if (ButtonCloseMenu.Visibility == Visibility.Visible)
            {
                ButtonCloseMenu.Visibility = Visibility.Collapsed;
                ButtonOpenMenu.Visibility = Visibility.Visible;
                Storyboard sb = this.FindResource("CloseMenu") as Storyboard;
                sb.Begin();
            }
        }
        private void loadSizeSomeControls()
        {
            gridForNavigation.Width = gridMain.ActualWidth / 20;
            ButtonOpenMenu.Width = gridForNavigation.Width;
            ButtonOpenMenu.Height = gridMain.ActualHeight / 15;
            ButtonCloseMenu.Width = gridForNavigation.Width;
            ButtonCloseMenu.Height = gridMain.ActualHeight / 15;
            EasingDoubleKeyFrame keyFrameOpenMenuToOpen = ((this.Resources["OpenMenu"] as Storyboard).Children[0]
                as DoubleAnimationUsingKeyFrames).KeyFrames[1] as EasingDoubleKeyFrame;
            keyFrameOpenMenuToOpen.Value = gridMain.ActualWidth / 5;
            EasingDoubleKeyFrame keyFrameOpenMenuToClose = ((this.Resources["OpenMenu"] as Storyboard).Children[0]
                as DoubleAnimationUsingKeyFrames).KeyFrames[0] as EasingDoubleKeyFrame;
            keyFrameOpenMenuToClose.Value = gridMain.ActualWidth / 20;
            EasingDoubleKeyFrame keyFrameCloseMenuToOpen = ((this.Resources["CloseMenu"] as Storyboard).Children[0]
                as DoubleAnimationUsingKeyFrames).KeyFrames[0] as EasingDoubleKeyFrame;
            keyFrameCloseMenuToOpen.Value = gridMain.ActualWidth / 5;
            EasingDoubleKeyFrame keyFrameCloseMenuToClose = ((this.Resources["CloseMenu"] as Storyboard).Children[0]
                as DoubleAnimationUsingKeyFrames).KeyFrames[1] as EasingDoubleKeyFrame;
            keyFrameCloseMenuToClose.Value = gridMain.ActualWidth / 20;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadSizeSomeControls();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl urc = null;
            gridForContent.Children.Clear();
            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemHome":
                    urc = new HomeScreen();
                    gridForContent.Children.Add(urc);
                    hideNavigation();
                    break;
                case "ItemMember":
                    urc = new ReaderScreen();
                    gridForContent.Children.Add(urc);
                    hideNavigation();
                    break;
                case "ItemBook":
                    urc = new BookScreen();
                    gridForContent.Children.Add(urc);
                    hideNavigation();
                    break;
                case "ItemReport":
                    urc = new ReportScreen();
                    gridForContent.Children.Add(urc);
                    hideNavigation();
                    break;
                case "ItemLogout":
                    break;
            }
        }
    }
}
