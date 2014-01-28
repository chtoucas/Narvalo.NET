namespace Narvalo.Fx
{
    using System;
    using Xunit;

    public static class MaybeFacts
    {
        #region Stubs

        struct StructStub_
        {
            int _value;

            public StructStub_(int value) { _value = value; }
        }

        class ClassStub_
        {
            public string Value { get; set; }
        }

        struct ComplexStructStub_ : IEquatable<ComplexStructStub_>
        {
            int _value;

            public ComplexStructStub_(int value) { _value = value; }

            public static bool operator ==(ComplexStructStub_ left, ComplexStructStub_ right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(ComplexStructStub_ left, ComplexStructStub_ right)
            {
                return !(left == right);
            }

            public bool Equals(ComplexStructStub_ other)
            {
                return _value == other._value;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) {
                    return false;
                }

                if (!(obj is ComplexStructStub_)) {
                    return false;
                }

                return Equals((ComplexStructStub_)obj);
            }

            public override int GetHashCode()
            {
                return _value.GetHashCode();
            }
        }

        class ComplexClassStub_ : IEquatable<ComplexClassStub_>
        {
            public string Value { get; set; }

            public static bool operator ==(ComplexClassStub_ left, ComplexClassStub_ right)
            {
                if (ReferenceEquals(left, null)) {
                    return ReferenceEquals(right, null);
                }

                return left.Equals(right);
            }

            public static bool operator !=(ComplexClassStub_ left, ComplexClassStub_ right)
            {
                return !(left == right);
            }

            public bool Equals(ComplexClassStub_ other)
            {
                if (ReferenceEquals(other, null)) {
                    return false;
                }

                return Value == other.Value;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) {
                    return false;
                }

                if (ReferenceEquals(obj, this)) {
                    return true;
                }

                var other = obj as ComplexClassStub_;
                if (ReferenceEquals(other, null)) {
                    return false;
                }

                return Equals(other);
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
        }

        #endregion

        #region Opérateurs == et !=

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

        #endregion

        public static class Equality
        {
            //// Réflexive : x == x

            [Fact]
            public static void IsReflexive_ForSimpleType()
            {
                // Arrange
                var option = Maybe.Create(1000);
                // Act & Assert
                Assert.True(option.Equals(option));
            }

            [Fact]
            public static void IsReflexive_ForStruct()
            {
                // Arrange
                var option = Maybe.Create(new StructStub_(3141));
                // Act & Assert
                Assert.True(option.Equals(option));
            }

            [Fact]
            public static void IsReflexive_ForClass()
            {
                // Arrange
                var option = Maybe.Create(new ClassStub_ { Value = "π" });
                // Act & Assert
                Assert.True(option.Equals(option));
            }

            //// Symétrique :  x == y <=> y == x

            [Fact]
            public static void IsSymmetric_ForSimpleType()
            {
                // Arrange
                var option1 = Maybe.Create(3141);
                var option2 = Maybe.Create(3141);
                var option3 = Maybe.Create(1570);

                // Act & Assert
                Assert.Equal(option1.Equals(option2), option2.Equals(option1));
                Assert.Equal(option1.Equals(option3), option3.Equals(option1));
            }

            [Fact]
            public static void IsSymmetric_ForStruct()
            {
                // Arrange
                var option1 = Maybe.Create(new StructStub_(3141));
                var option2 = Maybe.Create(new StructStub_(3141));
                var option3 = Maybe.Create(new StructStub_(1570));

                // Act & Assert
                Assert.Equal(option1.Equals(option2), option2.Equals(option1));
                Assert.Equal(option1.Equals(option3), option3.Equals(option1));
            }

            [Fact]
            public static void IsSymmetric_ForComplexStruct()
            {
                // Arrange
                var option1 = Maybe.Create(new ComplexStructStub_(3141));
                var option2 = Maybe.Create(new ComplexStructStub_(3141));
                var option3 = Maybe.Create(new ComplexStructStub_(1570));

                // Act & Assert
                Assert.Equal(option1.Equals(option2), option2.Equals(option1));
                Assert.Equal(option1.Equals(option3), option3.Equals(option1));
            }

            [Fact]
            public static void IsSymmetric_ForClass()
            {
                // Arrange
                var option1 = Maybe.Create(new ClassStub_ { Value = "π" });
                var option2 = Maybe.Create(new ClassStub_ { Value = "π" });
                var option3 = Maybe.Create(new ClassStub_ { Value = "pi" });

                // Act & Assert
                Assert.Equal(option1.Equals(option2), option2.Equals(option1));
                Assert.Equal(option1.Equals(option3), option3.Equals(option1));
            }

            [Fact]
            public static void IsSymmetric_ForComplexClass()
            {
                // Arrange
                var option1 = Maybe.Create(new ComplexClassStub_ { Value = "π" });
                var option2 = Maybe.Create(new ComplexClassStub_ { Value = "π" });
                var option3 = Maybe.Create(new ComplexClassStub_ { Value = "pi" });

                // Act & Assert
                Assert.Equal(option1.Equals(option2), option2.Equals(option1));
                Assert.Equal(option1.Equals(option3), option3.Equals(option1));
            }

            //// Transitive : x == y && y == z => x == z

            [Fact]
            public static void IsTransitive_ForSimpleType()
            {
                // Arrange
                var option1 = Maybe.Create(3141);
                var option2 = Maybe.Create(3141);
                var option3 = Maybe.Create(3141);
                var option4 = Maybe.Create(1570);

                // Act & Assert
                Assert.Equal(option1.Equals(option2) && option2.Equals(option3), option1.Equals(option3));
            }

            [Fact]
            public static void IsTransitive_ForStruct()
            {
                // Arrange
                var option1 = Maybe.Create(new StructStub_(3141));
                var option2 = Maybe.Create(new StructStub_(3141));
                var option3 = Maybe.Create(new StructStub_(3141));
                var option4 = Maybe.Create(new StructStub_(1570));

                // Act & Assert
                Assert.Equal(option1.Equals(option2) && option2.Equals(option3), option1.Equals(option3));
            }

            [Fact]
            public static void IsTransitive_ForClass()
            {
                // Arrange
                var option1 = Maybe.Create(new ClassStub_ { Value = "π" });
                var option2 = Maybe.Create(new ClassStub_ { Value = "π" });
                var option3 = Maybe.Create(new ClassStub_ { Value = "π" });
                var option4 = Maybe.Create(new ClassStub_ { Value = "pi" });

                // Act & Assert
                Assert.Equal(option1.Equals(option2) && option2.Equals(option3), option1.Equals(option3));
            }

            //// 

            [Fact]
            public static void Equals_MaybeNone_And_Null_ReturnsFalse()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.False(option.Equals(null));
            }

            [Fact]
            public static void Equals_MaybeNone_And_Unit_ReturnsFalse()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.False(option.Equals(Unit.Single));
            }

            [Fact]
            public static void Equals_Unit_And_MaybeNone_RReturnsFalse()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.False(Unit.Single.Equals(option));
            }

            [Fact(Skip = "En attente de la réécriture des règles d'égalité.")]
            public static void Equals_MaybeInt32_And_Int32_ReturnsTrue()
            {
                // Arrange
                var option = Maybe.Create(1);
                // Act & Assert
                Assert.True(option.Equals(1));
            }
        }

        public static class Create
        {
            [Fact]
            public static void ReturnsSome_ForInt32()
            {
                // Arrange
                int value = 3141;
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
                int? value = 3141;
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
