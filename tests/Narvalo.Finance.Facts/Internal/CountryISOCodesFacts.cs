// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal {
#if !NO_INTERNALS_VISIBLE_TO

    using System;

    using Xunit;

    public static class CountryISOCodesFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(CountryISOCodes), description) { }
        }

        internal sealed class TAttribute : TestTheoryAttribute {
            public TAttribute(string description) : base(nameof(CountryISOCodes), description) { }
        }

        [t("TwoLetterCodeExists(null) returns false.")]
        public static void TwoLetterCodeExists1()
            => Assert.False(CountryISOCodes.TwoLetterCodeExists(null));

        [t(@"TwoLetterCodeExists("""") returns false.")]
        public static void TwoLetterCodeExists2()
            => Assert.False(CountryISOCodes.TwoLetterCodeExists(String.Empty));

        [T("TwoLetterCodeExists() returns false if the input has an invalid length.")]
        [InlineData("A")]
        [InlineData("ABC")]
        [InlineData("ABCD")]
        public static void TwoLetterCodeExists3(string value)
            => Assert.False(CountryISOCodes.TwoLetterCodeExists(value));

        [t("TwoLetterCodeExists() returns false if the code does not exist.")]
        public static void TwoLetterCodeExists4()
            => Assert.False(CountryISOCodes.TwoLetterCodeExists("ZZ"));

        [t("TwoLetterCodeExists() returns true if the code does exist.")]
        public static void TwoLetterCodeExists5()
            => Assert.True(CountryISOCodes.TwoLetterCodeExists("FR"));
    }

#endif
}
