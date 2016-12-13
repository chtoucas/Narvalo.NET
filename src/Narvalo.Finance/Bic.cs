// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Internal;

    using static Narvalo.Finance.Utilities.BicFormat;

    /// <summary>
    /// Represents a Business Identifier Code (BIC).
    /// </summary>
    /// <remarks>
    /// It was previously understood to be an acronym for Bank Identifier Code.
    /// The standard format for a BIC is defined in ISO 9362:2014.
    /// </remarks>
    public partial struct Bic : IEquatable<Bic>
    {
        public const string PrimaryOfficeBranchCode = "XXX";

        private const char CONNECTED_IDENTIFIER = '1';

        private readonly string _branchCode;
        private readonly string _countryCode;
        private readonly string _institutionCode;
        private readonly string _locationCode;
        private readonly string _value;

        private Bic(
            string institutionCode,
            string countryCode,
            string locationCode,
            string branchCode,
            string value)
        {
            Demand.True(CheckInstitutionCode(institutionCode));
            Demand.True(CheckCountryCode(countryCode));
            Demand.True(CheckLocationCode(locationCode));
            Demand.True(CheckBranchCode(branchCode));
            Demand.True(CheckValue(value));

            _institutionCode = institutionCode;
            _countryCode = countryCode;
            _locationCode = locationCode;
            _branchCode = branchCode;
            _value = value;
        }

        /// <summary>
        /// Gets the branch code.
        /// </summary>
        public string BranchCode
        {
            get { Sentinel.Warrant.Lengths(0, BranchLength); return _branchCode; }
        }

        public string BusinessParty
        {
            get { Warrant.NotNull<string>(); return InstitutionCode + CountryCode + LocationCode; }
        }

        /// <summary>
        /// Gets the ISO country code.
        /// </summary>
        public string CountryCode
        {
            get { Sentinel.Warrant.Length(CountryLength); return _countryCode; }
        }

        /// <summary>
        /// Gets the institution code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also the Business party prefix.</remarks>
        public string InstitutionCode
        {
            get { Sentinel.Warrant.Length(PrefixLength); return _institutionCode; }
        }

        public bool IsConnected => LocationCode[1] != CONNECTED_IDENTIFIER;

        public bool IsPrimaryOffice => BranchCode.Length == 0 || BranchCode == PrimaryOfficeBranchCode;

        /// <summary>
        /// Gets the location code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also the Business party suffix.</remarks>
        public string LocationCode
        {
            get { Sentinel.Warrant.Length(SuffixLength); return _locationCode; }
        }

        public static Bic Create(string institutionCode, string countryCode, string locationCode, string branchCode)
        {
            Require.True(CheckInstitutionCode(institutionCode), nameof(institutionCode));
            Require.True(CheckCountryCode(countryCode), nameof(countryCode));
            Require.True(CheckLocationCode(locationCode), nameof(locationCode));
            Require.True(CheckBranchCode(branchCode), nameof(branchCode));

            var value = institutionCode + countryCode + locationCode + branchCode;
            Contract.Assume(CheckValue(value));

            return new Bic(institutionCode, countryCode, locationCode, branchCode, value);
        }

        public static Bic Parse(string value)
        {
            Require.NotNull(value, nameof(value));

            if (!CheckValue(value))
            {
                throw new FormatException("The BIC string MUST be 8 or 11 characters long.");
            }
            Check.True(CheckValue(value));

            return ParseCore(value);
        }

        public static Bic? TryParse(string value)
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

        public bool ValidateFormat() => Validate(this, true /* strict */);

        public bool ValidateSwiftFormat() => Validate(this, false /* strict */);

        // NB: We only perform basic validation on the input string.
        private static Bic ParseCore(string value)
        {
            Demand.True(CheckValue(value));

#if CONTRACTS_FULL // Helps CCCheck to decipher the precondition.

            // This is just the negation of CheckValue(value).
            if (value == null || (value.Length != PartyLength && value.Length != BicLength))
            {
                throw new InvalidOperationException();
            }

#endif

            // The first four letters or digits define the institution or bank code.
            string institutionCode = value.Substring(0, PrefixLength);
            Contract.Assume(CheckInstitutionCode(institutionCode));

            // The next two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(PrefixLength, CountryLength);
            Contract.Assume(CheckCountryCode(countryCode));

            // The next two letters or digits define the location code.
            string locationCode = value.Substring(PrefixLength + CountryLength, SuffixLength);
            Contract.Assume(CheckLocationCode(locationCode));

            // The next three letters or digits define the branch code (optional).
            string branchCode = value.Length == PartyLength
                ? String.Empty
                : value.Substring(PrefixLength + CountryLength + SuffixLength, BranchLength);
            Contract.Assume(CheckBranchCode(branchCode));

            return new Bic(institutionCode, countryCode, locationCode, branchCode, value);
        }
    }

    // Implements the IEquatable<Bic> interface.
    public partial struct Bic
    {
        public static bool operator ==(Bic left, Bic right) => left.Equals(right);

        public static bool operator !=(Bic left, Bic right) => !left.Equals(right);

        public bool Equals(Bic other) => _value == other._value;

        public override bool Equals(object obj)
        {
            if (!(obj is Bic))
            {
                return false;
            }

            return Equals((Bic)obj);
        }

        public override int GetHashCode() => _value.GetHashCode();
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.Utilities.BicFormat;

    public partial struct Bic
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(CheckBranchCode(_branchCode));
            Contract.Invariant(CheckCountryCode(_countryCode));
            Contract.Invariant(CheckInstitutionCode(_institutionCode));
            Contract.Invariant(CheckLocationCode(_locationCode));
            Contract.Invariant(CheckValue(_value));
        }
    }
}

#endif