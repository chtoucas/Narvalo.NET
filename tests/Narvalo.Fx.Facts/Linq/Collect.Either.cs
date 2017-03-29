// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("Collect() guards (Either).")]
        public static void Collect0a() {
            IEnumerable<Either<int, int>> nil = null;

            Assert.Throws<ArgumentNullException>("this", () => nil.Collect());
        }

        [t("Collect() uses deferred execution (Either).")]
        public static void Collect1a() {
            IEnumerable<Either<int, int>> source = new ThrowingEnumerable<Either<int, int>>();

            var q = Assert.DoesNotThrow(() => source.Collect());
            q.OnLeft(x => Assert.ThrowsOnNext(x));
            q.OnRight(x => Assert.Fail());
        }
    }
}
