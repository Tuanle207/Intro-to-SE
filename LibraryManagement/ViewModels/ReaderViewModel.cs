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
using System.ComponentModel;

namespace LibraryManagement.ViewModels
{
    class ReaderViewModel : BaseViewModel
    {
        private PagingCollectionView<Reader> _List;
        public PagingCollectionView<Reader> List { get => _List; set { _List = value; OnPropertyChanged(); } }


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
                    Debt = SelectedItem.debt;
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
                    _nameReader = value; OnPropertyChanged(); 
            } 
        }

        private string _email;
        public string Email { get => _email; set {
                _email = value;
                OnPropertyChanged();
            } }


        private string _addressReader;
        public string AddressReader
        {
            get => _addressReader;
            set
            {
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
                _createdAt = value; OnPropertyChanged(); } }
        public DateTime? DobReader { 
                                    get => _dobReader; 
                                    set 
                                    {
                                        _dobReader = value; OnPropertyChanged(); 
                                    } 
        }
        private int _debt;
        public int Debt
        {
            get => _debt;
            set
            { 
                _debt = value;
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
                List = new PagingCollectionView<Reader>(DataAdapter.Instance.DB.Readers.ToList(), 15);
                return;
            }
            try
            {
                var result = DataAdapter.Instance.DB.Readers.Where(
                                    reader => reader.nameReader.ToLower().StartsWith(ReaderSearchKeyword.ToLower())
                                    );
                List = new PagingCollectionView<Reader>(result.ToList(), 15);
            }
            catch (ArgumentNullException)
            {
                List = new PagingCollectionView<Reader>(DataAdapter.Instance.DB.Readers.ToList(), 15);
                MessageBox.Show("Từ khóa tìm kiếm rỗng!");
            }
        }


        public AppCommand<object> AddCommand { get; set; }
        public AppCommand<object> EditCommand { get; set; }
        public AppCommand<object> DeleteCommand { get; set; }
        public AppCommand<object> PrepareAddReaderCommand { get; set; }
        public AppCommand<object> CheckCommand { get; set;}
        public AppCommand<object> AddTypeReaderCommand { get; set;}
        public AppCommand<object> DeleteTypeReaderCommand { get; set;}
        public AppCommand<object> CancelCommand { get; set;}
        public AppCommand<object> ReloadTypeReaderCommand { get; set;}
        public ICommand MoveToPreviousReadersPage { get; set; }
        public ICommand MoveToNextReadersPage { get; set; }

        public ReaderViewModel()
        {
            // Retrieve data from DB
            RetrieveData();
            CancelCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedTypeReader == null)
                    return false;
                return true;

            }, (p) =>
            {
                SelectedItem.nameReader = NameReader;
                SelectedItem.dobReader = (DateTime)DobReader;
                SelectedItem.email = Email;
                SelectedItem.addressReader = AddressReader;
                SelectedItem.debt = Debt;
                SelectedItem.createdAt = (DateTime)CreatedAt;
                SelectedItem.idTypeReader = IdTypeReader;
                RetrieveData();
                OnPropertyChanged("SelectedItem");
            });
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
                if (NameReader == null || DobReader == null || Email == null || AddressReader == null || CreatedAt == null )
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
                        latestExtended = (DateTime)CreatedAt,
                        debt = 0,
                        idTypeReader = SelectedTypeReader.idTypeReader
                    };
                    DataAdapter.Instance.DB.Readers.Add(Reader);
                    DataAdapter.Instance.DB.SaveChanges();
                    InitProperty(Reader.idReader);
                    MessageBox.Show("Bạn đã thêm người dùng thành công");
                }
                catch(Exception)
                {
                    MessageBox.Show("Đã có lỗi xảy ra!");
                }
            });

            EditCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;

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
                DataAdapter.Instance.DB.SaveChanges();
                InitProperty(Reader.idReader);
                MessageBox.Show("Bạn đã sửa thông tin người dùng thành công");
            });
            DeleteCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;

            }, (p) =>
            {
                var Reader = DataAdapter.Instance.DB.Readers.Where(x => x.idReader == SelectedItem.idReader).SingleOrDefault();
                DataAdapter.Instance.DB.Readers.Remove(Reader);
                DataAdapter.Instance.DB.SaveChanges();
                InitProperty(-1);
                MessageBox.Show("Bạn đã xóa người dùng thành công");
            });
            PrepareAddReaderCommand = new AppCommand<object>(
                p => true,
                p =>
                {
                    NameReader = null;
                    AddressReader = null;
                    Email = null;
                    DobReader = new DateTime(2000, 1, 1);
                    CreatedAt = DateTime.Now;
                    Debt = 0;
                    SelectedTypeReader = TypeReader.FirstOrDefault();
                });
            CheckCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedTypeReader == null)
                    return false;
                return true;

            }, (p) =>
            {
               
            });
            MoveToPreviousReadersPage = new AppCommand<object>(
               p =>
               {
                   return List.CurrentPage > 1;
               },
               p =>
               {
                   List.MoveToPreviousPage();
               });
            MoveToNextReadersPage = new AppCommand<object>(
                p =>
                {
                    return List.CurrentPage < List.PageCount;
                },
                p =>
                {
                    List.MoveToNextPage();
                });

        }

        private void RetrieveData()
        {
            TypeReader = new ObservableCollection<TypeReader>(DataAdapter.Instance.DB.TypeReaders);
            InitProperty(-1);
        }
        private void InitProperty(int id)
        {
            List = new PagingCollectionView<Reader>(DataAdapter.Instance.DB.Readers.ToList(), 15);
            //if (List.Count > 0)
            //{
            //    SelectedItem = id == -1 ? (Reader)List.GetItemAt(0) : (Reader)List.GetItemById("Reader", id);
            //    if (id != -1)
            //    {
            //        List.MoveToSelectedItem("Reader", id);
            //    }
            //}
        }
    }

}
