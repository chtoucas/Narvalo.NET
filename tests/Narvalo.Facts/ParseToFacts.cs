// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using Xunit;

    public static class ParseToFacts
    {
        public static class TheBooleanMethod
        {
            [Fact]
            public static void ReturnsNull_ForNullString()
            {
                // Act
                bool? result = ParseTo.Boolean(null, BooleanStyles.Default);

                // Assert
                Assert.False(result.HasValue);
            }

            [Fact]
            public static void ReturnsNull_ForEmptyString()
            {
                // Act
                bool? result = ParseTo.Boolean(String.Empty, BooleanStyles.ZeroOrOne);

                // Assert
                Assert.False(result.HasValue);
            }

            [Fact]
            public static void ReturnsTrue_ForEmptyStringAndEmptyIsFalse()
            {
                // Act
                bool? result = ParseTo.Boolean(String.Empty, BooleanStyles.EmptyIsFalse);

                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsTrue_ForLiteralTrueAndLiteralStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("true", BooleanStyles.Literal);

                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(true, result);
            }

            [Fact]
            public static void ReturnsTrue_ForLiteralMixedCaseTrueAndLiteralStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("TrUe", BooleanStyles.Literal);

                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(true, result);
            }

            [Fact]
            public static void ReturnsTrue_ForLiteralFalseAndLiteralStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("false", BooleanStyles.Literal);

                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsTrue_ForLiteralMixedCaseFalseAndLiteralStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("fAlSe", BooleanStyles.Literal);

                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsTrue_ForLiteralTrueAndWhiteSpacesAndLiteralStyle()
            {
                // Act
                bool? result = ParseTo.Boolean(" true ", BooleanStyles.Literal);

                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(true, result);
            }

            [Fact]
            public static void ReturnsNull_ForStrictlyPositiveInt32AndIntegerStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("10", BooleanStyles.ZeroOrOne);

                // Assert
                Assert.False(result.HasValue);
            }

            [Fact]
            public static void ReturnsTrueAndPicksTrue_ForOneAndIntegerStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("1", BooleanStyles.ZeroOrOne);

                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(true, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksFalse_ForZeroAndIntegerStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("0", BooleanStyles.ZeroOrOne);

                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsNull_ForMinusOneAndIntegerStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("-1", BooleanStyles.ZeroOrOne);

                // Assert
                Assert.False(result.HasValue);
            }

            [Fact]
            public static void ReturnsNull_ForNegativeInt32AndIntegerStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("-10", BooleanStyles.ZeroOrOne);

                // Assert
                Assert.False(result.HasValue);
            }

            [Fact]
            public static void ReturnsNull_ForDecimalAndIntegerStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("-10.1", BooleanStyles.ZeroOrOne);

                // Assert
                Assert.False(result.HasValue);
            }
        }
    }
}
