using LibraryManagement.Models;
using LibraryManagement.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
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

        private PagingCollectionView<Book> _ListBook;
        public PagingCollectionView<Book> ListBook { get => _ListBook; set { _ListBook = value; OnPropertyChanged(); } }

        private ObservableCollection<Author> _ListAuthors;
        public ObservableCollection<Author> ListAuthors { get => _ListAuthors; set { _ListAuthors = value; OnPropertyChanged(); } }

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
                    BookImageCover = SelectedItem.image;
                    ListAuthors = new ObservableCollection<Author>(SelectedItem.Authors);
                }
            }
        }

        private Category _SelectedCategory;
        public Category SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                _SelectedCategory = value;
                OnPropertyChanged();
            }
        }

        private Publisher _SelectedPublisher;
        public Publisher SelectedPublisher
        {
            get => _SelectedPublisher;
            set
            {
                _SelectedPublisher = value;
                OnPropertyChanged();
            }
        }

        private Author _SelectedAuthor;
        public Author SelectedAuthor
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

        private ObservableCollection<Category> _category;
        public ObservableCollection<Category> category { get => _category; set { _category = value; OnPropertyChanged(); } }

        private ObservableCollection<Publisher> _publisher;
        public ObservableCollection<Publisher> publisher { get => _publisher; set { _publisher = value; OnPropertyChanged(); } }

        private ObservableCollection<Author> _author;
        public ObservableCollection<Author> author { get => _author; set { _author = value; OnPropertyChanged(); } }

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

        public string SourceImageFile { get; set; }

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


        private string bookImageCover;
        public string BookImageCover { get => bookImageCover; set { bookImageCover = value; OnPropertyChanged(); } }

        //open Window
        public ICommand AddBookCommand { get; set; }
        public ICommand DeleteBookCommand { get; set; }


        //effect DB
        public ICommand AddBookToDBCommand { get; set; }
        public ICommand EditBookToDBCommand { get; set; }

        //Pagination
        public ICommand MoveToPreviousBooksPage { get; set; }
        public ICommand MoveToNextBooksPage { get; set; }

        //Add Authors
        public ICommand AddAuthors { get; set; }

        //Delete Authors
        public ICommand UnSelectedAuthor { get; set; }
        
        //Add book image cover
        public ICommand AddImage { get; set; }

      

        public BookViewModel()
        {
            //BookImageCover = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())), $"/BookImageCover/default-image.png");
            SourceImageFile = null;
            InitProperties(-1);
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

            
            category = new ObservableCollection<Category>(DataAdapter.Instance.DB.Categories);
            publisher = new ObservableCollection<Publisher>(DataAdapter.Instance.DB.Publishers);
            author = new ObservableCollection<Author>(DataAdapter.Instance.DB.Authors);


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

                var displayList = DataAdapter.Instance.DB.Books.Where(x => x.nameBook == nameBook);
                if (displayList == null)
                    return false;

                return true;
            }, (p) =>
            {
                string newFileName = GetImageName();
                /// Copy image to DB
                if (SourceImageFile != null)
                {
                    string destinationFile = GetFullPath(newFileName);
                    try
                    {
                        System.IO.File.Copy(SourceImageFile, destinationFile, true);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Đã xảy ra lỗi khi lưu file!");
                    }
                }
               

                var book = new Book()
                {
                    nameBook = nameBook,
                    dateManufacture = dateManufacture,
                    dateAddBook = dateAddBook,
                    price = price,
                    idCategory = SelectedCategory.idCategory,
                    idPublisher = SelectedPublisher.idPublisher,
                    statusBook = "có sẵn",
                    image = SourceImageFile != null ? newFileName : "default-image.png"
                };

                for (int i = 0; i < ListAuthors.Count; i++)
                {
                    book.Authors.Add(ListAuthors[i]);
                }

                DataAdapter.Instance.DB.Books.Add(book);
                DataAdapter.Instance.DB.SaveChanges();
                SourceImageFile = null;
                InitProperties(book.idBook);
                MessageBox.Show("Thêm sách thành công!");
            });

            //Edit Book Information
            EditBookToDBCommand = new AppCommand<object>((p) =>
            {
                //Kiểm tra điều kiện
                if (string.IsNullOrEmpty(nameBook) || SelectedItem == null)
                    return false;
                if (price <= 0 || SelectedCategory == null || SelectedPublisher == null || ListAuthors.Count == 0)
                    return false;

                return true;
            }, (p) =>
            {
                var book = DataAdapter.Instance.DB.Books.Where(x => x.idBook == SelectedItem.idBook).SingleOrDefault();
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
                DataAdapter.Instance.DB.SaveChanges();
                InitProperties(book.idBook);
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
                var book = DataAdapter.Instance.DB.Books.Where(x => x.idBook == SelectedItem.idBook).SingleOrDefault();
                DataAdapter.Instance.DB.Books.Remove(book);
                DataAdapter.Instance.DB.SaveChanges();
                InitProperties(-1);
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

            AddImage = new AppCommand<object>(
                
                p => true,
                p =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                    if (fileDialog.ShowDialog() == true)
                    {
                        SourceImageFile = fileDialog.FileName;
                        BookImageCover = SourceImageFile;
                    }
                    else
                    {
                        SourceImageFile = null;
                    }
                });
            MoveToPreviousBooksPage = new AppCommand<object>(
                p =>
                {
                    return ListBook.CurrentPage > 1;
                },
                p =>
                {
                    ListBook.MoveToPreviousPage();
                });
            MoveToNextBooksPage = new AppCommand<object>(
                p =>
                {
                    return ListBook.CurrentPage < ListBook.PageCount;
                },
                p =>
                {
                    ListBook.MoveToNextPage();
                });

           
            
        }

        private string GetImageName()
        {
            List<String> images = DataAdapter.Instance.DB.Books.Select(b => b.image).ToList();
            int max = 0;
            foreach(var el in images)
            {
                string idPart = el.Split('-')[1].Split('.')[0]; // default-photo.png, photo-1.png
                int id;
                if (Int32.TryParse(idPart, out id))
                {
                    if (id > max) max = id;
                }
            }
            max += 1;
            return $"photo-{max}.png";
        }

        private string GetFullPath(string fileName)
        {
            string destPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string destinationFile = Path.Combine($"{destPath}\\BookImageCover", fileName);
            return destinationFile;
        }

        //Search Book function
        private void SearchBook()
        {
            if (BookSearchKeyword == null || BookSearchKeyword.Trim() == "")
            {
                ListBook = new PagingCollectionView<Book>(DataAdapter.Instance.DB.Books.ToList(), 15);
                return;
            }
            try
            {
                var result = DataAdapter.Instance.DB.Books.Where(
                                    book => book.nameBook.ToLower().StartsWith(bookSearchKeyword.ToLower())
                                    );
                ListBook = new PagingCollectionView<Book>(result.ToList(), 15);
            }
            catch (ArgumentNullException)
            {
                ListBook = new PagingCollectionView<Book>(DataAdapter.Instance.DB.Books.ToList(), 15);
                MessageBox.Show("Từ khóa tìm kiếm rỗng!");
            }
        }

        private void InitProperties(int id)
        {
            ListBook = new PagingCollectionView<Book>(DataAdapter.Instance.DB.Books.ToList(), 15);
            if (ListBook.Count > 0)
            {
                SelectedItem = id == -1 ? (Book)ListBook.GetItemAt(0) : (Book)ListBook.GetItemById("Book", id);
                if (id != -1)
                {
                    ListBook.MoveToSelectedItem("Book", id);
                }
            }
            
        }
    }
}
