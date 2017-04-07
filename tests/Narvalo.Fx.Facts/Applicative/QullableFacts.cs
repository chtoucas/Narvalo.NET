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
            short? source = null;
            Func<short, int> selector = x => x;

            int? m = source.Select(selector);
            Assert.Null(m);

            int? q = from _ in source select selector(_);
            Assert.Null(q);
        }

        [t("Select() applies selector if non-null.")]
        public static void Select2() {
            int? source = 1;
            Func<int, int> selector = x => 2 * x;

            int? m = source.Select(selector);
            Assert.NotNull(m);
            Assert.Equal(2, m.Value);

            int? q = from _ in source select selector(_);
            Assert.NotNull(q);
            Assert.Equal(2, q.Value);
        }

        [t("Where() returns null if null.")]
        public static void Where1() {
            int? source = null;
            Func<int, bool> pass = _ => true;
            Func<int, bool> fail = _ => true;

            int? m1 = source.Where(pass);
            Assert.Null(m1);

            int? m2 = source.Where(fail);
            Assert.Null(m2);

            int? q1 = from _ in source where pass(_) select _;
            Assert.Null(q1);

            int? q2 = from _ in source where fail(_) select _;
            Assert.Null(q2);
        }

        [t("Where() returns non-null if non-null and predicate is true.")]
        public static void Where2() {
            int? source = 1;
            Func<int, bool> predicate = _ => true;

            int? m = source.Where(predicate);
            Assert.NotNull(m);
            Assert.Equal(1, m.Value);

            int? q = from _ in source where predicate(_) select _;
            Assert.NotNull(q);
            Assert.Equal(1, q.Value);
        }

        [t("Where() returns null if non-null and predicate is false")]
        public static void Where3() {
            int? source = 1;
            Func<int, bool> predicate = _ => false;

            int? m = source.Where(predicate);
            Assert.Null(m);

            int? q = from _ in source where predicate(_) select _;
            Assert.Null(q);
        }

        [t("Bind() applies binder if non-null.")]
        public static void Bind1() {
            (short, short)? source = (1, 2);
            Func<(short, short), int?> binder = t => t.Item2 + 1;

            int? m = source.Bind(binder);
            Assert.NotNull(m);
            Assert.Equal(3, m.Value);

            // Bind via SelectMany.
            int? m1 = source.SelectMany(binder, (_, j) => j);
            Assert.NotNull(m1);
            Assert.Equal(3, m1.Value);

            // Bind via SelectMany using the query syntax.
            int? q1 = from t in source
                      from j in (int?)(t.Item2 + 1)
                      select j;
            Assert.NotNull(q1);
            Assert.Equal(3, q1.Value);
        }

        [t("Bind() returns null if null.")]
        public static void Bind2() {
            (short, short)? source = null;
            Func<(short, short), int?> binder = t => t.Item2 + 1;

            int? m = source.Bind(binder);
            Assert.Null(m);
        }

        [t("Bind() returns null if non-null and binder returns null.")]
        public static void Bind3() {
            (short, short)? source = (1, 2);
            Func<(short, short), int?> binder = t => null;

            int? m = source.Bind(binder);
            Assert.Null(m);
        }

        [t("SelectMany() returns null if null and middle is non-null.")]
        public static void SelectMany1() {
            short? source = null;
            Func<short, int?> valueSelector = i => 2 * i;
            Func<short, int, long> resultSelector = (i, j) => i + j;

            long? m = source.SelectMany(valueSelector, resultSelector);
            Assert.Null(m);

            long? q = from i in source
                      from j in valueSelector(i)
                      select resultSelector(i, j);
            Assert.Null(q);
        }

        [t("SelectMany() returns null if null and middle is null.")]
        public static void SelectMany2() {
            short? source = null;
            Func<short, int?> valueSelector = _ => null;
            Func<short, int, long> resultSelector = (i, j) => i + j;

            long? m = source.SelectMany(valueSelector, resultSelector);
            Assert.Null(m);

            long? q = from i in source
                      from j in valueSelector(i)
                      select resultSelector(i, j);
            Assert.Null(q);
        }

        [t("SelectMany() returns null if non-null and middle is null.")]
        public static void SelectMany3() {
            short? source = 1;
            Func<short, int?> valueSelector = _ => null;
            Func<short, int, long> resultSelector = (i, j) => i + j;

            long? m = source.SelectMany(valueSelector, resultSelector);
            Assert.Null(m);

            long? q = from i in source
                      from j in valueSelector(i)
                      select resultSelector(i, j);
            Assert.Null(q);
        }

        [t("SelectMany() applies selectors if non-null and middle is non-null.")]
        public static void SelectMany4() {
            short? source = 1;
            Func<short, int?> valueSelector = i => 2 * i;
            Func<short, int, long> resultSelector = (i, j) => i + j;

            long? m = source.SelectMany(valueSelector, resultSelector);
            Assert.NotNull(m);
            Assert.Equal(3, m.Value);

            long? q = from i in source
                      from j in valueSelector(i)
                      select resultSelector(i, j);
            Assert.NotNull(q);
            Assert.Equal(3, q.Value);
        }

        [t("Join() joins if non-null.")]
        public static void Join1() {
            (int, string)? outer = (1, "key");
            (string, int)? inner = ("key", 3);
            Func<(int, string), string> outerKeySelector = t => t.Item2;
            Func<(string, int), string> innerKeySelector = t => t.Item1;
            Func<(int, string), (string, int), (int, int)> resultSelector
                = (x, y) => (x.Item1, y.Item2);

            (int, int)? m = outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector);
            Assert.NotNull(m);
            Assert.Equal(1, m.Value.Item1);
            Assert.Equal(3, m.Value.Item2);

            (int, int)? q = from t1 in outer
                            join t2 in inner on t1.Item2 equals t2.Item1
                            select (t1.Item1, t2.Item2);
            Assert.NotNull(q);
            Assert.Equal(1, q.Value.Item1);
            Assert.Equal(3, q.Value.Item2);
        }

        [t("GroupJoin() joins if non-null.")]
        public static void GroupJoin1() {
            (int, string)? outer = (1, "key");
            (string, int)? inner = ("key", 3);
            Func<(int, string), string> outerKeySelector = t => t.Item2;
            Func<(string, int), string> innerKeySelector = t => t.Item1;
            Func<(int, string), (string, int)?, (int, int?)> resultSelector
                = (x, y) => (x.Item1, y?.Item2);

            (int, int?)? m = outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);
            Assert.NotNull(m);
            Assert.Equal(1, m.Value.Item1);
            Assert.NotNull(m.Value.Item2);
            Assert.Equal(3, m.Value.Item2.Value);

            (int, int?)? q
                = from t1 in outer
                  join t2 in inner on t1.Item2 equals t2.Item1
                  into g2
                  select (t1.Item1, g2?.Item2);
            Assert.NotNull(q);
            Assert.Equal(1, q.Value.Item1);
            Assert.NotNull(q.Value.Item2);
            Assert.Equal(3, q.Value.Item2.Value);
        }

        [t("Equi-join w/ SelectMany().")]
        public static void EquiJoin1() {
            (int, string)? outer = (1, "key");
            (string, int)? inner = ("key", 3);

            // No query syntax (see README in Narvalo.Fx for an explanation).
            var m = outer.SelectMany(x => inner, (o, i) => (o, i))
                .Where(t => t.Item1.Item2 == t.Item2.Item1)
                .Select(t => (t.Item1.Item1, t.Item2.Item2));
            Assert.NotNull(m);
            Assert.Equal(1, m.Value.Item1);
            Assert.Equal(3, m.Value.Item2);
        }

        [t("Cross join w/ SelectMany().")]
        public static void CrossJoin1() {
            short? source = 1;
            int? middle = 2;
            Func<short, int, long> resultSelector = (i, j) => i + j;

            long? m = source.SelectMany(_ => middle, resultSelector);
            Assert.NotNull(m);
            Assert.Equal(3, m.Value);

            long? q = from i in source
                      from j in middle
                      select resultSelector(i, j);
            Assert.NotNull(q);
            Assert.Equal(3, q.Value);
        }

        [t("Subquery w/ Select().")]
        public static void Subquery1() {
            (int, (int, int)?)? source = (1, (2, 3));
            Func<(int, (int, int)?), (int, int?)> selector
                = outer => (
                outer.Item1,
                (from inner in outer.Item2 select inner.Item1 + inner.Item2));

            // m is of type (int, int?)?
            var m = source.Select(selector);
            Assert.NotNull(m);
            Assert.NotNull(m.Value.Item2);
            Assert.Equal(1, m.Value.Item1);
            Assert.Equal(5, m.Value.Item2.Value);

            var q = from outer in source
                    select (
                        outer.Item1,
                        (from inner in outer.Item2 select inner.Item1 + inner.Item2));
            Assert.NotNull(q);
            Assert.NotNull(q.Value.Item2);
            Assert.Equal(1, q.Value.Item1);
            Assert.Equal(5, q.Value.Item2.Value);
        }

        [t("Outer join w/ SelectMany().")]
        public static void OuterJoin1() {
            (int, (int, int)?)? source = (1, (2, 3));

            // Compare w/ Subquery1(), the result is now flattened.
            // m is of type (int, int)?
            var q = from outer in source
                    from inner in outer.Item2
                    select (outer.Item1, inner.Item1 + inner.Item2);
            Assert.NotNull(q);
            Assert.Equal(1, q.Value.Item1);
            Assert.Equal(5, q.Value.Item2);
        }
    }
}
