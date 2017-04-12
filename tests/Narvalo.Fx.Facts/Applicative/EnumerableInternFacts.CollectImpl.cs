// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#if !NO_INTERNALS_VISIBLE_TO

namespace Narvalo.Applicative {
    using System.Collections.Generic;

    using Assert = Narvalo.AssertExtended;

    // CollectImpl for Either.
    public partial class EnumerableInternFacts {
        [t("CollectImpl() uses deferred execution (Either).")]
        public static void CollectImpl1a() {
            IEnumerable<Either<int, int>> source = new ThrowingEnumerable<Either<int, int>>();

            var q = Assert.DoesNotThrow(() => source.CollectImpl());
            q.OnLeft(x => Assert.ThrowsOnNext(x));
            q.OnRight(x => Assert.Fail());
        }
    }

    // CollectImpl for Fallible.
    public partial class EnumerableInternFacts {
        [t("CollectImpl() uses deferred execution (Fallible).")]
        public static void CollectImpl1b() {
            IEnumerable<Fallible<int>> source = new ThrowingEnumerable<Fallible<int>>();

            var q = Assert.DoesNotThrow(() => source.CollectImpl());
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }

    // CollectImpl for Maybe.
    public partial class EnumerableInternFacts {
        [t("CollectImpl() uses deferred execution (Maybe).")]
        public static void CollectImpl1c() {
            IEnumerable<Maybe<int>> source = new ThrowingEnumerable<Maybe<int>>();

            var q = Assert.DoesNotThrow(() => source.CollectImpl());
            q.OnSome(x => Assert.ThrowsOnNext(x));
            q.OnNone(() => Assert.Fail());
        }
    }

    // CollectImpl for Outcome.
    public partial class EnumerableInternFacts {
        [t("CollectImpl() uses deferred execution (Outcome).")]
        public static void CollectImpl1d() {
            IEnumerable<Outcome<int>> source = new ThrowingEnumerable<Outcome<int>>();

            var q = Assert.DoesNotThrow(() => source.CollectImpl());
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }

    // CollectImpl for Result.
    public partial class EnumerableInternFacts {
        [t("CollectImpl() uses deferred execution (Result).")]
        public static void CollectImpl1e() {
            IEnumerable<Result<int, int>> source = new ThrowingEnumerable<Result<int, int>>();

            var q = Assert.DoesNotThrow(() => source.CollectImpl());
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }
}

#endif