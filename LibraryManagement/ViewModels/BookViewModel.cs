﻿using LibraryManagement.Models;
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
using System.Windows.Ink;
using System.Windows.Input;

namespace LibraryManagement.ViewModels
{
    class BookViewModel : BaseViewModel
    {
        private BookPaginatingCollection list;
        public BookPaginatingCollection List { get => list; set { list = value; OnPropertyChanged(); } }

        private BookPaginatingCollection _ListLatestBooks;
        public BookPaginatingCollection ListLatestBooks { get => _ListLatestBooks; set { _ListLatestBooks = value; OnPropertyChanged(); } }

        private ObservableCollection<Author> _ListAuthors;
        public ObservableCollection<Author> ListAuthors { get => _ListAuthors; set { _ListAuthors = value; OnPropertyChanged(); } }

        private ObservableCollection<Author> _ListAuthorSelected;
        public ObservableCollection<Author> ListAuthorSelected { get => _ListAuthorSelected; set { _ListAuthorSelected = value; OnPropertyChanged(); } }

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
                InitBooks(bookSearchKeyword);
            }
        }


        private string bookImageCover;
        public string BookImageCover { get => bookImageCover; set { bookImageCover = value; OnPropertyChanged(); } }

        //open Window
        public ICommand AddBookCommand { get; set; }
        public ICommand DeleteBookCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        //effect DB
        public ICommand AddBookToDBCommand { get; set; }
        public ICommand EditBookToDBCommand { get; set; }

        //Pagination
        public ICommand MoveToPreviousBooksPage { get; set; }
        public ICommand MoveToNextBooksPage { get; set; }
        public ICommand PrevBooks { get; set; }
        public ICommand NextBooks { get; set; }

        //Add Authors
        public ICommand AddAuthors { get; set; }

        //Delete Authors
        public ICommand UnSelectedAuthor { get; set; }
        
        //Add book image cover
        public ICommand AddImage { get; set; }
        public ICommand PrepareUpdateImage { get; set; }



        public BookViewModel()
        {
            //BookImageCover = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())), $"/BookImageCover/default-image.png");
            SourceImageFile = null;
            InitBooks();
            GetLastestBooks();

            CancelCommand = new AppCommand<object>((p) =>
            {
                return true;

            }, (p) =>
            {
                if (SelectedItem != null)
                {
                    SelectedItem.nameBook = nameBook;
                    SelectedItem.Category = SelectedCategory;
                    SelectedItem.Publisher = SelectedPublisher;
                    SelectedItem.dateManufacture = dateManufacture;
                    SelectedItem.dateAddBook = dateAddBook;
                    SelectedItem.price = price;
                    OnPropertyChanged("SelectedItem");
                }
                BookSearchKeyword = null;
            });

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
                if (price <= 0 || SelectedCategory == null || SelectedPublisher == null || ListAuthors.Count == 0)
                    return false;
                return true;
            }, (p) =>
            {
                string newFileName = GetImageName();
                /// Copy image to project path
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
                // save changes
                DataAdapter.Instance.DB.Books.Add(book);
                DataAdapter.Instance.DB.SaveChanges();

                SourceImageFile = null;
                List.MoveToLastPage();
                SetSelectedItemToFirstItemOfPage(false);
                GetLastestBooks();
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

                string newFileName = GetImageName();
                /// Copy image to project path
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

                book.image = SourceImageFile != null ? newFileName : "default-image.png";

                //Clear Authors in book
                book.Authors.Clear();
                for (int i = 0; i < ListAuthors.Count; i++)
                {
                    book.Authors.Add(ListAuthors[i]);
                }
                DataAdapter.Instance.DB.SaveChanges();
                SourceImageFile = null;
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
                List.Refresh();
                SetSelectedItemToFirstItemOfPage(true);
                GetLastestBooks();
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
                    return this.List.CurrentPage > 1;
                },
                p =>
                {
                    List.MoveToPreviousPage();
                    SetSelectedItemToFirstItemOfPage(true);
                });
            MoveToNextBooksPage = new AppCommand<object>(
                p =>
                {
                    return this.List.CurrentPage < this.List.PageCount;
                },
                p =>
                {
                    List.MoveToNextPage();
                    SetSelectedItemToFirstItemOfPage(true);
                });

            PrevBooks = new AppCommand<object>(
                p =>
                {
                    return ListLatestBooks.CurrentPage > 1;
                },
                p =>
                {
                    ListLatestBooks.MoveToPreviousPage();
                    OnPropertyChanged("ListLatestBooks");
                });
            NextBooks = new AppCommand<object>(
                p =>
                {
                    return ListLatestBooks.CurrentPage < List.PageCount;
                },
                p =>
                {
                    ListLatestBooks.MoveToNextPage();
                    OnPropertyChanged("ListLatestBooks");
                });
            PrepareUpdateImage = new AppCommand<object>(
                p => true,
                p =>
                {
                    BookImageCover = GetFullPath(SelectedItem.image);
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


        private void InitBooks(string keyword = null)
        {
            if (keyword != null)
            {
                List = new BookPaginatingCollection(20, keyword);
            }
            else
            {
                List = new BookPaginatingCollection(20);
            }
            SetSelectedItemToFirstItemOfPage(true);
        }
        private void SetSelectedItemToFirstItemOfPage(bool isFirstItem)
        {
            if (List.Books == null || List.Books.Count == 0)
            {
               return;
            }
            if (isFirstItem)
            {
                SelectedItem = List.Books.FirstOrDefault();
            }
            else
            {
                SelectedItem = List.Books.LastOrDefault();
            }
        }

        private void GetLastestBooks()
        {
            ListLatestBooks = new LatestBookPaginationCollection(9, 18);
        }
    }
}
