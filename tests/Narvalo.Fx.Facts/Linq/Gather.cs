// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;

    using Assert = Narvalo.AssertExtended;

    public partial class SequenceFacts {
        [t("Gather() guards.")]
        public static void Gather0() {
            Func<int, int> generator = i => i + 1;
            Func<int, int> resultSelector = i => i + 1;
            Func<int, bool> predicate = _ => true;

            Assert.Throws<ArgumentNullException>("generator", () => Sequence.Gather(0, default(Func<int, int>)));

            Assert.Throws<ArgumentNullException>("generator", () => Sequence.Gather(0, null, predicate));
            Assert.Throws<ArgumentNullException>("predicate", () => Sequence.Gather(0, generator, default(Func<int, bool>)));

            Assert.Throws<ArgumentNullException>("generator", () => Sequence.Gather(0, null, resultSelector));
            Assert.Throws<ArgumentNullException>("resultSelector", () => Sequence.Gather(0, generator, default(Func<int, int>)));

            Assert.Throws<ArgumentNullException>("generator", () => Sequence.Gather(0, null, resultSelector, predicate));
            Assert.Throws<ArgumentNullException>("resultSelector", () => Sequence.Gather(0, generator, default(Func<int, int>), predicate));
            Assert.Throws<ArgumentNullException>("predicate", () => Sequence.Gather(0, generator, resultSelector, null));
        }

        [t("Gather() uses deferred (streaming) execution.")]
        public static void Gather1() {
            Func<int, int> generator = _ => throw new InvalidOperationException();
            Func<int, int> resultSelector = _ => throw new InvalidOperationException();
            Func<int, bool> predicate = _ => throw new InvalidOperationException();

            var q1 = Assert.DoesNotThrow(() => Sequence.Gather(0, generator));
            Assert.ThrowsAfter(q1, 1);

            var q2 = Assert.DoesNotThrow(() => Sequence.Gather(0, generator, _ => true));
            Assert.ThrowsAfter(q2, 1);
            var q3 = Assert.DoesNotThrow(() => Sequence.Gather(0, i => i + 1, predicate));
            Assert.ThrowsOnNext(q3);

            var q4 = Assert.DoesNotThrow(() => Sequence.Gather(0, generator, i => i + 1));
            Assert.ThrowsAfter(q4, 1);
            var q5 = Assert.DoesNotThrow(() => Sequence.Gather(0, i => i + 1, resultSelector));
            Assert.ThrowsOnNext(q5);

            var q6 = Assert.DoesNotThrow(() => Sequence.Gather(0, generator, i => i + 1, _ => true));
            Assert.ThrowsAfter(q6, 1);
            var q7 = Assert.DoesNotThrow(() => Sequence.Gather(0, i => i + 1, resultSelector, _ => true));
            Assert.ThrowsOnNext(q7);
            var q8 = Assert.DoesNotThrow(() => Sequence.Gather(0, i => i + 1, i => i + 1, predicate));
            Assert.ThrowsOnNext(q8);
        }
    }
}
