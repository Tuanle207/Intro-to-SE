using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LibraryManagement.ViewModels
{
    public class AuthorValidation : ValidationRule
    {
        public AuthorValidation()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (string.IsNullOrEmpty((string)value) == true)
                    return new ValidationResult(false, "Vui lòng nhập tên tác giả");
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Vui lòng nhập tên tác giả");
            }
            return ValidationResult.ValidResult;
        }
    }
}