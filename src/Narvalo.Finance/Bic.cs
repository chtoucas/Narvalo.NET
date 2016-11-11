// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

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
        private readonly string _value;

        private Bic(string institutionCode, string countryCode, string locationCode, string branchCode, string value)
        {
            Demand.NotNull(institutionCode);
            Demand.NotNull(countryCode);
            Demand.NotNull(locationCode);
            Demand.NotNull(branchCode);
            Demand.NotNull(value);
            Demand.True(institutionCode.Length == PREFIX_LENGTH);
            Demand.True(countryCode.Length == COUNTRY_LENGTH);
            Demand.True(locationCode.Length == SUFFIX_LENGTH);
            Demand.Unproven.True(branchCode.Length == 0 || branchCode.Length == BRANCH_LENGTH);
            Demand.Unproven.True(value.Length == PARTY_LENGTH || value.Length == BIC_LENGTH);

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
            get
            {
                Ensures(Result<string>() != null);
                //Ensures(Result<string>().Length == 0 || Result<string>().Length == BRANCH_LENGTH);

                return _branchCode;
            }
        }

        public string BusinessParty
        {
            get
            {
                Ensures(Result<string>() != null);

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
                Ensures(Result<string>() != null);
                Ensures(Result<string>().Length == COUNTRY_LENGTH);

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
                Ensures(Result<string>() != null);
                Ensures(Result<string>().Length == PREFIX_LENGTH);

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
                Ensures(Result<string>() != null);
                Ensures(Result<string>().Length == SUFFIX_LENGTH);

                return _locationCode;
            }
        }

        public static Bic Create(string institutionCode, string countryCode, string locationCode, string branchCode)
        {
            Require.NotNull(institutionCode, nameof(institutionCode));
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(locationCode, nameof(locationCode));
            Require.NotNull(branchCode, nameof(branchCode));
            Require.True(institutionCode.Length == PREFIX_LENGTH, nameof(institutionCode));
            Require.True(countryCode.Length == COUNTRY_LENGTH, nameof(countryCode));
            Require.True(locationCode.Length == SUFFIX_LENGTH, nameof(locationCode));
            Require.Unproven.True(branchCode.Length == 0 || branchCode.Length == BRANCH_LENGTH, nameof(branchCode));

            return new Bic(
                institutionCode,
                countryCode,
                locationCode,
                branchCode,
                institutionCode + countryCode + locationCode + branchCode
            );
        }

        public static Bic Parse(string value)
        {
            Require.NotNull(value, nameof(value));

            Bic? iban = ParseCore(value, true /* throwOnError */);
            Assume(iban.HasValue);

            return iban.Value;
        }

        public static Bic? TryParse(string value)
        {
            if (value == null)
            {
                return null;
            }

            return ParseCore(value, false /* throwOnError */);
        }

        public override string ToString() => _value;

        public bool CheckFormat() => CheckFormat(true /* isoConformance */);

        public bool CheckSwiftFormat() => CheckFormat(false /* isoConformance */);

//#if CONTRACTS_FULL // Contract Class and Object Invariants.

//        [System.Diagnostics.Contracts.ContractInvariantMethod]
//        private void ObjectInvariant()
//        {
//            Invariant(BranchCode != null);
//            //Invariant(BranchCode.Length == 0 || BranchCode.Length == BRANCH_LENGTH);
//            Invariant(CountryCode != null);
//            //Invariant(CountryCode.Length == COUNTRY_LENGTH);
//            Invariant(InstitutionCode != null);
//            //Invariant(InstitutionCode.Length == PREFIX_LENGTH);
//            Invariant(LocationCode != null);
//            //Invariant(LocationCode.Length == SUFFIX_LENGTH);
//            //Invariant(_value != null);
//            //Invariant(_value.Length == PARTY_LENGTH || _value.Length == BIC_LENGTH);
//        }

//#endif

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

            // The first four letters or digits define the institution or bank code.
            // NB: SWIFT is more restrictive than ISO as it only expects letters.
            string institutionCode = value.Substring(0, PREFIX_LENGTH);
            Check.True(institutionCode.Length == PREFIX_LENGTH);

            // The next two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(PREFIX_LENGTH, COUNTRY_LENGTH);
            Check.True(countryCode.Length == COUNTRY_LENGTH);

            // The next two letters or digits define the location code.
            string locationCode = value.Substring(PREFIX_LENGTH + COUNTRY_LENGTH, SUFFIX_LENGTH);
            Check.True(locationCode.Length == SUFFIX_LENGTH);

            // The next three letters or digits define the branch code (optional).
            string branchCode = value.Length == PARTY_LENGTH
                ? String.Empty
                : value.Substring(PREFIX_LENGTH + COUNTRY_LENGTH + SUFFIX_LENGTH, BRANCH_LENGTH);
            Check.Unproven.True(branchCode.Length == 0 || branchCode.Length == BRANCH_LENGTH);

            return new Bic(institutionCode, countryCode, locationCode, branchCode, value);
        }

        private bool CheckFormat(bool isoConformance)
            // NB: We do not need to check properties length.
            => (isoConformance ? IsDigitOrUpperLetter(InstitutionCode) : IsUpperLetter(InstitutionCode))
                && IsUpperLetter(CountryCode)
                && IsDigitOrUpperLetter(LocationCode)
                && (BranchCode.Length == 0 ? true : IsDigitOrUpperLetter(BranchCode));
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
