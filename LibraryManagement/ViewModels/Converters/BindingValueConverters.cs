﻿using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LibraryManagement.ViewModels
{

    /// <summary>
    /// Convert the bill return to total amount of fine of the reader
    /// </summary>
    class BillReturnToReaderDebt: IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null || DataAdapter.Instance.DB.Readers.Find(((BillReturn)value).idReader) == null)
                return 0;
            return ((BillReturn)value).sumFine + DataAdapter.Instance.DB.Readers.Find(((BillReturn)value).idReader).debt;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }

    }

    /// <summary>
    /// Convert the date a book was borrowed to the amount of fine. 
    /// </summary>
    class DateBorrowedToFine : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null)
                return value;
            int fine = 0;
            int noDaysBorrowAllowed = 14; // Get this from DB
            int amountFinePerDayExcess = 1000; // Get this from DB
            int excessDays = DateTime.Now.Subtract((DateTime)value).Days - noDaysBorrowAllowed;
            if (excessDays > 0)
            {
                fine = amountFinePerDayExcess * excessDays;
            }
            return fine;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }

    }

    /// <summary>
    /// Convert the date a book was borrowed to the number of excess days. 
    /// </summary>
    class DateBorrowedToExcessDays : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null)
                return value;
            int noDaysBorrowAllowed = 14; // Get this from DB
            int excessDays = DateTime.Now.Subtract((DateTime)value).Days - noDaysBorrowAllowed;
            if (excessDays < 0)
            {
                excessDays = 0;
            }
            return excessDays;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }

    }

    /// <summary>
    /// Convert the date a book was borrowed to the number of days borrowed. 
    /// </summary>
    class DateBorrowedToDaysBorrowed : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null)
                return value;
            int noDaysBorrowes = DateTime.Now.Subtract((DateTime)value).Days;
            return noDaysBorrowes;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }

    }

    /// <summary>
    /// Convert the date a book was borrowed to the number of remaining days to be returned according to the rule. 
    /// </summary>
    class DateBorrowedToRemainingDay : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null)
                return value;
            int noDaysBorrowAllowed = 14; // Get this from DB
            int noRemainingDay = noDaysBorrowAllowed - DateTime.Now.Subtract((DateTime)value).Days;
            return noRemainingDay;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }

    }


    /// <summary>
    /// Concatenate authors collection into a string to display in a cell: 
    /// </summary>
    class AuthorsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null)
                return value;
            HashSet<Author> Authors = (HashSet<Author>)value;
            List<Author> list = Authors.ToList();
            string result = "";
            foreach (var item in list)
            {
                if (item != list.First())
                {
                    result += ", ";
                }
                result += item.nameAuthor;
            }
            return result;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }

    }

    /// <summary>
    /// Convert Boolean value: true/false to Visibility value: Visible/Hidden
    /// </summary>
    class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null || (bool)value == true)
                return "Visible";
            return "Hidden";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }
    }
}