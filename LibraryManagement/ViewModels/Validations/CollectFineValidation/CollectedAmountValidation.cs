using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagement.ViewModels
{
    public class Wrapper : DependencyObject
    {
        public static readonly DependencyProperty maxAmount =
             DependencyProperty.Register("MaxAmount", typeof(int),
             typeof(Wrapper), new FrameworkPropertyMetadata(int.MaxValue));

        public int MaxAmount
        {
            get { return (int)GetValue(maxAmount); }
            set { SetValue(maxAmount, value); }
        }
    }

    public class CollectedAmountValidation : ValidationRule
    {
        public Wrapper Wrapper { get; set; }

        public CollectedAmountValidation()
        {
        }
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int collectedAmount = 0;

            if (((string)value).Contains('.') || ((string)value).Contains(','))
            {
                return new ValidationResult(false, "Số tiền thu phải là bội của 1000đ");
            }

            try
            {
                if (((string)value).Length > 0)
                    collectedAmount = Int32.Parse((String)value);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Giá trị nhập vào không hợp lệ");
            }

            if (collectedAmount <= 0)
            {
                return new ValidationResult(false, "Số tiền thu phải lớn hơn 0đ và nhỏ hơn tổng nợ");
            }
            return ValidationResult.ValidResult;
        }
    }


}
