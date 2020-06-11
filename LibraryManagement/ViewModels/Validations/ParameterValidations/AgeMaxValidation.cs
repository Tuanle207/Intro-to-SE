using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LibraryManagement.ViewModels
{
    public class AgeMaxValidation : ValidationRule
    {
        public AgeMaxValidation()
        {

        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int ageMax = 0;

            try
            {
                if (((string)value).Length > 0)
                    ageMax = Int32.Parse((String)value);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Giá trị không phù hợp");
            }

            if (ageMax <= 0)
            {
                return new ValidationResult(false, "Giá trị phải lớn hơn 0");
            }
            return ValidationResult.ValidResult;
        }
    }
}
