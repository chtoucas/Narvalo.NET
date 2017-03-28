// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class QperatorsFacts {
        [t("ElementAtOrNone() guards.")]
        public static void ElementAtOrNone0() {
            IEnumerable<int> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.ElementAtOrNone(1));
        }
    }
}
