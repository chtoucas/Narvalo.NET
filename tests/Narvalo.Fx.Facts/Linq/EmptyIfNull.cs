// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public static partial class QperatorsFacts {
        [t("EmptyIfNull(null) returns empty.")]
        public static void EmptyIfNull1() {
            IEnumerable<int> seq = null;
            Assert.Empty(seq.EmptyIfNull());
        }

        [t("EmptyIfNull(non-null) returns the input.")]
        public static void EmptyIfNull2() {
            var seq = Enumerable.Range(0, 1);
            Assert.Same(seq, seq.EmptyIfNull());
        }
    }
}
