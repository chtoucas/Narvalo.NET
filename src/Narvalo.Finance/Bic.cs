// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

    using static Narvalo.Finance.AsciiHelpers;
    using static Narvalo.Finance.BicFormat;

    /// <summary>
    /// Represents a Business Identifier Code (BIC).
    /// </summary>
    /// <remarks>
    /// It was previously understood to be an acronym for Bank Identifier Code.
    /// The standard format for a BIC is defined in ISO 9362-2.
    /// </remarks>
    public partial struct Bic : IEquatable<Bic>
    {
        public const string PrimaryOfficeBranchCode = "XXX";

        private const char SWIFT_TT_MARK = '0';
        private const char SWIFT_NOT_CONNECTED_MARK = '1';

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

        // Connected to the SWIFTNet FIN network?
        public bool IsSwiftConnected => !IsSwiftTest && LocationCode[1] != SWIFT_NOT_CONNECTED_MARK;

        // SWIFTNet FIN network: Test & Training (T&T) service.
        public bool IsSwiftTest => LocationCode[1] == SWIFT_TT_MARK;

        public bool IsPrimaryOffice => BranchCode.Length == 0 || BranchCode == PrimaryOfficeBranchCode;

        /// <summary>
        /// Gets the location code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also the Business party suffix.</remarks>
        public string LocationCode
        {
            get { Sentinel.Warrant.Length(SuffixLength); return _locationCode; }
        }

        public static Bic Create(
            string institutionCode,
            string countryCode,
            string locationCode,
            string branchCode)
        {
            Expect.True(CheckInstitutionCode(institutionCode));
            Expect.True(CheckCountryCode(countryCode));
            Expect.True(CheckLocationCode(locationCode));
            Expect.True(CheckBranchCode(branchCode));

            return Create(institutionCode, countryCode, locationCode, branchCode, BicFormatVersion.Default);
        }

        public static Bic Create(
            string institutionCode,
            string countryCode,
            string locationCode,
            string branchCode,
            BicFormatVersion version)
        {
            // REVIEW: We check for non-null twice...
            Require.NotNull(institutionCode, nameof(institutionCode));
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(locationCode, nameof(locationCode));
            Require.NotNull(branchCode, nameof(branchCode));
            Require.True(CheckInstitutionCode(institutionCode), nameof(institutionCode));
            Require.True(CheckCountryCode(countryCode), nameof(countryCode));
            Require.True(CheckLocationCode(locationCode), nameof(locationCode));
            Require.True(CheckBranchCode(branchCode), nameof(branchCode));

            var value = institutionCode + countryCode + locationCode + branchCode;
            Contract.Assume(CheckValue(value));

            var bic = new Bic(institutionCode, countryCode, locationCode, branchCode, value);
            if (!bic.CheckFormat(version))
            {
                throw new FormatException(Strings.Bic_InvalidFormat);
            }

            return bic;
        }

        public static Bic Parse(string value)
        {
            Expect.NotNull(value);

            return Parse(value, BicFormatVersion.Default);
        }

        public static Bic Parse(string value, BicFormatVersion version)
        {
            Require.NotNull(value, nameof(value));

            if (!CheckValue(value))
            {
                throw new FormatException(Strings.Bic_InvalidFormat);
            }
            Check.True(CheckValue(value));

            var bic = ParseCore(value);
            if (!bic.CheckFormat(version))
            {
                throw new FormatException(Strings.Bic_InvalidFormat);
            }

            return bic;
        }

        public static Bic? TryParse(string value) => TryParse(value, BicFormatVersion.Default);

        public static Bic? TryParse(string value, BicFormatVersion version)
        {
            if (!CheckValue(value)) { return null; }
            Check.True(CheckValue(value));

            var bic = ParseCore(value);
            if (!bic.CheckFormat(version)) { return null; }

            return bic;
        }

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            Warrant.NotNull<string>();

            return _value;
        }

        // The SWIFT implementation is more restrictive as it does not allow for digits in the institution code.
        internal bool CheckFormat(BicFormatVersion version)
            // NB: We do not need to check properties length.
            => (version == BicFormatVersion.ISO ? IsDigitOrUpperLetter(InstitutionCode) : IsUpperLetter(InstitutionCode))
                && IsUpperLetter(CountryCode)
                && IsDigitOrUpperLetter(LocationCode)
                && (BranchCode.Length == 0 || IsDigitOrUpperLetter(BranchCode));

        // This method never throws.
        internal static Bic ParseCore(string value)
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