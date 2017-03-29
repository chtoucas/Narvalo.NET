// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("CollectAny() guards (Maybe).")]
        public static void CollectAny0b() {
            IEnumerable<Maybe<int>> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.CollectAny());
        }

        [t("CollectAny() uses deferred execution (Maybe).")]
        public static void CollectAny1b() {
            IEnumerable<Maybe<int>> source = new ThrowingEnumerable<Maybe<int>>();

            Assert.DoesNotThrow(() => source.CollectAny());
        }
    }
}
