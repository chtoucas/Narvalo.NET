// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;

    using Assert = Narvalo.AssertExtended;

    public partial class SequenceFacts {
        [t("Unfold() guards.")]
        public static void Unfold0() {
            Func<int, (int, int)> generator = i => (i, i + 1);

            Assert.Throws<ArgumentNullException>("generator", () => Sequence.Unfold(0, default(Func<int, (int, int)>)));
            Assert.Throws<ArgumentNullException>("predicate", () => Sequence.Unfold(0, generator, null));
        }

        [t("Unfold() uses deferred (streaming) execution.")]
        public static void Unfold1() {
            Func<int, (int, int)> generator = _ => throw new InvalidOperationException();

            Assert.DoesNotThrow(() => Sequence.Unfold(0, generator));
            Assert.DoesNotThrow(() => Sequence.Unfold(0, generator, _ => true));
        }
    }
}
