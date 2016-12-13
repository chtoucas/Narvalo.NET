// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Internal;

    using static Narvalo.Finance.IbanFormat;

    /// <summary>
    /// Represents an International Bank Account Number (IBAN).
    /// </summary>
    /// <remarks>
    /// The standard format for an IBAN is defined in ISO 13616.
    /// </remarks>
    public partial struct Iban : IEquatable<Iban>
    {
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
            Demand.True(CheckCountryCode(countryCode));
            Demand.True(CheckCheckDigit(checkDigit));
            Demand.True(CheckBban(bban));
            Demand.True(CheckInnerValue(innerValue));

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
            get { Guards.Warrant.LengthRange(BbanMinLength, BbanMaxLength); return _bban; }
        }

        /// <summary>
        /// Gets the check digit.
        /// </summary>
        public string CheckDigit
        {
            get { Guards.Warrant.Length(CheckDigitLength); return _checkDigit; }
        }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode
        {
            get { Guards.Warrant.Length(CountryLength); return _countryCode; }
        }

        public static Iban Create(string countryCode, string checkDigit, string bban)
        {
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(checkDigit, nameof(checkDigit));
            Require.NotNull(bban, nameof(bban));
            Require.True(CheckCountryCode(countryCode), nameof(countryCode));
            Require.True(CheckCheckDigit(checkDigit), nameof(checkDigit));
            Require.True(CheckBban(bban), nameof(bban));

            var innerValue = countryCode + checkDigit + bban;
            Contract.Assume(CheckInnerValue(innerValue));
            Check.True(CheckInnerValue(innerValue));

            return new Iban(countryCode, checkDigit, bban, innerValue);
        }

        public static Iban Parse(string value)
        {
            Require.NotNull(value, nameof(value));

            Iban? iban = ParseCore(value, true /* throwOnError */);
            Contract.Assume(iban.HasValue);
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

            if (value.Length < MinLength || value.Length > MaxLength)
            {
                if (throwOnError)
                {
                    throw new FormatException(
                        "The IBAN string MUST be at most 34 characters long and at least 14 characters long.");
                }

                return null;
            }
            Contract.Assume(CheckInnerValue(value));
            Check.True(CheckInnerValue(value));

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
            string countryCode = value.Substring(0, CountryLength);
            Check.True(CheckCountryCode(countryCode));

            string checkDigit = value.Substring(CountryLength, CheckDigitLength);
            Check.True(CheckCheckDigit(checkDigit));

            string bban = value.Substring(CountryLength + CheckDigitLength);
            Contract.Assume(CheckBban(bban));
            Check.True(CheckBban(bban));

            return new Iban(countryCode, checkDigit, bban, value);
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

#if CONTRACTS_FULL

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
