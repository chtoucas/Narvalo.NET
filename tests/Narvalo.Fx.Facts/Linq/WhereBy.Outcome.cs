// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("WhereBy() guards (Outcome).")]
        public static void WhereBy0d() {
            Func<int, Outcome<bool>> predicate = _ => Outcome.Of(true);

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("this", () => nil.WhereBy(predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereBy(default(Func<int, Outcome<bool>>)));
        }

        [t("WhereBy() uses deferred execution (Outcome).")]
        public static void WhereBy1d() {
            bool notCalled = true;
            Func<Outcome<bool>> fun = () => { notCalled = false; return Outcome.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereBy(f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }
}
