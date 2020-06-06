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
                calcRemainingDebt();
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
            set { collectedAmount = value; calcRemainingDebt(); OnPropertyChanged(); }
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
                    if (ReaderSelected == null || collectedAmount <= 0 || (ReaderSelected != null && ReaderSelected.debt == 0))
                        return false;
                    return true;
                },
                p =>
                {

                    var reader = DataAdapter.Instance.DB.Readers.Find(ReaderSelected.idReader);
                    Payment.idReader = ReaderSelected.idReader;
                    Payment.paymentDate = DateTime.Now;
                    Payment.collectedAmount = CollectedAmount;
                    Payment.currentDebt = ReaderSelected.debt;
                    Payment.remainDebt = Payment.currentDebt - Payment.collectedAmount;
                    reader.debt = Payment.remainDebt;

                    DataAdapter.Instance.DB.Payments.Add(Payment);

                    try
                    {
                        // Save DB state
                        DataAdapter.Instance.DB.SaveChanges();
                        Init();
                        MessageBox.Show("Thu tiền phạt thành công!");
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


                }
            );
        }

        private void Init()
        {
            Payment = new Payment { paymentDate = DateTime.Now };
            ListReader = new ObservableCollection<Reader>(DataAdapter.Instance.DB.Readers);
            if (ListReader.Count != 0)
            {
                ReaderSelected = ListReader.FirstOrDefault();
            }
            else ReaderSelected = null;
            CollectedAmount = 0;
            
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
        private void calcRemainingDebt()
        {
            if (Payment != null && ReaderSelected != null)
            {
                Payment.remainDebt = ReaderSelected.debt - CollectedAmount >= 0 ? ReaderSelected.debt - CollectedAmount : 0;
                OnPropertyChanged("Payment");
            }
        }
    }
}
