namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    public static class MaybeFacts
    {
        static readonly Uri SampleUri_ = new Uri("http://localhost");
        static readonly Uri AnotherUri_ = new Uri("http://narvalo.org");

        #region Stubs

        struct ValueStub_ : IEquatable<ValueStub_>
        {
            int _value;

            public ValueStub_(int value) { _value = value; }

            public static bool operator ==(ValueStub_ left, ValueStub_ right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(ValueStub_ left, ValueStub_ right)
            {
                return !left.Equals(right);
            }

            public bool Equals(ValueStub_ other)
            {
                return _value == other._value;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) {
                    return false;
                }

                if (!(obj is ValueStub_)) {
                    return false;
                }

                return Equals((ValueStub_)obj);
            }

            public override int GetHashCode()
            {
                return _value.GetHashCode();
            }
        }

        class ClassStub_
        {
            public Uri Url { get; set; }
        }

        class AlmostValueStub_ : IEquatable<AlmostValueStub_>
        {
            public string Value { get; set; }

            public bool Equals(AlmostValueStub_ other)
            {
                if (ReferenceEquals(other, null)) {
                    return false;
                }

                return Value == other.Value;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, null)) {
                    return false;
                }

                if (ReferenceEquals(obj, this)) {
                    return true;
                }

                if (obj.GetType() != this.GetType()) {
                    return false;
                }

                return Equals((AlmostValueStub_)obj);
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
        }

        #endregion

        //[Fact]
        //public static void Maybe_ForValueType_IsImmutable()
        //{
        //    // Arrange
        //    var option1 = Maybe.Create(new ValueStub_(3141));
        //    var option2 = Maybe.Create(new ValueStub_(3141));

        //    // Act & Assert
        //    Assert.True(option1 != option2);
        //    Assert.True(option1.Equals(option2));
        //}

        //[Fact]
        //public static void Maybe_ForValueType()
        //{
        //    // Arrange
        //    var value = SampleUri_;
        //    // Act
        //    var option1 = Maybe.Create(new ClassStub_ { Url = value });
        //    value = AnotherUri_;
        //    var option2 = Maybe.Create(new ClassStub_ { Url = AnotherUri_ });
        //    // Assert
        //    //Assert.True(option1.Value == AnotherUri_);
        //    Assert.False(option1.Equals(option2));
        //}

        //[Fact]
        //public static void Maybe_ForValueType_List()
        //{
        //    // Arrange
        //    IList<int> list = new List<int>();
        //    list.Add(0);
        //    // Act
        //    var option = Maybe.Create(list);

        //    // Assert
        //    Assert.True(option.IsSome);

        //    Assert.True(option.Value[0] == 0);

        //    list.Add(1);
        //    Assert.True(option.Value[1] == 1);

        //    list.Remove(1);
        //    Assert.Throws<ArgumentOutOfRangeException>(() => option.Value[1] == 1);

        //    option.Value.Add(1);
        //    Assert.True(option.Value[1] == 1);

        //    list = null;
        //    Assert.False(ReferenceEquals(option.Value, null));
        //    Assert.False(option.Equals(Maybe<IList<int>>.None));
        //}

        public static class Equality
        {
            [Fact]
            public static void MaybeNull_Equals_Null_Succeeds()
            {
                // Arrange
                var option = (Maybe<int>)null;
                // Act & Assert
                Assert.True(option == null);
                Assert.False(option != null);
            }

            [Fact]
            public static void MaybeNone_Equals_Null_Fails()
            {
                // Arrange
                var option = Maybe<int>.None;
                // Act & Assert
                Assert.False(option == null);
                Assert.True(option != null);
            }
        }

        public static class StructuralEquality
        {
            //// Réflexivité : x == x

            [Fact]
            public static void IsReflexive()
            {
                // Arrange
                var option1 = Maybe.Create(1000);
                var option2 = Maybe.Create(new ValueStub_(3141));
                var option3 = Maybe.Create(new ClassStub_ { Url = SampleUri_ });
                // Act & Assert
                Assert.True(option1.Equals(option1));
                Assert.True(option2.Equals(option2));
                Assert.True(option3.Equals(option3));
            }

            //// Commutativité :  x == y <=> y == x

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
                var option1 = Maybe.Create(new ValueStub_(3141));
                var option2 = Maybe.Create(new ValueStub_(3141));
                var option3 = Maybe.Create(new ValueStub_(1570));

                // Act & Assert
                Assert.Equal(option1.Equals(option2), option2.Equals(option1));
                Assert.Equal(option1.Equals(option3), option3.Equals(option1));
            }

            [Fact]
            public static void IsSymmetric_ForComplexStruct()
            {
                // Arrange
                var option1 = Maybe.Create(new ValueStub_(3141));
                var option2 = Maybe.Create(new ValueStub_(3141));
                var option3 = Maybe.Create(new ValueStub_(1570));

                // Act & Assert
                Assert.Equal(option1.Equals(option2), option2.Equals(option1));
                Assert.Equal(option1.Equals(option3), option3.Equals(option1));
            }

            [Fact]
            public static void IsSymmetric_ForClass()
            {
                // Arrange
                var option1 = Maybe.Create(new ClassStub_ { Url = SampleUri_ });
                var option2 = Maybe.Create(new ClassStub_ { Url = SampleUri_ });
                var option3 = Maybe.Create(new ClassStub_ { Url = AnotherUri_ });

                // Act & Assert
                Assert.Equal(option1.Equals(option2), option2.Equals(option1));
                Assert.Equal(option1.Equals(option3), option3.Equals(option1));
            }

            [Fact]
            public static void IsSymmetric_ForComplexClass()
            {
                // Arrange
                var option1 = Maybe.Create(new AlmostValueStub_ { Value = "π" });
                var option2 = Maybe.Create(new AlmostValueStub_ { Value = "π" });
                var option3 = Maybe.Create(new AlmostValueStub_ { Value = "pi" });

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
                var option1 = Maybe.Create(new ValueStub_(3141));
                var option2 = Maybe.Create(new ValueStub_(3141));
                var option3 = Maybe.Create(new ValueStub_(3141));
                var option4 = Maybe.Create(new ValueStub_(1570));

                // Act & Assert
                Assert.Equal(option1.Equals(option2) && option2.Equals(option3), option1.Equals(option3));
            }

            [Fact]
            public static void IsTransitive_ForClass()
            {
                // Arrange
                var option1 = Maybe.Create(new ClassStub_ { Url = SampleUri_ });
                var option2 = Maybe.Create(new ClassStub_ { Url = SampleUri_ });
                var option3 = Maybe.Create(new ClassStub_ { Url = SampleUri_ });
                var option4 = Maybe.Create(new ClassStub_ { Url = AnotherUri_ });

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
