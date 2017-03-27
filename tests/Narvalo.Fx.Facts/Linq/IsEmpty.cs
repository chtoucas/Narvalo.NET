// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System.Linq;

    using Xunit;

    public static partial class QperatorsFacts {
        [t("IsEmpty() returns true if empty.")]
        public static void IsEmpty1() {
            var seq = Enumerable.Empty<int>();
            Assert.True(seq.IsEmpty());
        }

        [t("IsEmpty() returns false if non-empty.")]
        public static void IsEmpty2() {
            var seq = Enumerable.Range(0, 1);
            Assert.False(seq.IsEmpty());
        }
    }
}
