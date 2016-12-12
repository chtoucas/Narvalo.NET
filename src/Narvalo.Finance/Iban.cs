// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Internal;

    using static System.Diagnostics.Contracts.Contract;

    /// <summary>
    /// Represents an International Bank Account Number (IBAN).
    /// </summary>
    /// <remarks>
    /// The standard format for an IBAN is defined in ISO 13616.
    /// </remarks>
    public partial struct Iban : IEquatable<Iban>
    {
        private const int MIN_LENGTH = 14;
        private const int MAX_LENGTH = 34;
        private const int COUNTRY_LENGTH = 2;
        private const int CHECKDIGIT_LENGTH = 2;
        private const int BBAN_MIN_LENGTH = MIN_LENGTH - COUNTRY_LENGTH - CHECKDIGIT_LENGTH;
        private const int BBAN_MAX_LENGTH = MAX_LENGTH - COUNTRY_LENGTH - CHECKDIGIT_LENGTH;

        private readonly string _bban;
        private readonly string _checkDigit;
        private readonly string _countryCode;
        private readonly string _innerValue;

        private Iban(string countryCode, string checkDigit, string bban, string innerValue)
        {
            Demand.NotNull(countryCode);
            Demand.NotNull(checkDigit);
            Demand.NotNull(bban);
            Demand.NotNull(innerValue);
            Demand.True(ValidateCountryCode(countryCode));
            Demand.True(ValidateCheckDigit(checkDigit));
            Demand.True(ValidateBban(bban));
            Demand.True(ValidateInnerValue(innerValue));

            _countryCode = countryCode;
            _checkDigit = checkDigit;
            _bban = bban;
            _innerValue = innerValue;
        }

        /// <summary>
        /// Gets the Basic Bank Account Number (BBAN).
        /// </summary>
        public string Bban
        {
            get
            {
                Warrant.NotNull<string>();
                Ensures(Result<string>().Length >= BBAN_MIN_LENGTH || Result<string>().Length <= BBAN_MAX_LENGTH);

                return _bban;
            }
        }

        /// <summary>
        /// Gets the check digit.
        /// </summary>
        public string CheckDigit
        {
            get
            {
                Guards.Warrant.Length(CHECKDIGIT_LENGTH);

                return _checkDigit;
            }
        }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode
        {
            get
            {
                Guards.Warrant.Length(COUNTRY_LENGTH);

                return _countryCode;
            }
        }

        public static Iban Create(string countryCode, string checkDigit, string bban)
        {
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(checkDigit, nameof(checkDigit));
            Require.NotNull(bban, nameof(bban));
            Require.True(ValidateCountryCode(countryCode), nameof(countryCode));
            Require.True(ValidateCheckDigit(checkDigit), nameof(checkDigit));
            Require.True(ValidateBban(bban), nameof(bban));

            var innerValue = countryCode + checkDigit + bban;
            Assume(ValidateInnerValue(innerValue));
            Check.True(ValidateInnerValue(innerValue));

            return new Iban(countryCode, checkDigit, bban, innerValue);
        }

        public static Iban Parse(string value)
        {
            Require.NotNull(value, nameof(value));

            Iban? iban = ParseCore(value, true /* throwOnError */);
            Assume(iban.HasValue);
            Check.True(iban.HasValue);

            return iban.Value;
        }

        public static Iban? TryParse(string value)
        {
            if (value == null)
            {
                return null;
            }

            return ParseCore(value, false /* throwOnError */);
        }

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return _innerValue;
        }

        private static Iban? ParseCore(string value, bool throwOnError)
        {
            Demand.NotNull(value);

            if (value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
            {
                if (throwOnError)
                {
                    throw new FormatException(
                        "The IBAN string MUST be at most 34 characters long and at least 14 characters long.");
                }

                return null;
            }
            Assume(ValidateInnerValue(value));
            Check.True(ValidateInnerValue(value));

            //if (!IsDigitOrUpperLetter(value))
            //{
            //    if (throwOnError)
            //    {
            //        throw new FormatException(
            //            "The IBAN string MUST only contains digits and ASCII uppercase letters.");
            //    }

            //    return null;
            //}

            // The first two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(0, COUNTRY_LENGTH);
            Check.True(ValidateCountryCode(countryCode));

            string checkDigit = value.Substring(COUNTRY_LENGTH, CHECKDIGIT_LENGTH);
            Check.True(ValidateCheckDigit(checkDigit));

            string bban = value.Substring(COUNTRY_LENGTH + CHECKDIGIT_LENGTH);
            Assume(ValidateBban(bban));
            Check.True(ValidateBban(bban));

            return new Iban(countryCode, checkDigit, bban, value);
        }
    }

    // Validation helpers.
    public partial struct Iban
    {
        [Pure]
        public static bool ValidateBban(string value)
        {
            if (value == null) { return false; }

            return value.Length >= BBAN_MIN_LENGTH && value.Length <= BBAN_MAX_LENGTH;
        }

        [Pure]
        public static bool ValidateCheckDigit(string value)
        {
            if (value == null) { return false; }

            return value.Length == CHECKDIGIT_LENGTH;
        }

        [Pure]
        public static bool ValidateCountryCode(string value)
        {
            if (value == null) { return false; }

            return value.Length == COUNTRY_LENGTH;
        }

        [Pure]
        public static bool ValidateInnerValue(string value)
        {
            if (value == null) { return false; }

            return value.Length >= MIN_LENGTH && value.Length <= MAX_LENGTH;
        }
    }

    // Implements the IEquatable<Iban> interface.
    public partial struct Iban
    {
        public static bool operator ==(Iban left, Iban right) => left.Equals(right);

        public static bool operator !=(Iban left, Iban right) => !left.Equals(right);

        public bool Equals(Iban other) => _innerValue == other._innerValue;

        public override bool Equals(object obj)
        {
            if (!(obj is Iban))
            {
                return false;
            }

            return Equals((Iban)obj);
        }

        public override int GetHashCode() => _innerValue.GetHashCode();
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    public partial struct Iban
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_bban != null);
            Contract.Invariant(_checkDigit != null);
            Contract.Invariant(_countryCode != null);
            Contract.Invariant(_innerValue != null);
        }
    }
}

#endif
