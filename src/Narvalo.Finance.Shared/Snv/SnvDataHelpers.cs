// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Snv
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;

    internal static class SnvDataHelpers
    {
        private const string MINOR_UNITS_NOT_AVAILABLE = "N.A.";

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
            if (value == MINOR_UNITS_NOT_AVAILABLE)
            {
                return null;
            }
            else
            {
                short minorUnits;
                var ok = Int16.TryParse(value, NumberStyles.None, NumberFormatInfo.InvariantInfo, out minorUnits);
                if (!ok)
                {
                    throw new InvalidDataException("Found an invalid value (" + value + ") for the minor units.");
                }

                return minorUnits;
            }
        }

        public static short? TryParseNumericCode(string value)
        {
            Demand.NotNull(value);

            short numeriCode;
            var ok = Int16.TryParse(value, NumberStyles.None, NumberFormatInfo.InvariantInfo, out numeriCode);
            if (!ok) { return null; }

            if (numeriCode <= 0 || numeriCode >= 1000)
            {
                throw new InvalidDataException(
                    "The numeric code MUST (" + value + ") be strictly greater than 0 and strictly less than 1000.");
            }

            return numeriCode;
        }

        public static short ParseNumericCode(string value)
        {
            Demand.NotNull(value);

            short numeriCode;
            var ok = Int16.TryParse(value, NumberStyles.None, NumberFormatInfo.InvariantInfo, out numeriCode);
            if (!ok)
            {
                throw new InvalidDataException("Found an invalid value (" + value + ") for the numeric code.");
            }

            if (numeriCode <= 0 || numeriCode >= 1000)
            {
                throw new InvalidDataException(
                    "The numeric code MUST (" + value + ") be strictly greater than 0 and strictly less than 1000.");
            }

            return numeriCode;
        }

        public static DateTime ParsePubDate(string value)
        {
            Demand.NotNull(value);

            DateTime pubDate;
            var ok = DateTime.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out pubDate);
            if (!ok)
            {
                throw new InvalidDataException("Found an invalid value (" + value + ") for the publication date.");
            }

            return pubDate;
        }
    }
}
