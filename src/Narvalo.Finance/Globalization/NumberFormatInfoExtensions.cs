// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    internal static class NumberFormatInfoExtensions
    {
        // The volatile keyword is only for correctness.
        private static volatile Dictionary<int, int> s_SpaceReversedNegativePatterns;

        private static Dictionary<int, int> SpaceReversedNegativePatterns
        {
            get
            {
                if (s_SpaceReversedNegativePatterns == null)
                {
                    var dic = new Dictionary<int, int>(16)
                    {
                        // Without space -> with space.
                        { 0, 14 },   // ($n)
                        { 1, 9 },    // -$n
                        { 2, 12 },   // $-n
                        { 3, 11 },   // $n-
                        { 4, 15 },   // (n$)
                        { 5, 8 },    // -n$
                        { 6, 13 },   // n-$
                        { 7, 10 },   // n$-
                        // With space -> without space.
                        { 8, 5 },    // -n $    => -n$  (5)
                        { 9, 1 },    // -$ n    => -$n  (1)
                        { 10, 7 },   // n $-    => n$-  (7)
                        { 11, 3 },   // $ n-    => $n-  (3)
                        { 12, 2 },   // $ -n    => $-n  (2)
                        { 13, 6 },   // n- $    => n-$  (6)
                        { 14, 0 },   // ($ n)   => ($n) (0)
                        { 15, 4 },   //(n $)    => (n$) (4)
                    };

                    s_SpaceReversedNegativePatterns = dic;
                }

                return s_SpaceReversedNegativePatterns;
            }
        }

        private static bool CurrencyPositivePatternContainsSpace(this NumberFormatInfo @this)
        {
            Demand.NotNull(@this);
            return @this.CurrencyPositivePattern >= 2;
        }
        private static bool CurrencyNegativePatternContainsSpace(this NumberFormatInfo @this)
        {
            Demand.NotNull(@this);
            return @this.CurrencyNegativePattern >= 8;
        }

        public static NumberFormatInfo GetCurrencyCodeAndSpaceClone(
            this NumberFormatInfo @this,
            string currencyCode)
        {
            Demand.NotNull(@this);
            Demand.NotNull(currencyCode);

            var nf = (NumberFormatInfo)@this.Clone();
            nf.CurrencySymbol = currencyCode;

            // If there is no space between the amount and the currency symbol, add one.
            if (!nf.CurrencyPositivePatternContainsSpace())
            {
                // 0 "$n" is mapped to 2 "$ n".
                // 1 "n$" is mapped to 3 "n $".
                nf.CurrencyPositivePattern += 2;
            }
            if (!nf.CurrencyNegativePatternContainsSpace())
            {
                nf.CurrencyNegativePattern = SpaceReversedNegativePatterns[nf.CurrencyNegativePattern];
            }

            return nf;
        }

        public static NumberFormatInfo GetNoSymbolNoSpaceClone(
            this NumberFormatInfo @this)
        {
            Demand.NotNull(@this);

            var nf = (NumberFormatInfo)@this.Clone();
            nf.CurrencySymbol = String.Empty;

            // If there is a space between the amount and the currency symbol, remove it.
            if (nf.CurrencyPositivePatternContainsSpace())
            {
                nf.CurrencyPositivePattern -= 2;
            }
            if (nf.CurrencyNegativePatternContainsSpace())
            {
                nf.CurrencyNegativePattern = SpaceReversedNegativePatterns[nf.CurrencyNegativePattern];
            }

            return nf;
        }
    }
}
