﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    using Xunit;

    public static class SwitchFacts
    {
        #region LeftOrNone

        [Fact]
        public static void LeftOrNone_ReturnsNone_ForEmptySwitch()
        {
            // Arrange
            var sw = Switch<string, string>.Empty;

            // Act
            var result = sw.LeftOrNone();

            // Assert
            Assert.False(result.IsSome);
        }

        [Fact]
        public static void LeftOrNone_ReturnsNone_ForRightSwitch()
        {
            // Arrange
            var sw = Switch.Right<string, string>("leftValue");

            // Act
            var result = sw.LeftOrNone();

            // Assert
            Assert.False(result.IsSome);
        }

        [Fact]
        public static void LeftOrNone_ReturnsSome_ForLeftSwitch()
        {
            // Arrange
            var leftValue = "leftValue";
            var sw = Switch.Left<string, string>(leftValue);
            var expectedResult = Maybe.Of(leftValue);

            // Act
            var result = sw.LeftOrNone();

            // Assert
            Assert.True(result.IsSome);
            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region RightOrNone

        [Fact]
        public static void RightOrNone_ReturnsNone_ForEmptySwitch()
        {
            // Arrange
            var sw = Switch<string, string>.Empty;

            // Act
            var result = sw.RightOrNone();

            // Assert
            Assert.False(result.IsSome);
        }

        [Fact]
        public static void RightOrNone_ReturnsNone_ForLeftSwitch()
        {
            // Arrange
            var sw = Switch.Left<string, string>("rightValue");

            // Act
            var result = sw.RightOrNone();

            // Assert
            Assert.False(result.IsSome);
        }

        [Fact]
        public static void RightOrNone_ReturnsSome_ForRightSwitch()
        {
            // Arrange
            var rightValue = "rightValue";
            var sw = Switch.Right<string, string>(rightValue);
            var expectedResult = Maybe.Of(rightValue);

            // Act
            var result = sw.RightOrNone();

            // Assert
            Assert.True(result.IsSome);
            Assert.Equal(expectedResult, result);
        }

        #endregion
    }
}
