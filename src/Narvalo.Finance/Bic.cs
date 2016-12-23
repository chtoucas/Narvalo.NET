// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Properties;
    using Narvalo.Finance.Utilities;

    using static Narvalo.Finance.Utilities.AsciiHelpers;

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

        internal const int PrefixLength = 4;
        internal const int CountryLength = 2;
        internal const int SuffixLength = 2;
        internal const int BranchLength = 3;

        internal const int PartyLength = PrefixLength + CountryLength + SuffixLength;
        internal const int BicLength = PartyLength + BranchLength;

        private readonly string _branchCode;
        private readonly string _countryCode;
        private readonly string _institutionCode;
        private readonly string _locationCode;
        private readonly string _value;

        private Bic(
            string institutionCode,
            string countryCode,
            string locationCode,
            string branchCode)
            : this(
                institutionCode,
                countryCode,
                locationCode,
                branchCode,
                institutionCode + countryCode + locationCode + branchCode)
        {
            Expect.NotNull(institutionCode);
            Expect.NotNull(countryCode);
            Expect.NotNull(locationCode);
            Expect.NotNull(branchCode);
        }

        private Bic(
            string institutionCode,
            string countryCode,
            string locationCode,
            string branchCode,
            string value)
        {
            Demand.NotNull(institutionCode);
            Demand.NotNull(countryCode);
            Demand.NotNull(locationCode);
            Demand.NotNull(branchCode);
            Demand.NotNull(value);

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
            get { Warrant.NotNull<string>(); return _branchCode; }
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
            get { Warrant.NotNull<string>(); return _countryCode; }
        }

        /// <summary>
        /// Gets the institution code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also the Business party prefix.</remarks>
        public string InstitutionCode
        {
            get { Warrant.NotNull<string>(); return _institutionCode; }
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
            get { Warrant.NotNull<string>(); return _locationCode; }
        }

        public static Bic Create(
            string institutionCode,
            string countryCode,
            string locationCode,
            string branchCode)
        {
            Expect.NotNull(institutionCode);
            Expect.NotNull(countryCode);
            Expect.NotNull(locationCode);
            Expect.NotNull(branchCode);

            return Create(institutionCode, countryCode, locationCode, branchCode, BicVersion.Default);
        }

        public static Bic Create(
            string institutionCode,
            string countryCode,
            string locationCode,
            string branchCode,
            BicVersion version)
        {
            Require.NotNull(institutionCode, nameof(institutionCode));
            Require.NotNull(countryCode, nameof(countryCode));
            Require.NotNull(locationCode, nameof(locationCode));
            Require.NotNull(branchCode, nameof(branchCode));

            if (CheckInstitutionCode(institutionCode, version)
                && CheckCountryCode(countryCode)
                && CheckLocationCode(locationCode)
                && CheckBranchCode(branchCode))
            {
                return new Bic(institutionCode, countryCode, locationCode, branchCode);
            }
            else
            {
                throw new ArgumentException(Strings.Argument_InvalidBicParts);
            }
        }

        public static Bic? Parse(string value) => Parse(value, BicVersion.Default);

        public static Bic? Parse(string value, BicVersion version)
        {
            if (!CheckValue(value)) { return null; }

            string institutionCode = GetInstitutionCode(value);
            if (!CheckInstitutionCode(institutionCode, version)) { return null; }

            string countryCode = GetCountryCode(value);
            if (!CheckCountryCode(countryCode)) { return null; }

            string locationCode = GetLocationCode(value);
            if (!CheckLocationCode(locationCode)) { return null; }

            string branchCode = GetBranchCode(value);
            if (!CheckBranchCode(branchCode)) { return null; }

            return new Bic(institutionCode, countryCode, locationCode, branchCode, value);
        }

        public static Outcome<Bic> TryParse(string value)
        {
            Expect.NotNull(value);

            return TryParse(value, BicVersion.Default);
        }

        public static Outcome<Bic> TryParse(string value, BicVersion version)
        {
            Require.NotNull(value, nameof(value));

            if (!CheckValue(value))
            {
                return Outcome<Bic>.Failure(Strings.Parse_InvalidBicValue);
            }

            string institutionCode = GetInstitutionCode(value);
            if (!CheckInstitutionCode(institutionCode, version))
            {
                return Outcome<Bic>.Failure(Strings.Parse_InvalidInstitutionCode);
            }

            string countryCode = GetCountryCode(value);
            if (!CheckCountryCode(countryCode))
            {
                return Outcome<Bic>.Failure(Strings.Parse_InvalidCountryCode);
            }

            string locationCode = GetLocationCode(value);
            if (!CheckLocationCode(locationCode))
            {
                return Outcome<Bic>.Failure(Strings.Parse_InvalidLocationCode);
            }

            string branchCode = GetBranchCode(value);
            if (!CheckBranchCode(branchCode))
            {
                return Outcome<Bic>.Failure(Strings.Parse_InvalidBranchCode);
            }

            return Outcome.Success(new Bic(institutionCode, countryCode, locationCode, branchCode, value));
        }

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            Warrant.NotNull<string>();

            return _value;
        }

        #region Validation helpers.

        [Pure]
        public static bool CheckBranchCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == 0 || (value.Length == BranchLength && IsDigitOrUpperLetter(value));
        }

        [Pure]
        public static bool CheckCountryCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == CountryLength && CountryISOCodes.TwoLetterCodeExists(value);
        }

        // The SWIFT implementation is more restrictive as it does not allow for digits in the institution code.
        [Pure]
        public static bool CheckInstitutionCode(string value, BicVersion version)
        {
            if (value == null) { return false; }
            return value.Length == PrefixLength
                && (version == BicVersion.ISO ? IsDigitOrUpperLetter(value) : IsUpperLetter(value));
        }

        [Pure]
        public static bool CheckLocationCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == SuffixLength && IsDigitOrUpperLetter(value);
        }

        [Pure]
        public static bool CheckValue(string value)
        {
            if (value == null) { return false; }
            return value.Length == PartyLength || value.Length == BicLength;
        }

        #endregion

        #region Parsing helpers.

        private static string GetBranchCode(string value)
        {
            Demand.True(CheckValue(value));
            return value.Length == PartyLength
                ? String.Empty
                : value.Substring(PrefixLength + CountryLength + SuffixLength, BranchLength);
        }

        private static string GetCountryCode(string value)
        {
            Demand.True(CheckValue(value));
            return value.Substring(PrefixLength, CountryLength);
        }

        private static string GetInstitutionCode(string value)
        {
            Demand.True(CheckValue(value));
            return value.Substring(0, PrefixLength);
        }

        private static string GetLocationCode(string value)
        {
            Demand.True(CheckValue(value));
            return value.Substring(PrefixLength + CountryLength, SuffixLength);
        }

        #endregion
    }

    // Implements the IEquatable<Bic> interface.
    public partial struct Bic
    {
        public static bool operator ==(Bic left, Bic right) => left.Equals(right);

        public static bool operator !=(Bic left, Bic right) => !left.Equals(right);

        public bool Equals(Bic other) => _value == other._value;

        public override bool Equals(object obj)
        {
            if (!(obj is Bic)) { return false; }

            return Equals((Bic)obj);
        }

        public override int GetHashCode() => _value.GetHashCode();
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
            Contract.Invariant(_institutionCode != null);
            Contract.Invariant(_locationCode != null);
            Contract.Invariant(_value != null);
        }
    }
}

#endif