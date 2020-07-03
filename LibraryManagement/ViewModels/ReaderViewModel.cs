using LibraryManagement.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModels
{
    class ReaderViewModel : BaseViewModel
    {
        private ReaderPaginatingCollection _List;
        public ReaderPaginatingCollection List { get => _List; set { _List = value; OnPropertyChanged(); } }

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
        public TypeReader SelectedTypeReader { get => _SelectedTypeReader; set { _SelectedTypeReader = value; OnPropertyChanged(); } }

        private int _idReader;
        public int IdReader { get => _idReader; set { _idReader = value; OnPropertyChanged(); } }

        private string _nameReader;
        public string NameReader {get => _nameReader; set { _nameReader = value; OnPropertyChanged(); } }

        private string _email;
        public string Email { get => _email; set {_email = value;OnPropertyChanged();} }


        private string _addressReader;
        public string AddressReader { get => _addressReader; set { _addressReader = value; OnPropertyChanged(); }}

        private int _idTypeReader;
        public int IdTypeReader { get => _idTypeReader; set { _idTypeReader = value; OnPropertyChanged(); } }

        private DateTime? _createdAt ;
        private DateTime? _dobReader;

        public DateTime? CreatedAt {
            get => _createdAt;
            set
            {   
                _createdAt = value; OnPropertyChanged(); } }
        public DateTime? DobReader { get => _dobReader; set {  _dobReader = value; OnPropertyChanged(); } 
        }
        private int _debt;
        public int Debt{ get => _debt; set { _debt = value; OnPropertyChanged();}}

        //Search Reader
        private string readerSearchKeyword;
        public string ReaderSearchKeyword
        {
            get => readerSearchKeyword;
            set
            {
                readerSearchKeyword = value;
                OnPropertyChanged();
                InitReaders(readerSearchKeyword);
            }
        }


        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand PrepareAddReaderCommand { get; set; }
        public ICommand CheckCommand { get; set;}
        public ICommand AddTypeReaderCommand { get; set;}
        public ICommand DeleteTypeReaderCommand { get; set;}
        public ICommand CancelCommand { get; set;}
        public ICommand CancelAddCommand { get; set; }
        public ICommand ReloadTypeReaderCommand { get; set;}
        public ICommand MoveToPreviousReadersPage { get; set; }
        public ICommand MoveToNextReadersPage { get; set; }
        public ICommand ExtendReaderCard { get; set; }

        public ReaderViewModel()
        {
            // Retrieve data from DB
            InitReaders();
           
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
                OnPropertyChanged("SelectedItem");
                ReaderSearchKeyword = null;
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
                if (NameReader == null || Email == null || AddressReader == null )
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
                    List.MoveToLastPage();
                    SetSelectedItemToFirstItemOfPage(false);
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
                List.Refresh();
                SetSelectedItemToFirstItemOfPage(true);
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
                   SetSelectedItemToFirstItemOfPage(true);
               });
            MoveToNextReadersPage = new AppCommand<object>(
                p =>
                {
                    return List.CurrentPage < List.PageCount;
                },
                p =>
                {
                    List.MoveToNextPage();
                    SetSelectedItemToFirstItemOfPage(true);
                });
            ExtendReaderCard = new AppCommand<object>(
                p => SelectedItem != null,
                p =>
                {
                    DateTime latestExtendedDate = SelectedItem.latestExtended;
                    int expiryDays = DataAdapter.Instance.DB.Paramaters.Find(4).valueParameter * 30;
                    if (DateTime.Now >= latestExtendedDate.AddDays(expiryDays))
                    {
                        SelectedItem.latestExtended = DateTime.Now;
                    }
                    else
                    {
                        SelectedItem.latestExtended = DateTime.Now.AddDays(SelectedItem.latestExtended.AddDays(expiryDays).Subtract(DateTime.Now).Days);
                    }
                    DataAdapter.Instance.DB.SaveChanges();
                });
            CancelAddCommand = new AppCommand<object>(
                p => true,
                p =>
                {
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
                        SelectedItem = SelectedItem;
                    }
                });
        }

        private void InitReaders(string keyword = null)
        {
            TypeReader = new ObservableCollection<TypeReader>(DataAdapter.Instance.DB.TypeReaders);
            if (keyword != null)
            {
                List = new ReaderPaginatingCollection(15, keyword);
            }
            else
            {
                List = new ReaderPaginatingCollection(15);
            }
            SetSelectedItemToFirstItemOfPage(true);
        }
        private void SetSelectedItemToFirstItemOfPage(bool isFirstItem)
        {
            if (List.Readers == null || List.Readers.Count == 0)
            {
                return;
            }
            if (isFirstItem)
            {
                SelectedItem = List.Readers.FirstOrDefault();
            }
            else
            {
                SelectedItem = List.Readers.LastOrDefault();
            }
        }
    }

}
