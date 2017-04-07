// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Globalization;

    using Narvalo.Properties;

    public sealed class MoneyFormat
    {
        public const char DefaultMainFormat = 'G';

        public MoneyFormat(char mainFormat)
        {
            MainFormat = mainFormat;
        }

        public MoneyFormat(char mainFormat, int? decimalPlaces)
        {
            MainFormat = mainFormat;
            DecimalPlaces = decimalPlaces;
        }

        public int? DecimalPlaces { get; set; }

        public char MainFormat { get; }

        public static MoneyFormat Parse(string format, int? currencyDecimalPlaces)
        {
            if (format == null || format.Length == 0)
            {
                // format = "G"
                return new MoneyFormat(DefaultMainFormat);
            }
            if (format.Length == 1)
            {
                // format = A
                return new MoneyFormat(format[0]);
            }
            if (format.Length == 2 && format[1] == 'Z')
            {
                // format = AZ
                return new MoneyFormat(format[0], currencyDecimalPlaces);
            }
            // Fail fast for "X00"..."X09" which are not valid formats.
            if (format.Length == 3 && format[1] == '0')
            {
                throw new FormatException(
                    Format.Current(Strings_Money.Money_BadPrecisionSpecifier, format));
            }

            if (format.Length <= 3)
            {
                // The user wishes to use a custom number of decimal places.
                // format = Ann
                bool succeed = Int32.TryParse(
                    format.Substring(1),
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out int decimalPlaces);

                if (!succeed)
                {
                    throw new FormatException(
                    Format.Current(Strings_Money.Money_BadPrecisionSpecifier, format));
                }

                return new MoneyFormat(format[0], decimalPlaces);
            }

            throw new FormatException(
                    Format.Current(Strings_Money.Money_BadFormatSpecifier, format));
        }
    }
}
