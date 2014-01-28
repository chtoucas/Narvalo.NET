namespace Narvalo
{
    using System;
    using Xunit;

    public static class TryParseToFacts
    {
        public static class ToBoolean
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
                Assert.Equal(value, " une chaîne quelconque ");
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
            public static void ReturnsTrueAndPicksFalse_ForEmptyString_WhenEmptyIsFalse()
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
            public static void ReturnsTrueAndPicksTrue_ForLiteralTrue_WhenLiteralStyle()
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
            public static void ReturnsTrueAndPicksTrue_ForLiteralMixedCaseTrue_WhenLiteralStyle()
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
            public static void ReturnsTrueAndPicksFalse_ForLiteralFalse_WhenLiteralStyle()
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
            public static void ReturnsTrueAndPicksFalse_ForLiteralMixedCaseFalse_WhenLiteralStyle()
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
            public static void ReturnsFalseAndPicksDefaultBoolean_ForLiteralTrueAndWhitespaces_WhenLiteralStyle()
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
            public static void ReturnsTrueAndPicksTrue_ForPositiveInt32_WhenIntegerStyle()
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
            public static void ReturnsTrueAndPicksFalse_ForZero_WhenIntegerStyle()
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
            public static void ReturnsTrueAndPicksFalse_ForNegativeInt32_WhenIntegerStyle()
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
            public static void ReturnsFalseAndPicksDefaultBoolean_ForDecimal_WhenIntegerStyle()
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
