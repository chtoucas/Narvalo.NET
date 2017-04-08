// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

/*
 * DISABLED
 */
namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    // ZipWith for Either.
    public partial class QperatorsFacts {
        [t("ZipWith() uses deferred execution (Either).")]
        public static void ZipWith1a() {
            IEnumerable<int> first = new ThrowingEnumerable<int>();
            var second = Enumerable.Range(0, 1);
            Func<int, int, Either<int, int>> resultSelector = (i, j) => Either<int, int>.OfLeft(i + j);

            var q = Assert.DoesNotThrow(() => first.ZipWith(second, resultSelector));
            q.OnLeft(x => Assert.ThrowsOnNext(x));
            q.OnRight(x => Assert.Fail());
        }
    }

    // ZipWith for Fallible.
    public partial class QperatorsFacts {
        [t("ZipWith() uses deferred execution (Fallible).")]
        public static void ZipWith1b() {
            IEnumerable<int> first = new ThrowingEnumerable<int>();
            var second = Enumerable.Range(0, 1);
            Func<int, int, Fallible<int>> resultSelector = (i, j) => Fallible.Of(i + j);

            var q = Assert.DoesNotThrow(() => first.ZipWith(second, resultSelector));
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }

    // ZipWith for Maybe.
    public partial class QperatorsFacts {
        [t("ZipWith() uses deferred execution (Maybe).")]
        public static void ZipWith1c() {
            IEnumerable<int> first = new ThrowingEnumerable<int>();
            var second = Enumerable.Range(0, 1);
            Func<int, int, Maybe<int>> resultSelector = (i, j) => Maybe.Of(i + j);

            var q = Assert.DoesNotThrow(() => first.ZipWith(second, resultSelector));
            q.OnSome(x => Assert.ThrowsOnNext(x));
            q.OnNone(() => Assert.Fail());
        }
    }

    // ZipWith for Outcome.
    public partial class QperatorsFacts {
        [t("ZipWith() uses deferred execution (Outcome).")]
        public static void ZipWith1d() {
            IEnumerable<int> first = new ThrowingEnumerable<int>();
            var second = Enumerable.Range(0, 1);
            Func<int, int, Outcome<int>> resultSelector = (i, j) => Outcome.Of(i + j);

            var q = Assert.DoesNotThrow(() => first.ZipWith(second, resultSelector));
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }

    // ZipWith for Result.
    public partial class QperatorsFacts {
        [t("ZipWith() uses deferred execution (Result).")]
        public static void ZipWith1e() {
            IEnumerable<int> first = new ThrowingEnumerable<int>();
            var second = Enumerable.Range(0, 1);
            Func<int, int, Result<int, int>> resultSelector = (i, j) => Result<int, int>.Of(i + j);

            var q = Assert.DoesNotThrow(() => first.ZipWith(second, resultSelector));
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }
}
