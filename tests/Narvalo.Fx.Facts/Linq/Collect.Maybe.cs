// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("Collect() guards (Maybe).")]
        public static void Collect0c() {
            IEnumerable<Maybe<int>> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.Collect());
        }

        [t("Collect() uses deferred execution (Maybe).")]
        public static void Collect1c() {
            IEnumerable<Maybe<int>> source = new ThrowingEnumerable<Maybe<int>>();

            Assert.DoesNotThrow(() => source.Collect());
        }
    }
}
