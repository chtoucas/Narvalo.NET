// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.Internal.AsciiUtility;

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
            Contract.Requires(institutionCode != null);
            Contract.Requires(institutionCode.Length == PREFIX_LENGTH);
            Contract.Requires(countryCode != null);
            Contract.Requires(countryCode.Length == COUNTRY_LENGTH);
            Contract.Requires(locationCode != null);
            Contract.Requires(locationCode.Length == SUFFIX_LENGTH);
            Contract.Requires(branchCode != null);
            //Contract.Requires(branchCode.Length == 0 || branchCode.Length == BRANCH_LENGTH);
            Contract.Requires(value != null);
            //Contract.Requires(value.Length == PARTY_LENGTH || value.Length == BIC_LENGTH);

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
                Contract.Ensures(Contract.Result<string>() != null);
                //Contract.Ensures(branchCode.Length == 0 || branchCode.Length == BRANCH_LENGTH);
                return _branchCode;
            }
        }

        public string BusinessParty
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
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
                Contract.Ensures(Contract.Result<string>() != null);
                Contract.Ensures(Contract.Result<string>().Length == COUNTRY_LENGTH);
                return _countryCode;
            }
        }

        /// <summary>
        /// Gets the institution code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also called the Business party prefix.</remarks>
        public string InstitutionCode
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                Contract.Ensures(Contract.Result<string>().Length == PREFIX_LENGTH);
                return _institutionCode;
            }
        }

        public bool IsConnected => LocationCode[1] != CONNECTED_IDENTIFIER;

        public bool IsPrimaryOffice => BranchCode.Length == 0 || BranchCode == PrimaryOfficeBranchCode;

        /// <summary>
        /// Gets the location code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also called the Business party suffix.</remarks>
        public string LocationCode
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                Contract.Ensures(Contract.Result<string>().Length == SUFFIX_LENGTH);
                return _locationCode;
            }
        }

        public static Bic Create(string institutionCode, string countryCode, string locationCode, string branchCode)
        {
            Require.NotNull(institutionCode, nameof(institutionCode));
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(locationCode, nameof(locationCode));
            Require.NotNull(branchCode, nameof(branchCode));
            Contract.Requires(institutionCode.Length == PREFIX_LENGTH);
            Contract.Requires(countryCode.Length == COUNTRY_LENGTH);
            Contract.Requires(locationCode.Length == SUFFIX_LENGTH);

            if (institutionCode.Length != PREFIX_LENGTH)
            {
                throw new ArgumentException("XXX", nameof(institutionCode));
            }
            if (countryCode.Length != COUNTRY_LENGTH)
            {
                throw new ArgumentException("XXX", nameof(countryCode));
            }
            if (locationCode.Length != SUFFIX_LENGTH)
            {
                throw new ArgumentException("XXX", nameof(locationCode));
            }

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
            Contract.Assume(iban.HasValue);

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

        [Pure]
        public bool CheckFormat() => CheckFormat(true /* isoConformance */);

        [Pure]
        public bool CheckSwiftFormat() => CheckFormat(false /* isoConformance */);

//#if CONTRACTS_FULL // Contract Class and Object Invariants.

//        [ContractInvariantMethod]
//        private void ObjectInvariant()
//        {
//            Contract.Invariant(BranchCode != null);
//            //Contract.Invariant(BranchCode.Length == 0 || BranchCode.Length == BRANCH_LENGTH);
//            Contract.Invariant(CountryCode != null);
//            //Contract.Invariant(CountryCode.Length == COUNTRY_LENGTH);
//            Contract.Invariant(InstitutionCode != null);
//            //Contract.Invariant(InstitutionCode.Length == PREFIX_LENGTH);
//            Contract.Invariant(LocationCode != null);
//            //Contract.Invariant(LocationCode.Length == SUFFIX_LENGTH);
//            Contract.Invariant(_value != null);
//            //Contract.Invariant(_value.Length == PARTY_LENGTH || _value.Length == BIC_LENGTH);
//        }

//#endif

        // NB: We only perform basic validation on the input string.
        private static Bic? ParseCore(string value, bool throwOnError)
        {
            Contract.Requires(value != null);

            if (value.Length != BIC_LENGTH && value.Length != PARTY_LENGTH)
            {
                if (throwOnError)
                {
                    throw new ArgumentException(
                        "The BIC string MUST be 8 or 11 characters long.",
                        nameof(value));
                }

                return null;
            }

            // The first four letters or digits define the institution or bank code.
            // NB: SWIFT is more restrictive than ISO as it only expects letters.
            string institutionCode = value.Substring(0, PREFIX_LENGTH);
            Contract.Assert(institutionCode.Length == PREFIX_LENGTH);

            // The next two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(PREFIX_LENGTH, COUNTRY_LENGTH);
            Contract.Assert(countryCode.Length == COUNTRY_LENGTH);

            // The next two letters or digits define the location code.
            string locationCode = value.Substring(PREFIX_LENGTH + COUNTRY_LENGTH, SUFFIX_LENGTH);
            Contract.Assert(locationCode.Length == SUFFIX_LENGTH);

            // The next three letters or digits define the branch code (optional).
            string branchCode = value.Length == PARTY_LENGTH
                ? String.Empty
                : value.Substring(PREFIX_LENGTH + COUNTRY_LENGTH + SUFFIX_LENGTH, BRANCH_LENGTH);
            //Contract.Assert(branchCode.Length == 0 || branchCode.Length == BRANCH_LENGTH);

            return new Bic(institutionCode, countryCode, locationCode, branchCode, value);
        }

        [Pure]
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
