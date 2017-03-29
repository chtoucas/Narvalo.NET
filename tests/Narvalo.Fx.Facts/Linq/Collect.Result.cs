// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("Collect() guards (Result).")]
        public static void Collect0e() {
            IEnumerable<Result<int, int>> nil = null;

            Assert.Throws<ArgumentNullException>("this", () => nil.Collect());
        }

        [t("Collect() uses deferred execution (Result).")]
        public static void Collect1e() {
            IEnumerable<Result<int, int>> source = new ThrowingEnumerable<Result<int, int>>();

            var q = Assert.DoesNotThrow(() => source.Collect());
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
        }
    }
}
