// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Globalization;

    internal sealed class FormatParts
    {
        public const char DefaultMainFormat = 'G';

        public FormatParts(int? decimalPlaces) : this(DefaultMainFormat, decimalPlaces) { }
        public FormatParts(char format, int? decimalPlaces)
        {
            // Uppercase it (ASCII only).
            MainFormat = (char)(format & 0xDF);
            DecimalPlaces = decimalPlaces;
        }

        public int? DecimalPlaces { get; }

        public char MainFormat { get; }

        public string DefaultAmountFormat { get; set; } = "N";

        public string AmountFormat
            => DefaultAmountFormat + DecimalPlaces?.ToString(CultureInfo.InvariantCulture);

        public static FormatParts Parse(string format, int? moneyPrecision)
        {
            if (format == null || format.Length == 0) { return new FormatParts(moneyPrecision); }
            if (format.Length == 1) { return new FormatParts(format[0], moneyPrecision); }

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

                return new FormatParts(format[0], decimalPlaces);
            }

            throw new FormatException("XXX");
        }
    }
}
