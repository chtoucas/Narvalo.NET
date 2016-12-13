// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Internal;

    using static Narvalo.Finance.Utilities.IbanFormat;

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
        private readonly string _value;

        private Iban(string countryCode, string checkDigit, string bban, string value)
        {
            Demand.True(CheckCountryCode(countryCode));
            Demand.True(CheckCheckDigit(checkDigit));
            Demand.True(CheckBban(bban));
            Demand.True(CheckValue(value));

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
            get { Sentinel.Warrant.LengthRange(BbanMinLength, BbanMaxLength); return _bban; }
        }

        /// <summary>
        /// Gets the check digit.
        /// </summary>
        public string CheckDigit
        {
            get { Sentinel.Warrant.Length(CheckDigitLength); return _checkDigit; }
        }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode
        {
            get { Sentinel.Warrant.Length(CountryLength); return _countryCode; }
        }

        public static Iban Create(string countryCode, string checkDigit, string bban)
        {
            Require.True(CheckCountryCode(countryCode), nameof(countryCode));
            Require.True(CheckCheckDigit(checkDigit), nameof(checkDigit));
            Require.True(CheckBban(bban), nameof(bban));

            var value = countryCode + checkDigit + bban;
            Contract.Assume(CheckValue(value));

            return new Iban(countryCode, checkDigit, bban, value);
        }

        public static Iban Parse(string value)
        {
            Require.NotNull(value, nameof(value));

            if (!CheckValue(value))
            {
                throw new FormatException(
                    "The IBAN string MUST be at most 34 characters long and at least 14 characters long.");
            }
            Check.True(CheckValue(value));

            return ParseCore(value);
        }

        public static Iban? TryParse(string value)
        {
            if (!CheckValue(value))
            {
                return null;
            }
            Check.True(CheckValue(value));

            return ParseCore(value);
        }

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return _value;
        }

        private static Iban ParseCore(string value)
        {
            Demand.True(CheckValue(value));

            // The first two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(0, CountryLength);
            Check.True(CheckCountryCode(countryCode));

            string checkDigit = value.Substring(CountryLength, CheckDigitLength);
            Check.True(CheckCheckDigit(checkDigit));

            string bban = value.Substring(CountryLength + CheckDigitLength);
            Contract.Assume(CheckBban(bban));

            return new Iban(countryCode, checkDigit, bban, value);
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
            Contract.Invariant(CheckCheckDigit(_checkDigit));
            Contract.Invariant(CheckCountryCode(_countryCode));
            Contract.Invariant(CheckValue(_value));
        }
    }
}

#endif
