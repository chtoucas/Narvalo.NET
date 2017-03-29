// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("CollectAny() guards (Result).")]
        public static void CollectAny0d() {
            IEnumerable<Result<int, int>> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.CollectAny());
        }

        [t("CollectAny() uses deferred execution (Result).")]
        public static void CollectAny1d() {
            IEnumerable<Result<int, int>> source = new ThrowingEnumerable<Result<int, int>>();

            Assert.DoesNotThrow(() => source.CollectAny());
        }
    }
}
