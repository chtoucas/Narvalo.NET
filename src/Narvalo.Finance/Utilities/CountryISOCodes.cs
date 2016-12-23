// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System.Collections.Generic;

    // Only covers ISO 3166-1 alpha-2 codes.
    // REVIEW: Should we manually include the deleted codes?
    internal static partial class CountryISOCodes
    {
        // The list is automatically generated using the data obtained from the ISO website.
        private static volatile HashSet<string> s_TwoLetterCodeSet;

        public static bool TwoLetterCodeExists(string code)
        {
            // Fast track.
            if (code == null || code.Length != 2) { return false; }

            return TwoLetterCodeSet.Contains(code);
        }
    }
}
