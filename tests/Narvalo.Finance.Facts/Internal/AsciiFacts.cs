// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal {
#if !NO_INTERNALS_VISIBLE_TO

    using System;

    using Xunit;

    public static class AsciiFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Ascii), description) { }
        }

        internal sealed class TAttribute : TestTheoryAttribute {
            public TAttribute(string description) : base(nameof(Ascii), description) { }
        }

        [t("IsDigit(null) returns false.")]
        public static void IsDigit1() => Assert.False(Ascii.IsDigit(null));

        [t(@"IsDigit("""") returns false.")]
        public static void IsDigit2() => Assert.False(Ascii.IsDigit(String.Empty));

        [T("IsDigit() returns true for valid input.")]
        [InlineData("1")]
        [InlineData("10")]
        [InlineData("100")]
        [InlineData("1000")]
        [InlineData("10000")]
        public static void IsDigit3(string value) => Assert.True(Ascii.IsDigit(value));

        [T("IsDigit() returns false for invalid input.")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("A")]
        [InlineData("é")]
        [InlineData("'")]
        [InlineData("@")]
        [InlineData("ab")]
        [InlineData("AB")]
        [InlineData("1a2b3c")]
        [InlineData("1A2B3C")]
        [InlineData(" a")]
        [InlineData("a ")]
        [InlineData(" a ")]
        [InlineData(" 1a")]
        [InlineData("1a ")]
        [InlineData(" 1a ")]
        public static void IsDigit4(string value) => Assert.False(Ascii.IsDigit(value));

        [t("IsDigitOrUpperLetter(null) returns false.")]
        public static void IsDigitOrUpperLetter1()
            => Assert.False(Ascii.IsDigitOrUpperLetter(null));

        [t(@"IsDigitOrUpperLetter("""") returns false.")]
        public static void IsDigitOrUpperLetter2()
            => Assert.False(Ascii.IsDigitOrUpperLetter(String.Empty));

        [T("IsDigitOrUpperLetter() returns true for valid input.")]
        [InlineData("1")]
        [InlineData("10")]
        [InlineData("A")]
        [InlineData("AB")]
        [InlineData("1A2B3C")]
        public static void IsDigitOrUpperLetter3(string value)
            => Assert.True(Ascii.IsDigitOrUpperLetter(value));

        [T("IsDigitOrUpperLetter() returns false for invalid input.")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("é")]
        [InlineData("'")]
        [InlineData("@")]
        [InlineData("ab")]
        [InlineData("1a2b3c")]
        [InlineData(" a")]
        [InlineData("a ")]
        [InlineData(" a ")]
        [InlineData(" 1a")]
        [InlineData("1a ")]
        [InlineData(" 1a ")]
        public static void IsDigitOrUpperLetter4(string value)
            => Assert.False(Ascii.IsDigitOrUpperLetter(value));

        [t("IsUpperLetter(null) returns false.")]
        public static void IsUpperLetter1() => Assert.False(Ascii.IsUpperLetter(null));

        [t(@"IsUpperLetter("""") returns false.")]
        public static void IsUpperLetter2() => Assert.False(Ascii.IsUpperLetter(String.Empty));

        [T("IsUpperLetter() returns true for valid input.")]
        [InlineData("A")]
        [InlineData("AB")]
        public static void IsUpperLetter3(string value) => Assert.True(Ascii.IsUpperLetter(value));

        [T("IsUpperLetter() returns false for invalid input.")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("é")]
        [InlineData("'")]
        [InlineData("@")]
        [InlineData("ab")]
        [InlineData("1")]
        [InlineData("10")]
        [InlineData("1a2b3c")]
        [InlineData("1A2B3C")]
        [InlineData(" a")]
        [InlineData("a ")]
        [InlineData(" a ")]
        [InlineData(" A")]
        [InlineData("A ")]
        [InlineData(" A ")]
        [InlineData(" 1a")]
        [InlineData("1a ")]
        [InlineData(" 1a ")]
        public static void IsUpperLetter4(string value) => Assert.False(Ascii.IsUpperLetter(value));
    }

#endif
}
