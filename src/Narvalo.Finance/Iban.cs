// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

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

        private readonly string _bban;
        private readonly string _checkDigit;
        private readonly string _countryCode;
        private readonly string _value;

        private Iban(string countryCode, string checkDigit, string bban, string value)
        {
            Demand.NotNull(countryCode);
            Demand.NotNull(checkDigit);
            Demand.NotNull(bban);
            Demand.NotNull(value);
            Demand.True(countryCode.Length == COUNTRY_LENGTH);
            Demand.True(checkDigit.Length == CHECKDIGIT_LENGTH);

            _countryCode = countryCode;
            _checkDigit = checkDigit;
            _bban = bban;
            _value = value;
        }

        /// <summary>
        /// Gets the Basic Bank Account Number (BBAN).
        /// </summary>
        public string Bban
        {
            get
            {
                Ensure<string>.NotNull();
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
                Ensure<string>.NotNull();
                Ensures(Result<string>().Length == CHECKDIGIT_LENGTH);

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
                Ensure<string>.NotNull();
                Ensures(Result<string>().Length == COUNTRY_LENGTH);

                return _countryCode;
            }
        }

        public static Iban Create(string countryCode, string checkDigit, string bban)
        {
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(checkDigit, nameof(checkDigit));
            Require.NotNull(bban, nameof(bban));
            Require.True(countryCode.Length == COUNTRY_LENGTH, nameof(countryCode));
            Require.True(checkDigit.Length == CHECKDIGIT_LENGTH, nameof(checkDigit));

            return new Iban(countryCode, checkDigit, bban, countryCode + checkDigit + bban);
        }

        public static Iban Parse(string value)
        {
            Require.NotNull(value, nameof(value));

            Iban? iban = ParseCore(value, true /* throwOnError */);
            Assume(iban.HasValue);

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

        public override string ToString() => _value;

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
            Assert(countryCode.Length == COUNTRY_LENGTH);

            string checkDigit = value.Substring(COUNTRY_LENGTH, CHECKDIGIT_LENGTH);
            Assert(checkDigit.Length == CHECKDIGIT_LENGTH);

            string bban = value.Substring(COUNTRY_LENGTH + CHECKDIGIT_LENGTH);

            return new Iban(countryCode, checkDigit, bban, value);
        }

        //#if CONTRACTS_FULL // Contract Class and Object Invariants.

        //        [System.Diagnostics.Contracts.ContractInvariantMethod]
        //        private void ObjectInvariant()
        //        {
        //            Invariant(_bban != null);
        //            Invariant(_checkDigit != null);
        //            Invariant(_countryCode != null);
        //            Invariant(_value != null);
        //        }

        //#endif
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
