namespace Narvalo.Fx
{
    using System;
    using Xunit;

    public static class MaybeTests
    {
        public static class Equality
        {
            [Fact]
            public static void Equals_WithNull_ForNull_ReturnsFalse()
            {
                // Arrange
                var option = (Maybe<int>)null;
                // Act & Assert
                Assert.True(option == null);
            }

            [Fact]
            public static void IndirectEquals_WithNone_ForNull_ReturnsFalse()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.False(option == null);
            }

            [Fact]
            public static void Equals_WithNone_ForNull_ReturnsFalse()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.False(option.Equals(null));
            }

            [Fact]
            public static void Equals_WithNone_ForUnit_ReturnsTrue()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.True(option.Equals(Unit.Single));
            }
        }

        public static class Create
        {
            [Fact]
            public static void ReturnsSome_ForInt32()
            {
                // Arrange
                int value = 1290;
                // Act
                Maybe<int> result = Maybe.Create(value);
                // Assert
                Assert.True(result.IsSome);
                Assert.Equal(value, result.Value);
            }

            [Fact]
            public static void ReturnsSome_ForNullableInt32WithValue()
            {
                // Arrange
                int? value = 1718;
                // Act
                Maybe<int> result = Maybe.Create(value);
                // Assert
                Assert.True(result.IsSome);
                Assert.Equal(value.Value, result.Value);
            }

            [Fact]
            public static void ReturnsNone_ForNullableInt32WithoutValue()
            {
                // Arrange
                int? value = null;
                // Act
                Maybe<int> result = Maybe.Create(value);
                // Assert
                Assert.True(result.IsNone);
            }

            [Fact]
            public static void ReturnsNone_ForNullString()
            {
                // Arrange
                string value = null;
                // Act
                Maybe<string> result = Maybe.Create(value);
                // Assert
                Assert.True(result.IsNone);
            }

            [Fact]
            public static void ReturnsSome_ForEmptyString()
            {
                // Arrange
                string value = String.Empty;
                // Act
                Maybe<string> result = Maybe.Create(value);
                // Assert
                Assert.True(result.IsSome);
                Assert.Equal(value, result.Value);
            }

            [Fact]
            public static void ReturnsSome_ForNotNullOrEmptyString()
            {
                // Arrange
                string value = "Une chaîne de caractère";
                // Act
                Maybe<string> result = Maybe.Create(value);
                // Assert
                Assert.True(result.IsSome);
                Assert.Equal(value, result.Value);
            }
        }
    }
}
