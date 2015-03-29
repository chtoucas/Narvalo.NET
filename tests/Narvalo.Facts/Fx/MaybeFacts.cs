// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Internal;
    using Xunit;

    public static partial class MaybeFacts
    {
        private struct ValueStub_ : IEquatable<ValueStub_>
        {
            private int _value;

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
                if (obj == null)
                {
                    return false;
                }

                if (!(obj is ValueStub_))
                {
                    return false;
                }

                return Equals((ValueStub_)obj);
            }

            public override int GetHashCode()
            {
                return _value.GetHashCode();
            }
        }

        private sealed class AlmostValueStub_ : IEquatable<AlmostValueStub_>
        {
            public string Value { get; set; }

            public bool Equals(AlmostValueStub_ other)
            {
                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                return Value == other.Value;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, null))
                {
                    return false;
                }

                if (ReferenceEquals(obj, this))
                {
                    return true;
                }

                if (obj.GetType() != this.GetType())
                {
                    return false;
                }

                return Equals((AlmostValueStub_)obj);
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
        }
    }

    public static partial class MaybeFacts
    {
        #region None

        [Fact]
        public static void None_IsNone()
        {
            // Act & Assert
            Assert.False(Maybe.None.IsSome);
        }

        #endregion

        #region IsSome

        [Fact]
        public static void IsSome_IsFalse_WhenNone()
        {
            // Arrange
            var simple = Maybe<int>.None;
            var value = Maybe<ValueStub_>.None;
            var reference = Maybe<List<int>>.None;

            // Act & Assert
            Assert.False(simple.IsSome);
            Assert.False(value.IsSome);
            Assert.False(reference.IsSome);
        }

        [Fact]
        public static void IsSome_IsTrue_WhenSome()
        {
            // Arrange
            var simple = Maybe.Of(3141);
            var value = Maybe.Of(new ValueStub_(3141));
            var reference = Maybe.Of(new List<int>());

            // Act & Assert
            Assert.True(simple.IsSome);
            Assert.True(value.IsSome);
            Assert.True(reference.IsSome);
        }

        [Fact]
        public static void IsSome_IsImmutable_OnceTrue()
        {
            // Arrange
            var list = new List<int>();
            var option = Maybe.Of(list);

            // Act
            list = null;

            // Assert
            Assert.True(option.IsSome);
        }

        [Fact]
        public static void IsSome_IsImmutable_OnceFalse()
        {
            // Arrange
            List<int> list = null;
            var option = Maybe.Of(list);

            // Act
            list = new List<int>();

            // Assert
            Assert.True(!option.IsSome);
        }

        #endregion

        #region op_Equality()

        [Fact]
        public static void Equality_ReturnsTrue_ForNullAsMaybeAndNull()
        {
            // Arrange
            // REVIEW: Cast
            var simple = (Maybe<int>)null;
            var value = (Maybe<ValueStub_>)null;
            var reference = (Maybe<List<int>>)null;

            // Act & Assert
            Assert.True(simple == null);
            Assert.True(value == null);
            Assert.True(reference == null);
        }

        [Fact]
        public static void Equality_ReturnsFalse_ForMaybeNoneAndNull()
        {
            // Arrange
            var simple = Maybe<int>.None;
            var value = Maybe<ValueStub_>.None;
            var reference = Maybe<List<int>>.None;

            // Act & Assert
            Assert.False(simple == null);
            Assert.False(value == null);
            Assert.False(reference == null);
        }

        [Fact]
        public static void Equality_FollowsReferentialEqualityRules()
        {
            // Arrange
            var simpleA0 = Maybe.Of(3141);
            var simpleA1 = Maybe.Of(3141);

            var valueA0 = Maybe.Of(new ValueStub_(3141));
            var valueA1 = Maybe.Of(new ValueStub_(3141));

            var almostValueA0 = Maybe.Of(new AlmostValueStub_ { Value = "Une chaîne de caractère" });
            var almostValueA1 = Maybe.Of(new AlmostValueStub_ { Value = "Une chaîne de caractère" });

            var referenceA0 = Maybe.Of(new List<int>());
            var referenceA1 = Maybe.Of(new List<int>());

            // Act & Assert
            Assert.False(simpleA0 == simpleA1);
            Assert.False(valueA0 == valueA1);
            Assert.False(almostValueA0 == almostValueA1);
            Assert.False(referenceA0 == referenceA1);
        }

        #endregion

        #region op_Inequality()

        [Fact]
        public static void Inequality_ReturnsFalse_ForNullAsMaybeAndNull()
        {
            // Arrange
            // REVIEW: Cast
            var simple = (Maybe<int>)null;
            var value = (Maybe<ValueStub_>)null;
            var reference = (Maybe<List<int>>)null;

            // Act & Assert
            Assert.False(simple != null);
            Assert.False(value != null);
            Assert.False(reference != null);
        }

        [Fact]
        public static void Inequality_ReturnsTrue_ForMaybeNoneAndNull()
        {
            // Arrange
            var simple = Maybe<int>.None;
            var value = Maybe<ValueStub_>.None;
            var reference = Maybe<List<int>>.None;

            // Act & Assert
            Assert.True(simple != null);
            Assert.True(value != null);
            Assert.True(reference != null);
        }

        [Fact]
        public static void Inequality_FollowsReferentialEqualityRules()
        {
            // Arrange
            var simpleA0 = Maybe.Of(3141);
            var simpleA1 = Maybe.Of(3141);

            var valueA0 = Maybe.Of(new ValueStub_(3141));
            var valueA1 = Maybe.Of(new ValueStub_(3141));

            var almostValueA0 = Maybe.Of(new AlmostValueStub_ { Value = "Une chaîne de caractère" });
            var almostValueA1 = Maybe.Of(new AlmostValueStub_ { Value = "Une chaîne de caractère" });

            var referenceA0 = Maybe.Of(new List<int>());
            var referenceA1 = Maybe.Of(new List<int>());

            // Act & Assert
            Assert.True(simpleA0 != simpleA1);
            Assert.True(valueA0 != valueA1);
            Assert.True(almostValueA0 != almostValueA1);
            Assert.True(referenceA0 != referenceA1);
        }

        #endregion

        #region Equals()

        [Fact]
        public static void Equals_IsReflexive()
        {
            // Arrange
            var simple = Maybe.Of(3141);
            var value = Maybe.Of(new ValueStub_(3141));
            var reference = Maybe.Of(new List<int>());

            // Act & Assert
            Assert.True(simple.Equals(simple));
            Assert.True(value.Equals(value));
            Assert.True(reference.Equals(reference));
        }

        [Fact]
        public static void Equals_IsAbelian()
        {
            // Arrange
            var simpleA0 = Maybe.Of(3141);
            var simpleA1 = Maybe.Of(3141);
            var simple = Maybe.Of(1570);

            var valueA0 = Maybe.Of(new ValueStub_(3141));
            var valueA1 = Maybe.Of(new ValueStub_(3141));
            var value = Maybe.Of(new ValueStub_(1570));

            var referenceA0 = Maybe.Of(new List<int>());
            var referenceA1 = Maybe.Of(new List<int>());
            var reference = Maybe.Of(new List<int>() { 0 });

            // Act & Assert
            Assert.Equal(simpleA0.Equals(simpleA1), simpleA1.Equals(simpleA0));
            Assert.Equal(simpleA0.Equals(simple), simple.Equals(simpleA0));
            Assert.Equal(valueA0.Equals(valueA1), valueA1.Equals(valueA0));
            Assert.Equal(valueA0.Equals(value), value.Equals(valueA0));
            Assert.Equal(referenceA0.Equals(referenceA1), referenceA1.Equals(referenceA0));
            Assert.Equal(referenceA0.Equals(reference), reference.Equals(referenceA0));
        }

        [Fact]
        public static void Equals_IsTransitive()
        {
            // Arrange
            var simple1 = Maybe.Of(3141);
            var simple2 = Maybe.Of(3141);
            var simple3 = Maybe.Of(3141);

            var value1 = Maybe.Of(new ValueStub_(3141));
            var value2 = Maybe.Of(new ValueStub_(3141));
            var value3 = Maybe.Of(new ValueStub_(3141));

            var reference1 = Maybe.Of(new List<int>());
            var reference2 = Maybe.Of(new List<int>());
            var reference3 = Maybe.Of(new List<int>());

            // Act & Assert
            Assert.Equal(simple1.Equals(simple2) && simple2.Equals(simple3), simple1.Equals(simple3));
            Assert.Equal(value1.Equals(value2) && value2.Equals(value3), value1.Equals(value3));
            Assert.Equal(reference1.Equals(reference2) && reference2.Equals(reference3), reference1.Equals(reference3));
        }

        [Fact]
        public static void Equals_ReturnsTrue_WhenNone_ForNull()
        {
            // Arrange
            var simple = Maybe<int>.None;
            var value = Maybe<ValueStub_>.None;
            var reference = Maybe<List<int>>.None;

            // Act & Assert
            Assert.True(simple.Equals(null));
            Assert.True(value.Equals(null));
            Assert.True(reference.Equals((object)null));

            // REVIEW: Cast
            Assert.True(reference.Equals((List<int>)null));
            Assert.True(reference.Equals((Maybe<List<int>>)null));
        }

        [Fact]
        public static void Equals_ReturnsTrue_ForOriginalValue()
        {
            // Arrange
            var simple = 3141;
            var simpleOpt = Maybe.Of(simple);

            var value = new ValueStub_(3141);
            var valueOpt = Maybe.Of(value);

            var reference = new List<int>();
            var referenceOpt = Maybe.Of(reference);

            // Act & Assert
            Assert.True(simpleOpt.Equals(simple));
            Assert.True(valueOpt.Equals(value));
            Assert.True(referenceOpt.Equals(reference));
        }

        [Fact]
        public static void Equals_ReturnsTrue_ForOriginalValueCastedToObject()
        {
            // Arrange
            var simple = 3141;
            var simpleOpt = Maybe.Of(simple);

            var value = new ValueStub_(3141);
            var valueOpt = Maybe.Of(value);

            var reference = new List<int>();
            var referenceOpt = Maybe.Of(reference);

            // Act & Assert
            Assert.True(simpleOpt.Equals((object)simple));
            Assert.True(valueOpt.Equals((object)value));
            Assert.True(referenceOpt.Equals((object)reference));
        }

        [Fact]
        public static void Equals_FollowsStructuralEqualityRules()
        {
            // Arrange
            var simpleA0 = Maybe.Of(3141);
            var simpleA1 = Maybe.Of(3141);

            var valueA0 = Maybe.Of(new ValueStub_(3141));
            var valueA1 = Maybe.Of(new ValueStub_(3141));

            var almostValueA0 = Maybe.Of(new AlmostValueStub_ { Value = "Une chaîne de caractère" });
            var almostValueA1 = Maybe.Of(new AlmostValueStub_ { Value = "Une chaîne de caractère" });

            // Act & Assert
            Assert.True(simpleA0.Equals(simpleA1));
            Assert.True(valueA0.Equals(valueA1));
            Assert.True(almostValueA0.Equals(almostValueA1));
        }

        [Fact]
        public static void Equals_FollowsStructuralEqualityRulesAfterCastToObject()
        {
            // Arrange
            var simpleA0 = Maybe.Of(3141);
            var simpleA1 = Maybe.Of(3141);

            var valueA0 = Maybe.Of(new ValueStub_(3141));
            var valueA1 = Maybe.Of(new ValueStub_(3141));

            var almostValueA0 = Maybe.Of(new AlmostValueStub_ { Value = "Une chaîne de caractère" });
            var almostValueA1 = Maybe.Of(new AlmostValueStub_ { Value = "Une chaîne de caractère" });

            // Act & Assert
            Assert.True(simpleA0.Equals((object)simpleA1));
            Assert.True(valueA0.Equals((object)valueA1));
            Assert.True(almostValueA0.Equals((object)almostValueA1));
        }

        #endregion

        #region Of()

        [Fact]
        public static void Of_ReturnsSome_ForNotNull()
        {
            // Arrange
            var simple = 3141;
            var value = new ValueStub_(3141);
            ValueStub_? nullableValue = new ValueStub_(3141);
            var reference = new List<int>();

            // Act
            var simpleOpt = Maybe.Of(simple);
            var valueOpt = Maybe.Of(value);
            var nullableValueOpt = Maybe.Of(nullableValue);
            var referenceOpt = Maybe.Of(reference);

            // Assert
            Assert.True(simpleOpt.IsSome);
            Assert.True(valueOpt.IsSome);
            Assert.True(nullableValueOpt.IsSome);
            Assert.True(referenceOpt.IsSome);
        }

        [Fact]
        public static void Of_ReturnsNone_ForNull()
        {
            // Arrange
            ValueStub_? value = null;
            List<int> reference = null;

            // Act
            var valueOpt = Maybe.Of(value);
            var referenceOpt = Maybe.Of(reference);

            // Assert
            Assert.False(valueOpt.IsSome);
            Assert.False(referenceOpt.IsSome);
        }

        #endregion

        #region Bind()

        /// <summary>
        /// <![CDATA[
        /// Maybe<T>.Bind(selector) returned null when selector returned null. 
        /// The correct behaviour is to return Maybe<T>.None.
        /// ]]>
        /// </summary>
        [Fact, Issue]
        public static void Bind_ReturnsNone_WhenSelectorReturnsNull()
        {
            // Arrange
            var source = Maybe.Of(1);
            Func<int, Maybe<int>> selector = _ => null;

            // Act
            var m = source.Bind(selector);

            // Assert
            Assert.True(m != null);
            Assert.False(m.IsSome);
        }

        #endregion

        #region Linq Operators

        [Fact]
        public static void Where_ThrowsArgumentNullException_ForNullObject()
        {
            // Arrange
            // REVIEW: Cast
            var source = (Maybe<int>)null;
            Func<int, bool> predicate = _ => _ == 1;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.Where(predicate));
            Assert.Throws<ArgumentNullException>(() => from _ in source where predicate(_) select _);
        }

        [Fact]
        public static void Where_ThrowsArgumentNullException_ForNullPredicate()
        {
            // Arrange
            var source = Maybe.Of(1);
            Func<int, bool> predicate = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.Where(predicate));
        }

        [Fact]
        public static void Where_ReturnsNone_ForUnsuccessfulPredicate()
        {
            // Arrange
            var source = Maybe.Of(1);
            Func<int, bool> predicate = _ => _ == 2;

            // Act
            var m = source.Where(predicate);
            var q = from _ in source where predicate(_) select _;

            // Assert
            Assert.False(m.IsSome);
            Assert.False(q.IsSome);
        }

        [Fact]
        public static void Select_ThrowsArgumentNullException_ForNullObject()
        {
            // Arrange
            // REVIEW: Cast
            var source = (Maybe<int>)null;
            Func<int, int> selector = _ => _;

            // Act & Assert
            // NB: Apply only if Select is provided by an extension method.
            ////Assert.Throws<ArgumentNullException>(() => source.Select(selector));
            ////Assert.Throws<ArgumentNullException>(() => from _ in source select selector(_));
            Assert.Throws<NullReferenceException>(() => source.Select(selector));
            Assert.Throws<NullReferenceException>(() => from _ in source select selector(_));
        }

        [Fact]
        public static void Select_ThrowsArgumentNullException_ForNullSelector()
        {
            // Arrange
            var source = Maybe.Of(1);
            Func<int, int> selector = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.Select(selector));
        }

        [Fact]
        public static void Select_ReturnsNone_ForNoneObject()
        {
            // Arrange
            var source = Maybe<int>.None;
            Func<int, int> selector = _ => _;

            // Act
            var m = source.Select(selector);
            var q = from _ in source select selector(_);

            // Assert
            Assert.False(m.IsSome);
            Assert.False(q.IsSome);
        }

        [Fact]
        public static void SelectMany_ThrowsArgumentNullException_ForNullObject()
        {
            // Arrange
            // REVIEW: Cast
            var source = (Maybe<int>)null;
            var middle = Maybe.Of(2);
            Func<int, Maybe<int>> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
            Assert.Throws<ArgumentNullException>(() => from i in source
                                                       from j in middle
                                                       select resultSelector(i, j));
        }

        [Fact]
        public static void SelectMany_ThrowsArgumentNullException_ForNullValueSelector()
        {
            // Arrange
            var source = Maybe.Of(1);
            Func<int, Maybe<int>> valueSelector = null;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
        }

        [Fact]
        public static void SelectMany_ThrowsArgumentNullException_ForNullResultSelector()
        {
            // Arrange
            var source = Maybe.Of(1);
            var middle = Maybe.Of(2);
            Func<int, Maybe<int>> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
        }

        [Fact]
        public static void SelectMany_ReturnsNone_ForNoneObject()
        {
            // Arrange
            var source = Maybe<int>.None;
            var middle = Maybe.Of(2);
            Func<int, Maybe<int>> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Act
            var m = source.SelectMany(valueSelector, resultSelector);
            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);

            // Assert
            Assert.False(m.IsSome);
            Assert.False(q.IsSome);
        }

        [Fact]
        public static void SelectMany_ReturnsNone_ForMiddleIsNone()
        {
            // Arrange
            var source = Maybe.Of(1);
            var middle = Maybe<int>.None;
            Func<int, Maybe<int>> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Act
            var m = source.SelectMany(valueSelector, resultSelector);
            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);

            // Assert
            Assert.False(m.IsSome);
            Assert.False(q.IsSome);
        }

        [Fact]
        public static void SelectMany_ReturnsNone_ForNoneObjectAndNoneMiddle()
        {
            // Arrange
            var source = Maybe<int>.None;
            var middle = Maybe<int>.None;
            Func<int, Maybe<int>> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Act
            var m = source.SelectMany(valueSelector, resultSelector);
            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);

            // Assert
            Assert.False(m.IsSome);
            Assert.False(q.IsSome);
        }

        [Fact]
        public static void Join_ReturnsNone_WhenJoinFailed()
        {
            // Arrange
            var source = Maybe.Of(1);
            var inner = Maybe.Of(2);

            // Act
            var m = source.Join(inner, _ => _, _ => _, (i, j) => i + j);
            var q = from i in source
                    join j in inner on i equals j
                    select i + j;

            // Assert
            Assert.False(m.IsSome);
            Assert.False(q.IsSome);
        }

        #endregion

        #region Monad Laws

        [Fact]
        public static void Maybe_SatisfiesFirstMonoidLaw()
        {
            // Arrange
            var option = Maybe.Of(1);

            // Act
            var left = Maybe<int>.None.OrElse(option);
            var right = option;

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesSecondMonoidLaw()
        {
            // Arrange
            var option = Maybe.Of(1);

            // Act
            var left = option.OrElse(Maybe<int>.None);
            var right = option;

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesThirdMonoidLaw()
        {
            // Arrange
            var optionA = Maybe.Of(1);
            var optionB = Maybe.Of(2);
            var optionC = Maybe.Of(3);

            // Act
            var left = optionA.OrElse(optionB.OrElse(optionC));
            var right = optionA.OrElse(optionB).OrElse(optionC);

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesFirstMonadLaw()
        {
            // Arrange
            int value = 1;
            Func<int, Maybe<long>> kun = _ => Maybe.Of((long)2 * _);

            // Act
            var left = Maybe.Of(value).Bind(kun);
            var right = kun(value);

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesSecondMonadLaw()
        {
            // Arrange
            Func<int, Maybe<int>> create = _ => Maybe.Of(_);
            var option = Maybe.Of(1);

            // Act
            var left = option.Bind(create);
            var right = option;

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesThirdMonadLaw()
        {
            // Arrange
            Maybe<short> m = Maybe.Of((short)1);
            Func<short, Maybe<int>> f = _ => Maybe.Of((int)3 * _);
            Func<int, Maybe<long>> g = _ => Maybe.Of((long)2 * _);

            // Act
            var left = m.Bind(f).Bind(g);
            var right = m.Bind(_ => f(_).Bind(g));

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesMonadZeroRule()
        {
            // Arrange
            Func<int, Maybe<long>> kun = _ => Maybe.Of((long)2 * _);

            // Act
            var left = Maybe<int>.None.Bind(kun);
            var right = Maybe<long>.None;

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesMonadMoreRule()
        {
            // Act
            var leftSome = Maybe.Of(1).Bind(_ => Maybe<int>.None);
            var leftNone = Maybe<int>.None.Bind(_ => Maybe<int>.None);
            var right = Maybe<int>.None;

            // Assert
            Assert.True(leftSome.Equals(right));
            Assert.True(leftNone.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesMonadOrRule()
        {
            // Arrange
            var option = Maybe.Of(2);

            // Act
            var leftSome = option.OrElse(Maybe.Of(1));
            var leftNone = option.OrElse(Maybe<int>.None);
            var right = option;

            // Assert
            Assert.True(leftSome.Equals(right));
            Assert.True(leftNone.Equals(right));
        }

        [Fact]
        public static void Maybe_DoesNotSatisfyRightZeroForPlus()
        {
            // Arrange
            var option = Maybe.Of(2);

            // Act
            var leftSome = Maybe.Of(1).OrElse(option);
            var leftNone = Maybe<int>.None.OrElse(option);
            var right = option;

            // Assert
            Assert.False(leftSome.Equals(right));   // NB: Fails here the "Unit is a right zero for Plus".
            Assert.True(leftNone.Equals(right));
        }

        #endregion
    }

#if NO_INTERNALS_VISIBLE_TO // White-box tests.

    public static partial class MaybeFacts
    {
        [Fact(Skip = "White-box tests disabled in this configuration.")]
        public static void Maybe_BlackBox() { }
    }

#else

    public static partial class MaybeFacts
    {
        private sealed class Stub_
        {
            private readonly int _value;

            public Stub_(int value)
            {
                _value = value;
            }

            public int Value { get { return _value; } }
        }
    }

    public static partial class MaybeFacts
    {
        [Fact]
        public static void Maybe_IsApparentlyImmutable()
        {
            // Arrange
            var stub = new Stub_(1);
            var option = Maybe.Of(stub);

            // Act
            stub = null;

            // Assert
            Assert.True(option.IsSome);
            Assert.NotEqual(null, option.Value);
            Assert.Equal(1, option.Value.Value);
        }

        #region Unit

        [Fact]
        public static void Unit_IsSome()
        {
            // Act & Assert
            Assert.True(Maybe.Unit.IsSome);
            Assert.Equal(Unit.Single, Maybe.Unit.Value);
        }

        #endregion

        #region Value

        [Fact]
        public static void Value_ReturnsTheOriginalValue_WhenSome()
        {
            // Arrange
            var simple = 3141;
            var value = new ValueStub_(3141);
            ValueStub_? nullableValue = new ValueStub_(3141);
            var reference = new List<int>();

            var simpleOpt = Maybe.Of(simple);
            var valueOpt = Maybe.Of(value);
            var nullableValueOpt = Maybe.Of(nullableValue);
            var referenceOpt = Maybe.Of(reference);

            // Act & Assert
            Assert.True(simpleOpt.Value == simple);
            Assert.True(valueOpt.Value == value);
            Assert.True(nullableValueOpt.Value == nullableValue.Value);
            Assert.True(referenceOpt.Value == reference);
        }

        #endregion

        #region Bind()

        [Fact]
        public static void Bind_ReturnsSomeAndApplySelector_WhenSourceIsSome()
        {
            // Arrange
            var source = Maybe.Of(1);
            Func<int, Maybe<int>> selector = _ => Maybe.Of(2 * _);

            // Act
            var m = source.Bind(selector);

            // Assert
            Assert.True(m.IsSome);
            Assert.Equal(2, m.Value);
        }

        #endregion

        #region Linq Operators

        [Fact]
        public static void Where_ReturnsSome_ForSuccessfulPredicate()
        {
            // Arrange
            var source = Maybe.Of(1);
            Func<int, bool> predicate = _ => _ == 1;

            // Act
            var m = source.Where(predicate);
            var q = from _ in source where predicate(_) select _;

            // Assert
            Assert.True(m.IsSome);
            Assert.True(q.IsSome);
            Assert.Equal(1, m.Value);
            Assert.Equal(1, q.Value);
        }

        [Fact]
        public static void Select_ReturnsSomeAndApplySelector_WhenSourceIsSome()
        {
            // Arrange
            var source = Maybe.Of(1);
            Func<int, int> selector = _ => 2 * _;

            // Act
            var m = source.Select(selector);
            var q = from _ in source select selector(_);

            // Assert
            Assert.True(m.IsSome);
            Assert.True(q.IsSome);
            Assert.Equal(2, m.Value);
            Assert.Equal(2, q.Value);
        }

        [Fact]
        public static void SelectMany_ReturnsSomeAndApplySelector()
        {
            // Arrange
            var source = Maybe.Of(1);
            var middle = Maybe.Of(2);
            Func<int, Maybe<int>> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Act
            var m = source.SelectMany(valueSelector, resultSelector);
            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);

            // Assert
            Assert.True(m.IsSome);
            Assert.True(q.IsSome);
            Assert.Equal(3, m.Value);
            Assert.Equal(3, q.Value);
        }

        [Fact]
        public static void Join_ReturnsSome_WhenJoinSucceed()
        {
            // Arrange
            var source = Maybe.Of(1);
            var inner = Maybe.Of(2);

            // Act
            var m = source.Join(inner, _ => 2 * _, _ => _, (i, j) => i + j);
            var q = from i in source
                    join j in inner on 2 * i equals j
                    select i + j;

            // Assert
            Assert.True(m.IsSome);
            Assert.True(q.IsSome);
            Assert.Equal(3, m.Value);
            Assert.Equal(3, q.Value);
        }

        #endregion
    }

#endif
}
