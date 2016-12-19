// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    // Keep this class public; its methods are member of the Bic's contract.
    public static class BicFormat
    {
        internal const int PrefixLength = 4;
        internal const int CountryLength = 2;
        internal const int SuffixLength = 2;
        internal const int BranchLength = 3;

        internal const int PartyLength = PrefixLength + CountryLength + SuffixLength;
        internal const int BicLength = PartyLength + BranchLength;

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
