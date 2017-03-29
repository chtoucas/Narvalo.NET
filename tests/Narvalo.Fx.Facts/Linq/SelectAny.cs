// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public partial class QperatorsFacts {
        [t("SelectAny() guards.")]
        public static void SelectAny0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.SelectAny(x => (int?)x));
            Assert.Throws<ArgumentNullException>("this", () => nullsource.SelectAny(_ => String.Empty));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, int?>)));
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, string>)));
        }

        [t("SelectAny() uses deferred execution (1).")]
        public static void SelectAny1_1() {
            bool notCalled = true;
            Func<int?> fun = () => { notCalled = false; return 1; };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
        }

        [t("SelectAny() uses deferred execution (2).")]
        public static void SelectAny1_2() {
            bool notCalled = true;
            Func<string> fun = () => { notCalled = false; return String.Empty; };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
        }
    }
}
