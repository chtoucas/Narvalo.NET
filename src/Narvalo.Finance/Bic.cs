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

        internal const int PartyLength = InstitutionPart.Length + CountryPart.Length + LocationPart.Length;
        internal const int BicLength = PartyLength + BranchPart.Length;

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

            if (ValidateInstitutionCode(institutionCode, version)
                && ValidateCountryCode(countryCode)
                && ValidateLocationCode(locationCode)
                && ValidateBranchCode(branchCode))
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

        public static Outcome<Bic> TryParse(string value)
        {
            Expect.NotNull(value);

            return TryParse(value, BicVersion.Default);
        }

        public static Outcome<Bic> TryParse(string value, BicVersion version)
        {
            Require.NotNull(value, nameof(value));

            if (!CheckLength(value))
            {
                return Outcome<Bic>.Failure(Strings.Parse_InvalidBicValue);
            }

            string institutionCode = InstitutionPart.FromBic(value, version);
            if (institutionCode == null)
            {
                return Outcome<Bic>.Failure(Strings.Parse_InvalidInstitutionCode);
            }

            string countryCode = CountryPart.FromBic(value);
            if (countryCode == null)
            {
                return Outcome<Bic>.Failure(Strings.Parse_InvalidCountryCode);
            }

            string locationCode = LocationPart.FromBic(value);
            if (locationCode == null)
            {
                return Outcome<Bic>.Failure(Strings.Parse_InvalidLocationCode);
            }

            string branchCode = BranchPart.FromBic(value);
            if (branchCode == null)
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

        [Pure]
        public static bool CheckLength(string value)
        {
            if (value == null) { return false; }
            return value.Length == PartyLength || value.Length == BicLength;
        }

        [Pure]
        public static bool ValidateBranchCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == 0 || (value.Length == BranchPart.Length && BranchPart.Check(value));
        }

        [Pure]
        public static bool ValidateCountryCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == CountryPart.Length && CountryPart.Check(value);
        }

        [Pure]
        public static bool ValidateInstitutionCode(string value, BicVersion version)
        {
            if (value == null) { return false; }
            return value.Length == InstitutionPart.Length && InstitutionPart.Check(value, version);
        }

        [Pure]
        public static bool ValidateLocationCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == LocationPart.Length && LocationPart.Check(value);
        }

        private static class BranchPart
        {
            public const int Length = 3;

            public static string FromBic(string value)
            {
                Demand.True(CheckLength(value));

                if (value.Length == PartyLength)
                {
                    return String.Empty;
                }
                else
                {
                    var retval = value.Substring(
                        InstitutionPart.Length + CountryPart.Length + LocationPart.Length, Length);

                    return Check(retval) ? retval : null;
                }
            }

            public static bool Check(string value) => IsDigitOrUpperLetter(value);
        }

        private static class CountryPart
        {
            public const int Length = 2;

            public static string FromBic(string value)
            {
                Demand.True(CheckLength(value));

                var retval = value.Substring(InstitutionPart.Length, Length);

                return Check(retval) ? retval : null;
            }

            public static bool Check(string value) => CountryISOCodes.TwoLetterCodeExists(value);
        }

        private static class InstitutionPart
        {
            internal const int Length = 4;

            public static string FromBic(string value, BicVersion version)
            {
                Demand.True(CheckLength(value));

                var retval = value.Substring(0, Length);

                return Check(retval, version) ? retval : null;
            }

            // The SWIFT implementation is more restrictive as it does not allow for digits in the institution code.
            public static bool Check(string value, BicVersion version)
                => version == BicVersion.ISO ? IsDigitOrUpperLetter(value) : IsUpperLetter(value);
        }

        private static class LocationPart
        {
            internal const int Length = 2;

            public static string FromBic(string value)
            {
                Demand.True(CheckLength(value));

                var retval = value.Substring(InstitutionPart.Length + CountryPart.Length, Length);

                return Check(retval) ? retval : null;
            }

            public static bool Check(string value) => IsDigitOrUpperLetter(value);
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