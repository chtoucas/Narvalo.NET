// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("Collect() guards (Outcome).")]
        public static void Collect0d() {
            IEnumerable<Outcome<int>> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.Collect());
        }

        [t("Collect() uses deferred execution (Outcome).")]
        public static void Collect1d() {
            IEnumerable<Outcome<int>> source = new ThrowingEnumerable<Outcome<int>>();

            Assert.DoesNotThrow(() => source.Collect());
        }
    }
}
