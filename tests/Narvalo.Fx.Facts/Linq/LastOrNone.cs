// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Tests;

    using Narvalo.Applicative;
    using Xunit;

    // Largely inspired by
    // https://github.com/dotnet/corefx/blob/master/src/System.Linq/tests/LastOrDefaultTests.cs
    public partial class QperatorsFacts {
        [t("LastOrNone() guards.")]
        public static void LastOrNone0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.LastOrNone());
            Assert.Throws<ArgumentNullException>("this", () => nullsource.LastOrNone(_ => true));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.LastOrNone(default(Func<int, bool>)));
        }

        [t("LastOrNone() for int's returns the same result when called repeatedly.")]
        public static void LastOrNone1() {
            var q = from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
                    where x > Int32.MinValue
                    select x;

            Assert.Equal(q.LastOrNone(), q.LastOrNone());
        }

        [t("LastOrNone() for string's returns the same result when called repeatedly.")]
        public static void LastOrNone2() {
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
        public static void LastOrNone3() {
            LastOrNone3Impl<int>();
            LastOrNone3Impl<string>();
            LastOrNone3Impl<DateTime>();
            LastOrNone3Impl<QperatorsFacts>();
        }

        [t("IListTOneElement")]
        public static void LastOrNone4() {
            int[] source = { 5 };
            var expected = Maybe.Of(5);

            Assert.IsAssignableFrom<IList<int>>(source);
            Assert.Equal(expected, source.LastOrNone());
        }

        [t("IListTManyElementsLastIsDefault")]
        public static void LastOrNone5() {
            string[] source = { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", null };
            var expected = Maybe<string>.None;

            Assert.IsAssignableFrom<IList<string>>(source);
            Assert.Equal(expected, source.LastOrNone());
        }

        [t("IListTManyElementsLastIsNotDefault")]
        public static void LastOrNone6() {
            string[] source = { "!@#$%^", "C", "AAA", "", "Calling Twice", null, "SoS" };
            var expected = Maybe.Of("SoS");

            Assert.IsAssignableFrom<IList<string>>(source);
            Assert.Equal(expected, source.LastOrNone());
        }

        private static void LastOrNone7Impl<T>() {
            var source = EmptySource<T>();
            var expected = Maybe<T>.None;

            Assert.Null(source as IList<T>);
            Assert.Equal(expected, source.RunOnce().LastOrNone());
        }

        [t("EmptyNotIListT")]
        public static void LastOrNone7() {
            LastOrNone7Impl<int>();
            LastOrNone7Impl<string>();
            LastOrNone7Impl<DateTime>();
            LastOrNone7Impl<QperatorsFacts>();
        }

        [t("OneElementNotIListT")]
        public static void LastOrNone8() {
            IEnumerable<int> source = NumberRangeGuaranteedNotCollectionType(-5, 1);
            var expected = Maybe.Of(-5);

            Assert.Null(source as IList<int>);
            Assert.Equal(expected, source.LastOrNone());
        }

        [t("ManyElementsNotIListT")]
        public static void LastOrNone9() {
            IEnumerable<int> source = NumberRangeGuaranteedNotCollectionType(3, 10);
            var expected = Maybe.Of(12);

            Assert.Null(source as IList<int>);
            Assert.Equal(expected, source.LastOrNone());
        }

        [t("EmptyIListSource")]
        public static void LastOrNone10() {
            string[] source = { };

            Assert.Equal(Maybe<string>.None, source.LastOrNone(x => true));
            Assert.Equal(Maybe<string>.None, source.LastOrNone(x => false));
        }

        [t("OneElementIListTruePredicate")]
        public static void LastOrNone11() {
            int[] source = { 4 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(4);

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("ManyElementsIListPredicateFalseForAll")]
        public static void LastOrNone12() {
            int[] source = { 9, 5, 1, 3, 17, 21 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("IListPredicateTrueOnlyForLast")]
        public static void LastOrNone13() {
            int[] source = { 9, 5, 1, 3, 17, 21, 50 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(50);

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("IListPredicateTrueForSome")]
        public static void LastOrNone14() {
            int[] source = { 3, 7, 10, 7, 9, 2, 11, 18, 13, 9 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(18);

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("IListPredicateTrueForSomeRunOnce")]
        public static void LastOrNone15() {
            int[] source = { 3, 7, 10, 7, 9, 2, 11, 18, 13, 9 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(18);

            Assert.Equal(expected, source.RunOnce().LastOrNone(predicate));
        }

        [t("EmptyNotIListSource")]
        public static void LastOrNone16() {
            IEnumerable<string> source = Enumerable.Repeat("value", 0);

            Assert.Equal(Maybe<string>.None, source.LastOrNone(x => true));
            Assert.Equal(Maybe<string>.None, source.LastOrNone(x => false));
        }

        [t("OneElementNotIListTruePredicate")]
        public static void LastOrNone17() {
            IEnumerable<int> source = ForceNotCollection(new[] { 4 });
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(4);

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("ManyElementsNotIListPredicateFalseForAll")]
        public static void LastOrNone18() {
            IEnumerable<int> source = ForceNotCollection(new int[] { 9, 5, 1, 3, 17, 21 });
            Func<int, bool> predicate = IsEven;
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("NotIListPredicateTrueOnlyForLast")]
        public static void LastOrNone19() {
            IEnumerable<int> source = ForceNotCollection(new int[] { 9, 5, 1, 3, 17, 21, 50 });
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(50);

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("NotIListPredicateTrueForSome")]
        public static void LastOrNone20() {
            IEnumerable<int> source = ForceNotCollection(new int[] { 3, 7, 10, 7, 9, 2, 11, 18, 13, 9 });
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(18);

            Assert.Equal(expected, source.LastOrNone(predicate));
        }

        [t("NotIListPredicateTrueForSomeRunOnce")]
        public static void LastOrNone21() {
            IEnumerable<int> source = ForceNotCollection(new int[] { 3, 7, 10, 7, 9, 2, 11, 18, 13, 9 });
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(18);

            Assert.Equal(expected, source.RunOnce().LastOrNone(predicate));
        }
    }
}
