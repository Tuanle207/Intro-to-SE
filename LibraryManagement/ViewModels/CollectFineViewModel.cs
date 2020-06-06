using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModels
{
    class CollectFineViewModel : BaseViewModel
    {
        /// <summary>
        /// Definition of fields
        /// </summary>
        private ObservableCollection<Reader> listReader;
        private Reader readerSelected;
        private Payment payment;
        private int collectedAmount;
        private string readerKeyword;
        /// <summary>
        /// Fefinition of properties
        /// </summary>
        public ObservableCollection<Reader> ListReader
        {
            get => listReader;
            set
            {
                listReader = value;
                OnPropertyChanged();
            }
        }
        public Reader ReaderSelected
        {
            get => readerSelected;
            set
            {
                readerSelected = value;
                OnPropertyChanged();
            }
        }
        public Payment Payment
        {
            get => payment;
            set
            {
                payment = value;
                OnPropertyChanged();
            }
        }
        public int CollectedAmount
        {
            get => collectedAmount;
            set { collectedAmount = value; OnPropertyChanged(); }
        }
        public string ReaderKeyword { get => readerKeyword; set { readerKeyword = value; OnPropertyChanged(); SearchReader(); } }
        /// <summary>
        /// Command for buttons
        /// </summary>
        public ICommand CollectFine { get; set; }
        


        /// <summary>
        /// Contructor
        /// </summary>
        public CollectFineViewModel()
        {
            // Init some data
            Init();
            // Define command
            CollectFine = new AppCommand<object>(
                p =>
                {
                    if (ReaderSelected == null || collectedAmount <= 0) return false;
                    return true;
                },
                p =>
                {
                    DataAdapter.Instance.DB.Payments.Add(Payment);
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
                        Init();
                    }
                }
            );
        }

        private void Init()
        {
            ReaderSelected = null;
            CollectedAmount = 0;
            Payment = new Payment { paymentDate = DateTime.Now };
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
    }
}
