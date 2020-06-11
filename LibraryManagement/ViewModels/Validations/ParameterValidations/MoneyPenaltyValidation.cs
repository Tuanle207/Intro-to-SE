using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LibraryManagement.ViewModels
{
    public class MoneyPenaltyValidation : ValidationRule
    {
        public MoneyPenaltyValidation()
        {

        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int moneyPenalty = 0;

            try
            {
                if (((string)value).Length > 0)
                    moneyPenalty = Int32.Parse((String)value);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Giá trị không phù hợp");
            }

            if (moneyPenalty < 0)
            {
                return new ValidationResult(false, "Giá trị phải lớn hơn hoặc bằng 0");
            }
            return ValidationResult.ValidResult;
        }
    }
}
