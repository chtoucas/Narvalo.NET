// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Tests;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("FirstOrNone() guards.")]
        public static void FirstOrNone0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.FirstOrNone());
            Assert.Throws<ArgumentNullException>("this", () => nullsource.FirstOrNone(_ => true));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.FirstOrNone(default(Func<int, bool>)));
        }

        // Adapted from https://github.com/dotnet/corefx/blob/master/src/System.Linq/tests/FirstOrDefaultTests.cs

        [t("SameResultsRepeatCallsIntQuery")]
        public static void FirstOrNone1() {
            IEnumerable<int> source = Enumerable.Range(0, 0);

            var q = from x in source select x;

            Assert.Equal(q.FirstOrNone(), q.FirstOrNone());
        }

        [t("SameResultsRepeatCallsStringQuery")]
        public static void FirstOrNone2() {
            var q = from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", String.Empty }
                    where !String.IsNullOrEmpty(x)
                    select x;

            Assert.Equal(q.FirstOrNone(), q.FirstOrNone());
        }

        private static void FirstOrNone3Impl<T>() {
            T[] source = { };
            var expected = Maybe<T>.None;

            Assert.IsAssignableFrom<IList<T>>(source);

            Assert.Equal(expected, source.RunOnce().FirstOrNone());
        }

        [t("EmptyIListT")]
        public static void FirstOrNone3() {
            FirstOrNone3Impl<int>();
            FirstOrNone3Impl<string>();
            FirstOrNone3Impl<DateTime>();
            FirstOrNone3Impl<QperatorsFacts>();
        }

        [t("IListTOneElement")]
        public static void FirstOrNone4() {
            int[] source = { 5 };
            var expected = Maybe.Of(5);

            Assert.IsAssignableFrom<IList<int>>(source);

            Assert.Equal(expected, source.FirstOrNone());
        }

        //[Fact]
        //public static void IListTManyElementsFirstIsDefault() {
        //    int?[] source = { null, -10, 2, 4, 3, 0, 2 };
        //    var expected = Maybe<int>.None;

        //    Assert.IsAssignableFrom<IList<int?>>(source);

        //    Assert.Equal(expected, source.FirstOrNone());
        //}

        //[Fact]
        //public static void IListTManyElementsFirstIsNotDefault() {
        //    int?[] source = { 19, null, -10, 2, 4, 3, 0, 2 };
        //    var expected = Maybe.Of(19);

        //    Assert.IsAssignableFrom<IList<int?>>(source);

        //    Assert.Equal(expected, source.FirstOrNone());
        //}

        //private static IEnumerable<T> EmptySource<T>() {
        //    yield break;
        //}

        private static void FirstOrNone5Impl<T>() {
            var source = EmptySource<T>();
            var expected = Maybe<T>.None;

            Assert.Null(source as IList<T>);

            Assert.Equal(expected, source.RunOnce().FirstOrNone());
        }

        [t("EmptyNotIListT")]
        public static void FirstOrNone5() {
            FirstOrNone5Impl<int>();
            FirstOrNone5Impl<string>();
            FirstOrNone5Impl<DateTime>();
            FirstOrNone5Impl<QperatorsFacts>();
        }

        [t("OneElementNotIListT")]
        public static void FirstOrNone6() {
            IEnumerable<int> source = NumberRangeGuaranteedNotCollectionType(-5, 1);
            var expected = Maybe.Of(-5);

            Assert.Null(source as IList<int>);

            Assert.Equal(expected, source.FirstOrNone());
        }

        [t("ManyElementsNotIListT")]
        public static void FirstOrNone7() {
            IEnumerable<int> source = NumberRangeGuaranteedNotCollectionType(3, 10);
            var expected = Maybe.Of(3);

            Assert.Null(source as IList<int>);

            Assert.Equal(expected, source.FirstOrNone());
        }

        //[Fact]
        //public static void EmptySource() {
        //    int?[] source = { };
        //    var expected = Maybe<int>.None;

        //    Assert.Equal(expected, source.FirstOrNone(x => true));
        //    Assert.Equal(expected, source.FirstOrNone(x => false));
        //}

        [t("OneElementTruePredicate")]
        public static void FirstOrNone8() {
            int[] source = { 4 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(4);

            Assert.Equal(expected, source.FirstOrNone(predicate));
        }

        [t("ManyElementsPredicateFalseForAll")]
        public static void FirstOrNone9() {
            int[] source = { 9, 5, 1, 3, 17, 21 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.FirstOrNone(predicate));
        }

        [t("PredicateTrueOnlyForLast")]
        public static void FirstOrNone10() {
            int[] source = { 9, 5, 1, 3, 17, 21, 50 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(50);

            Assert.Equal(expected, source.FirstOrNone(predicate));
        }

        [t("PredicateTrueForSome")]
        public static void FirstOrNone11() {
            int[] source = { 3, 7, 10, 7, 9, 2, 11, 17, 13, 8 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(10);

            Assert.Equal(expected, source.FirstOrNone(predicate));
        }

        [t("PredicateTrueForSomeRunOnce")]
        public static void FirstOrNone12() {
            int[] source = { 3, 7, 10, 7, 9, 2, 11, 17, 13, 8 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(10);

            Assert.Equal(expected, source.RunOnce().FirstOrNone(predicate));
        }
    }
}
