// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public static partial class QperatorsFacts {
        [t("SelectAny() guards.")]
        public static void SelectAny0() {
            IEnumerable<int> source = null;
            Func<int, int?> selector = x => x;

            Assert.Throws<ArgumentNullException>("this", () => source.SelectAny(selector));
            Assert.Throws<ArgumentNullException>("this", () => source.SelectAny(_ => String.Empty));
            Assert.Throws<ArgumentNullException>("this", () => source.SelectAny(default(Func<int, int?>)));
            Assert.Throws<ArgumentNullException>("this", () => source.SelectAny(default(Func<int, string>)));
        }

        [t("SelectAny() uses deferred execution.")]
        public static void SelectAny1() {
            bool notCalled = true;
            Func<string> fun = () => { notCalled = false; return String.Empty; };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
        }

        [t("SelectAny() uses deferred execution.")]
        public static void SelectAny2() {
            bool notCalled = true;
            Func<int?> fun = () => { notCalled = false; return 1; };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
        }
    }
}
