// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class ParseToFacts
    {
        [Fact]
        public static void Boolean_ReturnsNull_ForNullString()
        {
            // Act
            bool? result = ParseTo.Boolean(null, BooleanStyles.Default);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Boolean_ReturnsNull_ForEmptyString()
        {
            // Act
            bool? result = ParseTo.Boolean(String.Empty, BooleanStyles.ZeroOrOne);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Boolean_ReturnsTrue_ForEmptyStringAndEmptyIsFalse()
        {
            // Act
            bool? result = ParseTo.Boolean(String.Empty, BooleanStyles.EmptyIsFalse);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(false, result);
        }

        [Fact]
        public static void Boolean_ReturnsTrue_ForLiteralTrueAndLiteralStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("true", BooleanStyles.Literal);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(true, result);
        }

        [Fact]
        public static void Boolean_ReturnsTrue_ForLiteralMixedCaseTrueAndLiteralStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("TrUe", BooleanStyles.Literal);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(true, result);
        }

        [Fact]
        public static void Boolean_ReturnsTrue_ForLiteralFalseAndLiteralStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("false", BooleanStyles.Literal);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(false, result);
        }

        [Fact]
        public static void Boolean_ReturnsTrue_ForLiteralMixedCaseFalseAndLiteralStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("fAlSe", BooleanStyles.Literal);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(false, result);
        }

        [Fact]
        public static void Boolean_ReturnsTrue_ForLiteralTrueAndWhiteSpacesAndLiteralStyle()
        {
            // Act
            bool? result = ParseTo.Boolean(" true ", BooleanStyles.Literal);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(true, result);
        }

        [Fact]
        public static void Boolean_ReturnsNull_ForStrictlyPositiveInt32AndIntegerStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("10", BooleanStyles.ZeroOrOne);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Boolean_ReturnsTrueAndPicksTrue_ForOneAndIntegerStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("1", BooleanStyles.ZeroOrOne);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(true, result);
        }

        [Fact]
        public static void Boolean_ReturnsTrueAndPicksFalse_ForZeroAndIntegerStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("0", BooleanStyles.ZeroOrOne);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(false, result);
        }

        [Fact]
        public static void Boolean_ReturnsNull_ForMinusOneAndIntegerStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("-1", BooleanStyles.ZeroOrOne);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Boolean_ReturnsNull_ForNegativeInt32AndIntegerStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("-10", BooleanStyles.ZeroOrOne);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Boolean_ReturnsNull_ForDecimalAndIntegerStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("-10.1", BooleanStyles.ZeroOrOne);

            // Assert
            Assert.False(result.HasValue);
        }
    }
}
