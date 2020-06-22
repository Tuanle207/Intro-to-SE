﻿using LibraryManagement.Models;
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
        private List<CheckBox> cb = new List<CheckBox>();
        public StaffScreen()
        {
            InitializeComponent();
            cb.Add(cbLapTheDocGia);
            cb.Add(cbTiepNhanSachMoi);
            cb.Add(cbTraCuuSach);
            cb.Add(cbChoMuonSach);
            cb.Add(cbNhanTraSach);
            cb.Add(cbLapPhieuThuTien);
            cb.Add(cbLapBaoCao);
            cb.Add(cbThayDoiQuyDinh);
            cb.Add(cbPhanQuyen);
        }

        private void lvNhanVien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvNhanVien.SelectedItems.Count > 0)
            {
                Staff staff = (Staff)lvNhanVien.SelectedItem;
                setPermissionView(staff.idPermission);
            }
        }

        private void setPermissionView(int idPermission)
        {
            if (idPermission == 1)
            {
                cbPermission.SelectedIndex = 0;
                foreach (var item in cb)
                {
                    item.IsChecked = true;
                }
            }
            else
            {
                cbPermission.SelectedIndex = 1;
                foreach (var item in cb)
                {
                    if (item != cbThayDoiQuyDinh && item != cbPhanQuyen)
                        item.IsChecked = true;
                    else item.IsChecked = false;
                }
            }
        }

        private void btnAddStaff_Click(object sender, RoutedEventArgs e)
        {
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
        }

        private void cbPermission_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPermission.SelectedIndex == 0)
            {
                setPermissionView(1);
            }
            else setPermissionView(2);
        }
    }
}