// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Internal;

    using static System.Diagnostics.Contracts.Contract;
    using static Narvalo.Finance.BicFormat;

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
        private readonly string _innerValue;

        private Bic(
            string institutionCode,
            string countryCode,
            string locationCode,
            string branchCode,
            string innerValue)
        {
            Demand.NotNull(institutionCode);
            Demand.NotNull(countryCode);
            Demand.NotNull(locationCode);
            Demand.NotNull(branchCode);
            Demand.NotNull(innerValue);
            Demand.True(CheckInstitutionCode(institutionCode));
            Demand.True(CheckCountryCode(countryCode));
            Demand.True(CheckLocationCode(locationCode));
            Demand.True(CheckBranchCode(branchCode));
            Demand.True(CheckInnerValue(innerValue));

            _institutionCode = institutionCode;
            _countryCode = countryCode;
            _locationCode = locationCode;
            _branchCode = branchCode;
            _innerValue = innerValue;
        }

        /// <summary>
        /// Gets the branch code.
        /// </summary>
        public string BranchCode
        {
            get { Guards.Warrant.Lengths(0, BranchLength); return _branchCode; }
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
            get { Guards.Warrant.Length(CountryLength); return _countryCode; }
        }

        /// <summary>
        /// Gets the institution code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also the Business party prefix.</remarks>
        public string InstitutionCode
        {
            get { Guards.Warrant.Length(PrefixLength); return _institutionCode; }
        }

        public bool IsConnected => LocationCode[1] != CONNECTED_IDENTIFIER;

        public bool IsPrimaryOffice => BranchCode.Length == 0 || BranchCode == PrimaryOfficeBranchCode;

        /// <summary>
        /// Gets the location code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also the Business party suffix.</remarks>
        public string LocationCode
        {
            get { Guards.Warrant.Length(SuffixLength); return _locationCode; }
        }

        public static Bic Create(string institutionCode, string countryCode, string locationCode, string branchCode)
        {
            Require.NotNull(institutionCode, nameof(institutionCode));
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(locationCode, nameof(locationCode));
            Require.NotNull(branchCode, nameof(branchCode));
            Require.True(CheckInstitutionCode(institutionCode), nameof(institutionCode));
            Require.True(CheckCountryCode(countryCode), nameof(countryCode));
            Require.True(CheckLocationCode(locationCode), nameof(locationCode));
            Require.True(CheckBranchCode(branchCode), nameof(branchCode));

            var innerValue = institutionCode + countryCode + locationCode + branchCode;
            Assume(CheckInnerValue(innerValue));
            Narvalo.Check.True(CheckInnerValue(innerValue));

            return new Bic(institutionCode, countryCode, locationCode, branchCode, innerValue);
        }

        public static Bic Parse(string value)
        {
            Require.NotNull(value, nameof(value));

            Bic? bic = ParseCore(value, true /* throwOnError */);
            Assume(bic.HasValue);
            Narvalo.Check.True(bic.HasValue);

            return bic.Value;
        }

        public static Bic? TryParse(string value)
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

        public bool CheckFormat() => Check(this, true /* conforming */);

        public bool CheckSwiftFormat() => Check(this, false /* conforming */);

        // NB: We only perform basic validation on the input string.
        private static Bic? ParseCore(string value, bool throwOnError)
        {
            Demand.NotNull(value);

            if (value.Length != BicLength && value.Length != PartyLength)
            {
                if (throwOnError)
                {
                    throw new FormatException("The BIC string MUST be 8 or 11 characters long.");
                }

                return null;
            }
            Assume(CheckInnerValue(value));
            Narvalo.Check.True(CheckInnerValue(value));

            // The first four letters or digits define the institution or bank code.
            // NB: SWIFT is more restrictive than ISO as it only expects letters.
            string institutionCode = value.Substring(0, PrefixLength);
            Narvalo.Check.True(CheckInstitutionCode(institutionCode));

            // The next two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(PrefixLength, CountryLength);
            Narvalo.Check.True(CheckCountryCode(countryCode));

            // The next two letters or digits define the location code.
            string locationCode = value.Substring(PrefixLength + CountryLength, SuffixLength);
            Narvalo.Check.True(CheckLocationCode(locationCode));

            // The next three letters or digits define the branch code (optional).
            string branchCode = value.Length == PartyLength
                ? String.Empty
                : value.Substring(PrefixLength + CountryLength + SuffixLength, BranchLength);
            Assume(CheckBranchCode(branchCode));
            Narvalo.Check.True(CheckBranchCode(branchCode));

            return new Bic(institutionCode, countryCode, locationCode, branchCode, value);
        }
    }

    // Implements the IEquatable<Bic> interface.
    public partial struct Bic
    {
        public static bool operator ==(Bic left, Bic right) => left.Equals(right);

        public static bool operator !=(Bic left, Bic right) => !left.Equals(right);

        public bool Equals(Bic other) => _innerValue == other._innerValue;

        public override bool Equals(object obj)
        {
            if (!(obj is Bic))
            {
                return false;
            }

            return Equals((Bic)obj);
        }

        public override int GetHashCode() => _innerValue.GetHashCode();
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    public partial struct Bic
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_branchCode != null);
            Contract.Invariant(_countryCode != null);
            Contract.Invariant(_innerValue != null);
            Contract.Invariant(_institutionCode != null);
            Contract.Invariant(_locationCode != null);
        }
    }
}

#endif