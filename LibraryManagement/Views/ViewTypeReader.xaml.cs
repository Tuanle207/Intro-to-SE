﻿using System;
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
using System.Windows.Shapes;

namespace LibraryManagement.Views
{
    /// <summary>
    /// Interaction logic for ViewTypeReader.xaml
    /// </summary>
    public partial class ViewTypeReader : Window
    {
        public ViewTypeReader()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddTypeReader_Click(object sender, RoutedEventArgs e)
        {
            txtAddTypeReader.Text = "";
        }
    }
}
