// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Globalization;
    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

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

        internal const int PartyLength = InstitutionPart.Length + CountryPart.Length + LocationPart.Length;
        internal const int BicLength = PartyLength + BranchPart.Length;

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
        { }

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
        /// <remarks>As of ISO 9362:2014, this is also the Business party prefix.</remarks>
        public string InstitutionCode { get; }

        // Connected to the SWIFTNet FIN network?
        public bool IsSwiftConnected => !IsSwiftTest && LocationCode[1] != SWIFT_NOT_CONNECTED_MARK;

        // SWIFTNet FIN network: Test & Training (T&T) service.
        public bool IsSwiftTest => LocationCode[1] == SWIFT_TT_MARK;

        public bool IsPrimaryOffice => BranchCode.Length == 0 || BranchCode == PrimaryOfficeBranchCode;

        /// <summary>
        /// Gets the location code.
        /// </summary>
        /// <remarks>As of ISO 9362:2014, this is also the Business party suffix.</remarks>
        public string LocationCode { get; }

        public static Bic Create(
            string institutionCode,
            string countryCode,
            string locationCode,
            string branchCode)
            => Create(institutionCode, countryCode, locationCode, branchCode, BicVersion.Default);

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
            Require.True(InstitutionPart.Validate(institutionCode, version), nameof(institutionCode));
            Require.True(CountryPart.Validate(countryCode), nameof(countryCode));
            Require.True(LocationPart.Validate(locationCode), nameof(locationCode));
            Require.True(BranchPart.Validate(branchCode), nameof(branchCode));

            return new Bic(institutionCode, countryCode, locationCode, branchCode);
        }

        public static Bic? Parse(string value) => Parse(value, BicVersion.Default);

        public static Bic? Parse(string value, BicVersion version)
        {
            if (!CheckLength(value)) { return null; }

            string institutionCode = InstitutionPart.FromBic(value, version);
            if (institutionCode == null) { return null; }

            string countryCode = CountryPart.FromBic(value);
            if (countryCode == null) { return null; }

            string locationCode = LocationPart.FromBic(value);
            if (locationCode == null) { return null; }

            string branchCode = BranchPart.FromBic(value);
            if (branchCode == null) { return null; }

            return new Bic(institutionCode, countryCode, locationCode, branchCode, value);
        }

        public static Result<Bic> TryParse(string value)
            => TryParse(value, BicVersion.Default);

        public static Result<Bic> TryParse(string value, BicVersion version)
        {
            if (!CheckLength(value)) { return Result<Bic>.FromError(Strings.Parse_InvalidBicValue); }

            string institutionCode = InstitutionPart.FromBic(value, version);
            if (institutionCode == null) { return Result<Bic>.FromError(Strings.Parse_InvalidInstitutionCode); }

            string countryCode = CountryPart.FromBic(value);
            if (countryCode == null) { return Result<Bic>.FromError(Strings.Parse_InvalidCountryCode); }

            string locationCode = LocationPart.FromBic(value);
            if (locationCode == null) { return Result<Bic>.FromError(Strings.Parse_InvalidLocationCode); }

            string branchCode = BranchPart.FromBic(value);
            if (branchCode == null) { return Result<Bic>.FromError(Strings.Parse_InvalidBranchCode); }

            return Result.Of(new Bic(institutionCode, countryCode, locationCode, branchCode, value));
        }

        public override string ToString() => _value;

        private static bool CheckLength(string value)
            => value != null && (value.Length == PartyLength || value.Length == BicLength);

        private static class BranchPart
        {
            public const int StartIndex = PartyLength;
            public const int Length = 3;

            public static string FromBic(string value)
            {
                Demand.True(value.Length == StartIndex || value.Length >= StartIndex + Length);

                if (value.Length == PartyLength)
                {
                    return String.Empty;
                }
                else
                {
                    var retval = value.Substring(StartIndex, Length);
                    return CheckContent(retval) ? retval : null;
                }
            }

            public static bool Validate(string value)
            {
                Demand.NotNull(value);
                return value.Length == 0 || (value.Length == Length && CheckContent(value));
            }

            private static bool CheckContent(string value) => Ascii.IsDigitOrUpperLetter(value);
        }

        private static class CountryPart
        {
            public const int StartIndex = InstitutionPart.Length;
            public const int Length = 2;

            public static string FromBic(string value)
            {
                Demand.True(value.Length >= StartIndex + Length);
                var retval = value.Substring(StartIndex, Length);
                return CheckContent(retval) ? retval : null;
            }

            public static bool Validate(string value)
            {
                Demand.NotNull(value);
                return value.Length == Length && CheckContent(value);
            }

            private static bool CheckContent(string value) => CountryISOCodes.TwoLetterCodeExists(value);
        }

        private static class InstitutionPart
        {
            public const int StartIndex = 0;
            public const int Length = 4;

            public static string FromBic(string value, BicVersion version)
            {
                var retval = value.Substring(StartIndex, Length);

                return CheckContent(retval, version) ? retval : null;
            }

            public static bool Validate(string value, BicVersion version)
            {
                Demand.NotNull(value);
                return value.Length == Length && CheckContent(value, version);
            }

            // The SWIFT implementation is more restrictive as it does not allow for digits in the institution code.
            private static bool CheckContent(string value, BicVersion version)
                => version == BicVersion.ISO ? Ascii.IsDigitOrUpperLetter(value) : Ascii.IsUpperLetter(value);
        }

        private static class LocationPart
        {
            public const int StartIndex = InstitutionPart.Length + CountryPart.Length;
            public const int Length = 2;

            public static string FromBic(string value)
            {
                Demand.True(value.Length >= StartIndex + Length);
                var retval = value.Substring(StartIndex, Length);
                return CheckContent(retval) ? retval : null;
            }

            public static bool Validate(string value)
            {
                Demand.NotNull(value);
                return value.Length == Length && CheckContent(value);
            }

            private static bool CheckContent(string value) => Ascii.IsDigitOrUpperLetter(value);
        }
    }

    // Implements the IEquatable<Bic> interface.
    public partial struct Bic
    {
        public static bool operator ==(Bic left, Bic right) => left.Equals(right);

        public static bool operator !=(Bic left, Bic right) => !left.Equals(right);

        public bool Equals(Bic other) => _value == other._value;

        public override bool Equals(object obj) => (obj is Bic) && Equals((Bic)obj);

        public override int GetHashCode() => _value.GetHashCode();
    }
}
