// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    // SelectWith for Either.
    public partial class QperatorsFacts {
        [t("SelectWith() uses deferred execution (Either).")]
        public static void SelectWith1a() {
            bool notCalled = true;
            Func<Either<int, int>> fun = () => { notCalled = false; return Either<int, int>.OfLeft(1); };
            var q = Enumerable.Repeat(fun, 1).SelectWith(f => f());

            Assert.True(notCalled);
            q.OnLeft(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnRight(x => Assert.Fail());
        }
    }

    // SelectWith for Fallible.
    public partial class QperatorsFacts {
        [t("SelectWith() uses deferred execution (Fallible).")]
        public static void SelectWith1b() {
            bool notCalled = true;
            Func<Fallible<int>> fun = () => { notCalled = false; return Fallible.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectWith(f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }

    // SelectWith for Maybe.
    public partial class QperatorsFacts {
        [t("SelectWith() uses deferred execution (Maybe).")]
        public static void SelectWith1c() {
            bool notCalled = true;
            Func<Maybe<int>> fun = () => { notCalled = false; return Maybe.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectWith(f => f());

            Assert.True(notCalled);
            q.OnSome(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnNone(() => Assert.Fail());
        }
    }

    // SelectWith for Outcome.
    public partial class QperatorsFacts {
        [t("SelectWith() uses deferred execution (Outcome).")]
        public static void SelectWith1d() {
            bool notCalled = true;
            Func<Outcome<int>> fun = () => { notCalled = false; return Outcome.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectWith(f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }

    // SelectWith for Result.
    public partial class QperatorsFacts {
        [t("SelectWith() uses deferred execution (Result).")]
        public static void SelectWith1e() {
            bool notCalled = true;
            Func<Result<int, int>> fun = () => { notCalled = false; return Result<int, int>.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectWith(f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }
}
