// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Globalization;

    public class MoneyFormatSpecifier
    {
        public const char DefaultMainFormat = 'G';

        private MoneyFormatSpecifier(int? decimalPlaces) : this(DefaultMainFormat, decimalPlaces) { }

        private MoneyFormatSpecifier(char mainFormat, int? decimalPlaces)
        {
            // Uppercase it (ASCII only).
            MainFormat = (char)(mainFormat & 0xDF);
            DecimalPlaces = decimalPlaces;
        }

        public int? DecimalPlaces { get; }

        public char MainFormat { get; }

        public char NumericFormat { get; private set; } = 'N';

        public string AmountFormat
            => NumericFormat + DecimalPlaces?.ToString(CultureInfo.InvariantCulture);

        public static MoneyFormatSpecifier Parse(string format, int? moneyPrecision, char numericFormat)
        {
            var fmt = Parse(format, moneyPrecision);
            fmt.NumericFormat = numericFormat;
            return fmt;
        }

        public static MoneyFormatSpecifier Parse(string format, int? moneyPrecision)
        {
            if (format == null || format.Length == 0) { return new MoneyFormatSpecifier(moneyPrecision); }
            if (format.Length == 1) { return new MoneyFormatSpecifier(format[0], moneyPrecision); }

            // "X00"..."X09" are not valid formats.
            if (format.Length == 3 && format[1] == '0') { throw new FormatException("XXX"); }

            // The user wishes to use a custom number of decimal places.
            if (format.Length <= 3)
            {
                int decimalPlaces;
                bool succeed = Int32.TryParse(
                    format.Substring(1, format.Length - 1),
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out decimalPlaces);

                if (!succeed) { throw new FormatException("XXX"); }

                return new MoneyFormatSpecifier(format[0], decimalPlaces);
            }

            throw new FormatException("XXX");
        }
    }
}
