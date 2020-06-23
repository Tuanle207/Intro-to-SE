using LibraryManagement.Models;
using LibraryManagement.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Data.Entity.Migrations;

namespace LibraryManagement.ViewModels
{
    class ReaderViewModel : BaseViewModel
    {
        private ObservableCollection<Reader> _List;
        public ObservableCollection<Reader> List { get => _List; set { _List = value; OnPropertyChanged(); } }


        private ObservableCollection<TypeReader> _TypeReader;
        public ObservableCollection<TypeReader> TypeReader { get => _TypeReader; set { _TypeReader = value; OnPropertyChanged(); } }
        private Reader _SelectedItem;
        public Reader SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    IdReader = SelectedItem.idReader;
                    NameReader = SelectedItem.nameReader;
                    DobReader = SelectedItem.dobReader;
                    Email = SelectedItem.email;
                    AddressReader = SelectedItem.addressReader;
                    CreatedAt = SelectedItem.createdAt;
                    Debt = SelectedItem.debt.ToString();
                    IdTypeReader = SelectedItem.idTypeReader;
                    SelectedTypeReader = SelectedItem.TypeReader;
                }
            }
        }
        private TypeReader _SelectedTypeReader;
        public TypeReader SelectedTypeReader
        {
            get => _SelectedTypeReader;
            set
            {
                _SelectedTypeReader = value;
                OnPropertyChanged();
            }
        }

        private int _idReader;
        public int IdReader { get => _idReader; set { _idReader = value; OnPropertyChanged(); } }

        private string _nameReader;
        public string NameReader {
            get => _nameReader;
            set {
                    string str = value as string;
                    if (!(str.Length > 0 && str != null))
                    {
                        throw new Exception("Tên người dùng là bắt buộc");
                    }
                    if (value.Length < 3)
                    {
                        throw new Exception("Số ký tự tối thiểu là 3");
                    }
                    _nameReader = value; OnPropertyChanged(); 
            } 
        }

        private string _email;
        public string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
        public string Email { get => _email; set {
                string str = value as string;
                if (!(str.Length > 0 && str != null))
                {
                    throw new Exception("Email người dùng là bắt buộc");
                }
                if (!Regex.IsMatch(value, pattern))
                {
                    throw new Exception("Bạn cần nhập đúng email!");
                }
                _email = value;
                OnPropertyChanged();
            } }


        private string _addressReader;
        public string AddressReader
        {
            get => _addressReader;
            set
            {
                if (!(value.Length > 0 && value != null))
                {
                    throw new Exception("Địa chỉ người dùng là bắt buộc");
                }
                if (value.Length < 6)
                {
                    throw new Exception("Số ký tự tối thiểu là 6");
                }
                _addressReader = value; OnPropertyChanged();
            }
        }

        


        private int _idTypeReader;
        public int IdTypeReader { 
                                    get => _idTypeReader; 
                                    set 
                                    {
                                        _idTypeReader = value;
                                        OnPropertyChanged(); 
                                    } 
                                }


        private DateTime? _createdAt ;
        private DateTime? _dobReader;

        public DateTime? CreatedAt {
            get => _createdAt;
            set
            {   
                string str = value.ToString() as string;
                if (str.Length < 8 || str == null)
                {
                    throw new Exception("Ngày tạo người dùng là bắt buộc");
                }
                _createdAt = value; OnPropertyChanged(); } }
        public DateTime? DobReader { 
                                    get => _dobReader; 
                                    set 
                                    {
                                        string str = value.ToString() as string;
                                        if (str.Length < 8 || str == null)
                                        {
                                            throw new Exception("Ngày sinh người dùng là bắt buộc");
                                        }
                                        _dobReader = value; OnPropertyChanged(); 
                                    } 
        }
        private float _debt;
        public string Debt
        {
            get => _debt.ToString();
            set
            {
                float f;
                if (!(float.TryParse(value, out f) && value != null && value != ""))
                {
                    throw new Exception("Vui lòng nhập số!");
                }    
                _debt = Convert.ToSingle(value);
                OnPropertyChanged();
            }
        }
        //Search Reader
        private string readerSearchKeyword;
        public string ReaderSearchKeyword
        {
            get => readerSearchKeyword;
            set
            {
                readerSearchKeyword = value;
                OnPropertyChanged();
                SearchReader();
            }
        }
        private void SearchReader()
        {
            if (ReaderSearchKeyword == null || ReaderSearchKeyword.Trim() == "")
            {
                List = new ObservableCollection<Reader>(DataAdapter.Instance.DB.Readers);
                return;
            }
            try
            {
                var result = DataAdapter.Instance.DB.Readers.Where(
                                    reader => reader.nameReader.ToLower().StartsWith(ReaderSearchKeyword.ToLower())
                                    );
                List = new ObservableCollection<Reader>(result);
            }
            catch (ArgumentNullException)
            {
                List = new ObservableCollection<Reader>(DataAdapter.Instance.DB.Readers);
                MessageBox.Show("Từ khóa tìm kiếm rỗng!");
            }
        }


        public AppCommand<object> AddCommand { get; }
        public AppCommand<object> EditCommand { get; }
        public AppCommand<object> DeleteCommand { get; }
        public AppCommand<object> AddTypeReaderCommand { get; }
        public AppCommand<object> DeleteTypeReaderCommand { get; }

        //Reload TypeReader
        public AppCommand<object> ReloadTypeReaderCommand { get; }
        public ReaderViewModel()
        {
            // Retrieve data from DB
            RetrieveData();
            ReloadTypeReaderCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedTypeReader == null)
                    return false;
                return true;

            }, (p) =>
            {
                TypeReader = new ObservableCollection<TypeReader>(DataAdapter.Instance.DB.TypeReaders);
            });
            AddCommand = new AppCommand<object>((p) =>
            {
                if (TypeReader == null)
                    return false;
                return true;

            }, (p) =>
            {
                try
                {
                    var Reader = new Reader()
                    {
                        nameReader = NameReader,
                        dobReader = (DateTime)DobReader,
                        email = Email,
                        addressReader = AddressReader,
                        createdAt = DateTime.Today,
                        debt = 0,
                        idTypeReader = SelectedTypeReader.idTypeReader
                    };
                    DataAdapter.Instance.DB.Readers.Add(Reader);
                    DataAdapter.Instance.DB.SaveChanges();
                    List.Add(Reader);
                    MessageBox.Show("Bạn đã thêm người dùng thành công");
                }
                catch(Exception)
                {
                    MessageBox.Show("Bạn chưa chọn loại người đọc!");
                }
            });

            EditCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedTypeReader == null)
                    return false;

                var displayList = DataAdapter.Instance.DB.Readers.Where(x => x.idReader == SelectedItem.idReader);
                if (displayList != null && displayList.Count() != 0)
                    return true;

                return false;

            }, (p) =>
            {
                TypeReader = new ObservableCollection<TypeReader>(DataAdapter.Instance.DB.TypeReaders);
                var Reader = DataAdapter.Instance.DB.Readers.Where(x => x.idReader == SelectedItem.idReader).SingleOrDefault();
                Reader.nameReader = SelectedItem.nameReader;
                Reader.dobReader = (DateTime)SelectedItem.dobReader;
                Reader.email = SelectedItem.email;
                Reader.addressReader = SelectedItem.addressReader;
                Reader.debt = SelectedItem.debt;
                Reader.createdAt = (DateTime)SelectedItem.createdAt;
                Reader.idTypeReader = SelectedTypeReader.idTypeReader;
                DataAdapter.Instance.DB.Readers.AddOrUpdate(Reader);
                DataAdapter.Instance.DB.SaveChanges();
                System.ComponentModel.ICollectionView view = CollectionViewSource.GetDefaultView(List);
                view.Refresh();
                MessageBox.Show("Bạn đã sửa thông tin người dùng thành công");
            });
            DeleteCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedTypeReader == null)
                    return false;

                var displayList = DataAdapter.Instance.DB.Readers.Where(x => x.idReader == SelectedItem.idReader);
                if (displayList != null && displayList.Count() != 0)
                    return true;

                return false;

            }, (p) =>
            {
                var Reader = DataAdapter.Instance.DB.Readers.Where(x => x.idReader == SelectedItem.idReader).SingleOrDefault();
                DataAdapter.Instance.DB.Readers.Remove(Reader);
                DataAdapter.Instance.DB.SaveChanges();
                List.Remove(Reader);
                MessageBox.Show("Bạn đã xóa người dùng thành công");
            });
        }

        private void RetrieveData()
        {
            List = new ObservableCollection<Reader>(DataAdapter.Instance.DB.Readers);
            TypeReader = new ObservableCollection<TypeReader>(DataAdapter.Instance.DB.TypeReaders);
        }
    }

}
