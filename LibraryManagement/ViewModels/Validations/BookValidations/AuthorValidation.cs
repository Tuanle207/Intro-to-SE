﻿using System;
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
            if (value == null || ((string)value).Trim().Length == 0)
                return new ValidationResult(false, "Vui lòng nhập tác giả");
            return ValidationResult.ValidResult;
        }
    }
}
