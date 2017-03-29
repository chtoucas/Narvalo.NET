// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("WhereBy() guards (Outcome).")]
        public static void WhereBy0d() {
            IEnumerable<int> nullsource = null;
            Func<int, Outcome<bool>> predicate = _ => Outcome.Of(true);

            Assert.Throws<ArgumentNullException>("this", () => nullsource.WhereBy(predicate));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereBy(default(Func<int, Outcome<bool>>)));
        }

        [t("WhereBy() uses deferred execution (Outcome).")]
        public static void WhereBy1d() {
            bool notCalled = true;
            Func<Outcome<bool>> fun = () => { notCalled = false; return Outcome.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereBy(f => f());

            Assert.True(notCalled);
        }
    }
}
