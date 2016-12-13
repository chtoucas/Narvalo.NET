// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.Utilities.AsciiHelpers;

    public static class BicFormat
    {
        internal const int PrefixLength = 4;
        internal const int CountryLength = 2;
        internal const int SuffixLength = 2;
        internal const int BranchLength = 3;

        internal const int PartyLength = PrefixLength + CountryLength + SuffixLength;
        internal const int BicLength = PartyLength + BranchLength;

        // strict = ISO conformance, loosy = SWIFT implementation.
        // The SWIFT implementation is more restrictive than ISO as it only expects letters.
        // In real world cases, strict conformance is rarely used.
        public static bool Validate(Bic bic, bool strict)
            // NB: We do not need to check properties length.
            => (strict ? IsDigitOrUpperLetter(bic.InstitutionCode) : IsUpperLetter(bic.InstitutionCode))
                && IsUpperLetter(bic.CountryCode)
                && IsDigitOrUpperLetter(bic.LocationCode)
                && (bic.BranchCode.Length == 0 || IsDigitOrUpperLetter(bic.BranchCode));

        [Pure]
        public static bool CheckBranchCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == 0 || value.Length == BranchLength;
        }

        [Pure]
        public static bool CheckCountryCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == CountryLength;
        }

        [Pure]
        public static bool CheckInstitutionCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == PrefixLength;
        }

        [Pure]
        public static bool CheckLocationCode(string value)
        {
            if (value == null) { return false; }
            return value.Length == SuffixLength;
        }

        [Pure]
        public static bool CheckValue(string value)
        {
            if (value == null) { return false; }
            return value.Length == PartyLength || value.Length == BicLength;
        }
    }
}
