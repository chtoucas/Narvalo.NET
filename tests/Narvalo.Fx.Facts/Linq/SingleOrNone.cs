// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public static partial class QperatorsFacts {
        [t("SingleOrNone() guards.")]
        public static void SingleOrNone0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.SingleOrNone());
            Assert.Throws<ArgumentNullException>("this", () => nullsource.SingleOrNone(_ => true));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.SingleOrNone(default(Func<int, bool>)));
        }
    }
}
