// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("SelectAny() guards.")]
        public static void SelectAny0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.SelectAny(x => (int?)x));
            Assert.Throws<ArgumentNullException>("this", () => nullsource.SelectAny(_ => String.Empty));
            Assert.Throws<ArgumentNullException>("this", () => nullsource.SelectAny(x => Maybe.Of(x)));
            Assert.Throws<ArgumentNullException>("this", () => nullsource.SelectAny(x => Outcome.Of(x)));
            Assert.Throws<ArgumentNullException>("this", () => nullsource.SelectAny(x => Fallible.Of(x)));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, int?>)));
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, string>)));
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, Maybe<int>>)));
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, Outcome<int>>)));
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, Fallible<int>>)));
        }

        [t("SelectAny() uses deferred execution (1).")]
        public static void SelectAny1() {
            bool notCalled = true;
            Func<int?> fun = () => { notCalled = false; return 1; };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
        }

        [t("SelectAny() uses deferred execution (2).")]
        public static void SelectAny2() {
            bool notCalled = true;
            Func<string> fun = () => { notCalled = false; return String.Empty; };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
        }

        [t("SelectAny() uses deferred execution (3).")]
        public static void SelectAny3() {
            bool notCalled = true;
            Func<Maybe<int>> fun = () => { notCalled = false; return Maybe.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
        }

        [t("SelectAny() uses deferred execution (4).")]
        public static void SelectAny4() {
            bool notCalled = true;
            Func<Outcome<int>> fun = () => { notCalled = false; return Outcome.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
        }

        [t("SelectAny() uses deferred execution (5).")]
        public static void SelectAny5() {
            bool notCalled = true;
            Func<Fallible<int>> fun = () => { notCalled = false; return Fallible.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
        }
    }
}
