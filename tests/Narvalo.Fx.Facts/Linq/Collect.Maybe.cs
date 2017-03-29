// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    public partial class QperatorsFacts {
        [t("Collect() guards (Maybe).")]
        public static void Collect0c() {
            IEnumerable<Maybe<int>> nil = null;

            Assert.Throws<ArgumentNullException>("this", () => nil.Collect());
        }

        [t("Collect() uses deferred execution (Maybe).")]
        public static void Collect1c() {
            IEnumerable<Maybe<int>> source = new ThrowingEnumerable<Maybe<int>>();

            var q = Assert.DoesNotThrow(() => source.Collect());
            q.OnSome(x => Assert.ThrowsOnNext(x));
        }
    }
}
