namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Xunit;

    public static class MaybeFacts
    {
        #region Stubs

        struct ValueStub : IEquatable<ValueStub>
        {
            int _value;

            public ValueStub(int value) { _value = value; }

            public static bool operator ==(ValueStub left, ValueStub right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(ValueStub left, ValueStub right)
            {
                return !left.Equals(right);
            }

            public bool Equals(ValueStub other)
            {
                return _value == other._value;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) {
                    return false;
                }

                if (!(obj is ValueStub)) {
                    return false;
                }

                return Equals((ValueStub)obj);
            }

            public override int GetHashCode()
            {
                return _value.GetHashCode();
            }
        }

        class AlmostValueStub : IEquatable<AlmostValueStub>
        {
            public string Value { get; set; }

            public bool Equals(AlmostValueStub other)
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

                return Equals((AlmostValueStub)obj);
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
        }

        #endregion

        public static class TheIsSomePropery
        {
            [Fact]
            public static void IsImmutable_OnceTrue()
            {
                // Arrange
                var list = new List<int>();

                // Act
                var option = Maybe.Create(list);
                list = null;

                // Act & Assert
                Assert.True(option.IsSome);
            }

            [Fact]
            public static void IsImmutable_OnceFalse()
            {
                // Arrange
                List<int> list = null;

                // Act
                var option = Maybe.Create(list);
                list = new List<int>();

                // Act & Assert
                Assert.True(!option.IsSome);
            }
        }

        public static class TheValueProperty
        {
            [Fact]
            public static void ThrowsInvalidOperationException_WhenNone()
            {
                // Arrange
                var option = Maybe<int>.None;

                // Act & Assert
                Assert.Throws<InvalidOperationException>(() => option.Value);
            }

            [Fact]
            public static void ReturnsTheOriginalValue_WhenSome()
            {
                // Arrange
                var simple = 3141;
                var value = new ValueStub(3141);
                ValueStub? nullableValue = new ValueStub(3141);
                var reference = new List<int>();

                // Act
                var simpleOpt = Maybe.Create(simple);
                var valueOpt = Maybe.Create(value);
                var nullableValueOpt = Maybe.Create(nullableValue);
                var referenceOpt = Maybe.Create(reference);

                // Assert
                Assert.True(simpleOpt.Value == simple);
                Assert.True(valueOpt.Value == value);
                Assert.True(nullableValueOpt.Value == nullableValue.Value);
                Assert.True(referenceOpt.Value == reference);
            }
        }

        public static class TheEqualityOperator
        {
            [Fact]
            public static void ReturnsTrue_ForMaybeNullAndNull()
            {
                // Arrange
                var simple = (Maybe<int>)null;
                var value = (Maybe<ValueStub>)null;
                var reference = (Maybe<List<int>>)null;

                // Act & Assert
                Assert.True(simple == null);
                Assert.True(value == null);
                Assert.True(reference == null);
            }

            [Fact]
            public static void ReturnsFalse_ForMaybeNoneAndNull()
            {
                // Arrange
                var simple = Maybe<int>.None;
                var value = Maybe<ValueStub>.None;
                var reference = Maybe<List<int>>.None;

                // Act & Assert
                Assert.False(simple == null);
                Assert.False(value == null);
                Assert.False(reference == null);
            }
        }

        public static class TheInequalityOperator
        {
            [Fact]
            public static void ReturnsFalse_ForMaybeNullAndNull()
            {
                // Arrange
                var simple = (Maybe<int>)null;
                var value = (Maybe<ValueStub>)null;
                var reference = (Maybe<List<int>>)null;

                // Act & Assert
                Assert.False(simple != null);
                Assert.False(value != null);
                Assert.False(reference != null);
            }

            [Fact]
            public static void ReturnsTrue_ForMaybeNoneAndNull()
            {
                // Arrange
                var simple = Maybe<int>.None;
                var value = Maybe<ValueStub>.None;
                var reference = Maybe<List<int>>.None;

                // Act & Assert
                Assert.True(simple != null);
                Assert.True(value != null);
                Assert.True(reference != null);
            }
        }

        public static class TheEqualsMethod
        {
            [Fact]
            public static void IsReflexive()
            {
                // Arrange
                var simple = Maybe.Create(3141);
                var value = Maybe.Create(new ValueStub(3141));
                var reference = Maybe.Create(new List<int>());

                // Act & Assert
                Assert.True(simple.Equals(simple));
                Assert.True(value.Equals(value));
                Assert.True(reference.Equals(reference));
            }

            [Fact]
            public static void IsAbelian()
            {
                // Arrange
                var simpleA0 = Maybe.Create(3141);
                var simpleA1 = Maybe.Create(3141);
                var simple = Maybe.Create(1570);

                var valueA0 = Maybe.Create(new ValueStub(3141));
                var valueA1 = Maybe.Create(new ValueStub(3141));
                var value = Maybe.Create(new ValueStub(1570));

                var referenceA0 = Maybe.Create(new List<int>());
                var referenceA1 = Maybe.Create(new List<int>());
                var reference = Maybe.Create(new List<int>() { 0 });

                // Act & Assert
                Assert.Equal(simpleA0.Equals(simpleA1), simpleA1.Equals(simpleA0));
                Assert.Equal(simpleA0.Equals(simple), simple.Equals(simpleA0));
                Assert.Equal(valueA0.Equals(valueA1), valueA1.Equals(valueA0));
                Assert.Equal(valueA0.Equals(value), value.Equals(valueA0));
                Assert.Equal(referenceA0.Equals(referenceA1), referenceA1.Equals(referenceA0));
                Assert.Equal(referenceA0.Equals(reference), reference.Equals(referenceA0));
            }

            [Fact]
            public static void IsTransitive()
            {
                // Arrange
                var simple1 = Maybe.Create(3141);
                var simple2 = Maybe.Create(3141);
                var simple3 = Maybe.Create(3141);

                var value1 = Maybe.Create(new ValueStub(3141));
                var value2 = Maybe.Create(new ValueStub(3141));
                var value3 = Maybe.Create(new ValueStub(3141));

                var reference1 = Maybe.Create(new List<int>());
                var reference2 = Maybe.Create(new List<int>());
                var reference3 = Maybe.Create(new List<int>());

                // Act & Assert
                Assert.Equal(simple1.Equals(simple2) && simple2.Equals(simple3), simple1.Equals(simple3));
                Assert.Equal(value1.Equals(value2) && value2.Equals(value3), value1.Equals(value3));
                Assert.Equal(reference1.Equals(reference2) && reference2.Equals(reference3), reference1.Equals(reference3));
            }

            [Fact]
            public static void ReturnsTrue_WhenNone_ForNull()
            {
                // Arrange
                var simple = Maybe<int>.None;
                var value = Maybe<ValueStub>.None;
                var reference = Maybe<List<int>>.None;

                // Act & Assert
                Assert.True(simple.Equals(null));
                Assert.True(value.Equals(null));
                Assert.True(reference.Equals((object)null));
                Assert.True(reference.Equals((List<int>)null));
                Assert.True(reference.Equals((Maybe<List<int>>)null));
            }

            [Fact]
            public static void ReturnsTrue_ForOriginalValue()
            {
                // Arrange
                var simple = 3141;
                var simpleOpt = Maybe.Create(simple);

                var value = new ValueStub(3141);
                var valueOpt = Maybe.Create(value);

                var reference = new List<int>();
                var referenceOpt = Maybe.Create(reference);

                // Act & Assert
                Assert.True(simpleOpt.Equals(simple));
                Assert.True(valueOpt.Equals(value));
                Assert.True(referenceOpt.Equals(reference));
            }

            [Fact]
            public static void ReturnsTrue_ForOriginalValueCastedToObject()
            {
                // Arrange
                var simple = 3141;
                var simpleOpt = Maybe.Create(simple);

                var value = new ValueStub(3141);
                var valueOpt = Maybe.Create(value);

                var reference = new List<int>();
                var referenceOpt = Maybe.Create(reference);

                // Act & Assert
                Assert.True(simpleOpt.Equals((object)simple));
                Assert.True(valueOpt.Equals((object)value));
                Assert.True(referenceOpt.Equals((object)reference));
            }

            [Fact]
            public static void FollowsStructuralEqualityRules()
            {
                // Arrange
                var simpleA0 = Maybe.Create(3141);
                var simpleA1 = Maybe.Create(3141);

                var valueA0 = Maybe.Create(new ValueStub(3141));
                var valueA1 = Maybe.Create(new ValueStub(3141));

                var almostValueA0 = Maybe.Create(new AlmostValueStub { Value = "Une chaîne de caractère" });
                var almostValueA1 = Maybe.Create(new AlmostValueStub { Value = "Une chaîne de caractère" });

                // Act & Assert
                Assert.True(simpleA0.Equals(simpleA1));
                Assert.True(valueA0.Equals(valueA1));
                Assert.True(almostValueA0.Equals(almostValueA1));
            }

            [Fact]
            public static void FollowsStructuralEqualityRulesAfterCastToObject()
            {
                // Arrange
                var simpleA0 = Maybe.Create(3141);
                var simpleA1 = Maybe.Create(3141);

                var valueA0 = Maybe.Create(new ValueStub(3141));
                var valueA1 = Maybe.Create(new ValueStub(3141));

                var almostValueA0 = Maybe.Create(new AlmostValueStub { Value = "Une chaîne de caractère" });
                var almostValueA1 = Maybe.Create(new AlmostValueStub { Value = "Une chaîne de caractère" });

                // Act & Assert
                Assert.True(simpleA0.Equals((object)simpleA1));
                Assert.True(valueA0.Equals((object)valueA1));
                Assert.True(almostValueA0.Equals((object)almostValueA1));
            }
        }

        public static class TheCreateMethod
        {
            [Fact]
            public static void ReturnsSome_ForNotNull()
            {
                // Arrange
                var simple = 3141;
                var value = new ValueStub(3141);
                ValueStub? nullableValue = new ValueStub(3141);
                var reference = new List<int>();

                // Act
                var simpleOpt = Maybe.Create(simple);
                var valueOpt = Maybe.Create(value);
                var nullableValueOpt = Maybe.Create(nullableValue);
                var referenceOpt = Maybe.Create(reference);

                // Assert
                Assert.True(simpleOpt.IsSome);
                Assert.True(valueOpt.IsSome);
                Assert.True(nullableValueOpt.IsSome);
                Assert.True(referenceOpt.IsSome);
            }

            [Fact]
            public static void ReturnsNone_ForNull()
            {
                // Arrange
                ValueStub? value = null;
                List<int> reference = null;

                // Act
                var valueOpt = Maybe.Create(value);
                var referenceOpt = Maybe.Create(reference);

                // Assert
                Assert.True(valueOpt.IsNone);
                Assert.True(referenceOpt.IsNone);
            }
        }
    }
}
