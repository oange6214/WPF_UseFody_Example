using System.Globalization;
using System.Windows.Controls;

namespace WPF_Fody.ValidationRules
{
    public class RangeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (double.TryParse(value.ToString(), out double myValue))
            {
                if (myValue >= 0 && myValue <= 100)
                {
                    return new ValidationResult(true, null);
                }
            }
            return new ValidationResult(false, "Input should between 0 and 100");
        }
    }
}
