using System.Globalization;

namespace System
{
    public static class DoubleExtensions
    {
        public static bool TryParseAmountApiFormat(this string value)
        {
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberDecimalSeparator = ".";
            format.NumberDecimalDigits = 2;


            double amount = 0;
            return double.TryParse(value, NumberStyles.Currency, format, out amount);
        }

        public static string ToAmountApiFormat(this double amount)
        {
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberDecimalSeparator = ".";
            format.NumberDecimalDigits = 2;

            return amount.ToString(format);
        }

        public static string ToAmountWebFormat(this double amount)
        {
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberDecimalSeparator = ".";
            format.NumberDecimalDigits = 2;

            return string.Format(format, "{0:N}", amount);
        }

        public static string ToPercentageApiFormat(this double percentage)
        {
            return percentage.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static double ToDouble(this string value)
        {
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberDecimalSeparator = ".";
            format.NumberDecimalDigits = 2;

            return double.Parse(value, format);
        }

        public static decimal ToDecimal(this string value)
        {
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberDecimalSeparator = ".";
            format.NumberDecimalDigits = 2;

            return Decimal.Parse(value, format);
        }
    }
}
