// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Text
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Properties;

    using static Narvalo.Finance.Text.AsciiHelpers;

    public partial struct IbanParts : IEquatable<IbanParts>
    {
        internal const int MinLength = 14;
        internal const int MaxLength = 34;

        internal const int CountryLength = 2;
        internal const int CheckDigitsLength = 2;

        internal const int BbanMinLength = MinLength - CountryLength - CheckDigitsLength;
        internal const int BbanMaxLength = MaxLength - CountryLength - CheckDigitsLength;

        private readonly string _bban;
        private readonly string _checkDigits;
        private readonly string _countryCode;
        private readonly string _value;

        private IbanParts(string countryCode, string checkDigits, string bban, string value)
        {
            Demand.NotNull(countryCode);
            Demand.NotNull(checkDigits);
            Demand.NotNull(bban);
            Demand.NotNull(value);

            _bban = bban;
            _checkDigits = checkDigits;
            _countryCode = countryCode;
            _value = value;
        }

        public string Bban
        {
            get { Warrant.NotNull<string>(); return _bban; }
        }

        public string CheckDigits
        {
            get { Warrant.NotNull<string>(); return _checkDigits; }
        }

        public string CountryCode
        {
            get { Warrant.NotNull<string>(); return _countryCode; }
        }

        internal string LiteralValue => _value;

        public static IbanParts Create(string countryCode, string checkDigits, string bban)
        {
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(checkDigits, nameof(checkDigits));
            Require.NotNull(bban, nameof(bban));

            if (CheckCountryCode(countryCode)
                && CheckCheckDigits(checkDigits)
                && CheckBban(bban))
            {
                var value = countryCode + checkDigits + bban;

                return new IbanParts(countryCode, checkDigits, bban, value);
            }

            throw new FormatException(Strings.Iban_InvalidFormat);
        }

        public static IbanParts Parse(string value)
        {
            Require.NotNull(value, nameof(value));

            if (!CheckValue(value))
            {
                throw new FormatException(Strings.Iban_InvalidFormat);
            }

            string countryCode = GetCountryCode(value);
            if (!CheckCountryCode(countryCode))
            {
                throw new FormatException(Strings.Iban_InvalidCountryCode);
            }

            string checkDigits = GetCheckDigits(value);
            if (!CheckCheckDigits(checkDigits))
            {
                throw new FormatException(Strings.Iban_InvalidCheckDigits);
            }

            string bban = GetBban(value);
            if (!CheckBban(bban))
            {
                throw new FormatException(Strings.Iban_InvalidBban);
            }

            return new IbanParts(countryCode, checkDigits, bban, value);
        }

        public static IbanParts? TryParse(string value)
        {
            if (!CheckValue(value)) { return null; }

            string countryCode = GetCountryCode(value);
            if (!CheckCountryCode(countryCode)) { return null; }

            string checkDigits = GetCheckDigits(value);
            if (!CheckCheckDigits(checkDigits)) { return null; }

            string bban = GetBban(value);
            if (!CheckBban(bban)) { return null; }

            return new IbanParts(countryCode, checkDigits, bban, value);
        }

        private static string GetBban(string value)
        {
            Expect.NotNull(value);
            return value.Substring(CountryLength + CheckDigitsLength);
        }

        private static string GetCheckDigits(string value)
        {
            Expect.NotNull(value);
            return value.Substring(CountryLength, CheckDigitsLength);
        }

        private static string GetCountryCode(string value)
        {
            Expect.NotNull(value);
            return value.Substring(0, CountryLength);
        }

        #region Validation helpers.

        [Pure]
        public static bool CheckBban(string value)
        {
            if (value == null) { return false; }
            return value.Length >= BbanMinLength
                && value.Length <= BbanMaxLength
                && IsDigitOrUpperLetter(value);
        }

        [Pure]
        public static bool CheckCheckDigits(string value)
        {
            if (value == null) { return false; }
            return value.Length == CheckDigitsLength && IsDigit(value);
        }

        [Pure]
        public static bool CheckCountryCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == CountryLength && IsUpperLetter(value);
        }

        [Pure]
        public static bool CheckValue(string value)
        {
            if (value == null) { return false; }
            return value.Length >= MinLength && value.Length <= MaxLength;
        }

        #endregion
    }

    // Implements the IEquatable<IbanParts> interface.
    public partial struct IbanParts
    {
        public static bool operator ==(IbanParts left, IbanParts right) => left.Equals(right);

        public static bool operator !=(IbanParts left, IbanParts right) => !left.Equals(right);

        public bool Equals(IbanParts other) => _value == other._value;

        public override bool Equals(object obj)
        {
            if (!(obj is IbanParts))
            {
                return false;
            }

            return Equals((IbanParts)obj);
        }

        public override int GetHashCode() => _value.GetHashCode();
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance.Text
{
    using System.Diagnostics.Contracts;

    public partial struct IbanParts
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_bban != null);
            Contract.Invariant(_checkDigits != null);
            Contract.Invariant(_countryCode != null);
            Contract.Invariant(_value != null);
        }
    }
}

#endif
