// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Tests;

    using Narvalo.Applicative;
    using Xunit;

    // Largely inspired by
    // https://github.com/dotnet/corefx/blob/master/src/System.Linq/tests/SingleOrDefaultTests.cs
    public partial class QperatorsFacts {
        [t("SingleOrNone() guards.")]
        public static void SingleOrNone0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.SingleOrNone());
            Assert.Throws<ArgumentNullException>("this", () => nullsource.SingleOrNone(_ => true));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.SingleOrNone(default(Func<int, bool>)));
        }

        [t("SingleOrNone() for int's returns the same result when called repeatedly.")]
        public static void SingleOrNone1() {
            var q = from x in new[] { 0.12335f }
                    select x;

            Assert.Equal(q.SingleOrNone(), q.SingleOrNone());
        }

        [t("SingleOrNone() for string's returns the same result when called repeatedly.")]
        public static void SingleOrNone2() {
            var q = from x in new[] { "" }
                    select x;

            Assert.Equal(q.SingleOrNone(String.IsNullOrEmpty), q.SingleOrNone(String.IsNullOrEmpty));
        }

        [t("EmptyIList")]
        public static void SingleOrNone3() {
            string[] source = { };
            var expected = Maybe<string>.None;

            Assert.Equal(expected, source.SingleOrNone());
        }

        [t("SingleElementIList")]
        public static void SingleOrNone4() {
            int[] source = { 4 };
            var expected = Maybe.Of(4);

            Assert.Equal(expected, source.SingleOrNone());
        }

        [t("ManyElementIList")]
        public static void SingleOrNone5() {
            int[] source = { 4, 4, 4, 4, 4 };
            var expected = Maybe<int>.None;

            // NB: SingleOrDefault() throws InvalidOperationException.
            Assert.Equal(expected, source.SingleOrNone());
        }

        [t("EmptyNotIList")]
        public static void SingleOrNone6() {
            IEnumerable<int> source = RepeatedNumberGuaranteedNotCollectionType(0, 0);
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.SingleOrNone());
        }

        [t("SingleElementNotIList")]
        public static void SingleOrNone7() {
            IEnumerable<int> source = RepeatedNumberGuaranteedNotCollectionType(-5, 1);
            var expected = Maybe.Of(-5);

            Assert.Equal(expected, source.SingleOrNone());
        }

        [t("ManyElementNotIList")]
        public static void SingleOrNone8() {
            IEnumerable<int> source = RepeatedNumberGuaranteedNotCollectionType(3, 5);
            var expected = Maybe<int>.None;

            // NB: SingleOrDefault() throws InvalidOperationException.
            Assert.Equal(expected, source.SingleOrNone());
        }

        [t("EmptySourceWithPredicate")]
        public static void SingleOrNone9() {
            int[] source = { };
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.SingleOrNone(i => i % 2 == 0));
        }

        [t("SingleElementPredicateTrue")]
        public static void SingleOrNone10() {
            int[] source = { 4 };
            var expected = Maybe.Of(4);

            Assert.Equal(expected, source.SingleOrNone(i => i % 2 == 0));
        }

        [t("SingleElementPredicateFalse")]
        public static void SingleOrNone11() {
            int[] source = { 3 };
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.SingleOrNone(i => i % 2 == 0));
        }

        [t("ManyElementsPredicateFalseForAll")]
        public static void SingleOrNone12() {
            int[] source = { 3, 1, 7, 9, 13, 19 };
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.SingleOrNone(i => i % 2 == 0));
        }

        [t("ManyElementsPredicateTrueForLast")]
        public static void SingleOrNone13() {
            int[] source = { 3, 1, 7, 9, 13, 19, 20 };
            var expected = Maybe.Of(20);

            Assert.Equal(expected, source.SingleOrNone(i => i % 2 == 0));
        }

        [t("ManyElementsPredicateTrueForFirstAndFifth")]
        public static void SingleOrNone14() {
            int[] source = { 2, 3, 1, 7, 10, 13, 19, 9 };
            var expected = Maybe<int>.None;

            // NB: SingleOrDefault() throws InvalidOperationException.
            Assert.Equal(expected, source.SingleOrNone(i => i % 2 == 0));
        }

        [T("FindSingleMatch")]
        [InlineData(1, 100)]
        [InlineData(42, 100)]
        public static void SingleOrNone15(int target, int range) {
            var expected = Maybe.Of(target);
            Assert.Equal(expected, Enumerable.Range(0, range).SingleOrNone(i => i == target));
        }

        [T("RunOnce")]
        [InlineData(1, 100)]
        [InlineData(42, 100)]
        public static void SingleOrNone16(int target, int range) {
            var expected = Maybe.Of(target);
            Assert.Equal(expected, Enumerable.Range(0, range).RunOnce().SingleOrNone(i => i == target));
        }
    }
}
