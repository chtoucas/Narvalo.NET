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

        private readonly string _value;

        public Bic(string institutionCode, string countryCode, string locationCode, string branchCode)
            : this(
                  institutionCode,
                  countryCode,
                  locationCode,
                  branchCode,
                  institutionCode + countryCode + locationCode + branchCode)
        { }

        private Bic(string institutionCode, string countryCode, string locationCode, string branchCode, string value)
        {
            Contract.Requires(institutionCode != null && institutionCode.Length == PREFIX_LENGTH);
            Contract.Requires(countryCode != null && countryCode.Length == COUNTRY_LENGTH);
            Contract.Requires(locationCode != null && locationCode.Length == SUFFIX_LENGTH);
            Contract.Requires(branchCode != null && (branchCode.Length == 0 || branchCode.Length == BRANCH_LENGTH));
            Contract.Requires(value != null && (value.Length == PARTY_LENGTH || value.Length == BIC_LENGTH));

            InstitutionCode = institutionCode;
            CountryCode = countryCode;
            LocationCode = locationCode;
            BranchCode = branchCode;
            _value = value;
        }

        /// <summary>
        /// Gets the branch code.
        /// </summary>
        public string BranchCode { get; }

        public string BusinessParty => InstitutionCode + CountryCode + LocationCode;

        /// <summary>
        /// Gets the ISO country code.
        /// </summary>
        public string CountryCode { get; }

        /// <summary>
        /// Gets the institution code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also called the Business party prefix.</remarks>
        public string InstitutionCode { get; }

        public bool IsConnected => LocationCode[1] != CONNECTED_IDENTIFIER;

        public bool IsPrimaryOffice => BranchCode.Length == 0 || BranchCode == PrimaryOfficeBranchCode;

        /// <summary>
        /// Gets the location code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also called the Business party suffix.</remarks>
        public string LocationCode { get; }

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

        public bool CheckFormat() => CheckFormat(true /* isoConformance */);

        public bool CheckSwiftFormat() => CheckFormat(false /* isoConformance */);

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(BranchCode != null && (BranchCode.Length == 0 || BranchCode.Length == BRANCH_LENGTH));
            Contract.Invariant(CountryCode != null && CountryCode.Length == COUNTRY_LENGTH);
            Contract.Invariant(InstitutionCode != null && InstitutionCode.Length == PREFIX_LENGTH);
            Contract.Invariant(LocationCode != null && LocationCode.Length == SUFFIX_LENGTH);
            Contract.Invariant(_value != null && (_value.Length == PARTY_LENGTH || _value.Length == BIC_LENGTH));
        }

#endif

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

            // The next two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(PREFIX_LENGTH, COUNTRY_LENGTH);

            // The next two letters or digits define the location code.
            string locationCode = value.Substring(PREFIX_LENGTH + COUNTRY_LENGTH, SUFFIX_LENGTH);

            // The next three letters or digits define the branch code (optional).
            string branchCode = value.Length == PARTY_LENGTH
                ? String.Empty
                : value.Substring(PREFIX_LENGTH + COUNTRY_LENGTH + SUFFIX_LENGTH, BRANCH_LENGTH);

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
