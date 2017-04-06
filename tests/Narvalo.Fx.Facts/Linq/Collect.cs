// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    using Assert = Narvalo.AssertExtended;

    // Collect for Either.
    public partial class QperatorsFacts {
        [t("Collect() guards (Either).")]
        public static void Collect0a() {
            IEnumerable<Either<int, int>> nil = null;

            Assert.Throws<ArgumentNullException>("source", () => nil.Collect());
        }

        [t("Collect() uses deferred execution (Either).")]
        public static void Collect1a() {
            IEnumerable<Either<int, int>> source = new ThrowingEnumerable<Either<int, int>>();

            var q = Assert.DoesNotThrow(() => source.Collect());
            q.OnLeft(x => Assert.ThrowsOnNext(x));
            q.OnRight(x => Assert.Fail());
        }
    }

    // Collect for Fallible.
    public partial class QperatorsFacts {
        [t("Collect() guards (Fallible).")]
        public static void Collect0b() {
            IEnumerable<Fallible<int>> nil = null;

            Assert.Throws<ArgumentNullException>("source", () => nil.Collect());
        }

        [t("Collect() uses deferred execution (Fallible).")]
        public static void Collect1b() {
            IEnumerable<Fallible<int>> source = new ThrowingEnumerable<Fallible<int>>();

            var q = Assert.DoesNotThrow(() => source.Collect());
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }

    // Collect for Maybe.
    public partial class QperatorsFacts {
        [t("Collect() guards (Maybe).")]
        public static void Collect0c() {
            IEnumerable<Maybe<int>> nil = null;

            Assert.Throws<ArgumentNullException>("source", () => nil.Collect());
        }

        [t("Collect() uses deferred execution (Maybe).")]
        public static void Collect1c() {
            IEnumerable<Maybe<int>> source = new ThrowingEnumerable<Maybe<int>>();

            var q = Assert.DoesNotThrow(() => source.Collect());
            q.OnSome(x => Assert.ThrowsOnNext(x));
            q.OnNone(() => Assert.Fail());
        }
    }

    // Collect for Outcome.
    public partial class QperatorsFacts {
        [t("Collect() guards (Outcome).")]
        public static void Collect0d() {
            IEnumerable<Outcome<int>> nil = null;

            Assert.Throws<ArgumentNullException>("source", () => nil.Collect());
        }

        [t("Collect() uses deferred execution (Outcome).")]
        public static void Collect1d() {
            IEnumerable<Outcome<int>> source = new ThrowingEnumerable<Outcome<int>>();

            var q = Assert.DoesNotThrow(() => source.Collect());
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }

    // Collect for Result.
    public partial class QperatorsFacts {
        [t("Collect() guards (Result).")]
        public static void Collect0e() {
            IEnumerable<Result<int, int>> nil = null;

            Assert.Throws<ArgumentNullException>("source", () => nil.Collect());
        }

        [t("Collect() uses deferred execution (Result).")]
        public static void Collect1e() {
            IEnumerable<Result<int, int>> source = new ThrowingEnumerable<Result<int, int>>();

            var q = Assert.DoesNotThrow(() => source.Collect());
            q.OnSuccess(x => Assert.ThrowsOnNext(x));
            q.OnError(x => Assert.Fail());
        }
    }
}
