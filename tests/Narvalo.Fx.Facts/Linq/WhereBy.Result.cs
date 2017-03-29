// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("WhereBy() guards (Result).")]
        public static void WhereBy0e() {
            IEnumerable<int> nullsource = null;
            Func<int, Result<bool, int>> predicate = _ => Result<bool, int>.Of(true);

            Assert.Throws<ArgumentNullException>("this", () => nullsource.WhereBy(predicate));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereBy(default(Func<int, Result<bool, int>>)));
        }

        [t("WhereBy() uses deferred execution (Result).")]
        public static void WhereBy1e() {
            bool notCalled = true;
            Func<Result<bool, int>> fun
                = () => { notCalled = false; return Result<bool, int>.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereBy(f => f());

            Assert.True(notCalled);
        }
    }
}
