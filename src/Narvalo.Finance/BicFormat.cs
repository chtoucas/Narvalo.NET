// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.AsciiHelpers;

    public static class BicFormat
    {
        internal const int BicLength = 11;
        internal const int PartyLength = 8;
        internal const int PrefixLength = 4;
        internal const int CountryLength = 2;
        internal const int SuffixLength = 2;
        internal const int BranchLength = 3;

        // conforming = ISO conformance.
        public static bool Check(Bic bic, bool conforming)
            // NB: We do not need to check properties length.
            => (conforming ? IsDigitOrUpperLetter(bic.InstitutionCode) : IsUpperLetter(bic.InstitutionCode))
                && IsUpperLetter(bic.CountryCode)
                && IsDigitOrUpperLetter(bic.LocationCode)
                && (bic.BranchCode.Length == 0 ? true : IsDigitOrUpperLetter(bic.BranchCode));

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
        public static bool CheckInnerValue(string value)
        {
            if (value == null) { return false; }

            return value.Length == PartyLength || value.Length == BicLength;
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
    }
}
