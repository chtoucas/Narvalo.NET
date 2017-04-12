// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Linq;
    using System.Linq.Tests;

    using Assert = Narvalo.AssertExtended;

    // Map for Either.
    public partial class EnumerableFacts : EnumerableTests {
        [t("Map() uses deferred execution (Either).")]
        public static void Map1a() {
            bool notCalled = true;
            Func<Either<int, int>> fun = () => { notCalled = false; return Either<int, int>.OfLeft(1); };
            var q = Either.Map(Enumerable.Repeat(fun, 1), f => f());

            Assert.True(notCalled);
            q.OnLeft(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnRight(x => Assert.Fail());
        }
    }

    // Map for Fallible.
    public partial class EnumerableFacts {
        [t("Map() uses deferred execution (Fallible).")]
        public static void Map1b() {
            bool notCalled = true;
            Func<Fallible<int>> fun = () => { notCalled = false; return Fallible.Of(1); };
            var q = Fallible.Map(Enumerable.Repeat(fun, 1), f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }

    // Map for Maybe.
    public partial class EnumerableFacts {
        [t("Map() uses deferred execution (Maybe).")]
        public static void Map1c() {
            bool notCalled = true;
            Func<Maybe<int>> fun = () => { notCalled = false; return Maybe.Of(1); };
            var q = Maybe.Map(Enumerable.Repeat(fun, 1), f => f());

            Assert.True(notCalled);
            q.OnSome(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnNone(() => Assert.Fail());
        }
    }

    // Map for Outcome.
    public partial class EnumerableFacts {
        [t("Map() uses deferred execution (Outcome).")]
        public static void Map1d() {
            bool notCalled = true;
            Func<Outcome<int>> fun = () => { notCalled = false; return Outcome.Of(1); };
            var q = Outcome.Map(Enumerable.Repeat(fun, 1), f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }

    // Map for Result.
    public partial class EnumerableFacts {
        [t("Map() uses deferred execution (Result).")]
        public static void Map1e() {
            bool notCalled = true;
            Func<Result<int, int>> fun = () => { notCalled = false; return Result<int, int>.Of(1); };
            var q = Result.Map(Enumerable.Repeat(fun, 1), f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }
}
