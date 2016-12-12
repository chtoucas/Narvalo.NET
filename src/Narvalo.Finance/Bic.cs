// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;
    using static Narvalo.Finance.Internal.AsciiHelpers;

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

        private const int BIC_LENGTH = 11;
        private const int PARTY_LENGTH = 8;
        private const int PREFIX_LENGTH = 4;
        private const int COUNTRY_LENGTH = 2;
        private const int SUFFIX_LENGTH = 2;
        private const int BRANCH_LENGTH = 3;

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
            Demand.True(PredicateFor.InstitutionCode(institutionCode));
            Demand.True(PredicateFor.CountryCode(countryCode));
            Demand.True(PredicateFor.LocationCode(locationCode));
            Demand.True(PredicateFor.BranchCode(branchCode));
            Demand.True(PredicateFor.InnerValue(innerValue));

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
            get
            {
                Warrant.NotNull<string>();
                Ensures(Result<string>().Length == 0 || Result<string>().Length == BRANCH_LENGTH);

                return _branchCode;
            }
        }

        public string BusinessParty
        {
            get
            {
                Warrant.NotNull<string>();

                return InstitutionCode + CountryCode + LocationCode;
            }
        }

        /// <summary>
        /// Gets the ISO country code.
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

        /// <summary>
        /// Gets the institution code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also the Business party prefix.</remarks>
        public string InstitutionCode
        {
            get
            {
                Warrant.NotNull<string>();
                Warrant.Length(PREFIX_LENGTH);

                return _institutionCode;
            }
        }

        public bool IsConnected => LocationCode[1] != CONNECTED_IDENTIFIER;

        public bool IsPrimaryOffice => BranchCode.Length == 0 || BranchCode == PrimaryOfficeBranchCode;

        /// <summary>
        /// Gets the location code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also the Business party suffix.</remarks>
        public string LocationCode
        {
            get
            {
                Warrant.NotNull<string>();
                Warrant.Length(SUFFIX_LENGTH);

                return _locationCode;
            }
        }

        public static Bic Create(string institutionCode, string countryCode, string locationCode, string branchCode)
        {
            Require.NotNull(institutionCode, nameof(institutionCode));
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(locationCode, nameof(locationCode));
            Require.NotNull(branchCode, nameof(branchCode));
            Require.True(PredicateFor.InstitutionCode(institutionCode), nameof(institutionCode));
            Require.True(PredicateFor.CountryCode(countryCode), nameof(countryCode));
            Require.True(PredicateFor.LocationCode(locationCode), nameof(locationCode));
            Require.True(PredicateFor.BranchCode(branchCode), nameof(branchCode));

            var innerValue = institutionCode + countryCode + locationCode + branchCode;
            Assume(PredicateFor.InnerValue(innerValue));
            Check.True(PredicateFor.InnerValue(innerValue));

            return new Bic(institutionCode, countryCode, locationCode, branchCode, innerValue);
        }

        public static Bic Parse(string value)
        {
            Require.NotNull(value, nameof(value));

            Bic? bic = ParseCore(value, true /* throwOnError */);
            Assume(bic.HasValue);
            Check.True(bic.HasValue);

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

        public bool CheckFormat() => CheckFormat(true /* isoConformance */);

        public bool CheckSwiftFormat() => CheckFormat(false /* isoConformance */);

        // NB: We only perform basic validation on the input string.
        private static Bic? ParseCore(string value, bool throwOnError)
        {
            Demand.NotNull(value);

            if (value.Length != BIC_LENGTH && value.Length != PARTY_LENGTH)
            {
                if (throwOnError)
                {
                    throw new FormatException("The BIC string MUST be 8 or 11 characters long.");
                }

                return null;
            }
            Assume(PredicateFor.InnerValue(value));
            Check.True(PredicateFor.InnerValue(value));

            // The first four letters or digits define the institution or bank code.
            // NB: SWIFT is more restrictive than ISO as it only expects letters.
            string institutionCode = value.Substring(0, PREFIX_LENGTH);
            Check.True(PredicateFor.InstitutionCode(institutionCode));

            // The next two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(PREFIX_LENGTH, COUNTRY_LENGTH);
            Check.True(PredicateFor.CountryCode(countryCode));

            // The next two letters or digits define the location code.
            string locationCode = value.Substring(PREFIX_LENGTH + COUNTRY_LENGTH, SUFFIX_LENGTH);
            Check.True(PredicateFor.LocationCode(locationCode));

            // The next three letters or digits define the branch code (optional).
            string branchCode = value.Length == PARTY_LENGTH
                ? String.Empty
                : value.Substring(PREFIX_LENGTH + COUNTRY_LENGTH + SUFFIX_LENGTH, BRANCH_LENGTH);
            Assume(PredicateFor.BranchCode(branchCode));
            Check.True(PredicateFor.BranchCode(branchCode));

            return new Bic(institutionCode, countryCode, locationCode, branchCode, value);
        }

        private bool CheckFormat(bool isoConformance)
            // NB: We do not need to check properties length.
            => (isoConformance ? IsDigitOrUpperLetter(InstitutionCode) : IsUpperLetter(InstitutionCode))
                && IsUpperLetter(CountryCode)
                && IsDigitOrUpperLetter(LocationCode)
                && (BranchCode.Length == 0 ? true : IsDigitOrUpperLetter(BranchCode));

        // REVIEW: Believe this is wrong.
#if CONTRACTS_FULL
        public
#else
        private
#endif
        static class PredicateFor
        {
            [Pure]
            public static bool BranchCode(string branchCode)
            {
                Demand.NotNull(branchCode);

                return branchCode.Length == 0 || branchCode.Length == BRANCH_LENGTH;
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

                return value.Length == PARTY_LENGTH || value.Length == BIC_LENGTH;
            }

            [Pure]
            public static bool InstitutionCode(string institutionCode)
            {
                Demand.NotNull(institutionCode);

                return institutionCode.Length == PREFIX_LENGTH;
            }

            [Pure]
            public static bool LocationCode(string locationCode)
            {
                Demand.NotNull(locationCode);

                return locationCode.Length == SUFFIX_LENGTH;
            }
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

#if CONTRACTS_FULL // Contract Class and Object Invariants.

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