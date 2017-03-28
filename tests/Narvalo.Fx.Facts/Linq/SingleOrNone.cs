// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Tests;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("SingleOrNone() guards.")]
        public static void SingleOrNone0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.SingleOrNone());
            Assert.Throws<ArgumentNullException>("this", () => nullsource.SingleOrNone(_ => true));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.SingleOrNone(default(Func<int, bool>)));
        }

        // Adapted from https://github.com/dotnet/corefx/blob/master/src/System.Linq/tests/SingleOrDefaultTests.cs

        [Fact]
        public void SameResultsRepeatCallsIntQuery() {
            var q = from x in new[] { 0.12335f }
                    select x;

            Assert.Equal(q.SingleOrNone(), q.SingleOrNone());
        }

        [Fact(Skip = "Need work.")]
        public void SameResultsRepeatCallsStringQuery() {
            var q = from x in new[] { "" }
                    select x;

            Assert.Equal(q.SingleOrDefault(String.IsNullOrEmpty), q.SingleOrDefault(String.IsNullOrEmpty));
        }

        //[Fact]
        //public void EmptyIList() {
        //    int?[] source = { };
        //    int? expected = null;

        //    Assert.Equal(expected, source.SingleOrDefault());
        //}

        [Fact]
        public void SingleElementIList() {
            int[] source = { 4 };
            var expected = Maybe.Of(4);

            Assert.Equal(expected, source.SingleOrNone());
        }

        [Fact(Skip = "Need work.")]
        public void ManyElementIList() {
            int[] source = { 4, 4, 4, 4, 4 };

            Assert.Throws<InvalidOperationException>(() => source.SingleOrNone());
        }

        [Fact]
        public void EmptyNotIList() {
            IEnumerable<int> source = RepeatedNumberGuaranteedNotCollectionType(0, 0);
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.SingleOrNone());
        }

        [Fact]
        public void SingleElementNotIList() {
            IEnumerable<int> source = RepeatedNumberGuaranteedNotCollectionType(-5, 1);
            var expected = Maybe.Of(-5);

            Assert.Equal(expected, source.SingleOrNone());
        }

        [Fact(Skip = "Need work.")]
        public void ManyElementNotIList() {
            IEnumerable<int> source = RepeatedNumberGuaranteedNotCollectionType(3, 5);

            Assert.Throws<InvalidOperationException>(() => source.SingleOrNone());
        }

        [Fact]
        public void EmptySourceWithPredicate() {
            int[] source = { };
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.SingleOrNone(i => i % 2 == 0));
        }

        [Fact]
        public void SingleElementPredicateTrue() {
            int[] source = { 4 };
            var expected = Maybe.Of(4);

            Assert.Equal(expected, source.SingleOrNone(i => i % 2 == 0));
        }

        [Fact]
        public void SingleElementPredicateFalse() {
            int[] source = { 3 };
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.SingleOrNone(i => i % 2 == 0));
        }

        [Fact]
        public void ManyElementsPredicateFalseForAll() {
            int[] source = { 3, 1, 7, 9, 13, 19 };
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.SingleOrNone(i => i % 2 == 0));
        }

        [Fact]
        public void ManyElementsPredicateTrueForLast() {
            int[] source = { 3, 1, 7, 9, 13, 19, 20 };
            var expected = Maybe.Of(20);

            Assert.Equal(expected, source.SingleOrNone(i => i % 2 == 0));
        }

        [Fact(Skip = "Need work.")]
        public void ManyElementsPredicateTrueForFirstAndFifth() {
            int[] source = { 2, 3, 1, 7, 10, 13, 19, 9 };

            Assert.Throws<InvalidOperationException>(() => source.SingleOrNone(i => i % 2 == 0));
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(42, 100)]
        public void FindSingleMatch(int target, int range) {
            var expected = Maybe.Of(target);
            Assert.Equal(expected, Enumerable.Range(0, range).SingleOrNone(i => i == target));
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(42, 100)]
        public void RunOnce(int target, int range) {
            var expected = Maybe.Of(target);
            Assert.Equal(expected, Enumerable.Range(0, range).RunOnce().SingleOrNone(i => i == target));
        }
    }
}
