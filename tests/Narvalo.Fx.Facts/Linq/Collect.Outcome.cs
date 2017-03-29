// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("Collect() guards (Outcome).")]
        public static void Collect0d() {
            IEnumerable<Outcome<int>> nil = null;

            Assert.Throws<ArgumentNullException>("this", () => nil.Collect());
        }

        [t("Collect() uses deferred execution (Outcome).")]
        public static void Collect1d() {
            IEnumerable<Outcome<int>> source = new ThrowingEnumerable<Outcome<int>>();

            var q = Assert.DoesNotThrow(() => source.Collect());
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }
}
