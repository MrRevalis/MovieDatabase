﻿using MovieDatabase.Validation;
using System.Text.RegularExpressions;

namespace MovieDatabase.ValidationRules
{
    public class IsValidEmailRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            Regex emailPattern = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            if (emailPattern.IsMatch($"{value}"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
