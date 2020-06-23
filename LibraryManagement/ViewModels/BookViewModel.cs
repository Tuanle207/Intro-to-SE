using LibraryManagement.Models;
using LibraryManagement.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace LibraryManagement.ViewModels
{
    class BookViewModel : BaseViewModel
    {
        LibraryManagementEntities DB = new LibraryManagementEntities();

        private ObservableCollection<Models.Book> _ListBook;
        public ObservableCollection<Models.Book> ListBook { get => _ListBook; set { _ListBook = value; OnPropertyChanged(); } }

        private ObservableCollection<Models.Author> _ListAuthors;
        public ObservableCollection<Models.Author> ListAuthors { get => _ListAuthors; set { _ListAuthors = value; OnPropertyChanged(); } }

        //Selected data of DB
        private Book _SelectedItem;
        public Book SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    idBook = SelectedItem.idBook;
                    nameBook = SelectedItem.nameBook;
                    SelectedPublisher = SelectedItem.Publisher;
                    SelectedCategory = SelectedItem.Category;
                    dateAddBook = SelectedItem.dateAddBook;
                    dateManufacture = SelectedItem.dateManufacture;
                    price = SelectedItem.price;
                    ListAuthors = new ObservableCollection<Author>(SelectedItem.Authors);
                }
            }
        }

        private Models.Category _SelectedCategory;
        public Models.Category SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                _SelectedCategory = value;
                OnPropertyChanged();
            }
        }

        private Models.Publisher _SelectedPublisher;
        public Models.Publisher SelectedPublisher
        {
            get => _SelectedPublisher;
            set
            {
                _SelectedPublisher = value;
                OnPropertyChanged();
            }
        }

        private Models.Author _SelectedAuthor;
        public Models.Author SelectedAuthor
        {
            get => _SelectedAuthor;
            set
            {
                _SelectedAuthor = value;
                OnPropertyChanged();
            }
        }

        private int _idBook;
        public int idBook { get => _idBook; set { _idBook = value; OnPropertyChanged(); } }

        private string _nameBook;
        public string nameBook { get => _nameBook; set { _nameBook = value; OnPropertyChanged(); } }

        private ObservableCollection<Models.Category> _category;
        public ObservableCollection<Models.Category> category { get => _category; set { _category = value; OnPropertyChanged(); } }

        private ObservableCollection<Models.Publisher> _publisher;
        public ObservableCollection<Models.Publisher> publisher { get => _publisher; set { _publisher = value; OnPropertyChanged(); } }

        private ObservableCollection<Models.Author> _author;
        public ObservableCollection<Models.Author> author { get => _author; set { _author = value; OnPropertyChanged(); } }

        private DateTime _dateManufacture;
        public DateTime dateManufacture { get => _dateManufacture; set { _dateManufacture = value; OnPropertyChanged(); } }

        private DateTime _dateAddBook;
        public DateTime dateAddBook
        {
            get
            {
                return _dateAddBook;
            }
            set { _dateAddBook = value; OnPropertyChanged(); }
        }

        private int _price;
        public int price { get => _price; set { _price = value; OnPropertyChanged(); } }

        private string _statusBook;
        public string statusBook { get => _statusBook; set { _statusBook = value; OnPropertyChanged(); } }

        //open Window
        public ICommand AddBookCommand { get; set; }
        public ICommand EditBookCommand { get; set; }
        public ICommand DeleteBookCommand { get; set; }


        //effect DB
        public ICommand AddBookToDBCommand { get; set; }
        public ICommand EditBookToDBCommand { get; set; }


        //Add Authors
        public ICommand AddAuthors { get; set; }

        //Delete Authors
        public ICommand UnSelectedAuthor { get; set; }

        //Search Book
        private string bookSearchKeyword;
        public string BookSearchKeyword
        {
            get => bookSearchKeyword;
            set
            {
                bookSearchKeyword = value;
                OnPropertyChanged();
                SearchBook();
            }
        }

        //Search Book function
        private void SearchBook()
        {
            if (BookSearchKeyword == null || BookSearchKeyword.Trim() == "")
            {
                ListBook = new ObservableCollection<Book>(DB.Books);
                return;
            }
            try
            {
                var result = DB.Books.Where(
                                    book => book.nameBook.ToLower().StartsWith(bookSearchKeyword.ToLower())
                                    );
                ListBook = new ObservableCollection<Book>(result);
            }
            catch (ArgumentNullException)
            {
                ListBook = new ObservableCollection<Book>(DB.Books);
                MessageBox.Show("Từ khóa tìm kiếm rỗng!");
            }
        }

        public BookViewModel()
        {
            AddBookCommand = new AppCommand<object>((p) => 
            { 
                return true; 
            }, (p) => 
            { 
                AddBookScreen wd = new AddBookScreen(); 
                nameBook = "";
                SelectedCategory = null;
                SelectedPublisher = null;
                SelectedAuthor = null;
                ListAuthors = new ObservableCollection<Author>();
                dateAddBook = DateTime.Now;
                dateManufacture = new DateTime(2000, 1, 1);
                price = 0;
                wd.ShowDialog();

            });
            EditBookCommand = new AppCommand<object>((p) => 
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) => 
            { 
                EditBookScreen wd = new EditBookScreen(); 
                wd.ShowDialog(); 
            });

            ListBook = new ObservableCollection<Book>(DB.Books);
            category = new ObservableCollection<Models.Category>(DB.Categories);
            publisher = new ObservableCollection<Models.Publisher>(DB.Publishers);
            author = new ObservableCollection<Models.Author>(DB.Authors);


            //Command Add Authors
            AddAuthors = new AppCommand<object>((p) =>
            {
                if (SelectedAuthor != null)
                    return true;
                return false;
            }, (p) =>
            {
                ListAuthors.Add(SelectedAuthor);
            });
            ListAuthors = new ObservableCollection<Author>();


            AddBookToDBCommand = new AppCommand<object>((p) =>
            {
                //Kiểm tra điều kiện
                if (string.IsNullOrEmpty(nameBook))
                    return false;
                if (price <= 0 || SelectedCategory == null ||SelectedPublisher == null || ListAuthors.Count == 0)
                    return false;

                var displayList = DB.Books.Where(x => x.nameBook == nameBook);
                if (displayList == null)
                    return false;

                return true;
            }, (p) =>
            { 
                var book = new Book()
                {
                    nameBook = nameBook,
                    dateManufacture = dateManufacture,
                    dateAddBook = dateAddBook,
                    price = price,
                    idCategory = SelectedCategory.idCategory,
                    idPublisher = SelectedPublisher.idPublisher,
                    statusBook = "có sẵn",
                };

                for (int i = 0; i < ListAuthors.Count; i++)
                {
                    book.Authors.Add(ListAuthors[i]);
                }

                DB.Books.Add(book);
                DB.SaveChanges();

                ListBook.Add(book);
                MessageBox.Show("Thêm sách mới thành công");
            });

            //Edit Book Information
            EditBookToDBCommand = new AppCommand<object>((p) =>
            {
                //Kiểm tra điều kiện
                if (string.IsNullOrEmpty(nameBook) || SelectedItem == null)
                    return false;
                if (price <= 0 || SelectedCategory == null || SelectedPublisher == null || ListAuthors.Count == 0)
                    return false;

                var displayList = DB.Books.Where(x => x.nameBook == nameBook);
                if (displayList == null)
                    return false;

                return true;
            }, (p) =>
            {
                var book = DB.Books.Where(x => x.idBook == SelectedItem.idBook).SingleOrDefault();
                book.nameBook = nameBook;
                book.dateManufacture = dateManufacture;
                book.dateAddBook = dateAddBook;
                book.price = price;
                book.idCategory = SelectedCategory.idCategory;
                book.idPublisher = SelectedPublisher.idPublisher;
                
                //Clear Authors in book
                book.Authors.Clear();
                for (int i = 0; i < ListAuthors.Count; i++)
                {
                    book.Authors.Add(ListAuthors[i]);
                }
                DB.SaveChanges();
                OnPropertyChanged();

                ListBook = new ObservableCollection<Book>(DB.Books);
                MessageBox.Show("Sửa sách thành công");

            });

            //Delete Book
            DeleteBookCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                var book = DB.Books.Where(x => x.idBook == SelectedItem.idBook).SingleOrDefault();
                DB.Books.Remove(book);
                DB.SaveChanges();
                ListBook.Remove(book);
                MessageBox.Show("Xóa sách thành công");
            });

            //Delete Author in List Author
            UnSelectedAuthor = new AppCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                Author removeAuthor = p as Author;
                ListAuthors.Remove(removeAuthor);
            });

        }
    }
}
