// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("WhereAny() guards.")]
        public static void WhereAny0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.WhereAny(_ => true));
            Assert.Throws<ArgumentNullException>("this", () => nullsource.WhereAny(_ => Maybe.Of(true)));
            Assert.Throws<ArgumentNullException>("this", () => nullsource.WhereAny(_ => Outcome.Of(true)));
            Assert.Throws<ArgumentNullException>("this", () => nullsource.WhereAny(_ => Fallible.Of(true)));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, bool?>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, Maybe<bool>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, Outcome<bool>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, Fallible<bool>>)));
        }

        [t("WhereAny() uses deferred execution (1).")]
        public static void WhereAny1() {
            bool notCalled = true;
            Func<bool?> fun = () => { notCalled = false; return true; };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
        }

        [t("WhereAny() uses deferred execution (2).")]
        public static void WhereAny2() {
            bool notCalled = true;
            Func<Maybe<bool>> fun = () => { notCalled = false; return Maybe.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
        }

        [t("WhereAny() uses deferred execution (3).")]
        public static void WhereAny3() {
            bool notCalled = true;
            Func<Outcome<bool>> fun = () => { notCalled = false; return Outcome.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
        }

        [t("WhereAny() uses deferred execution (4).")]
        public static void WhereAny4() {
            bool notCalled = true;
            Func<Fallible<bool>> fun = () => { notCalled = false; return Fallible.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
        }
    }
}
