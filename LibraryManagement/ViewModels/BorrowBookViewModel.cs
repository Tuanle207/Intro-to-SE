using LibraryManagement.Models;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.Entity;

namespace LibraryManagement.ViewModels
{
    class BorrowBookViewModel : BaseViewModel
    {
        /// <summary>
        /// Variables, properties definition
        /// </summary>
        private PagingCollectionView<Book> books;
        private ObservableCollection<Book> listBooksSelected;
        private PagingCollectionView<Reader> readers;
        private Reader readerSelected;
        private string bookKeyword;
        private string readerKeyword;
        public ObservableCollection<Book> ListBooksSelected { 
            get => listBooksSelected; 
            set { 
                listBooksSelected = value; 
                OnPropertyChanged(); 
            } }
        public PagingCollectionView<Book> Books
        {
            get => books;
            set
            {
                books = value;
                OnPropertyChanged();
            }
        }
        public PagingCollectionView<Reader> Readers { 
            get => readers; 
            set { 
                readers = value;
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
        public ICommand MoveToPreviousBooksPage { get; set; }
        public ICommand MoveToNextBooksPage { get; set; }
        public ICommand MoveToPreviousReadersPage { get; set; }
        public ICommand MoveToNextReadersPage { get; set; }

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

            Books = new PagingCollectionView<Book>(DataAdapter.Instance.DB.Books.ToList());

            if (Readers == null)
            {
                Readers = new PagingCollectionView<Reader>(DataAdapter.Instance.DB.Readers.ToList());
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
                    // Select reader first for checking pre condition
                    if (ReaderSelected == null)
                    {
                        MessageBox.Show("Vui lòng chọn độc giả trước tiên!");
                        return;
                    }

                    if (!CheckReaderExpiry(readerSelected))
                    {
                        MessageBox.Show("Thẻ độc giả đã hết hạn!");
                        return;
                    }

                    Book bookSelected = p as Book;
                    if (ListBooksSelected.Contains(bookSelected))
                    {
                        MessageBox.Show("Sách này được chọn rồi!");
                        return;
                    }
                    else
                    {
                        // Check if reader has any borrowed book that's expired
                        int maxBorrowBookAllowed = DataAdapter.Instance.DB.Paramaters.Find(5).valueParameter;
                        if (CheckBookExpiryOfReader(ReaderSelected))
                        {
                            MessageBox.Show("Độc giả đang có sách quá hạn không thể mượn thêm sách!");
                            return;
                        }
                        // Check if reader reach max book allowed to be borrowed
                        if (GetBookBorrowedOfReader(ReaderSelected) + ListBooksSelected.Count
                        >= maxBorrowBookAllowed) // Max Book
                        {
                            MessageBox.Show($"Độc giả đã đạt tới số sách tối đa được mượn ({maxBorrowBookAllowed} cuốn)!");
                            return;
                        }
                    }
                    
                    if (bookSelected.statusBook == "có sẵn")
                    {
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
                });
            UnselectAllBooks = new AppCommand<object>(
                p =>
                {
                    return ListBooksSelected?.Count > 0;
                },
                p =>
                {
                    ListBooksSelected = new ObservableCollection<Book>();
                });
            MoveToPreviousBooksPage = new AppCommand<object>(
                p =>
                {
                    return Books.CurrentPage > 1;
                },
                p =>
                {
                    Books.MoveToPreviousPage();
                });
            MoveToNextBooksPage = new AppCommand<object>(
                p =>
                {
                    return Books.CurrentPage < Books.PageCount;
                },
                p =>
                {
                    Books.MoveToNextPage();
                });
            MoveToPreviousReadersPage = new AppCommand<object>(
                p =>
                {
                    return Readers.CurrentPage > 1;
                },
                p =>
                {
                    Books.MoveToPreviousPage();
                });
            MoveToNextReadersPage = new AppCommand<object>(
                p =>
                {
                    return Readers.CurrentPage < Books.PageCount;
                },
                p =>
                {
                    Books.MoveToNextPage();
                });
        }

        private void SearchReader()
        {
            if (ReaderKeyword == null || ReaderKeyword.Trim() == "")
            {
                Readers = new PagingCollectionView<Reader>(DataAdapter.Instance.DB.Readers.ToList());
                return;
            }
            try
            {
                var result = DataAdapter.Instance.DB.Readers.Where(
                    reader => reader.nameReader.ToLower().StartsWith(ReaderKeyword.ToLower())
                    );
                Readers = new PagingCollectionView<Reader>(result.ToList());
            }
            catch (ArgumentNullException)
            {
                Readers = new PagingCollectionView<Reader>(DataAdapter.Instance.DB.Readers.ToList());
                MessageBox.Show("Nhập tên độc giả để tìm kiếm!");
            }
        }

        private void SearchBook()
        {
            if (BookKeyword == null || BookKeyword.Trim() == "")
            {
                Books = new PagingCollectionView<Book>(DataAdapter.Instance.DB.Books.ToList());
                return;
            }
            try
            {
                var result = DataAdapter.Instance.DB.Books.Where(
                                    book => book.nameBook.ToLower().StartsWith(BookKeyword.ToLower())
                                    );
                Books = new PagingCollectionView<Book>(result.ToList());
            }
            catch (ArgumentNullException)
            {
                Books = new PagingCollectionView<Book>(DataAdapter.Instance.DB.Books.ToList());
                MessageBox.Show("Nhập tên sách để tìm kiếm!");
            }
        }

        private int GetBookBorrowedOfReader(Reader reader)
        {
            int result = (from br in DataAdapter.Instance.DB.BillBorrows
                        join dbr in DataAdapter.Instance.DB.DetailBillBorrows
                        on br.idBillBorrow equals dbr.idBillBorrow
                        where br.idReader == reader.idReader && dbr.returned == 0
                        select 1
                        ).Count();
            return result;
        }

        private bool CheckBookExpiryOfReader(Reader reader)
        {
            int maxBorrowDays = DataAdapter.Instance.DB.Paramaters.Find(7).valueParameter;
            int borrowExpired = (from br in DataAdapter.Instance.DB.BillBorrows
                                 join dbr in DataAdapter.Instance.DB.DetailBillBorrows
                                 on br.idBillBorrow equals dbr.idBillBorrow
                                 where br.idReader == reader.idReader && DbFunctions.DiffDays(DateTime.Now, br.borrowDate) > maxBorrowDays
                                 select 1
                    ).Count();
            
            return borrowExpired > 0 ? true : false;
        }
        private bool CheckReaderExpiry(Reader reader)
        {
            int expiryMonths = DataAdapter.Instance.DB.Paramaters.Find(3).valueParameter;
            int daysLeft = (DateTime.Now.AddDays(expiryMonths * 30) - reader.latestExtended).Days;
            return daysLeft >= 0 ? true : false;
        }
    }
}
