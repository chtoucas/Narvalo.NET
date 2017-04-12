// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Assert = Narvalo.AssertExtended;

    // Zip for Either.
    public partial class EnumerableFacts {
        [t("Zip() uses deferred execution (Either).")]
        public static void Zip1a() {
            IEnumerable<int> first = new ThrowingEnumerable<int>();
            var second = Enumerable.Range(0, 1);
            Func<int, int, Either<int, int>> resultSelector = (i, j) => Either<int, int>.OfLeft(i + j);

            var q = Assert.DoesNotThrow(() => Either.Zip(first, second, resultSelector));
            q.OnLeft(x => Assert.ThrowsOnNext(x));
            q.OnRight(x => Assert.Fail());
        }
    }

    // Zip for Fallible.
    public partial class EnumerableFacts {
        [t("Zip() uses deferred execution (Fallible).")]
        public static void Zip1b() {
            IEnumerable<int> first = new ThrowingEnumerable<int>();
            var second = Enumerable.Range(0, 1);
            Func<int, int, Fallible<int>> resultSelector = (i, j) => Fallible.Of(i + j);

            var q = Assert.DoesNotThrow(() => Fallible.Zip(first, second, resultSelector));
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }

    // Zip for Maybe.
    public partial class EnumerableFacts {
        [t("Zip() uses deferred execution (Maybe).")]
        public static void Zip1c() {
            IEnumerable<int> first = new ThrowingEnumerable<int>();
            var second = Enumerable.Range(0, 1);
            Func<int, int, Maybe<int>> resultSelector = (i, j) => Maybe.Of(i + j);

            var q = Assert.DoesNotThrow(() => Maybe.Zip(first, second, resultSelector));
            q.OnSome(x => Assert.ThrowsOnNext(x));
            q.OnNone(() => Assert.Fail());
        }
    }

    // Zip for Outcome.
    public partial class EnumerableFacts {
        [t("Zip() uses deferred execution (Outcome).")]
        public static void Zip1d() {
            IEnumerable<int> first = new ThrowingEnumerable<int>();
            var second = Enumerable.Range(0, 1);
            Func<int, int, Outcome<int>> resultSelector = (i, j) => Outcome.Of(i + j);

            var q = Assert.DoesNotThrow(() => Outcome.Zip(first, second, resultSelector));
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }

    // Zip for Result.
    public partial class EnumerableFacts {
        [t("Zip() uses deferred execution (Result).")]
        public static void Zip1e() {
            IEnumerable<int> first = new ThrowingEnumerable<int>();
            var second = Enumerable.Range(0, 1);
            Func<int, int, Result<int, int>> resultSelector = (i, j) => Result<int, int>.Of(i + j);

            var q = Assert.DoesNotThrow(() => Result.Zip(first, second, resultSelector));
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }
}
