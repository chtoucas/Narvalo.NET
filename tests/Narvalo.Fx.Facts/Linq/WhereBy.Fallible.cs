// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("WhereBy() guards (Fallible).")]
        public static void WhereBy0b() {
            IEnumerable<int> nullsource = null;
            Func<int, Fallible<bool>> predicate = _ => Fallible.Of(true);

            Assert.Throws<ArgumentNullException>("this", () => nullsource.WhereBy(predicate));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereBy(default(Func<int, Fallible<bool>>)));
        }

        [t("WhereBy() uses deferred execution (Fallible).")]
        public static void WhereBy1b() {
            bool notCalled = true;
            Func<Fallible<bool>> fun = () => { notCalled = false; return Fallible.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereBy(f => f());

            Assert.True(notCalled);
        }
    }
}
