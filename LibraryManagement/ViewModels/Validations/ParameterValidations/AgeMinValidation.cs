using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LibraryManagement.ViewModels
{
    public class AgeMinValidation : ValidationRule
    {
        public AgeMinValidation()
        {

        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int ageMin = 0;

            try
            {
                if (((string)value).Length > 0)
                    ageMin = Int32.Parse((String)value);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Giá trị không phù hợp");
            }

            if (ageMin <= 0)
            {
                return new ValidationResult(false, "Giá trị phải lớn hơn 0");
            }
            return ValidationResult.ValidResult;
        }
    }
}
