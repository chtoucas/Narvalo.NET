// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("SelectAny() guards (Outcome).")]
        public static void SelectAny0c() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.SelectAny(x => Outcome.Of(x)));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, Outcome<int>>)));
        }

        [t("SelectAny() uses deferred execution (Outcome).")]
        public static void SelectAny1c() {
            bool notCalled = true;
            Func<Outcome<int>> fun = () => { notCalled = false; return Outcome.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
        }
    }
}
