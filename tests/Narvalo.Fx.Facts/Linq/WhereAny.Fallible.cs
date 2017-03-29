// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("WhereAny() guards (Fallible).")]
        public static void WhereAny0a() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.WhereAny(_ => Fallible.Of(true)));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, Fallible<bool>>)));
        }

        [t("WhereAny() uses deferred execution (Fallible).")]
        public static void WhereAny1a() {
            bool notCalled = true;
            Func<Fallible<bool>> fun = () => { notCalled = false; return Fallible.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
        }
    }
}
