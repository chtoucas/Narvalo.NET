// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Tests;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("ElementAtOrNone() guards.")]
        public static void ElementAtOrNone0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.ElementAtOrNone(1));
        }

        // Adapted from https://github.com/dotnet/corefx/blob/master/src/System.Linq/tests/ElementAtOrDefaultTests.cs

        [t("SameResultsRepeatCallsIntQuery")]
        public void ElementAtOrNone1() {
            var q = from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
                    where x > Int32.MinValue
                    select x;

            Assert.Equal(q.ElementAtOrNone(3), q.ElementAtOrNone(3));
        }

        [t("SameResultsRepeatCallsStringQuery")]
        public void ElementAtOrNone2() {
            var q = from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", string.Empty }
                    where !String.IsNullOrEmpty(x)
                    select x;

            Assert.Equal(q.ElementAtOrNone(4), q.ElementAtOrNone(4));
        }

        public static IEnumerable<object[]> TestData() {
            yield return new object[] { NumberRangeGuaranteedNotCollectionType(9, 1), 0, Maybe.Of(9) };
            yield return new object[] { NumberRangeGuaranteedNotCollectionType(9, 10), 9, Maybe.Of(18) };
            yield return new object[] { NumberRangeGuaranteedNotCollectionType(-4, 10), 3, Maybe.Of(-1) };

            yield return new object[] { new int[] { 1, 2, 3, 4 }, 4, Maybe<int>.None };
            yield return new object[] { new int[0], 0, Maybe<int>.None };
            yield return new object[] { new int[] { -4 }, 0, Maybe.Of(-4) };
            yield return new object[] { new int[] { 9, 8, 0, -5, 10 }, 4, Maybe.Of(10) };

            yield return new object[] { NumberRangeGuaranteedNotCollectionType(-4, 5), -1, Maybe<int>.None };
            yield return new object[] { NumberRangeGuaranteedNotCollectionType(5, 5), 5, Maybe<int>.None };
            yield return new object[] { NumberRangeGuaranteedNotCollectionType(0, 0), 0, Maybe<int>.None };
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void ElementAtOrDefault(IEnumerable<int> source, int index, Maybe<int> expected) {
            Assert.Equal(expected, source.ElementAtOrNone(index));
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void ElementAtOrDefaultRunOnce(IEnumerable<int> source, int index, Maybe<int> expected) {
            Assert.Equal(expected, source.RunOnce().ElementAtOrNone(index));
        }

        [Fact(Skip = "Need work")]
        public void NullableArray_NegativeIndex_ReturnsNull() {
            int?[] source = { 9, 8 };
            Assert.Null(source.ElementAtOrDefault(-1));
        }

        [Fact(Skip = "Need work")]
        public void NullableArray_ValidIndex_ReturnsCorrectObjecvt() {
            int?[] source = { 9, 8, null, -5, 10 };

            Assert.Null(source.ElementAtOrDefault(2));
            Assert.Equal(-5, source.ElementAtOrDefault(3));
        }
    }
}
