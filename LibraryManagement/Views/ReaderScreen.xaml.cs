﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using LibraryManagement.Models;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Views
{
    /// <summary>
    /// Interaction logic for MemberScreen.xaml
    /// </summary>
    public partial class ReaderScreen : UserControl
    {
        public ReaderScreen()
        {
            InitializeComponent();
        }

        private void ButtonAddReader_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Command.CanExecute(null))
            {
                button.Command.Execute(null);
            }
            AddReader wd = new AddReader();
            wd.ShowDialog();
        }

        private void CapnhatReader_Click(object sender, RoutedEventArgs e)
        {
            LuuReader.Visibility = Visibility.Visible;
            CapnhatReader.Visibility = Visibility.Hidden;
            NameReader.IsReadOnly = false;
            Address.IsReadOnly = false;
            Email.IsReadOnly = false;
            DobReader.IsEnabled = true;
            CreatAt.IsEnabled = true;
            Debt.IsReadOnly = false;
            ListDisplayReader.IsEnabled = false;
            TypeReader.IsEnabled = true;
            btnDeleteReader.IsEnabled = false;
            btnAddReader.IsEnabled = false;
            btnTypeReader.IsEnabled = false;
            btnCancel.IsEnabled = true;
        }

        private void LuuReader_Click(object sender, RoutedEventArgs e)
        {
            LuuReader.Visibility = Visibility.Hidden;
            CapnhatReader.Visibility = Visibility.Visible;
            NameReader.IsReadOnly = true;
            Address.IsReadOnly = true;
            Email.IsReadOnly = true;
            DobReader.IsEnabled = false;
            CreatAt.IsEnabled = false;
            Debt.IsReadOnly = true;
            TypeReader.IsEnabled = false;
            ListDisplayReader.IsEnabled = true;
            btnDeleteReader.IsEnabled = true;
            btnAddReader.IsEnabled = true;
            btnTypeReader.IsEnabled = true;
            btnCancel.IsEnabled = false;
        }

        private void ButtonTypeReader_Click(object sender, RoutedEventArgs e)
        {
            ViewTypeReader w = new ViewTypeReader();
            w.ShowDialog();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            LuuReader.Visibility = Visibility.Hidden;
            CapnhatReader.Visibility = Visibility.Visible;
            NameReader.IsReadOnly = true;
            Address.IsReadOnly = true;
            Email.IsReadOnly = true;
            DobReader.IsEnabled = false;
            CreatAt.IsEnabled = false;
            Debt.IsReadOnly = true;
            TypeReader.IsEnabled = false;
            ListDisplayReader.IsEnabled = true;
            btnDeleteReader.IsEnabled = true;
            btnAddReader.IsEnabled = true;
            btnTypeReader.IsEnabled = true;
            btnCancel.IsEnabled = false;
        }

        private void ListDisplayReader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                ListDisplayReader.UnselectAll();
        }
    }
}
