// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("WhereBy() guards (Result).")]
        public static void WhereBy0e() {
            Func<int, Result<bool, int>> predicate = _ => Result<bool, int>.Of(true);

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("this", () => nil.WhereBy(predicate));

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
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
        }
    }
}
