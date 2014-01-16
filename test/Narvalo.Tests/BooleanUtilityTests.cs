namespace Narvalo
{
    using System;
    using Xunit;

    public static class BooleanUtilityTests
    {
        #region > TryParse <

        public static class TryParse
        {
            [Fact]
            public static void DoesNotAlterInputValue()
            {
                // Arrange
                string value = " une chaîne quelconque ";
                bool result;
                // Act
                bool succeed = BooleanUtility.TryParse(value, out result);
                // Assert
                Assert.Equal(value, " une chaîne quelconque ");
            }

            [Fact]
            public static void ReturnsFalseAndPicksDefaultBoolean_ForNullString()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = BooleanUtility.TryParse(null, BooleanStyles.None, out result);
                // Assert
                Assert.False(succeed);
                Assert.Equal(default(Boolean), result);
            }

            [Fact]
            public static void ReturnsFalseAndPicksDefaultBoolean_ForEmptyString()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = BooleanUtility.TryParse(String.Empty, BooleanStyles.None, out result);
                // Assert
                Assert.False(succeed);
                Assert.Equal(default(Boolean), result);
            }

            [Fact]
            public static void ReturnsTrueAndPicksFalse_ForEmptyString_WhenEmptyIsFalse()
            {
                // Arrange
                bool result;
                // Act
                bool succeed = BooleanUtility.TryParse(String.Empty, BooleanStyles.EmptyIsFalse, out result);
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
                bool succeed = BooleanUtility.TryParse("true", BooleanStyles.Literal, out result);
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
                bool succeed = BooleanUtility.TryParse("TrUe", BooleanStyles.Literal, out result);
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
                bool succeed = BooleanUtility.TryParse("false", BooleanStyles.Literal, out result);
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
                bool succeed = BooleanUtility.TryParse("fAlSe", BooleanStyles.Literal, out result);
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
                bool succeed = BooleanUtility.TryParse(" true ", BooleanStyles.Literal, out result);
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
                bool succeed = BooleanUtility.TryParse("10", BooleanStyles.Integer, out result);
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
                bool succeed = BooleanUtility.TryParse("0", BooleanStyles.Integer, out result);
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
                bool succeed = BooleanUtility.TryParse("-10", BooleanStyles.Integer, out result);
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
                bool succeed = BooleanUtility.TryParse("-10.1", BooleanStyles.Integer, out result);
                // Assert
                Assert.False(succeed);
                Assert.Equal(default(Boolean), result);
            }

            [Fact]
            public static void ThrowsNotImplementedException_ForHtmlInput()
            {
                // Act && Assert
                Assert.Throws<NotImplementedException>(
                    () => MayParse.ToBoolean("on", BooleanStyles.HtmlInput));
            }
        }

        #endregion
    }
}
