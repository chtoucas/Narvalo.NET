namespace Narvalo.Fx
{
    using System;
    using Xunit;

    public static class MaybeFacts
    {
        public static class Equality
        {
            //// Tests sur les opérateurs == et !=

            [Fact]
            public static void MaybeNull_Equals_Null_Succeeds()
            {
                // Arrange
                var option = (Maybe<int>)null;
                // Act & Assert
                Assert.True(option == null);
            }

            [Fact]
            public static void Null_Equals_MaybeNull_Succeeds()
            {
                // Arrange
                var option = (Maybe<int>)null;
                // Act & Assert
                Assert.True(null == option);
            }

            [Fact]
            public static void MaybeNone_Equals_Null_Fails()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.False(option == null);
            }

            [Fact]
            public static void MaybeNone_DoNotEqual_Null_Succeeds()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.True(option != null);
            }

            [Fact]
            public static void Null_Equals_MaybeNone_Fails()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.False(null == option);
            }

            [Fact]
            public static void Null_DoNotEqual_MaybeNone_Succeeds()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.True(null != option);
            }

            //// Tests sur les méthodes Equals

            [Fact]
            public static void Equals_MaybeNone_And_Null_ReturnsFalse()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.False(option.Equals(null));
            }

            [Fact]
            public static void Equals_MaybeNone_And_Unit_ReturnsTrue()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.False(option.Equals(Unit.Single));
            }

            [Fact]
            public static void Equals_Unit_And_MaybeNone_ReturnsTrue()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.False(Unit.Single.Equals(option));
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
