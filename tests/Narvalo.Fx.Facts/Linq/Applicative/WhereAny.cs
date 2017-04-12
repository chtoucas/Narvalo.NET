// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq.Applicative {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    // WhereAny for Either.
    public partial class EnumerableFacts {
        [t("WhereAny() guards (Either).")]
        public static void WhereAny0a() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.WhereAny(_ => Either<bool, int>.OfLeft(true)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, Either<bool, int>>)));
        }

        [t("WhereAny() uses deferred execution (Either).")]
        public static void WhereAny1a() {
            bool notCalled = true;
            Func<Either<bool, int>> fun = () => { notCalled = false; return Either<bool, int>.OfLeft(true); };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }

    // WhereAny for Fallible.
    public partial class EnumerableFacts {
        [t("WhereAny() guards (Fallible).")]
        public static void WhereAny0b() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.WhereAny(_ => Fallible.Of(true)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, Fallible<bool>>)));
        }

        [t("WhereAny() uses deferred execution (Fallible).")]
        public static void WhereAny1b() {
            bool notCalled = true;
            Func<Fallible<bool>> fun = () => { notCalled = false; return Fallible.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }

    // WhereAny for Maybe.
    public partial class EnumerableFacts {
        [t("WhereAny() guards (Maybe).")]
        public static void WhereAny0c() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.WhereAny(_ => Maybe.Of(true)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, Maybe<bool>>)));
        }

        [t("WhereAny() uses deferred execution (Maybe).")]
        public static void WhereAny1c() {
            bool notCalled = true;
            Func<Maybe<bool>> fun = () => { notCalled = false; return Maybe.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }

    // WhereAny for Outcome.
    public partial class EnumerableFacts {
        [t("WhereAny() guards (Outcome).")]
        public static void WhereAny0d() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.WhereAny(_ => Outcome.Of(true)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, Outcome<bool>>)));
        }

        [t("WhereAny() uses deferred execution (Outcome).")]
        public static void WhereAny1d() {
            bool notCalled = true;
            Func<Outcome<bool>> fun = () => { notCalled = false; return Outcome.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }

    // WhereAny for Result.
    public partial class EnumerableFacts {
        [t("WhereAny() guards (Result).")]
        public static void WhereAny0e() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.WhereAny(_ => Result<bool, int>.Of(true)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("predicate", () => source.WhereAny(default(Func<int, Result<bool, int>>)));
        }

        [t("WhereAny() uses deferred execution (Result).")]
        public static void WhereAny1e() {
            bool notCalled = true;
            Func<Result<bool, int>> fun = () => { notCalled = false; return Result<bool, int>.Of(true); };
            var q = Enumerable.Repeat(fun, 1).WhereAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }
}
