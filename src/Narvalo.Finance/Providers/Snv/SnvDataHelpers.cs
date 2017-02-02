// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Providers.Snv
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;

    using Narvalo.Finance.Properties;

    internal static class SnvDataHelpers
    {
        public static string CleanupCountryName(string name)
            => name.Replace('’', '\'').Replace("\n", String.Empty);

        public static string CleanupCurrencyName(string name)
            => name.Replace("\"", "\"\"");

        public static bool ParseIsFund(string value)
        {
            // NB: A blank value is interpreted to be the same as no attribute.
            // Only found in the legacy XML source.
            if (String.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            else
            {
                Debug.WriteLineIf(
                    value != "true",
                    "When present, the 'IsFund' attribute value is expected to be equal to 'true'.");

                return value == "true";
            }
        }

        public static short? ParseMinorUnits(string value)
        {
            const string NOT_APPLICABLE = "N.A.";

            if (value == NOT_APPLICABLE)
            {
                return null;
            }
            else
            {
                short minorUnits;
                if (!Int16.TryParse(value, NumberStyles.None, NumberFormatInfo.InvariantInfo, out minorUnits))
                {
                    throw new InvalidDataException(Format.Current(Strings.SnvDataHelpers_InvalidMinorUnits, value));
                }

                return minorUnits;
            }
        }

        public static short? TryParseNumericCode(string value)
        {
            short numeriCode;
            if (!Int16.TryParse(value, NumberStyles.None, NumberFormatInfo.InvariantInfo, out numeriCode))
            {
                return null;
            }

            if (numeriCode <= 0 || numeriCode >= 1000)
            {
                throw new InvalidDataException(Format.Current(Strings.SnvDataHelpers_InvalidRangeForNumericCode, value));
            }

            return numeriCode;
        }

        public static short ParseNumericCode(string value)
        {
            short numeriCode;
            if (!Int16.TryParse(value, NumberStyles.None, NumberFormatInfo.InvariantInfo, out numeriCode))
            {
                throw new InvalidDataException(Format.Current(Strings.SnvDataHelpers_InvalidNumericCode, value));
            }

            if (numeriCode <= 0 || numeriCode >= 1000)
            {
                throw new InvalidDataException(Format.Current(Strings.SnvDataHelpers_InvalidRangeForNumericCode, value));
            }

            return numeriCode;
        }

        public static DateTime ParsePubDate(string value)
        {
            DateTime pubDate;
            if (!DateTime.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out pubDate))
            {
                throw new InvalidDataException(Format.Current(Strings.SnvDataHelpers_InvalidPubDate, value));
            }

            return pubDate;
        }
    }
}
