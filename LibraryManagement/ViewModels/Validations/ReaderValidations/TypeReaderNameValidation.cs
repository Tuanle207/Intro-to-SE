using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LibraryManagement.ViewModels
{
    public class TypeReaderNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string name = (string)value;
            // Check empty string?
            if (value == null || name.Trim().Length == 0)
            {
                return new ValidationResult(false, "Vui lòng nhập tên loại độc giả");
            }
            return ValidationResult.ValidResult;
        }
    }
}
