﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
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
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
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
            string value,
            bool validated)
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
            Validated = validated;
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
        public bool IsSwiftConnected => !IsSwiftTest &&  LocationCode[1] != SWIFT_NOT_CONNECTED_MARK;

        // SWIFTNet FIN network: Test & Training (T&T) service.
        public bool IsSwiftTest => LocationCode[1] == SWIFT_TT_MARK;

        public bool IsPrimaryOffice => BranchCode.Length == 0 || BranchCode == PrimaryOfficeBranchCode;

        public bool Validated { get; }

        private string DebuggerDisplay => _value;

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

            return new Bic(institutionCode, countryCode, locationCode, branchCode, value, false);
        }

        public static Bic Parse(string value)
        {
            Require.NotNull(value, nameof(value));

            if (!CheckValue(value))
            {
                throw new FormatException(Strings.Bic_InvalidFormat);
            }
            Check.True(CheckValue(value));

            return ParseCore(value, false);
        }

        public static Bic ParseExact(string value)
        {
            Expect.NotNull(value);

            return ParseExact(value, BicVersion.Default);
        }

        public static Bic ParseExact(string value, BicVersion version)
        {
            Require.NotNull(value, nameof(value));

            if (!CheckValue(value))
            {
                throw new FormatException(Strings.Bic_InvalidFormat);
            }
            Check.True(CheckValue(value));

            var bic = ParseCore(value, true);
            if (!bic.Validate(version))
            {
                throw new FormatException(Strings.Bic_InvalidFormat);
            }

            return bic;
        }

        public static Bic? TryParse(string value)
        {
            if (!CheckValue(value)) { return null; }
            Check.True(CheckValue(value));

            return ParseCore(value, false);
        }

        public static Bic? TryParseExact(string value) => TryParseExact(value, BicVersion.Default);

        public static Bic? TryParseExact(string value, BicVersion version)
        {
            if (!CheckValue(value)) { return null; }
            Check.True(CheckValue(value));

            var bic = ParseCore(value, true);
            if (!bic.Validate(version)) { return null; }

            return bic;
        }

        public static Bic? Validate(Bic bic, BicVersion version)
        {
            if (bic.Validated) { return bic; }
            if (!bic.Validate(version)) { return null; }

            return new Bic(bic.InstitutionCode, bic.CountryCode, bic.LocationCode, bic.BranchCode, bic._value, true);
        }

        // The SWIFT implementation is more restrictive than ISO as it only expects letters.
        public bool Validate(BicVersion version)
            // NB: We do not need to check properties length.
            => (version == BicVersion.ISO ? IsDigitOrUpperLetter(InstitutionCode) : IsUpperLetter(InstitutionCode))
                && IsUpperLetter(CountryCode)
                && IsDigitOrUpperLetter(LocationCode)
                && (BranchCode.Length == 0 || IsDigitOrUpperLetter(BranchCode));

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return _value;
        }

        // NB: We only perform basic validation on the input string.
        // NB: We mark the result as validated, even if we have not yet perform any validation
        //     work. The caller is in charge to do the right thing.
        private static Bic ParseCore(string value, bool validated)
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

            return new Bic(institutionCode, countryCode, locationCode, branchCode, value, validated);
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