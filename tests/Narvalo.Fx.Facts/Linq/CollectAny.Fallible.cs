// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("CollectAny() guards (Fallible).")]
        public static void CollectAny0a() {
            IEnumerable<Fallible<int>> nil = null;

            Assert.Throws<ArgumentNullException>("this", () => nil.CollectAny());
        }

        [t("CollectAny() uses deferred execution (Fallible).")]
        public static void CollectAny1a() {
            IEnumerable<Fallible<int>> source = new ThrowingEnumerable<Fallible<int>>();

            var q = Assert.DoesNotThrow(() => source.CollectAny());
            Assert.ThrowsOnNext(q);
        }
    }
}
