namespace Narvalo
{
    using System;
    using Xunit;

    public static class TryParseToFacts
    {
        public static class TheBooleanMethod
        {
            [Fact]
            public static void DoesNotAlterInputValue()
            {
                // Arrange
                string value = " une chaîne quelconque ";
                bool result;
                // Act
                bool succeed = TryParseTo.Boolean(value, out result);
                // Assert
                Assert.Equal(" une chaîne quelconque ", value);
            }

            //[Fact]
            //public static void ReturnsFalseAndPicksDefaultBoolean_ForNullString()
            //{
            //    // Arrange
            //    bool result;
            //    // Act
            //    bool succeed = TryParseTo.Boolean(null, BooleanStyles.None, out result);
            //    // Assert
            //    Assert.False(succeed);
            //    Assert.Equal(default(Boolean), result);
            //}

            //[Fact]
            //public static void ReturnsFalseAndPicksDefaultBoolean_ForEmptyString()
            //{
            //    // Arrange
            //    bool result;
            //    // Act
            //    bool succeed = TryParseTo.Boolean(String.Empty, BooleanStyles.None, out result);
            //    // Assert
            //    Assert.False(succeed);
            //    Assert.Equal(default(Boolean), result);
            //}

            [Fact]
            public static void ReturnsTrueAndPicksFalse_ForEmptyStringAndEmptyIsFalse()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = TryParseTo.Boolean(String.Empty, BooleanStyles.EmptyIsFalse, out result);
                // Assert
                Assert.True(succeed);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksTrue_ForLiteralTrueAndLiteralStyle()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = TryParseTo.Boolean("true", BooleanStyles.Literal, out result);
                // Assert
                Assert.True(succeed);
                Assert.Equal(true, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksTrue_ForLiteralMixedCaseTrueAndLiteralStyle()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = TryParseTo.Boolean("TrUe", BooleanStyles.Literal, out result);
                // Assert
                Assert.True(succeed);
                Assert.Equal(true, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksFalse_ForLiteralFalseAndLiteralStyle()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = TryParseTo.Boolean("false", BooleanStyles.Literal, out result);
                // Assert
                Assert.True(succeed);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksFalse_ForLiteralMixedCaseFalseAndLiteralStyle()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = TryParseTo.Boolean("fAlSe", BooleanStyles.Literal, out result);
                // Assert
                Assert.True(succeed);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsFalseAndPicksDefaultBoolean_ForLiteralTrueAndWhitespacesAndLiteralStyle()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = TryParseTo.Boolean(" true ", BooleanStyles.Literal, out result);
                // Assert
                Assert.True(succeed);
                Assert.Equal(true, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksTrue_ForPositiveInt32AndIntegerStyle()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = TryParseTo.Boolean("10", BooleanStyles.Integer, out result);
                // Assert
                Assert.True(succeed);
                Assert.Equal(true, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksFalse_ForZeroAndIntegerStyle()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = TryParseTo.Boolean("0", BooleanStyles.Integer, out result);
                // Assert
                Assert.True(succeed);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksFalse_ForNegativeInt32AndIntegerStyle()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = TryParseTo.Boolean("-10", BooleanStyles.Integer, out result);
                // Assert
                Assert.True(succeed);
                Assert.Equal(false, result);
            }

            [Fact]
            public static void ReturnsFalseAndPicksDefaultBoolean_ForDecimalAndIntegerStyle()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = TryParseTo.Boolean("-10.1", BooleanStyles.Integer, out result);
                // Assert
                Assert.False(succeed);
                Assert.Equal(default(Boolean), result);
            }

            //[Fact]
            //public static void ThrowsNotImplementedException_ForHtmlInput()
            //{
            //    // Act && Assert
            //    Assert.Throws<NotImplementedException>(
            //        () => MayParse.ToBoolean("on", BooleanStyles.HtmlInput));
            //}
        }
    }
}
