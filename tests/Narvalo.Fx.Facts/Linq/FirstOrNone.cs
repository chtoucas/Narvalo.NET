// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public static partial class QperatorsFacts {
        [t("FirstOrNone() guards.")]
        public static void FirstOrNone0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.FirstOrNone());
            Assert.Throws<ArgumentNullException>("this", () => nullsource.FirstOrNone(_ => true));

            var source = Enumerable.Range(0, 1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.FirstOrNone(default(Func<int, bool>)));
        }
    }
}
