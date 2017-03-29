// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Tests;

    using Narvalo.Applicative;
    using Xunit;

    // Largely inspired by
    // https://github.com/dotnet/corefx/blob/master/src/System.Linq/tests/FirstOrDefaultTests.cs
    public partial class QperatorsFacts {
        [t("FirstOrNone() guards.")]
        public static void FirstOrNone0() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("this", () => nil.FirstOrNone());
            Assert.Throws<ArgumentNullException>("this", () => nil.FirstOrNone(_ => true));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.FirstOrNone(default(Func<int, bool>)));
        }

        [t("FirstOrNone() for int's returns the same result when called repeatedly.")]
        public static void FirstOrNone1() {
            var source = Enumerable.Range(0, 0);

            var q = from x in source select x;

            Assert.Equal(q.FirstOrNone(), q.FirstOrNone());
        }

        [t("FirstOrNone() for string's returns the same result when called repeatedly.")]
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

        [t("FirstOrNone() for an empty IList<T>.")]
        public static void FirstOrNone3() {
            FirstOrNone3Impl<int>();
            FirstOrNone3Impl<string>();
            FirstOrNone3Impl<DateTime>();
            FirstOrNone3Impl<QperatorsFacts>();
        }

        [t("FirstOrNone() for an IList<T> of one element.")]
        public static void FirstOrNone4() {
            int[] source = { 5 };
            var expected = Maybe.Of(5);

            Assert.IsAssignableFrom<IList<int>>(source);
            Assert.Equal(expected, source.FirstOrNone());
        }

        [t("FirstOrNone() for an IList<T> of many elements whose first is none.")]
        public static void FirstOrNone5() {
            string[] source = { null, "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS" };
            var expected = Maybe<string>.None;

            Assert.IsAssignableFrom<IList<string>>(source);
            Assert.Equal(expected, source.FirstOrNone());
        }

        [t("FirstOrNone() for an IList<T> of many elements whose first is some.")]
        public static void FirstOrNone6() {
            string[] source = { "!@#$%^", null, "C", "AAA", "", "Calling Twice", "SoS" };
            var expected = Maybe.Of("!@#$%^");

            Assert.IsAssignableFrom<IList<string>>(source);
            Assert.Equal(expected, source.FirstOrNone());
        }

        private static void FirstOrNone7Impl<T>() {
            var source = EmptySource<T>();
            var expected = Maybe<T>.None;

            Assert.Null(source as IList<T>);
            Assert.Equal(expected, source.RunOnce().FirstOrNone());
        }

        [t("EmptyNotIListT")]
        public static void FirstOrNone7() {
            FirstOrNone7Impl<int>();
            FirstOrNone7Impl<string>();
            FirstOrNone7Impl<DateTime>();
            FirstOrNone7Impl<QperatorsFacts>();
        }

        [t("OneElementNotIListT")]
        public static void FirstOrNone8() {
            IEnumerable<int> source = NumberRangeGuaranteedNotCollectionType(-5, 1);
            var expected = Maybe.Of(-5);

            Assert.Null(source as IList<int>);
            Assert.Equal(expected, source.FirstOrNone());
        }

        [t("ManyElementsNotIListT")]
        public static void FirstOrNone9() {
            IEnumerable<int> source = NumberRangeGuaranteedNotCollectionType(3, 10);
            var expected = Maybe.Of(3);

            Assert.Null(source as IList<int>);
            Assert.Equal(expected, source.FirstOrNone());
        }

        [t("EmptySource")]
        public static void FirstOrNone10() {
            string[] source = { };
            var expected = Maybe<string>.None;

            Assert.Equal(expected, source.FirstOrNone(x => true));
            Assert.Equal(expected, source.FirstOrNone(x => false));
        }

        [t("FirstOrNone() on a list of one element returns some.")]
        public static void FirstOrNone11() {
            int[] source = { 4 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(4);

            Assert.Equal(expected, source.FirstOrNone(predicate));
        }

        [t("FirstOrNone() w/ predicate always false returns none.")]
        public static void FirstOrNone12() {
            int[] source = { 9, 5, 1, 3, 17, 21 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe<int>.None;

            Assert.Equal(expected, source.FirstOrNone(predicate));
        }

        [t("FirstOrNone() w/ predicate returns last.")]
        public static void FirstOrNone13() {
            int[] source = { 9, 5, 1, 3, 17, 21, 50 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(50);

            Assert.Equal(expected, source.FirstOrNone(predicate));
        }

        [t("FirstOrNone() w/ predicate returns some (1).")]
        public static void FirstOrNone14() {
            int[] source = { 3, 7, 10, 7, 9, 2, 11, 17, 13, 8 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(10);

            Assert.Equal(expected, source.FirstOrNone(predicate));
        }

        [t("FirstOrNone() w/ predicate returns some (2).")]
        public static void FirstOrNone15() {
            int[] source = { 3, 7, 10, 7, 9, 2, 11, 17, 13, 8 };
            Func<int, bool> predicate = IsEven;
            var expected = Maybe.Of(10);

            Assert.Equal(expected, source.RunOnce().FirstOrNone(predicate));
        }
    }
}
