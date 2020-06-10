using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LibraryManagement.ViewModels
{
    class ReportViewModel : BaseViewModel
    {

        private int _month;
        public int Month { get => _month; set { _month = value; OnPropertyChanged(); } }
        private int _year;
        public int Year { get => _year; set { _year = value; OnPropertyChanged(); } }
        public int _sumBorrow;
        public int SumBorrow { get => _sumBorrow; set { _sumBorrow = value; OnPropertyChanged(); } }

        public DateTime _dateExpired;
        public DateTime DateExpired { get => _dateExpired; set { _dateExpired = value; OnPropertyChanged(); } }

        private ObservableCollection<ReportCategory> _ListCategory;
        public ObservableCollection<ReportCategory> Category_List { get => _ListCategory; set { _ListCategory = value; OnPropertyChanged(); } }
        public AppCommand<object> LoadReportCategory { get; }
        //
        private ObservableCollection<ReportReturnLate> _ListLate;
        public ObservableCollection<ReportReturnLate> Late_List { get => _ListLate; set { _ListLate = value; OnPropertyChanged(); } }
        public AppCommand<object> LoadReportLate { get; }

        public ReportViewModel()
        {

            //Report Book borow with category
            Category_List = new ObservableCollection<ReportCategory>();
            var categoryList = DataAdapter.Instance.DB.Categories;
            //sumTurn là số lượt mượn ứng với từng thể loại
            int sumTurn = 0;
            int i = 1;
            // Load all Turn Borrow in DB
            foreach (var item in categoryList)
            {
                var Cate1 = from b in DataAdapter.Instance.DB.Books
                            join d in DataAdapter.Instance.DB.DetailBillBorrows on b.idBook equals d.idBook
                            join br in DataAdapter.Instance.DB.BillBorrows on d.idBillBorrow equals br.idBillBorrow
                            where (b.idCategory == item.idCategory)
                            select b;
                try
                {
                    if (Cate1 != null)
                    {
                        int sumBook = 0;

                        if (Cate1 != null)
                        {
                            sumBook = Cate1.Sum(b => 1);
                        }
                        sumTurn += sumBook;
                        i++;

                    }
                }
                catch (Exception)
                {

                }
            }

            //Sum Borrow
            SumBorrow = sumTurn;

            //Add report Category into Category_List
            i = 1;
            foreach (var item1 in categoryList)
            {
                var Cate2 = from b in DataAdapter.Instance.DB.Books
                            join d in DataAdapter.Instance.DB.DetailBillBorrows on b.idBook equals d.idBook
                            join br in DataAdapter.Instance.DB.BillBorrows on d.idBillBorrow equals br.idBillBorrow
                            where (b.idCategory == item1.idCategory)
                            select b;
                try
                {
                    if (Cate2 != null)
                    {
                        int sumBook = 0;

                        if (Cate2 != null)
                        {
                            sumBook = Cate2.Sum(b => 1);
                        }
                        ReportCategory report = new ReportCategory();
                        report.Name = item1.nameCategory;
                        report.No = i;
                        report.TurnBorrow = sumBook;
                        report.Ratio = (sumBook * 100) / sumTurn;
                        Category_List.Add(report);
                        i++;

                    }
                }
                catch (Exception)
                {

                }
            }
            // --End Load all Turn Borrow in DB

            //Load Turn Borrow with Month and Year User select.
            LoadReportCategory = new AppCommand<object>((p) =>
            {
                if (Month.ToString() != null && Year.ToString() != null)
                    return true;
                else return false;

            }, (p) =>
            {
                Category_List.Clear();
                sumTurn = 0;
                i = 1;
                //Calculator Sum turn borrow
                foreach (var item2 in categoryList)
                {
                    var Cate3 = from b in DataAdapter.Instance.DB.Books
                                join d in DataAdapter.Instance.DB.DetailBillBorrows on b.idBook equals d.idBook
                                join br in DataAdapter.Instance.DB.BillBorrows on d.idBillBorrow equals br.idBillBorrow
                                where (b.idCategory == item2.idCategory && br.borrowDate.Month == Month && br.borrowDate.Year == Year)
                                select b;
                    try
                    {
                        if (Cate3 != null)
                        {
                            int sumBook = 0;

                            if (Cate3 != null)
                            {
                                sumBook = Cate3.Sum(b => 1);
                            }
                            sumTurn += sumBook;
                            i++;

                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                //Sum Borrow
                SumBorrow = sumTurn;
                //Add report Category into Category_List
                i = 1;
                foreach (var item3 in categoryList)
                {
                    var Cate4 = from b in DataAdapter.Instance.DB.Books
                                join d in DataAdapter.Instance.DB.DetailBillBorrows on b.idBook equals d.idBook
                                join br in DataAdapter.Instance.DB.BillBorrows on d.idBillBorrow equals br.idBillBorrow
                                where (b.idCategory == item3.idCategory && br.borrowDate.Month == Month && br.borrowDate.Year == Year)
                                select b;
                    try
                    {
                        if (Cate4 != null)
                        {
                            int sumBook = 0;

                            if (Cate4 != null)
                            {
                                sumBook = Cate4.Sum(b => 1);
                            }
                            ReportCategory report = new ReportCategory();
                            report.Name = item3.nameCategory;
                            report.No = i;
                            report.TurnBorrow = sumBook;
                            report.Ratio = (sumBook * 100) / sumTurn;
                            Category_List.Add(report);
                            i++;

                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            });
            // --End Load Turn Borrow with Month and Year User select.

            //End Report Book borow with category


            // Report Book return late    
            Late_List = new ObservableCollection<ReportReturnLate>();
            var billBBorrow = DataAdapter.Instance.DB.BillBorrows;
            DateTime dateCal;
            int j, temp1;
            DateExpired = DateTime.Today;

            //Load book borrowing return late from today
            dateCal = DateExpired.AddDays(-4);
            j = 1;
            foreach (var item4 in billBBorrow)
            {
                var Late1 = from b in DataAdapter.Instance.DB.Books
                            join d in DataAdapter.Instance.DB.DetailBillBorrows on b.idBook equals d.idBook
                            join br in DataAdapter.Instance.DB.BillBorrows on d.idBillBorrow equals br.idBillBorrow
                            where br.idBillBorrow == item4.idBillBorrow && br.borrowDate < dateCal && d.returned == 0
                            select b;
                try
                {
                    temp1 = j;
                    if (Late1 != null)
                    {
                        foreach (var item5 in Late1)
                        {
                            var Late2 = from b1 in DataAdapter.Instance.DB.Books
                                        join d1 in DataAdapter.Instance.DB.DetailBillBorrows on b1.idBook equals d1.idBook
                                        join br1 in DataAdapter.Instance.DB.BillBorrows on d1.idBillBorrow equals br1.idBillBorrow
                                        where b1.idBook == item5.idBook && br1.borrowDate == item4.borrowDate && d1.returned == 0
                                        select br1;
                            try
                            {
                                //temp2 = temp1;
                                if (Late2 != null)
                                {

                                    foreach (var item0 in Late2)
                                    {
                                        ReportReturnLate reportlate = new ReportReturnLate();
                                        reportlate.Name = item5.nameBook;
                                        reportlate.No = temp1;
                                        reportlate.DateBorrow = item0.borrowDate;
                                        reportlate.DaysReturnLate = (int)((dateCal - item0.borrowDate).TotalDays);
                                        if ((dateCal - item0.borrowDate).TotalDays - (int)((dateCal - item0.borrowDate).TotalDays) > 0) reportlate.DaysReturnLate++;
                                        Late_List.Add(reportlate);
                                        temp1++;
                                    }

                                }
                                j = temp1;

                            }
                            catch (Exception)
                            {

                            }

                        }

                    }

                }
                catch (Exception)
                {

                }

            }
            //End load book borrowing return late from today

            //Command LoadReportLate
            LoadReportLate = new AppCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                //Load Book return late with day you select
                Late_List.Clear();
                dateCal = DateExpired.AddDays(-4);
                j = 1;
                foreach (var item5 in billBBorrow)
                {
                    var Late3 = from b in DataAdapter.Instance.DB.Books
                                join d in DataAdapter.Instance.DB.DetailBillBorrows on b.idBook equals d.idBook
                                join br in DataAdapter.Instance.DB.BillBorrows on d.idBillBorrow equals br.idBillBorrow
                                where br.idBillBorrow == item5.idBillBorrow  && d.returned == 0
                                select b;
                    try
                    {
                        temp1 = j;
                        if (Late3 != null)
                        {
                            foreach (var item6 in Late3)
                            {
                                var Late4 = from b1 in DataAdapter.Instance.DB.Books
                                            join d1 in DataAdapter.Instance.DB.DetailBillBorrows on b1.idBook equals d1.idBook
                                            join br1 in DataAdapter.Instance.DB.BillBorrows on d1.idBillBorrow equals br1.idBillBorrow
                                            where b1.idBook == item6.idBook && br1.borrowDate == item5.borrowDate && d1.returned == 0
                                            select br1;
                                try
                                {
                                    if (Late4 != null)
                                    {

                                        foreach (var item7 in Late4)
                                        {
                                            ReportReturnLate reportlate = new ReportReturnLate();
                                            reportlate.Name = item6.nameBook;
                                            reportlate.No = temp1;
                                            reportlate.DateBorrow = item7.borrowDate;
                                            reportlate.DaysReturnLate = (int)((dateCal - item7.borrowDate).TotalDays);
                                            if ((dateCal - item7.borrowDate).TotalDays - (int)((dateCal - item7.borrowDate).TotalDays) > 0) reportlate.DaysReturnLate++;
                                            if(reportlate.DaysReturnLate > 0)
                                            { 
                                                Late_List.Add(reportlate);
                                                temp1++;
                                            }
                                        }

                                    }
                                    j = temp1;

                                }
                                catch (Exception)
                                {

                                }

                            }

                        }

                    }
                    catch (Exception)
                    {

                    }

                }

                // --End Load Book return late with day you select
            });
        }
        // End Report Book return late 

    }


}
