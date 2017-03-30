// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;

    using Xunit;

    using static global::My;

    // Tests for Either<TLeft, TRight>.
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

    // Tests for the monadic methods.
    public static partial class EitherFacts {
        [t("SelectLeft() guards.")]
        public static void SelectLeft0() {
            Either<int, int> nil = null;
            Func<int, int> selector = x => x;
            Assert.Throws<ArgumentNullException>("this", () => nil.SelectLeft(selector));

            Either<int, int> either = Either<int, int>.OfLeft(1);
            Assert.Throws<ArgumentNullException>("selector", () => either.SelectLeft(default(Func<int, int>)));
        }

        [t("SelectRight() guards.")]
        public static void SelectRight0() {
            Either<int, int> nil = null;
            Func<int, int> selector = x => x;
            Assert.Throws<ArgumentNullException>("this", () => nil.SelectRight(selector));

            Either<int, int> either = Either<int, int>.OfLeft(1);
            Assert.Throws<ArgumentNullException>("selector", () => either.SelectRight(default(Func<int, int>)));
        }

        [t("Flatten() returns inner 'orientation' if lefty.")]
        public static void Flatten1() {
            var leftleft = Either<Either<int, int>, int>.OfLeft(Either<int, int>.OfLeft(1));
            var result1 = leftleft.Flatten();
            Assert.True(result1.IsLeft);

            var leftright = Either<Either<int, int>, int>.OfLeft(Either<int, int>.OfRight(1));
            var result2 = leftright.Flatten();
            Assert.True(result2.IsRight);
        }

        [t("Flatten() returns righty if righty.")]
        public static void Flatten2() {
            var right = Either<Either<int, int>, int>.OfRight(1);
            var result = right.Flatten();
            Assert.True(result.IsRight);
        }

        [t("FlattenLeft() returns inner 'orientation' if lefty.")]
        public static void FlattenLeft1() {
            var leftleft = Either<Either<int, int>, int>.OfLeft(Either<int, int>.OfLeft(1));
            var result1 = Either.FlattenLeft(leftleft);
            Assert.True(result1.IsLeft);

            var leftright = Either<Either<int, int>, int>.OfLeft(Either<int, int>.OfRight(1));
            var result2 = Either.FlattenLeft(leftright);
            Assert.True(result2.IsRight);
        }

        [t("FlattenLeft() returns righty if righty.")]
        public static void FlattenLeft2() {
            var right = Either<Either<int, int>, int>.OfRight(1);
            var result = Either.FlattenLeft(right);
            Assert.True(result.IsRight);
        }

        [t("FlattenRight() returns inner 'orientation' if righty.")]
        public static void FlattenRight1() {
            var rightright = Either<int, Either<int, int>>.OfRight(Either<int, int>.OfRight(1));
            var result1 = Either.FlattenRight(rightright);
            Assert.True(result1.IsRight);

            var rightleft = Either<int, Either<int, int>>.OfRight(Either<int, int>.OfLeft(1));
            var result2 = Either.FlattenRight(rightleft);
            Assert.True(result2.IsLeft);
        }

        [t("FlattenRight() returns lefty if lefty.")]
        public static void FlattenRight2() {
            var left = Either<int, Either<int, int>>.OfLeft(1);
            var result = Either.FlattenRight(left);
            Assert.True(result.IsLeft);
        }
    }
}
