// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("CollectAny() guards (Outcome).")]
        public static void CollectAny0c() {
            IEnumerable<Outcome<int>> nil = null;

            Assert.Throws<ArgumentNullException>("this", () => nil.CollectAny());
        }

        [t("CollectAny() uses deferred execution (Outcome).")]
        public static void CollectAny1c() {
            IEnumerable<Outcome<int>> source = new ThrowingEnumerable<Outcome<int>>();

            var q = Assert.DoesNotThrow(() => source.CollectAny());
            Assert.ThrowsOnNext(q);
        }
    }
}
