// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;

    using Narvalo.Finance.Properties;

    public static partial class MoneypennyFormatter
    {
        private const string DEFAULT_FORMAT = "G";

        public static string Format(Moneypenny penny, string format, IFormatProvider provider)
        {
            Warrant.NotNull<string>();

            if (format == null || format.Length == 0)
            {
                format = DEFAULT_FORMAT;
            }

            switch (format)
            {
                case "N":
                case "n":
                    // Numeric. Does not include any information about the currency.
                    return penny.Amount.ToString(provider);
                case "L":
                case "l":
                    // Left (Currency code placed on the).
                    return penny.PennyOrCurrencyCode + "\u00A0" + penny.Amount.ToString(provider);
                case "G":
                case "g":
                case "R":
                case "r":
                    // General (default) or Right (Currency code placed on the).
                    return penny.Amount.ToString(provider) + "\u00A0" + penny.PennyOrCurrencyCode;
                default:
                    throw new FormatException(
                        Narvalo.Format.Current(Strings.Moneypenny_InvalidFormatSpecification));
            }
        }
    }
}
