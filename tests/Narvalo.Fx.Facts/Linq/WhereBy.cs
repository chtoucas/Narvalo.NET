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

    // WhereBy for Either.
    public partial class QperatorsFacts {
        [t("WhereBy() guards (Either).")]
        public static void WhereBy0a() {
            Func<int, Either<bool, int>> predicate = _ => Either<bool, int>.OfLeft(true);

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.WhereBy(predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereBy(default(Func<int, Either<bool, int>>)));
        }

        [t("WhereBy() uses deferred execution (Either).")]
        public static void WhereBy1a() {
            bool notCalled = true;
            Func< Either<bool, int>> fun
                = () => { notCalled = false; return Either<bool, int>.OfLeft(true); };
            var q = Enumerable.Repeat(fun, 1).WhereBy(f => f());

            Assert.True(notCalled);
            q.OnLeft(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnRight(x => Assert.Fail());
        }
    }

    // WhereBy for Fallible.
    public partial class QperatorsFacts {
        [t("WhereBy() guards (Fallible).")]
        public static void WhereBy0b() {
            Func<int, Fallible<bool>> predicate = _ => Fallible.Of(true);

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.WhereBy(predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereBy(default(Func<int, Fallible<bool>>)));
        }

        [t("WhereBy() uses deferred execution (Fallible).")]
        public static void WhereBy1b() {
            bool notCalled = true;
            Func<Fallible<bool>> fun = () => { notCalled = false; return Fallible.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereBy(f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }

    // WhereBy for Maybe.
    public partial class QperatorsFacts {
        [t("WhereBy() guards (Maybe).")]
        public static void WhereBy0c() {
            Func<int, Maybe<bool>> predicate = _ => Maybe.Of(true);

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.WhereBy(predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereBy(default(Func<int, Maybe<bool>>)));
        }

        [t("WhereBy() uses deferred execution (Maybe).")]
        public static void WhereBy1c() {
            bool notCalled = true;
            Func<Maybe<bool>> fun = () => { notCalled = false; return Maybe.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereBy(f => f());

            Assert.True(notCalled);
            q.OnSome(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnNone(() => Assert.Fail());
        }
    }

    // WhereBy for Outcome.
    public partial class QperatorsFacts {
        [t("WhereBy() guards (Outcome).")]
        public static void WhereBy0d() {
            Func<int, Outcome<bool>> predicate = _ => Outcome.Of(true);

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.WhereBy(predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereBy(default(Func<int, Outcome<bool>>)));
        }

        [t("WhereBy() uses deferred execution (Outcome).")]
        public static void WhereBy1d() {
            bool notCalled = true;
            Func<Outcome<bool>> fun = () => { notCalled = false; return Outcome.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereBy(f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }

    // WhereBy for Result.
    public partial class QperatorsFacts {
        [t("WhereBy() guards (Result).")]
        public static void WhereBy0e() {
            Func<int, Result<bool, int>> predicate = _ => Result<bool, int>.Of(true);

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.WhereBy(predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereBy(default(Func<int, Result<bool, int>>)));
        }

        [t("WhereBy() uses deferred execution (Result).")]
        public static void WhereBy1e() {
            bool notCalled = true;
            Func<Result<bool, int>> fun
                = () => { notCalled = false; return Result<bool, int>.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereBy(f => f());

            Assert.True(notCalled);
            q.OnSuccess(x => Assert.CalledOnNext(x, ref notCalled));
            q.OnError(x => Assert.Fail());
        }
    }
}
