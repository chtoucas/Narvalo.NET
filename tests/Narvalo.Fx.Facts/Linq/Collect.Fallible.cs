// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("Collect() guards (Fallible).")]
        public static void Collect0b() {
            IEnumerable<Fallible<int>> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.Collect());
        }

        [t("Collect() uses deferred execution (Fallible).")]
        public static void Collect1b() {
            IEnumerable<Fallible<int>> source = new ThrowingEnumerable<Fallible<int>>();

            Assert.DoesNotThrow(() => source.Collect());
        }
    }
}
