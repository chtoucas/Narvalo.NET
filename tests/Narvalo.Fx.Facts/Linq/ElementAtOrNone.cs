// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Tests;

    using Narvalo.Applicative;
    using Xunit;

    // Largely inspired by
    // https://github.com/dotnet/corefx/blob/master/src/System.Linq/tests/ElementAtOrDefaultTests.cs
    public partial class QperatorsFacts {
        [t("ElementAtOrNone() guards.")]
        public static void ElementAtOrNone0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.ElementAtOrNone(1));
        }

        [t("ElementAtOrNone() for int's returns the same result when called repeatedly.")]
        public static void ElementAtOrNone1() {
            var q = from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
                    where x > Int32.MinValue
                    select x;

            Assert.Equal(q.ElementAtOrNone(3), q.ElementAtOrNone(3));
        }

        [t("ElementAtOrNone() for string's returns the same result when called repeatedly.")]
        public static void ElementAtOrNone2() {
            var q = from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", String.Empty }
                    where !String.IsNullOrEmpty(x)
                    select x;

            Assert.Equal(q.ElementAtOrNone(4), q.ElementAtOrNone(4));
        }

        [T("ElementAtOrDefault")]
        [MemberData(nameof(ElementAtOrNoneData))]
        public static void ElementAtOrNone3(IEnumerable<int> source, int index, Maybe<int> expected) {
            Assert.Equal(expected, source.ElementAtOrNone(index));
        }

        [T("ElementAtOrDefaultRunOnce")]
        [MemberData(nameof(ElementAtOrNoneData))]
        public static void ElementAtOrNone4(IEnumerable<int> source, int index, Maybe<int> expected) {
            Assert.Equal(expected, source.RunOnce().ElementAtOrNone(index));
        }

        [t("NullableArray_NegativeIndex_ReturnsNull")]
        public static void ElementAtOrNone5() {
            string[] source = { "a", "b" };
            Assert.Equal(Maybe<string>.None, source.ElementAtOrNone(-1));
        }

        [t("NullableArray_ValidIndex_ReturnsCorrectObjecvt")]
        public static void ElementAtOrNone6() {
            string[] source = { "a", "b", null, "d", "e" };

            Assert.Equal(Maybe<string>.None, source.ElementAtOrNone(2));
            Assert.Equal(Maybe.Of("d"), source.ElementAtOrNone(3));
        }
    }

    public partial class QperatorsFacts {
        public static IEnumerable<object[]> ElementAtOrNoneData {
            get {
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
        }
    }
}
