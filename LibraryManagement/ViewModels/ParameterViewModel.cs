using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModels
{
    class ParameterViewModel : BaseViewModel
    {
        static LibraryManagementEntities DB = new LibraryManagementEntities();

        private int _idParameter;
        public int idParameter { get => _idParameter; set { _idParameter = value; OnPropertyChanged(); } }

        private string _nameParameter;
        public string nameParameter
        {
            get => _nameParameter; set { _nameParameter = value; OnPropertyChanged(); }
        }

        private int _ageMin = DB.Paramaters.Where(x => x.idParameter == 1).SingleOrDefault().valueParameter;
        public int ageMin
        {
            get => _ageMin; set { _ageMin = value; OnPropertyChanged(); }
        }

        private int _ageMax = DB.Paramaters.Where(x => x.idParameter == 2).SingleOrDefault().valueParameter;
        public int ageMax
        {
            get => _ageMax; set { _ageMax = value; OnPropertyChanged(); }
        }

        private int _expiryDate = DB.Paramaters.Where(x => x.idParameter == 3).SingleOrDefault().valueParameter;
        public int expiryDate
        {
            get => _expiryDate; set { _expiryDate = value; OnPropertyChanged(); }
        }

        private int _distancePublish = DB.Paramaters.Where(x => x.idParameter == 4).SingleOrDefault().valueParameter;
        public int distancePublish
        {
            get => _distancePublish; set { _distancePublish = value; OnPropertyChanged(); }
        }

        private int _bookBorrowMax = DB.Paramaters.Where(x => x.idParameter == 5).SingleOrDefault().valueParameter;
        public int bookBorrowMax
        {
            get => _bookBorrowMax; set { _bookBorrowMax = value; OnPropertyChanged(); }
        }

        private int _moneyPenalty = DB.Paramaters.Where(x => x.idParameter == 6).SingleOrDefault().valueParameter;
        public int moneyPenalty
        {
            get => _moneyPenalty; set { _moneyPenalty = value; OnPropertyChanged(); }
        }

        private int _dateBorrowMax = DB.Paramaters.Where(x => x.idParameter == 7).SingleOrDefault().valueParameter;
        public int dateBorrowMax
        {
            get => _dateBorrowMax; set { _dateBorrowMax = value; OnPropertyChanged(); }
        }

        //Hàm kiểm tra text có phải số ko
        private bool IsNumber(string str)
        {
            for (int i = 0; i < str.Length; i++)
                if (str[i] >= '0' && str[i] <= '9')
                    return true;
            return false;
        }

        public ICommand EditParametersCommand { get; set; }

        public ParameterViewModel()
        {

            EditParametersCommand = new AppCommand<object>((p) =>
            {
                if (IsNumber(ageMin.ToString()) == true && IsNumber(ageMax.ToString()) == true && IsNumber(expiryDate.ToString()) == true && IsNumber(distancePublish.ToString()) == true &&
                IsNumber(bookBorrowMax.ToString()) == true && IsNumber(moneyPenalty.ToString()) == true && IsNumber(dateBorrowMax.ToString()) == true)
                    return true;
                if (ageMin > 0 && ageMax > 0 && expiryDate > 0 && distancePublish > 0 && bookBorrowMax > 0 && moneyPenalty >= 0 && dateBorrowMax > 0)
                    return true;
                return false;
            }, (p) =>
            {
                // Sửa lại các giá trị 
                // Tuổi tối thiểu
                DB.Paramaters.Where(x => x.idParameter == 1).SingleOrDefault().valueParameter = ageMin;
                // Tuổi tối đa
                DB.Paramaters.Where(x => x.idParameter == 2).SingleOrDefault().valueParameter = ageMax;
                // Thời hạn thẻ
                DB.Paramaters.Where(x => x.idParameter == 3).SingleOrDefault().valueParameter = expiryDate;
                // Khoảng cách xuất bản
                DB.Paramaters.Where(x => x.idParameter == 4).SingleOrDefault().valueParameter = distancePublish;
                // Số sách mượn tối đa
                DB.Paramaters.Where(x => x.idParameter == 5).SingleOrDefault().valueParameter = bookBorrowMax;
                // Số tiền phạt trễ theo từng ngày
                DB.Paramaters.Where(x => x.idParameter == 6).SingleOrDefault().valueParameter = moneyPenalty;
                // Số ngày mượn tối đa
                DB.Paramaters.Where(x => x.idParameter == 7).SingleOrDefault().valueParameter = dateBorrowMax;

                DB.SaveChanges();
                OnPropertyChanged();
            });

        }
    }
}