// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System.Collections.Generic;

    using Assert = Narvalo.AssertExtended;

    // See also System.Linq.Tests.AppendPrependTests
    public partial class QperatorsFacts {
        [t("Append() uses deferred execution.")]
        public static void Append1() => Assert.IsDeferred<int>(src => src.Append(1));

        [t("Prepend() uses deferred execution.")]
        public static void Prepend1() {
            // NB: One can not use Assert.IsDeferred here: the first
            // iteration in the loop will not throw (see the body of IsDeferred).
            IEnumerable<int> source = new ThrowingEnumerable<int>();

            Assert.DoesNotThrow(() => source.Prepend(1));
        }
    }
}
