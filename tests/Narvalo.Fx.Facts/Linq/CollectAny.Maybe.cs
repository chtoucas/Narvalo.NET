// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("CollectAny() guards (Maybe).")]
        public static void CollectAny0b() {
            IEnumerable<Maybe<int>> nil = null;

            Assert.Throws<ArgumentNullException>("this", () => nil.CollectAny());
        }

        [t("CollectAny() uses deferred execution (Maybe).")]
        public static void CollectAny1b() {
            IEnumerable<Maybe<int>> source = new ThrowingEnumerable<Maybe<int>>();

            var q = Assert.DoesNotThrow(() => source.CollectAny());
            Assert.ThrowsOnNext(q);
        }
    }
}
