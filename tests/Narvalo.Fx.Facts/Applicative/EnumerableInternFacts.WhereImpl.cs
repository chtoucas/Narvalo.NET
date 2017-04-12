// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#if !NO_INTERNALS_VISIBLE_TO

namespace Narvalo.Applicative {
    using System;
    using System.Linq;

    using Assert = Narvalo.AssertExtended;

    // WhereImpl for Either.
    public partial class EnumerableInternFacts {
        [t("WhereImpl() uses deferred execution (Either).")]
        public static void WhereImpl1a() {
            bool notCalled = true;
            Func<Either<bool, int>> fun
                = () => { notCalled = false; return Either<bool, int>.OfLeft(true); };
            var q = Enumerable.Repeat(fun, 1).WhereImpl(f => f());

            Assert.True(notCalled);
            q.OnLeft(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnRight(x => Assert.Fail());
        }
    }

    // WhereImpl for Fallible.
    public partial class EnumerableInternFacts {
        [t("WhereImpl() uses deferred execution (Fallible).")]
        public static void WhereImpl1b() {
            bool notCalled = true;
            Func<Fallible<bool>> fun = () => { notCalled = false; return Fallible.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereImpl(f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }

    // WhereImpl for Maybe.
    public partial class EnumerableInternFacts {
        [t("WhereImpl() uses deferred execution (Maybe).")]
        public static void WhereImpl1c() {
            bool notCalled = true;
            Func<Maybe<bool>> fun = () => { notCalled = false; return Maybe.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereImpl(f => f());

            Assert.True(notCalled);
            q.OnSome(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnNone(() => Assert.Fail());
        }
    }

    // WhereImpl for Outcome.
    public partial class EnumerableInternFacts {
        [t("WhereImpl() uses deferred execution (Outcome).")]
        public static void WhereImpl1d() {
            bool notCalled = true;
            Func<Outcome<bool>> fun = () => { notCalled = false; return Outcome.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereImpl(f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }

    // WhereImpl for Result.
    public partial class EnumerableInternFacts {
        [t("WhereImpl() uses deferred execution (Result).")]
        public static void WhereImpl1e() {
            bool notCalled = true;
            Func<Result<bool, int>> fun
                = () => { notCalled = false; return Result<bool, int>.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereImpl(f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }
}

#endif