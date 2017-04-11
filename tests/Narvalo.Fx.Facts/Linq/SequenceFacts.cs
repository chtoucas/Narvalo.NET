// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Linq.Tests;

    using Assert = Narvalo.AssertExtended;

    public partial class SequenceFacts : EnumerableTests {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Sequence), description) { }
        }

        [t("Unfold() guards (1).")]
        public static void Unfold0() {
            Func<int, (int, int)> generator = i => (i, i + 1);

            Assert.Throws<ArgumentNullException>("generator", () => Sequence.Unfold(0, default(Func<int, (int, int)>)));
            Assert.Throws<ArgumentNullException>("predicate", () => Sequence.Unfold(0, generator, null));
        }

        [t("Unfold() guards (2).")]
        public static void Unfold0b() {
            Func<int, int> generator = i => i + 1;
            Func<int, int> resultSelector = i => i + 1;
            Func<int, bool> predicate = _ => true;

            Assert.Throws<ArgumentNullException>("generator", () => Sequence.Unfold(0, default(Func<int, int>)));

            Assert.Throws<ArgumentNullException>("generator", () => Sequence.Unfold(0, null, predicate));
            Assert.Throws<ArgumentNullException>("predicate", () => Sequence.Unfold(0, generator, default(Func<int, bool>)));

            Assert.Throws<ArgumentNullException>("generator", () => Sequence.Unfold(0, null, resultSelector));
            Assert.Throws<ArgumentNullException>("resultSelector", () => Sequence.Unfold(0, generator, default(Func<int, int>)));

            Assert.Throws<ArgumentNullException>("generator", () => Sequence.Unfold(0, null, resultSelector, predicate));
            Assert.Throws<ArgumentNullException>("resultSelector", () => Sequence.Unfold(0, generator, default(Func<int, int>), predicate));
            Assert.Throws<ArgumentNullException>("predicate", () => Sequence.Unfold(0, generator, resultSelector, null));
        }

        [t("Unfold() uses deferred (streaming) execution (1).")]
        public static void Unfold1() {
            Func<int, (int, int)> generator = _ => throw new InvalidOperationException();

            Assert.DoesNotThrow(() => Sequence.Unfold(0, generator));
            Assert.DoesNotThrow(() => Sequence.Unfold(0, generator, _ => true));
        }

        [t("Unfold() uses deferred (streaming) execution (2).")]
        public static void Unfold1b() {
            Func<int, int> generator = _ => throw new InvalidOperationException();
            Func<int, int> resultSelector = _ => throw new InvalidOperationException();
            Func<int, bool> predicate = _ => throw new InvalidOperationException();

            var q1 = Assert.DoesNotThrow(() => Sequence.Unfold(0, generator));
            Assert.ThrowsAfter(q1, 1);

            var q2 = Assert.DoesNotThrow(() => Sequence.Unfold(0, generator, _ => true));
            Assert.ThrowsAfter(q2, 1);
            var q3 = Assert.DoesNotThrow(() => Sequence.Unfold(0, i => i + 1, predicate));
            Assert.ThrowsOnNext(q3);

            var q4 = Assert.DoesNotThrow(() => Sequence.Unfold(0, generator, i => i + 1));
            Assert.ThrowsAfter(q4, 1);
            var q5 = Assert.DoesNotThrow(() => Sequence.Unfold(0, i => i + 1, resultSelector));
            Assert.ThrowsOnNext(q5);

            var q6 = Assert.DoesNotThrow(() => Sequence.Unfold(0, generator, i => i + 1, _ => true));
            Assert.ThrowsAfter(q6, 1);
            var q7 = Assert.DoesNotThrow(() => Sequence.Unfold(0, i => i + 1, resultSelector, _ => true));
            Assert.ThrowsOnNext(q7);
            var q8 = Assert.DoesNotThrow(() => Sequence.Unfold(0, i => i + 1, i => i + 1, predicate));
            Assert.ThrowsOnNext(q8);
        }
    }
}
