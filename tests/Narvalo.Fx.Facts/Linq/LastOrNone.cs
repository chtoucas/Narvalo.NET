// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Tests;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("LastOrNone() guards.")]
        public static void LastOrNone0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.LastOrNone());
            Assert.Throws<ArgumentNullException>("this", () => nullsource.LastOrNone(_ => true));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.LastOrNone(default(Func<int, bool>)));
        }

        // Adapted from https://github.com/dotnet/corefx/blob/master/src/System.Linq/tests/LastOrDefaultTests.cs

        [t("SameResultsRepeatCallsIntQuery")]
        public void LastOrNone1() {
            var q = from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
                    where x > Int32.MinValue
                    select x;

            Assert.Equal(q.LastOrNone(), q.LastOrNone());
        }

        [t("SameResultsRepeatCallsStringQuery")]
        public void LastOrNone2() {
            var q = from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", String.Empty }
                    where !String.IsNullOrEmpty(x)
                    select x;

            Assert.Equal(q.LastOrNone(), q.LastOrNone());
        }

        private static void LastOrNone3Impl<T>() {
            T[] source = { };
            var expected = Maybe<T>.None;

            Assert.IsAssignableFrom<IList<T>>(source);

            Assert.Equal(expected, source.RunOnce().LastOrNone());
        }

        [t("EmptyIListT")]
        public void LastOrNone3() {
            LastOrNone3Impl<int>();
            LastOrNone3Impl<string>();
            LastOrNone3Impl<DateTime>();
            LastOrNone3Impl<QperatorsFacts>();
        }

        [t("IListTOneElement")]
        public void LastOrNone4() {
            int[] source = { 5 };
            var expected = Maybe.Of(5);

            Assert.IsAssignableFrom<IList<int>>(source);

            Assert.Equal(expected, source.LastOrNone());
        }

        //[Fact]
        //public void IListTManyElementsLastIsDefault() {
        //    int?[] source = { -10, 2, 4, 3, 0, 2, null };
        //    int? expected = null;

        //    Assert.IsAssignableFrom<IList<int?>>(source);

        //    Assert.Equal(expected, source.LastOrDefault());
        //}

        //[Fact]
        //public void IListTManyElementsLastIsNotDefault() {
        //    int?[] source = { -10, 2, 4, 3, 0, 2, null, 19 };
        //    int? expected = 19;

        //    Assert.IsAssignableFrom<IList<int?>>(source);

        //    Assert.Equal(expected, source.LastOrDefault());
        //}

        //private static IEnumerable<T> EmptySource<T>() {
        //    yield break;
        //}

        private static void LastOrNone5Impl<T>() {
            var source = EmptySource<T>();
            var expected = Maybe<T>.None;

            Assert.Null(source as IList<T>);

            Assert.Equal(expected, source.RunOnce().LastOrNone());
        }

        [t("EmptyNotIListT")]
        public void LastOrNone5() {
            LastOrNone5Impl<int>();
            LastOrNone5Impl<string>();
            LastOrNone5Impl<DateTime>();
            LastOrNone5Impl<QperatorsFacts>();
        }

        [t("OneElementNotIListT")]
        public void LastOrNone6() {
            IEnumerable<int> source = NumberRangeGuaranteedNotCollectionType(-5, 1);
            var expected = Maybe.Of(-5);

            Assert.Null(source as IList<int>);

            Assert.Equal(expected, source.LastOrNone());
        }

        [t("ManyElementsNotIListT")]
        public void LastOrNone7() {
            IEnumerable<int> source = NumberRangeGuaranteedNotCollectionType(3, 10);
            var expected = Maybe.Of(12);

            Assert.Null(source as IList<int>);

            Assert.Equal(expected, source.LastOrNone());
        }

        //[Fact]
        //public void EmptyIListSource() {
        //    int?[] source = { };

        //    Assert.Null(source.LastOrDefault(x => true));
        //    Assert.Null(source.LastOrDefault(x => false));
        //}

        [t("OneElementIListTruePredicate")]
        public void LastOrNone8() {
            int[] source = { 4 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(4);

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("ManyElementsIListPredicateFalseForAll")]
        public void LastOrNone9() {
            int[] source = { 9, 5, 1, 3, 17, 21 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("IListPredicateTrueOnlyForLast")]
        public void LastOrNone10() {
            int[] source = { 9, 5, 1, 3, 17, 21, 50 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(50);

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("IListPredicateTrueForSome")]
        public void LastOrNone11() {
            int[] source = { 3, 7, 10, 7, 9, 2, 11, 18, 13, 9 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(18);

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("IListPredicateTrueForSomeRunOnce")]
        public void LastOrNone12() {
            int[] source = { 3, 7, 10, 7, 9, 2, 11, 18, 13, 9 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(18);

            Assert.Equal(expected, source.RunOnce().LastOrNone(predicate));
        }

        //[Fact]
        //public void EmptyNotIListSource() {
        //    IEnumerable<int?> source = Enumerable.Repeat((int?)4, 0);

        //    Assert.Null(source.LastOrDefault(x => true));
        //    Assert.Null(source.LastOrDefault(x => false));
        //}

        [t("OneElementNotIListTruePredicate")]
        public void LastOrNone13() {
            IEnumerable<int> source = ForceNotCollection(new[] { 4 });
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(4);

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("ManyElementsNotIListPredicateFalseForAll")]
        public void LastOrNone14() {
            IEnumerable<int> source = ForceNotCollection(new int[] { 9, 5, 1, 3, 17, 21 });
            Func<int, bool> predicate = IsEven;
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("NotIListPredicateTrueOnlyForLast")]
        public void LastOrNone15() {
            IEnumerable<int> source = ForceNotCollection(new int[] { 9, 5, 1, 3, 17, 21, 50 });
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(50);

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("NotIListPredicateTrueForSome")]
        public void LastOrNone16() {
            IEnumerable<int> source = ForceNotCollection(new int[] { 3, 7, 10, 7, 9, 2, 11, 18, 13, 9 });
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(18);

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("NotIListPredicateTrueForSomeRunOnce")]
        public void LastOrNone17() {
            IEnumerable<int> source = ForceNotCollection(new int[] { 3, 7, 10, 7, 9, 2, 11, 18, 13, 9 });
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(18);

            Assert.Equal(expected, source.RunOnce().LastOrNone(predicate));
        }
    }
}
