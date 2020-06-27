using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LibraryManagement.ViewModels
{
    class StaffViewModel : BaseViewModel
    {
        private ObservableCollection<Staff> _List;
        public ObservableCollection<Staff> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Permission> _Permission;
        public ObservableCollection<Permission> Permission { get => _Permission; set { _Permission = value; OnPropertyChanged(); } }

        private Staff _SelectedItem;

        public Staff SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    IdStaff = SelectedItem.idStaff;
                    NameStaff = SelectedItem.nameStaff;
                    DobStaff = SelectedItem.dobStaff;
                    AddressStaff = SelectedItem.addressStaff;
                    PhoneNumberStaff = SelectedItem.phoneNumberStaff;
                    AccountStaff = SelectedItem.accountStaff;
                    PasswordStaff = SelectedItem.passwordStaff;
                    IdPermission = SelectedItem.idPermission;
                    SelectedPermission = SelectedItem.Permission;
                }
               
            }
        }
        private int _idStaff;
        public int IdStaff { get => _idStaff; set { _idStaff = value; OnPropertyChanged(); } }

        private string _nameStaff;
        public string NameStaff
        {
            get => _nameStaff;
            set
            {
                string str = value as string;
                _nameStaff = value; OnPropertyChanged();
            }
        }

        private string _addressStaff;
        public string AddressStaff
        {
            get => _addressStaff;
            set
            {
                _addressStaff = value; OnPropertyChanged();
            }
        }

        private DateTime? _dobStaff;
        public DateTime? DobStaff
        {
            get => _dobStaff;
            set
            {
                _dobStaff = value; OnPropertyChanged();
            }
        }

        private string _phoneNumberStaff;
        public string PhoneNumberStaff
        {
            get => _phoneNumberStaff;
            set
            {
                _phoneNumberStaff = value; OnPropertyChanged();
            }
        }

        private string _accountStaff;
        public string AccountStaff
        {
            get => _accountStaff;
            set
            {
                _accountStaff = value; OnPropertyChanged();
            }
        }

        private string _passwordStaff;
        public string PasswordStaff
        {
            get => _passwordStaff;
            set
            {
                _passwordStaff = value; OnPropertyChanged();
            }
        }

        private Permission _SelectedPermission;
        public Permission SelectedPermission
        {
            get => _SelectedPermission;
            set
            {
                _SelectedPermission = value;
                OnPropertyChanged();
            }
        }

        private int _idPermission;
        public int IdPermission
        {
            get => _idPermission;
            set
            {
                _idPermission = value;
                OnPropertyChanged();
            }
        }

        private string randomPassword()
        {
            Random rd = new Random();
            return rd.Next(1000, 9999).ToString();
        }

        //Search Reader
        private string staffSearchKeyword;
        public string StaffSearchKeyword
        {
            get => staffSearchKeyword;
            set
            {
                staffSearchKeyword = value;
                OnPropertyChanged();
                SearchStaff();
            }
        }

        private void SearchStaff()
        {
            if (StaffSearchKeyword == null || StaffSearchKeyword.Trim() == "")
            {
                List = new ObservableCollection<Staff>(DataAdapter.Instance.DB.Staffs);
                return;
            }
            try
            {
                var result = DataAdapter.Instance.DB.Staffs.Where(
                                    staff => staff.nameStaff.ToLower().StartsWith(StaffSearchKeyword.ToLower())
                                    );
                List = new ObservableCollection<Staff>(result);
            }
            catch (ArgumentNullException)
            {
                List = new ObservableCollection<Staff>(DataAdapter.Instance.DB.Staffs);
                MessageBox.Show("Từ khóa tìm kiếm rỗng!");
            }
        }

        public ICommand ResetPasswordCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand PrepareAddCommand { get; set; }
        public ICommand InitProperties { get; set; }
        public ICommand NotifyOnSelectedItemChange { get; set; }

        public StaffViewModel()
        {
            // Retrieve data from DB
            RetrieveData();
            AddCommand = new AppCommand<object>((p) =>
            {
                if (NameStaff == null || PhoneNumberStaff == null || AddressStaff == null || AccountStaff == null || PasswordStaff == null)
                    return false;
                return true;

            }, (p) =>
            {
                try
                {
                    var Staff = new Staff()
                    {
                        nameStaff = NameStaff,
                        dobStaff = (DateTime)DobStaff,
                        addressStaff = AddressStaff,
                        phoneNumberStaff = PhoneNumberStaff,
                        accountStaff = AccountStaff,
                        passwordStaff = EncryptSHA512Managed(PasswordStaff),
                        idPermission = SelectedPermission.idPermission
                    };
                    DataAdapter.Instance.DB.Staffs.Add(Staff);
                    DataAdapter.Instance.DB.SaveChanges();
                    List.Add(Staff);
                    SelectedItem = Staff;
                    MessageBox.Show("Bạn đã thêm nhân viên thành công");
                }
                catch (Exception)
                {
                    MessageBox.Show("Bạn chưa chọn loại quyền!");
                }
            });

            EditCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedPermission == null)
                    return false;

                var displayList = DataAdapter.Instance.DB.Staffs.Where(x => x.idStaff == SelectedItem.idStaff);
                if (displayList != null && displayList.Count() != 0)
                    return true;

                return false;

            }, (p) =>
            {

                var Staff = DataAdapter.Instance.DB.Staffs.Where(x => x.idStaff == SelectedItem.idStaff).SingleOrDefault();
                Staff.nameStaff = NameStaff;
                Staff.dobStaff = (DateTime)DobStaff;
                Staff.addressStaff = AddressStaff;
                Staff.phoneNumberStaff = PhoneNumberStaff;
                Staff.idPermission = SelectedPermission.idPermission;
                DataAdapter.Instance.DB.Staffs.AddOrUpdate(Staff);
                DataAdapter.Instance.DB.SaveChanges();
                System.ComponentModel.ICollectionView view = CollectionViewSource.GetDefaultView(List);
                view.Refresh();
                MessageBox.Show("Bạn đã sửa thông tin nhân viên thành công");
            });
            DeleteCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedItem?.accountStaff == "admin")
                    return false;

                return true;

            }, (p) =>
            {
                var Staff = DataAdapter.Instance.DB.Staffs.Where(x => x.idStaff == SelectedItem.idStaff).SingleOrDefault();
                DataAdapter.Instance.DB.Staffs.Remove(Staff);
                DataAdapter.Instance.DB.SaveChanges();
                List.Remove(Staff);
                SelectedItem = List.FirstOrDefault();
                MessageBox.Show("Bạn đã xóa nhân viên thành công");
            });

            ResetPasswordCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedPermission == null)
                    return false;

                var displayList = DataAdapter.Instance.DB.Staffs.Where(x => x.idStaff == SelectedItem.idStaff);
                if (displayList != null && displayList.Count() != 0)
                    return true;

                return false;

            }, (p) =>
            {

                var Staff = DataAdapter.Instance.DB.Staffs.Where(x => x.idStaff == SelectedItem.idStaff).SingleOrDefault();
                PasswordStaff = randomPassword();
                Staff.passwordStaff = EncryptSHA512Managed(PasswordStaff);
                DataAdapter.Instance.DB.Staffs.AddOrUpdate(Staff);
                DataAdapter.Instance.DB.SaveChanges();
                System.ComponentModel.ICollectionView view = CollectionViewSource.GetDefaultView(List);
                view.Refresh();
                MessageBox.Show("Bạn đã reset mật khẩu cho tài khoản " + Staff.accountStaff + " thành công, mật khẩu mới là: " + PasswordStaff + ", hãy đăng nhập và đổi mật khẩu mới!");
            });

            PrepareAddCommand = new AppCommand<object>(
                p => true,
                p =>
                {
                    NameStaff = null;
                    DobStaff = new DateTime(2000, 1, 1);
                    PhoneNumberStaff = null;
                    AddressStaff = null;
                    AccountStaff = null;
                    PasswordStaff = null;
                    SelectedPermission = Permission.FirstOrDefault();
                });
            InitProperties = new AppCommand<object>(
                p => true,
                p =>
                {
                    SelectedItem = List.FirstOrDefault();
                });
            NotifyOnSelectedItemChange = new AppCommand<object>(
                p => true,
                p =>
                {
                    OnPropertyChanged("SelectedItem");
                });
        }

        public string EncryptSHA512Managed(string password)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytPassword = uEncode.GetBytes(password);
            SHA512Managed sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(bytPassword);
            return Convert.ToBase64String(hash);
        }

        private void RetrieveData()
        {
            List = new ObservableCollection<Staff>(DataAdapter.Instance.DB.Staffs);
            Permission = new ObservableCollection<Permission>(DataAdapter.Instance.DB.Permissions);
        }
    }
}
