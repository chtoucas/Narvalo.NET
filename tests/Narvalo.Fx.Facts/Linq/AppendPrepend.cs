// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System.Collections.Generic;

    using Assert = Narvalo.AssertExtended;

    // See also System.Linq.Tests.AppendPrependTests
    public partial class SequenceFacts {
        [t("Append() uses deferred execution.")]
        public static void Append1() {
            IEnumerable<int> source = new ThrowingEnumerable<int>();

            var q = Assert.DoesNotThrow(() => source.Append(1));

            Assert.ThrowsOnNext(q);
        }

        [t("Prepend() uses deferred execution.")]
        public static void Prepend1() {
            IEnumerable<int> source = new ThrowingEnumerable<int>();

            var q = Assert.DoesNotThrow(() => source.Prepend(1));

            // We bypass the first iteration (we just prepended 1 to the sequence,
            // iterating won't throw immediately).
            Assert.ThrowsAfter(q, 1);
        }
    }
}
