// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("WhereAny() guards (Outcome).")]
        public static void WhereAny0d() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("this", () => nil.WhereAny(_ => Outcome.Of(true)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, Outcome<bool>>)));
        }

        [t("WhereAny() uses deferred execution (Outcome).")]
        public static void WhereAny1d() {
            bool notCalled = true;
            Func<Outcome<bool>> fun = () => { notCalled = false; return Outcome.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }
}
