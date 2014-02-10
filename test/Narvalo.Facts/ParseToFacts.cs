namespace Narvalo
{
    using System;
    using Xunit;

    public static class ParseToFacts
    {
        public static class TheBooleanMethod
        {
            //[Fact]
            //public static void ReturnsFalseAndPicksDefaultBoolean_ForNullString()
            //{
            //    // Act
            //    bool? result = ParseTo.Boolean(null, BooleanStyles.None);
            //    // Assert
            //    Assert.False(result.HasValue);
            //    Assert.Equal(default(Boolean), result);
            //}

            //[Fact]
            //public static void ReturnsFalseAndPicksDefaultBoolean_ForEmptyString()
            //{
            //    // Act
            //    bool? result = ParseTo.Boolean(String.Empty, BooleanStyles.None);
            //    // Assert
            //    Assert.False(result.HasValue);
            //    Assert.Equal(default(Boolean), result);
            //}

            [Fact]
            public static void ReturnsTrueAndPicksFalse_ForEmptyStringAndEmptyIsFalse()
            {
                // Act
                bool? result = ParseTo.Boolean(String.Empty, BooleanStyles.EmptyIsFalse);
                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksTrue_ForLiteralTrueAndLiteralStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("true", BooleanStyles.Literal);
                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(true, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksTrue_ForLiteralMixedCaseTrueAndLiteralStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("TrUe", BooleanStyles.Literal);
                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(true, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksFalse_ForLiteralFalseAndLiteralStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("false", BooleanStyles.Literal);
                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksFalse_ForLiteralMixedCaseFalseAndLiteralStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("fAlSe", BooleanStyles.Literal);
                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsFalseAndPicksDefaultBoolean_ForLiteralTrueAndWhitespacesAndLiteralStyle()
            {
                // Act
                bool? result = ParseTo.Boolean(" true ", BooleanStyles.Literal);
                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(true, result);
            }

            [Fact]
            public static void ReturnsFalseAndPicksDefaultBoolean_ForStrictlyPositiveInt32AndIntegerStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("10", BooleanStyles.OneOrZero);
                // Assert
                Assert.False(result.HasValue);
                Assert.Equal(default(Boolean), result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksTrue_ForOneAndIntegerStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("1", BooleanStyles.OneOrZero);
                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(true, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksFalse_ForZeroAndIntegerStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("0", BooleanStyles.OneOrZero);
                // Assert
                Assert.True(result.HasValue);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsFalseAndPicksDefaultBoolean_ForMinusOneAndIntegerStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("-1", BooleanStyles.OneOrZero);
                // Assert
                Assert.False(result.HasValue);
                Assert.Equal(default(Boolean), result);
            }

            [Fact]
            public static void ReturnsFalseAndPicksDefaultBoolean_ForNegativeInt32AndIntegerStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("-10", BooleanStyles.OneOrZero);
                // Assert
                Assert.False(result.HasValue);
                Assert.Equal(default(Boolean), result);
            }

            [Fact]
            public static void ReturnsFalseAndPicksDefaultBoolean_ForDecimalAndIntegerStyle()
            {
                // Act
                bool? result = ParseTo.Boolean("-10.1", BooleanStyles.OneOrZero);
                // Assert
                Assert.False(result.HasValue);
                Assert.Equal(default(Boolean), result);
            }
        }
    }
}
