// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq.Applicative {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    // SelectAny for Either.
    public partial class AperatorsFacts {
        [t("SelectAny() guards (Either).")]
        public static void SelectAny0a() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.SelectAny(x => Either<int, int>.OfLeft(x)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, Either<int, int>>)));
        }

        [t("SelectAny() uses deferred execution (Either).")]
        public static void SelectAny1a() {
            bool notCalled = true;
            Func<Either<int, int>> fun = () => { notCalled = false; return Either<int, int>.OfLeft(1); };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }

    // SelectAny for Fallible.
    public partial class AperatorsFacts {
        [t("SelectAny() guards (Fallible).")]
        public static void SelectAny0b() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.SelectAny(x => Maybe.Of(x)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, Fallible<int>>)));
        }

        [t("SelectAny() uses deferred execution (Fallible).")]
        public static void SelectAny1b() {
            bool notCalled = true;
            Func<Fallible<int>> fun = () => { notCalled = false; return Fallible.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }

    // SelectAny for Maybe.
    public partial class AperatorsFacts {
        [t("SelectAny() guards (Maybe).")]
        public static void SelectAny0c() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.SelectAny(x => Fallible.Of(x)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, Maybe<int>>)));
        }

        [t("SelectAny() uses deferred execution (Maybe).")]
        public static void SelectAny1c() {
            bool notCalled = true;
            Func<Maybe<int>> fun = () => { notCalled = false; return Maybe.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }

    // SelectAny for Outcome.
    public partial class AperatorsFacts {
        [t("SelectAny() guards (Outcome).")]
        public static void SelectAny0d() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.SelectAny(x => Outcome.Of(x)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, Outcome<int>>)));
        }

        [t("SelectAny() uses deferred execution (Outcome).")]
        public static void SelectAny1d() {
            bool notCalled = true;
            Func<Outcome<int>> fun = () => { notCalled = false; return Outcome.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }

    // SelectAny for Result.
    public partial class AperatorsFacts {
        [t("SelectAny() guards (Result).")]
        public static void SelectAny0e() {
            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.SelectAny(x => Result<int, int>.Of(x)));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectAny(default(Func<int, Result<int, int>>)));
        }

        [t("SelectAny() uses deferred execution (Result).")]
        public static void SelectAny1e() {
            bool notCalled = true;
            Func<Result<int, int>> fun = () => { notCalled = false; return Result<int, int>.Of(1); };
            var q = Enumerable.Repeat(fun, 1).SelectAny(f => f());

            Assert.True(notCalled);
            Assert.CalledOnNext(q, ref notCalled);
        }
    }
}
