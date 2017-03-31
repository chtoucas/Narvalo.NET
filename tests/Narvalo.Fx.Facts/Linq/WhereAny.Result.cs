// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("WhereAny() guards (Result).")]
        public static void WhereAny0e() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("this", () => nil.WhereAny(_ => Result<bool, int>.Of(true)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, Result<bool, int>>)));
        }

        [t("WhereAny() uses deferred execution (Result).")]
        public static void WhereAny1e() {
            bool notCalled = true;
            Func<Result<bool, int>> fun = () => { notCalled = false; return Result<bool, int>.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }
}
