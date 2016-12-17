﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

    using static Narvalo.Finance.IbanFormat;

    /// <summary>
    /// Represents an International Bank Account Number (IBAN).
    /// </summary>
    /// <remarks>
    /// The standard format for an IBAN is defined in ISO 13616.
    /// </remarks>
    public partial struct Iban : IEquatable<Iban>, IFormattable
    {
        private readonly string _bban;
        private readonly string _checkDigits;
        private readonly string _countryCode;
        private readonly string _value;

        private Iban(string countryCode, string checkDigits, string bban, string value, bool validated)
        {
            Demand.True(CheckCountryCode(countryCode));
            Demand.True(CheckCheckDigits(checkDigits));
            Demand.True(CheckBban(bban));
            Demand.True(CheckValue(value));

            _countryCode = countryCode;
            _checkDigits = checkDigits;
            _bban = bban;
            _value = value;
            Validated = validated;
        }

        /// <summary>
        /// Gets the Basic Bank Account Number (BBAN).
        /// </summary>
        public string Bban
        {
            get { Sentinel.Warrant.LengthRange(BbanMinLength, BbanMaxLength); return _bban; }
        }

        /// <summary>
        /// Gets the check digits.
        /// </summary>
        public string CheckDigits
        {
            get { Sentinel.Warrant.Length(CheckDigitsLength); return _checkDigits; }
        }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode
        {
            get { Sentinel.Warrant.Length(CountryLength); return _countryCode; }
        }

        public bool Validated { get; }

        internal string Value { get { Sentinel.Warrant.LengthRange(MinLength, MaxLength); return _value; } }

        public static Iban Create(string countryCode, string checkDigits, string bban)
        {
            // REVIEW: We check for non-null twice...
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(checkDigits, nameof(checkDigits));
            Require.NotNull(bban, nameof(bban));
            Require.True(CheckCountryCode(countryCode), nameof(countryCode));
            Require.True(CheckCheckDigits(checkDigits), nameof(checkDigits));
            Require.True(CheckBban(bban), nameof(bban));

            var value = countryCode + checkDigits + bban;
            Contract.Assume(CheckValue(value));

            return new Iban(countryCode, checkDigits, bban, value, false);
        }

        public static Iban Parse(string value)
        {
            Demand.NotNull(value);

            return Parse(value, IbanStyles.Any);
        }

        public static Iban Parse(string value, IbanStyles styles)
        {
            Require.NotNull(value, nameof(value));

            var val = RemoveUnwantedCharacters(value, styles);
            if (!CheckValue(val))
            {
                throw new FormatException(Strings.Iban_InvalidFormat);
            }
            Check.True(CheckValue(val));

            return ParseCore(val, false);
        }

        public static Iban ParseExact(string value)
        {
            Demand.NotNull(value);

            return ParseExact(value, IbanStyles.None);
        }

        public static Iban ParseExact(string value, IbanStyles styles)
        {
            Require.NotNull(value, nameof(value));

            var val = RemoveUnwantedCharacters(value, styles);
            if (!CheckValue(val))
            {
                throw new FormatException(Strings.Iban_InvalidFormat);
            }
            Check.True(CheckValue(val));

            var iban = ParseCore(val, true);
            if (!iban.Validate())
            {
                throw new FormatException(Strings.Iban_InvalidFormat);
            }

            return iban;
        }

        public static Iban? TryParse(string value) => TryParse(value, IbanStyles.Any);

        public static Iban? TryParse(string value, IbanStyles styles)
        {
            if (value == null) { return null; }

            var val = RemoveUnwantedCharacters(value, styles);
            if (!CheckValue(val)) { return null; }
            Check.True(CheckValue(val));

            return ParseCore(val, false); ;
        }

        public static Iban? TryParseExact(string value) => TryParseExact(value, IbanStyles.Any);

        public static Iban? TryParseExact(string value, IbanStyles styles)
        {
            if (value == null) { return null; }

            var val = RemoveUnwantedCharacters(value, styles);
            if (!CheckValue(val)) { return null; }
            Check.True(CheckValue(val));

            var iban = ParseCore(val, true);
            if (!iban.Validate()) { return null; }

            return iban;
        }

        public static Iban? Validate(Iban iban)
        {
            if (iban.Validated) { return iban; }
            if (!iban.Validate()) { return null; }

            return new Iban(iban.CountryCode, iban.CheckDigits, iban.Bban, iban._value, true);
        }

        public bool Validate()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return _value;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            // If none given, use the default .NET format.
            if (format == null) { format = "G"; }

            if (formatProvider != null)
            {
                var fmt = formatProvider.GetFormat(GetType()) as ICustomFormatter;
                if (fmt != null)
                {
                    return fmt.Format(format, this, formatProvider);
                }
            }

            // REVIEW: Add lowercase alts.
            switch (format)
            {
                case "IBAN":
                    return Format(this, IbanFormattingStyle.WhiteSpaces);
                case "IBAN-":
                    return Format(this, IbanFormattingStyle.Dashes);
                case "G":
                default:
                    // Same as return ToString();
                    return _value;
            }
        }

        private enum IbanFormattingStyle
        {
            WhiteSpaces,
            Dashes,
            Default = WhiteSpaces,
        }

        // FIXME: Quick and dirty, it is wrong.
        private static string Format(Iban iban, IbanFormattingStyle style)
        {
            Warrant.NotNull<string>();

            var arr = iban.Value.ToCharArray();
            var len = arr.Length;
            var r = len % 4;
            var q = (len - r) / 4;
            var outarr = new char[5 * q + r];

            var k = 1;
            var interch = GetInterCharacter(style);

            for (var i = 0; i < len; i++, k++)
            {
                if (k % 5 == 0)
                {
                    outarr[k - 1] = interch;
                    outarr[k] = arr[i];
                    k++;
                }
                else
                {
                    outarr[k] = arr[i];
                }
            }

            return new String(outarr);
        }

        private static char GetInterCharacter(IbanFormattingStyle style)
        {
            switch (style)
            {
                case IbanFormattingStyle.Dashes:
                    return '-';
                case IbanFormattingStyle.WhiteSpaces:
                    return ' ';
                default:
                    throw Check.Unreachable();
            }
        }

        // NB: We only perform basic validation on the input string.
        // NB: We mark the result as validated, even if we have not yet perform any validation
        //     work. The caller is in charge to do the right thing.
        private static Iban ParseCore(string value, bool validated)
        {
            Demand.True(CheckValue(value));

            // The first two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(0, CountryLength);
            Check.True(CheckCountryCode(countryCode));

            string checkDigits = value.Substring(CountryLength, CheckDigitsLength);
            Check.True(CheckCheckDigits(checkDigits));

            string bban = value.Substring(CountryLength + CheckDigitsLength);
            Contract.Assume(CheckBban(bban));

            return new Iban(countryCode, checkDigits, bban, value, validated);
        }
    }

    // Implements the IEquatable<Iban> interface.
    public partial struct Iban
    {
        public static bool operator ==(Iban left, Iban right) => left.Equals(right);

        public static bool operator !=(Iban left, Iban right) => !left.Equals(right);

        public bool Equals(Iban other) => _value == other._value;

        public override bool Equals(object obj)
        {
            if (!(obj is Iban))
            {
                return false;
            }

            return Equals((Iban)obj);
        }

        public override int GetHashCode() => _value.GetHashCode();
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.Utilities.IbanFormat;

    public partial struct Iban
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(CheckBban(_bban));
            Contract.Invariant(CheckCheckDigits(_checkDigits));
            Contract.Invariant(CheckCountryCode(_countryCode));
            Contract.Invariant(CheckValue(_value));
        }
    }
}

#endif
