using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LibraryManagement.ViewModels
{
    public class BorrowBookViewModel : BaseViewModel
    {
        /// <summary>
        /// Variables, properties definition
        /// </summary>
        private ObservableCollection<Book> listBook;
        private ObservableCollection<Book> listBooksSelected;
        private ObservableCollection<Reader> listReader;
        private Reader readerSelected;
        private string bookKeyword;
        private string readerKeyword;
        public ObservableCollection<Book> ListBooksSelected { 
            get => listBooksSelected; 
            set { 
                listBooksSelected = value; 
                OnPropertyChanged(); 
            } }
        public ObservableCollection<Book> ListBook
        {
            get => listBook;
            set
            {
                listBook = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Reader> ListReader { 
            get => listReader; 
            set { 
                listReader = value;
                OnPropertyChanged();
            } 
        }
        public Reader ReaderSelected {
            get => readerSelected; 
            set {
                readerSelected = value; 
                OnPropertyChanged();
            }
        }
        public string BookKeyword { get => bookKeyword; set { bookKeyword = value; OnPropertyChanged(); SearchBook(); } }
        public string ReaderKeyword { get => readerKeyword; set { readerKeyword = value; OnPropertyChanged(); SearchReader(); } }

        public ICommand BorrowCommand { get; set; }
        public ICommand SelectBook { get; set; }
        public ICommand UnselectBook { get; set; }
        public ICommand UnselectAllBooks { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BorrowBookViewModel()
        {
            RetrieveDataAndClearInput();
            DefineCommands();
        }

        /// <summary>
        /// Get default data from DB and init some properties
        /// </summary>
        private void RetrieveDataAndClearInput()
        {
            if (ListBooksSelected == null)
            {
                ListBooksSelected = new ObservableCollection<Book>();
            }
            else
            {
                ListBooksSelected.Clear();
            }

            ListBook = new ObservableCollection<Book>(DataAdapter.Instance.DB.Books);

            if (ListReader == null)
            {
                ListReader = new ObservableCollection<Reader>(DataAdapter.Instance.DB.Readers);
            }
            ReaderSelected = null;
        }
        /// <summary>
        /// Definitions for commands
        /// </summary>
        private void DefineCommands()
        {
            BorrowCommand = new AppCommand<object>(
                p =>
                {
                    if (ReaderSelected == null || ListBooksSelected.Count == 0)
                    {
                        return false;
                    }
                    return true;
                },
                p =>
                {
                    // Add Borrow
                    var borrow = new BillBorrow { borrowDate = DateTime.Now, idReader = ReaderSelected.idReader };
                    DataAdapter.Instance.DB.BillBorrows.Add(borrow);

                    // Add Detail Borrow
                    foreach (var book in ListBooksSelected)
                    {
                        var detailBorrow = new DetailBillBorrow { idBillBorrow = borrow.idBillBorrow, idBook = book.idBook, returned = 0 };
                        DataAdapter.Instance.DB.DetailBillBorrows.Add(detailBorrow);
                        // Change book state
                        book.statusBook = "đã mượn";
                    }

                    try
                    {
                        // Save DB state
                        DataAdapter.Instance.DB.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        MessageBox.Show("Không thể thao tác vì lỗi cơ sở dữ liệu!");
                    }
                    catch (DbEntityValidationException)
                    {
                        MessageBox.Show("Không thể thao tác vì lỗi cơ sở dữ liệu!");
                    }
                    catch (NotSupportedException)
                    {
                        MessageBox.Show("Không thể thao tác vì lỗi cơ sở dữ liệu!");
                    }
                    catch (ObjectDisposedException)
                    {
                        MessageBox.Show("Không thể thao tác vì lỗi cơ sở dữ liệu!");
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Không thể thao tác vì lỗi cơ sở dữ liệu!");
                    }
                    finally
                    {
                        // Clear UI
                        RetrieveDataAndClearInput();
                    }
                });
            SelectBook = new AppCommand<object>(
                p =>
                {
                    return true;
                },
                p => {
                    Book bookSelected = p as Book;
                    if (bookSelected.statusBook == "có sẵn")
                    {
                        ListBook.Remove(bookSelected);
                        ListBooksSelected.Add(bookSelected);
                    }
                    else
                    {
                        MessageBox.Show("Sách đã được mượn rồi");
                    }
                });
            UnselectBook = new AppCommand<object>(
                p =>
                {
                    return true;
                },
                p =>
                {
                    Book bookSelected = p as Book;
                    ListBooksSelected.Remove(bookSelected);
                    ListBook.Add(bookSelected);
                });
            UnselectAllBooks = new AppCommand<object>(
                p =>
                {
                    return ListBooksSelected?.Count > 0;
                },
                p =>
                {
                    ListBooksSelected = new ObservableCollection<Book>();
                    ListBook = new ObservableCollection<Book>(DataAdapter.Instance.DB.Books);
                });
        }

        private void SearchReader()
        {
            if (ReaderKeyword == null || ReaderKeyword.Trim() == "")
            {
                ListReader = new ObservableCollection<Reader>(DataAdapter.Instance.DB.Readers);
                return;
            }
            try
            {
                var result = DataAdapter.Instance.DB.Readers.Where(
                                    reader => reader.nameReader.ToLower().StartsWith(ReaderKeyword.ToLower())
                                    );
                ListReader = new ObservableCollection<Reader>(result);
            }
            catch (ArgumentNullException)
            {
                ListReader = new ObservableCollection<Reader>(DataAdapter.Instance.DB.Readers);
                MessageBox.Show("Nhập tên độc giả để tìm kiếm!");
            }
        }

        private void SearchBook()
        {
            if (BookKeyword == null || BookKeyword.Trim() == "")
            {
                ListBook = new ObservableCollection<Book>(DataAdapter.Instance.DB.Books);
                return;
            }
            try
            {
                var result = DataAdapter.Instance.DB.Books.Where(
                                    book => book.nameBook.ToLower().StartsWith(BookKeyword.ToLower())
                                    );
                ListBook = new ObservableCollection<Book>(result);
            }
            catch (ArgumentNullException)
            {
                ListBook = new ObservableCollection<Book>(DataAdapter.Instance.DB.Books);
                MessageBox.Show("Nhập tên sách để tìm kiếm!");
            }
        }
    }
}
