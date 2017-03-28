// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public static partial class QperatorsFacts {
        [t("LastOrNone() guards.")]
        public static void LastOrNone0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.LastOrNone());
            Assert.Throws<ArgumentNullException>("this", () => nullsource.LastOrNone(_ => true));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.LastOrNone(default(Func<int, bool>)));
        }
    }
}
