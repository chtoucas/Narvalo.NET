// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using Xunit;

    public static class EitherFacts
    {
        #region LeftOrNone

        [Fact]
        public static void LeftOrNone_ReturnsNone_WhenRightEither()
        {
            // Arrange
            var either = Either.Right<string, string>("leftValue");

            // Act
            var result = either.LeftOrNone();

            // Assert
            Assert.False(result.IsSome);
        }

        [Fact]
        public static void LeftOrNone_ReturnsSome_WhenLeftEither()
        {
            // Arrange
            var leftValue = "leftValue";
            var either = Either.Left<string, string>(leftValue);
            var expectedResult = Maybe.Of(leftValue);

            // Act
            var result = either.LeftOrNone();

            // Assert
            Assert.True(result.IsSome);
            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region RightOrNone

        [Fact]
        public static void RightOrNone_ReturnsNone_WhenLeftEither()
        {
            // Arrange
            var either = Either.Left<string, string>("rightValue");

            // Act
            var result = either.RightOrNone();

            // Assert
            Assert.False(result.IsSome);
        }

        [Fact]
        public static void RightOrNone_ReturnsSome_WhenRightEither()
        {
            // Arrange
            var rightValue = "rightValue";
            var either = Either.Right<string, string>(rightValue);
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
