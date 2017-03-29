// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("CollectAny() guards (Result).")]
        public static void CollectAny0d() {
            IEnumerable<Result<int, int>> nil = null;

            Assert.Throws<ArgumentNullException>("this", () => nil.CollectAny());
        }

        [t("CollectAny() uses deferred execution (Result).")]
        public static void CollectAny1d() {
            IEnumerable<Result<int, int>> source = new ThrowingEnumerable<Result<int, int>>();

            var q = Assert.DoesNotThrow(() => source.CollectAny());
            Assert.ThrowsOnNext(q);
        }
    }
}
