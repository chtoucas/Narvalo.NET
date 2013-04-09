namespace Narvalo.Fx
{
    using System;
    using Xunit;

    public static class MaybeTests
    {
        public static class Create
        {
            [Fact]
            public static void ReturnsSome_ForInt32()
            {
                // Arrange
                int value = 1290;
                // Act
                Maybe<int> result = Maybe.Create<int>(value);
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
                Maybe<int> result = Maybe.Create<int>(value);
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
                Maybe<string> result = Maybe.Create<string>(value);
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
