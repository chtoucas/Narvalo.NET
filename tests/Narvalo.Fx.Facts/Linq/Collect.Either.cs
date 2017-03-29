// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("Collect() guards (Either).")]
        public static void Collect0a() {
            IEnumerable<Either<int, int>> nullsource = null;

            Assert.Throws<ArgumentNullException>("this", () => nullsource.Collect());
        }

        [t("Collect() uses deferred execution (Either).")]
        public static void Collect1a() {
            IEnumerable<Either<int, int>> source = new ThrowingEnumerable<Either<int, int>>();

            Assert.DoesNotThrow(() => source.Collect());
        }
    }
}
