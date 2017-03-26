// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using Xunit;

    public static partial class EitherFacts {
        internal sealed class factAttribute : FactAttribute_ {
            public factAttribute(string message) : base(nameof(Either), message) { }
        }

        #region LeftOrNone

        [fact("")]
        public static void LeftOrNone_ReturnsNone_WhenRightEither() {
            // Arrange
            var either = Either<string, string>.OfRight("leftValue");

            // Act
            var result = either.LeftOrNone();

            // Assert
            Assert.False(result.IsSome);
        }

        [fact("")]
        public static void LeftOrNone_ReturnsSome_WhenLeftEither() {
            // Arrange
            var leftValue = "leftValue";
            var either = Either<string, string>.OfLeft(leftValue);
            var expectedResult = Maybe.Of(leftValue);

            // Act
            var result = either.LeftOrNone();

            // Assert
            Assert.True(result.IsSome);
            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region RightOrNone

        [fact("")]
        public static void RightOrNone_ReturnsNone_WhenLeftEither() {
            // Arrange
            var either = Either<string, string>.OfLeft("rightValue");

            // Act
            var result = either.RightOrNone();

            // Assert
            Assert.False(result.IsSome);
        }

        [fact("")]
        public static void RightOrNone_ReturnsSome_WhenRightEither() {
            // Arrange
            var rightValue = "rightValue";
            var either = Either<string, string>.OfRight(rightValue);
            var expectedResult = Maybe.Of(rightValue);

            // Act
            var result = either.RightOrNone();

            // Assert
            Assert.True(result.IsSome);
            Assert.Equal(expectedResult, result);
        }

        #endregion
    }
}
