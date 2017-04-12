// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq.Applicative {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Xunit;

    // Fold for Either.
    public partial class EnumerableFacts {
        [t("Fold() guards (Either).")]
        public static void Fold0a() {
            int seed = 0;
            Func<int, int, Either<int, int>> accumulator = (i, j) => Either<int, int>.OfLeft(i + j);
            Func<Either<int, int>, bool> predicate = _ => true;

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.Fold(seed, accumulator));
            Assert.Throws<ArgumentNullException>("source", () => nil.Fold(seed, accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Fold(seed, default(Func<int, int, Either<int, int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Fold(seed, accumulator, null));
        }
    }

    // Fold for Fallible.
    public partial class EnumerableFacts {
        [t("Fold() guards (Fallible).")]
        public static void Fold0b() {
            int seed = 0;
            Func<int, int, Fallible<int>> accumulator = (i, j) => Fallible.Of(i + j);
            Func<Fallible<int>, bool> predicate = _ => true;

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.Fold(seed, accumulator));
            Assert.Throws<ArgumentNullException>("source", () => nil.Fold(seed, accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Fold(seed, default(Func<int, int, Fallible<int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Fold(seed, accumulator, null));
        }
    }

    // Fold for Maybe.
    public partial class EnumerableFacts {
        [t("Fold() guards (Maybe).")]
        public static void Fold0c() {
            int seed = 0;
            Func<int, int, Maybe<int>> accumulator = (i, j) => Maybe.Of(i + j);
            Func<Maybe<int>, bool> predicate = _ => true;

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.Fold(seed, accumulator));
            Assert.Throws<ArgumentNullException>("source", () => nil.Fold(seed, accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Fold(seed, default(Func<int, int, Maybe<int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Fold(seed, accumulator, null));
        }
    }

    // Fold for Outcome.
    public partial class EnumerableFacts {
        [t("Fold() guards (Outcome).")]
        public static void Fold0d() {
            int seed = 0;
            Func<int, int, Outcome<int>> accumulator = (i, j) => Outcome.Of(i + j);
            Func<Outcome<int>, bool> predicate = _ => true;

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.Fold(seed, accumulator));
            Assert.Throws<ArgumentNullException>("source", () => nil.Fold(seed, accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Fold(seed, default(Func<int, int, Outcome<int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Fold(seed, accumulator, null));
        }
    }

    // Fold for Result.
    public partial class EnumerableFacts {
        [t("Fold() guards (Result).")]
        public static void Fold0e() {
            int seed = 0;
            Func<int, int, Result<int, int>> accumulator = (i, j) => Result<int, int>.Of(i + j);
            Func<Result<int, int>, bool> predicate = _ => true;

            IEnumerable<int> nil = null;
            Assert.Throws<ArgumentNullException>("source", () => nil.Fold(seed, accumulator));
            Assert.Throws<ArgumentNullException>("source", () => nil.Fold(seed, accumulator, predicate));

            var source = Enumerable.Range(0, 1);
            Assert.Throws<ArgumentNullException>("accumulator", () => source.Fold(seed, default(Func<int, int, Result<int, int>>)));
            Assert.Throws<ArgumentNullException>("predicate", () => source.Fold(seed, accumulator, null));
        }
    }
}
