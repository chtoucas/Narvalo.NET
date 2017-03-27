// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;

    using Xunit;

    public static class QullableFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Qullable), description) { }
        }

        [t("Select() returns null if null.")]
        public static void Select1() {
            int? source = null;
            Func<int, int> selector = x => x;

            var m = source.Select(selector);
            Assert.Null(m);

            var q = from _ in source select selector(_);
            Assert.Null(q);
        }

        [t("Select() applies selector if non-null.")]
        public static void Select2() {
            int? source = 1;
            Func<int, int> selector = x => 2 * x;

            var m = source.Select(selector);
            Assert.NotNull(m);
            Assert.Equal(2, m.Value);

            var q = from _ in source select selector(_);
            Assert.NotNull(q);
            Assert.Equal(2, q.Value);
        }

        [t("Where() returns null if null.")]
        public static void Where1() {
            int? source = null;
            Func<int, bool> pass = _ => true;
            Func<int, bool> fail = _ => true;

            var m1 = source.Where(pass);
            Assert.Null(m1);

            var m2 = source.Where(fail);
            Assert.Null(m2);

            var q1 = from _ in source where pass(_) select _;
            Assert.Null(q1);

            var q2 = from _ in source where fail(_) select _;
            Assert.Null(q2);
        }

        [t("Where() returns value if non-null and predicate is true.")]
        public static void Where2() {
            int? source = 1;
            Func<int, bool> predicate = _ => true;

            var m = source.Where(predicate);
            Assert.NotNull(m);
            Assert.Equal(1, m.Value);

            var q = from _ in source where predicate(_) select _;
            Assert.NotNull(q);
            Assert.Equal(1, q.Value);
        }

        [t("Where() returns null if non-null and predicate is false")]
        public static void Where3() {
            int? source = 1;
            Func<int, bool> predicate = _ => false;

            var m = source.Where(predicate);
            Assert.Null(m);

            var q = from _ in source where predicate(_) select _;
            Assert.Null(q);
        }

        [t("SelectMany() returns null if null and middle is non-null.")]
        public static void SelectMany1() {
            int? source = null;
            int? middle = 2;
            Func<int, int?> valueSelector = x => 2 * x;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            var m = source.SelectMany(valueSelector, resultSelector);
            Assert.Null(m);

            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);
            Assert.Null(q);
        }

        [t("SelectMany() returns null if null and middle is null.")]
        public static void SelectMany2() {
            int? source = null;
            int? middle = null;
            Func<int, int?> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            var m = source.SelectMany(valueSelector, resultSelector);
            Assert.Null(m);

            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);
            Assert.Null(q);
        }

        [t("SelectMany() returns null if non-null and middle is null.")]
        public static void SelectMany3() {
            int? source = 1;
            int? middle = null;
            Func<int, int?> valueSelector = _ => null;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            var m = source.SelectMany(valueSelector, resultSelector);
            Assert.Null(m);

            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);
            Assert.Null(q);
        }

        [t("SelectMany() applies selectors if non-null and middle is non-null.")]
        public static void SelectMany4() {
            int? source = 1;
            int? middle = 2;
            Func<int, int?> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            var m = source.SelectMany(valueSelector, resultSelector);
            Assert.NotNull(m);
            Assert.Equal(3, m.Value);

            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);
            Assert.NotNull(q);
            Assert.Equal(3, q.Value);
        }
    }
}
