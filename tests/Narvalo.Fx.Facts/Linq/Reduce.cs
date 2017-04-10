// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    public partial class QperatorsFacts {
        [t("Reduce() guards.")]
        public static void Reduce0() {
            Func<int, int, int> accumulator = (i, j) => i + j;
            Func<int, bool> predicate = _ => true;

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.Reduce(accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Reduce(null, predicate));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Reduce(accumulator, null));
        }
    }

    // Reduce for Either.
    public partial class QperatorsFacts {
        [t("Reduce() guards (Either).")]
        public static void Reduce0a() {
            Func<int, int, Either<int, int>> accumulator = (i, j) => Either<int, int>.OfLeft(i + j);
            Func<Either<int, int>, bool> predicate = _ => true;

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.Reduce(accumulator));
            Assert.Throws<ArgumentNullException>("source", () => nil.Reduce(accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Reduce(default(Func<int, int, Either<int, int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Reduce(accumulator, null));
        }
    }

    // Reduce for Fallible.
    public partial class QperatorsFacts {
        [t("Reduce() guards (Fallible).")]
        public static void Reduce0b() {
            Func<int, int, Fallible<int>> accumulator = (i, j) => Fallible.Of(i + j);
            Func<Fallible<int>, bool> predicate = _ => true;

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.Reduce(accumulator));
            Assert.Throws<ArgumentNullException>("source", () => nil.Reduce(accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Reduce(default(Func<int, int, Fallible<int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Reduce(accumulator, null));
        }
    }

    // Reduce for Maybe.
    public partial class QperatorsFacts {
        [t("Reduce() guards (Maybe).")]
        public static void Reduce0c() {
            Func<int, int, Maybe<int>> accumulator = (i, j) => Maybe.Of(i + j);
            Func<Maybe<int>, bool> predicate = _ => true;

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.Reduce(accumulator));
            Assert.Throws<ArgumentNullException>("source", () => nil.Reduce(accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Reduce(default(Func<int, int, Maybe<int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Reduce(accumulator, null));
        }
    }

    // Reduce for Outcome.
    public partial class QperatorsFacts {
        [t("Reduce() guards (Outcome).")]
        public static void Reduce0d() {
            Func<int, int, Outcome<int>> accumulator = (i, j) => Outcome.Of(i + j);
            Func<Outcome<int>, bool> predicate = _ => true;

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.Reduce(accumulator));
            Assert.Throws<ArgumentNullException>("source", () => nil.Reduce(accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Reduce(default(Func<int, int, Outcome<int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Reduce(accumulator, null));
        }
    }

    // Reduce for Result.
    public partial class QperatorsFacts {
        [t("Reduce() guards (Result).")]
        public static void Reduce0e() {
            Func<int, int, Result<int, int>> accumulator = (i, j) => Result<int, int>.Of(i + j);
            Func<Result<int, int>, bool> predicate = _ => true;

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.Reduce(accumulator));
            Assert.Throws<ArgumentNullException>("source", () => nil.Reduce(accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Reduce(default(Func<int, int, Result<int, int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Reduce(accumulator, null));
        }
    }
}
