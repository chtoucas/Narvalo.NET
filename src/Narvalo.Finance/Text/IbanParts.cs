// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Text
{
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Validation;

    using static Narvalo.Finance.Text.AsciiHelpers;

    // The validation helpers must remain public; they are an integral part of the Iban's contract.
    // These methods only enforce the length of each part, not their content.
    public partial struct IbanParts
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

        private IbanParts(string countryCode, string checkDigits, string bban)
        {
            Demand.True(CheckCountryCode(countryCode));
            Demand.True(CheckCheckDigits(checkDigits));
            Demand.True(CheckBban(bban));

            _bban = bban;
            _checkDigits = checkDigits;
            _countryCode = countryCode;
        }

        public string Bban
        {
            get { Sentinel.Warrant.LengthRange(BbanMinLength, BbanMaxLength); return _bban; }
        }

        public string CheckDigits
        {
            get { Sentinel.Warrant.Length(CheckDigitsLength); return _checkDigits; }
        }

        public string CountryCode
        {
            get { Sentinel.Warrant.Length(CountryLength); return _countryCode; }
        }

        public static IbanParts? Create(string countryCode, string checkDigits, string bban)
        {
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(checkDigits, nameof(checkDigits));
            Require.NotNull(bban, nameof(bban));

            if (CheckCountryCode(countryCode)
                && IsUpperLetter(countryCode)
                && CheckCheckDigits(checkDigits)
                && IsDigit(checkDigits)
                && CheckBban(bban)
                && IsDigitOrUpperLetter(bban))
            {
                return new IbanParts(countryCode, checkDigits, bban);
            }

            return null;
        }

        public static IbanParts? Parse(string value) => ParseIntern(value, true);

        internal static IbanParts? ParseIntern(string value, bool check)
        {
            if (!CheckValue(value)) { return null; }

            // The first two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(0, CountryLength);
            Check.True(CheckCountryCode(countryCode));
            if (check && !IsUpperLetter(countryCode)) { return null; }

            string checkDigits = value.Substring(CountryLength, CheckDigitsLength);
            Check.True(CheckCheckDigits(checkDigits));
            if (check && !IsDigit(checkDigits)) { return null; }

            string bban = value.Substring(CountryLength + CheckDigitsLength);
            Contract.Assume(CheckBban(bban));
            if (check && !IsDigitOrUpperLetter(bban)) { return null; }

            return new IbanParts(countryCode, checkDigits, bban);
        }

        #region Validation helpers.

        [Pure]
        public static bool CheckBban(string value)
        {
            if (value == null) { return false; }
            return value.Length >= BbanMinLength && value.Length <= BbanMaxLength;
        }

        [Pure]
        public static bool CheckCheckDigits(string value)
        {
            if (value == null) { return false; }
            return value.Length == CheckDigitsLength;
        }

        [Pure]
        public static bool CheckCountryCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == CountryLength;
        }

        [Pure]
        public static bool CheckValue(string value)
        {
            if (value == null) { return false; }
            return value.Length >= MinLength && value.Length <= MaxLength;
        }

        #endregion
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance.Validation
{
    using System.Diagnostics.Contracts;

    public partial struct IbanParts
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(CheckBban(_bban));
            Contract.Invariant(CheckCheckDigits(_checkDigits));
            Contract.Invariant(CheckCountryCode(_countryCode));
        }
    }
}

#endif
