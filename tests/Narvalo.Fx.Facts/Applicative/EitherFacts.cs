// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using Xunit;

    using static global::My;

    public static partial class EitherFacts {
        [t("LeftOrNone() returns none if righty.")]
        public static void LeftOrNone1() {
            var either = Either<string, string>.OfRight("leftValue");

            var result = either.LeftOrNone();
            Assert.True(result.IsNone);
        }

        [t("LeftOrNone() returns some if lefty.")]
        public static void LeftOrNone2() {
            var value = new Obj("leftValue");
            var either = Either<Obj, string>.OfLeft(value);
            var exp = Maybe.Of(value);

            var result = either.LeftOrNone();
            Assert.True(result.IsSome);
            Assert.Equal(exp, result);
        }

        [t("RightOrNone() returns none if lefty.")]
        public static void RightOrNone1() {
            var either = Either<string, string>.OfLeft("rightValue");

            var result = either.RightOrNone();
            Assert.True(result.IsNone);
        }

        [t("RightOrNone() returns some if righty.")]
        public static void RightOrNone2() {
            var value = new Obj("rightValue");
            var either = Either<string, Obj>.OfRight(value);
            var exp = Maybe.Of(value);

            var result = either.RightOrNone();
            Assert.True(result.IsSome);
            Assert.Equal(exp, result);
        }
    }
}
