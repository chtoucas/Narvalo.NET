// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

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
            Demand.True(PredicateFor.CountryCode(countryCode));
            Demand.True(PredicateFor.CheckDigit(checkDigit));
            Demand.True(PredicateFor.Bban(bban));
            Demand.True(PredicateFor.InnerValue(innerValue));

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
                Warrant.NotNull<string>();
                Warrant.Length(CHECKDIGIT_LENGTH);

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
                Warrant.NotNull<string>();
                Warrant.Length(COUNTRY_LENGTH);

                return _countryCode;
            }
        }

        public static Iban Create(string countryCode, string checkDigit, string bban)
        {
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(checkDigit, nameof(checkDigit));
            Require.NotNull(bban, nameof(bban));
            Require.True(PredicateFor.CountryCode(countryCode), nameof(countryCode));
            Require.True(PredicateFor.CheckDigit(checkDigit), nameof(checkDigit));
            Require.True(PredicateFor.Bban(bban), nameof(bban));

            var innerValue = countryCode + checkDigit + bban;
            Assume(PredicateFor.InnerValue(innerValue));
            Check.True(PredicateFor.InnerValue(innerValue));

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
            Assume(PredicateFor.InnerValue(value));
            Check.True(PredicateFor.InnerValue(value));

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
            Check.True(PredicateFor.CountryCode(countryCode));

            string checkDigit = value.Substring(COUNTRY_LENGTH, CHECKDIGIT_LENGTH);
            Check.True(PredicateFor.CheckDigit(checkDigit));

            string bban = value.Substring(COUNTRY_LENGTH + CHECKDIGIT_LENGTH);
            Assume(PredicateFor.Bban(bban));
            Check.True(PredicateFor.Bban(bban));

            return new Iban(countryCode, checkDigit, bban, value);
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Invariant(_bban != null);
            Invariant(_checkDigit != null);
            Invariant(_countryCode != null);
            Invariant(_innerValue != null);
        }

#endif

#if CONTRACTS_FULL
        public
#else
        private
#endif
        static class PredicateFor
        {
            [Pure]
            public static bool Bban(string bban)
            {
                Demand.NotNull(bban);

                return bban.Length >= BBAN_MIN_LENGTH && bban.Length <= BBAN_MAX_LENGTH;
            }

            [Pure]
            public static bool CheckDigit(string checkDigit)
            {
                Demand.NotNull(checkDigit);

                return checkDigit.Length == CHECKDIGIT_LENGTH;
            }

            [Pure]
            public static bool CountryCode(string countryCode)
            {
                Demand.NotNull(countryCode);

                return countryCode.Length == COUNTRY_LENGTH;
            }

            [Pure]
            public static bool InnerValue(string value)
            {
                Demand.NotNull(value);

                return value.Length >= MIN_LENGTH && value.Length <= MAX_LENGTH;
            }
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
