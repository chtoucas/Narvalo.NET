// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;

    using FsCheck.Xunit;
    using Xunit;

    public static partial class MaybeFacts
    {
        #region Unit

        [Fact]
        public static void Unit_IsSome() => Assert.True(Maybe.Unit.IsSome);

        #endregion

        #region None

        [Fact]
        public static void None_IsNone() => Assert.True(Maybe.None.IsNone);

        #endregion

        #region ValueOrDefault()

        [Fact]
        public static void ValueOrDefault_ReturnsValue_IfSome()
        {
            var exp = new My.SimpleObj();
            var some = Maybe.Of(exp);

            Assert.Same(exp, some.ValueOrDefault());
        }

        [Fact]
        public static void ValueOrDefault_ReturnsDefault_IfNone()
        {
            var none = Maybe<My.SimpleObj>.None;
            var exp = default(My.SimpleObj);

            Assert.Same(exp, none.ValueOrDefault());
        }

        #endregion

        #region ValueOrElse()

        [Fact]
        public static void ValueOrElse_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => Some.ValueOrElse((My.SimpleObj)null));
            Assert.Throws<ArgumentNullException>(() => Some.ValueOrElse((Func<My.SimpleObj>)null));
            Assert.Throws<ArgumentNullException>(() => None.ValueOrElse((My.SimpleObj)null));
            Assert.Throws<ArgumentNullException>(() => None.ValueOrElse((Func<My.SimpleObj>)null));
        }

        [Fact]
        public static void ValueOrElse_ReturnsValue_IfSome()
        {
            var exp = new My.SimpleObj();
            var some = Maybe.Of(exp);
            var other = new My.SimpleObj("other");

            Assert.Same(exp, some.ValueOrElse(other));
            Assert.Same(exp, some.ValueOrElse(() => other));
        }

        [Fact]
        public static void ValueOrElse_ReturnsOther_IfNone()
        {
            var none = Maybe<My.SimpleObj>.None;
            var exp = new My.SimpleObj();

            Assert.Same(exp, none.ValueOrElse(exp));
            Assert.Same(exp, none.ValueOrElse(() => exp));
        }

        #endregion

        #region ValueOrThrow()

        [Fact]
        public static void ValueOrThrow_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => Some.ValueOrThrow(null));
            Assert.Throws<ArgumentNullException>(() => None.ValueOrThrow(null));
        }

        [Fact]
        public static void ValueOrThrow_ReturnsValue_IfSome()
        {
            var exp = new My.SimpleObj();
            var some = Maybe.Of(exp);

            Assert.Same(exp, some.ValueOrThrow());
            Assert.Equal(exp, some.ValueOrThrow(() => new My.SimpleException()));
        }

        [Fact]
        public static void ValueOrThrow_Throws_IfNone()
        {
            Assert.Throws<InvalidOperationException>(() => None.ValueOrThrow());
            Assert.Throws<My.SimpleException>(() => None.ValueOrThrow(() => new My.SimpleException()));
        }

        #endregion

        #region Bind()

        [Fact]
        public static void Bind_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => Some.Bind<string>(null));
            Assert.Throws<ArgumentNullException>(() => None.Bind<string>(null));
        }

        #endregion

        #region Contains()

        [Fact]
        public static void Contains_Guards()
        {
            var value = new My.SimpleObj();

            Assert.Throws<ArgumentNullException>(() => Some.Contains(value, null));
            Assert.Throws<ArgumentNullException>(() => None.Contains(value, null));
        }

        [Fact]
        public static void Contains_Succeeds()
        {
            var value = new My.SimpleObj();
            var some = Maybe.Of(value);

            Assert.True(Maybe.Of(1).Contains(1), "Value type.");
            Assert.True(some.Contains(value), "Reference type.");
        }

        [Fact]
        public static void Contains_Succeeds_ForTypeImplementingStructuralEquality()
        {
            var some = Maybe.Of(Tuple.Create("value"));

            Assert.True(some.Contains(Tuple.Create("value")), "References differ but we have structural equality.");
        }

        [Fact]
        public static void Contains_Fails()
        {
            var some = Maybe.Of(new My.SimpleObj());
            var other = new My.SimpleObj("other");

            Assert.False(Maybe.Of(1).Contains(2));
            Assert.False(some.Contains(other));
            Assert.False(some.Contains(new My.SimpleObj()), "References differ.");
        }

        #endregion

        #region Match()

        [Fact]
        public static void Match_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => Some.Match(null, new My.SimpleObj()));
            Assert.Throws<ArgumentNullException>(() => Some.Match(null, () => new My.SimpleObj()));
            Assert.Throws<ArgumentNullException>(() => Some.Match(val => val, (Func<My.SimpleObj>)null));
            Assert.Throws<ArgumentNullException>(() => None.Match(null, new My.SimpleObj()));
            Assert.Throws<ArgumentNullException>(() => None.Match(null, () => new My.SimpleObj()));
            Assert.Throws<ArgumentNullException>(() => None.Match(val => val, (Func<My.SimpleObj>)null));
        }

        #endregion

        #region Coalesce()

        [Fact]
        public static void Coalesce_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => Some.Coalesce(null, new My.SimpleObj("this"), new My.SimpleObj("that")));
            Assert.Throws<ArgumentNullException>(() => Some.Coalesce(null, val => val, () => new My.SimpleObj()));
            Assert.Throws<ArgumentNullException>(() => Some.Coalesce(val => true, null, () => new My.SimpleObj()));
            Assert.Throws<ArgumentNullException>(() => Some.Coalesce(val => true, val => val, null));
            Assert.Throws<ArgumentNullException>(() => None.Coalesce(null, new My.SimpleObj("this"), new My.SimpleObj("that")));
            Assert.Throws<ArgumentNullException>(() => None.Coalesce(null, val => val, () => new My.SimpleObj()));
            Assert.Throws<ArgumentNullException>(() => None.Coalesce(val => true, null, () => new My.SimpleObj()));
            Assert.Throws<ArgumentNullException>(() => None.Coalesce(val => true, val => val, null));
        }

        #endregion

        #region When()

        [Fact]
        public static void When_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => Some.When(null, val => { }));
            Assert.Throws<ArgumentNullException>(() => Some.When(val => true, null));
            Assert.Throws<ArgumentNullException>(() => Some.When(null, val => { }, () => { }));
            Assert.Throws<ArgumentNullException>(() => Some.When(val => true, null, () => { }));
            Assert.Throws<ArgumentNullException>(() => Some.When(val => true, val => { }, null));
            Assert.Throws<ArgumentNullException>(() => None.When(null, val => { }));
            Assert.Throws<ArgumentNullException>(() => None.When(val => true, null));
            Assert.Throws<ArgumentNullException>(() => None.When(null, val => { }, () => { }));
            Assert.Throws<ArgumentNullException>(() => None.When(val => true, null, () => { }));
            Assert.Throws<ArgumentNullException>(() => None.When(val => true, val => { }, null));
        }

        #endregion

        #region Do()

        [Fact]
        public static void Do_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => Some.Do(null, () => { }));
            Assert.Throws<ArgumentNullException>(() => Some.Do(val => { }, null));
            Assert.Throws<ArgumentNullException>(() => None.Do(null, () => { }));
            Assert.Throws<ArgumentNullException>(() => None.Do(val => { }, null));
        }

        [Fact]
        public static void Do_InvokesOnSome_IfSome()
        {
            // Arrange
            var some = Maybe.Of(new My.SimpleObj());
            var onSomeWasCalled = false;
            var onNoneWasCalled = false;
            Action<My.SimpleObj> onSome = _ => onSomeWasCalled = true;
            Action onNone = () => onNoneWasCalled = true;

            // Act
            some.Do(onSome, onNone);

            // Assert
            Assert.True(onSomeWasCalled);
            Assert.False(onNoneWasCalled);
        }

        [Fact]
        public static void Do_InvokesOnNone_IfNone()
        {
            // Arrange
            var none = Maybe<My.SimpleObj>.None;
            var onSomeWasCalled = false;
            var onNoneWasCalled = false;
            Action<My.SimpleObj> onSome = _ => onSomeWasCalled = true;
            Action onNone = () => onNoneWasCalled = true;

            // Act
            none.Do(onSome, onNone);

            // Assert
            Assert.False(onSomeWasCalled);
            Assert.True(onNoneWasCalled);
        }

        #endregion

        #region OnSome()

        [Fact]
        public static void OnSome_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => Some.OnSome(null));
            Assert.Throws<ArgumentNullException>(() => None.OnSome(null));
        }

        [Fact]
        public static void OnSome_InvokesAction_IfSome()
        {
            // Arrange
            var some = Maybe.Of(new My.SimpleObj());
            var wasCalled = false;
            Action<My.SimpleObj> op = _ => wasCalled = true;

            // Act
            some.OnSome(op);

            // Assert
            Assert.True(wasCalled);
        }

        [Fact]
        public static void OnSome_DoesNotInvokeAction_IfNone()
        {
            // Arrange
            var none = Maybe<My.SimpleObj>.None;
            var wasCalled = false;
            Action<My.SimpleObj> op = _ => wasCalled = true;

            // Act
            none.OnSome(op);

            // Assert
            Assert.False(wasCalled);
        }

        #endregion

        #region OnNone()

        [Fact]
        public static void OnNone_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => Some.OnNone(null));
            Assert.Throws<ArgumentNullException>(() => None.OnNone(null));
        }

        [Fact]
        public static void OnNone_InvokesAction_IfNone()
        {
            // Arrange
            var none = Maybe<My.SimpleObj>.None;
            var wasCalled = false;
            Action op = () => wasCalled = true;

            // Act
            none.OnNone(op);

            // Assert
            Assert.True(wasCalled);
        }

        [Fact]
        public static void OnNone_DoesNotInvokeAction_IfSome()
        {
            // Arrange
            var some = Maybe.Of(new My.SimpleObj());
            var wasCalled = false;
            Action op = () => wasCalled = true;

            // Act
            some.OnNone(op);

            // Assert
            Assert.False(wasCalled);
        }

        #endregion

        #region Equals()

        [Fact]
        public static void Equals_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => Some.Equals(Some, null));
            Assert.Throws<ArgumentNullException>(() => Some.Equals(None, null));
            Assert.Throws<ArgumentNullException>(() => None.Equals(None, null));
            Assert.Throws<ArgumentNullException>(() => None.Equals(Some, null));
        }

        #endregion

        #region GetHashCode()

        [Fact]
        public static void GetHashCode_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => Some.GetHashCode(null));
            Assert.Throws<ArgumentNullException>(() => None.GetHashCode(null));
        }

        #endregion
    }

    public static partial class MaybeFacts
    {
        public static Maybe<My.SimpleObj> Some => Maybe.Of(new My.SimpleObj());

        public static Maybe<My.SimpleObj> None => Maybe<My.SimpleObj>.None;
    }

    public static partial class MaybeFacts
    {
        #region IsSome

        [Fact]
        public static void IsSome_IsFalse_IfNone()
        {
            var simple = Maybe<int>.None;
            var value = Maybe<My.SimpleStruct>.None;
            var reference = Maybe<List<int>>.None;

            Assert.False(simple.IsSome);
            Assert.False(value.IsSome);
            Assert.False(reference.IsSome);
        }

        [Fact]
        public static void IsSome_IsTrue_IfSome()
        {
            var simple = Maybe.Of(3141);
            var value = Maybe.Of(new My.SimpleStruct(3141));
            var reference = Maybe.Of(new List<int>());

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

        ////[Fact]
        ////public static void Equality_ReturnsTrue_ForNullAsMaybeAndNull()
        ////{
        ////    // Arrange
        ////    // REVIEW: Cast
        ////    var simple = (Maybe<int>)null;
        ////    var value = (Maybe<My.SimpleStruct>)null;
        ////    var reference = (Maybe<List<int>>)null;

        ////    // Act & Assert
        ////    Assert.True(simple == null);
        ////    Assert.True(value == null);
        ////    Assert.True(reference == null);
        ////}

        ////[Fact]
        ////public static void Equality_ReturnsFalse_ForMaybeNoneAndNull()
        ////{
        ////    // Arrange
        ////    var simple = Maybe<int>.None;
        ////    var value = Maybe<My.SimpleStruct>.None;
        ////    var reference = Maybe<List<int>>.None;

        ////    // Act & Assert
        ////    Assert.False(simple == null);
        ////    Assert.False(value == null);
        ////    Assert.False(reference == null);
        ////}

        [Fact]
        public static void Equality_Succeeds()
        {
            // Arrange
            var simpleA0 = Maybe.Of(3141);
            var simpleA1 = Maybe.Of(3141);

            var valueA0 = Maybe.Of(new My.SimpleStruct(3141));
            var valueA1 = Maybe.Of(new My.SimpleStruct(3141));

            var almostValueA0 = Maybe.Of(new My.EquatableObj("Une chaîne de caractère"));
            var almostValueA1 = Maybe.Of(new My.EquatableObj("Une chaîne de caractère"));

            //// FIXME
            ////var referenceA0 = Maybe.Of(new List<int>());
            ////var referenceA1 = Maybe.Of(new List<int>());

            // Act & Assert
            Assert.True(simpleA0 == simpleA1);
            Assert.True(valueA0 == valueA1);
            Assert.True(almostValueA0 == almostValueA1);
            ////Assert.True(referenceA0 != referenceA1);
        }

        #endregion

        #region op_Inequality()

        ////[Fact]
        ////public static void Inequality_ReturnsFalse_ForNullAsMaybeAndNull()
        ////{
        ////    // Arrange
        ////    // REVIEW: Cast
        ////    var simple = (Maybe<int>)null;
        ////    var value = (Maybe<My.SimpleStruct>)null;
        ////    var reference = (Maybe<List<int>>)null;

        ////    // Act & Assert
        ////    Assert.False(simple != null);
        ////    Assert.False(value != null);
        ////    Assert.False(reference != null);
        ////}

        ////[Fact]
        ////public static void Inequality_ReturnsTrue_ForMaybeNoneAndNull()
        ////{
        ////    // Arrange
        ////    var simple = Maybe<int>.None;
        ////    var value = Maybe<My.SimpleStruct>.None;
        ////    var reference = Maybe<List<int>>.None;

        ////    // Act & Assert
        ////    Assert.True(simple != null);
        ////    Assert.True(value != null);
        ////    Assert.True(reference != null);
        ////}

        [Fact]
        public static void Inequality_Succeeds()
        {
            // Arrange
            var simpleA0 = Maybe.Of(3141);
            var simpleA1 = Maybe.Of(3141);

            var valueA0 = Maybe.Of(new My.SimpleStruct(3141));
            var valueA1 = Maybe.Of(new My.SimpleStruct(3141));

            var almostValueA0 = Maybe.Of(new My.EquatableObj("Une chaîne de caractère"));
            var almostValueA1 = Maybe.Of(new My.EquatableObj("Une chaîne de caractère"));

            //// FIXME
            ////var referenceA0 = Maybe.Of(new List<int>());
            ////var referenceA1 = Maybe.Of(new List<int>());

            // Act & Assert
            Assert.False(simpleA0 != simpleA1);
            Assert.False(valueA0 != valueA1);
            Assert.False(almostValueA0 != almostValueA1);
            ////Assert.False(referenceA0 != referenceA1);
        }

        #endregion

        #region Equals()

        [Fact]
        public static void Equals_IsReflexive()
        {
            // Arrange
            var simple = Maybe.Of(3141);
            var value = Maybe.Of(new My.SimpleStruct(3141));
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

            var valueA0 = Maybe.Of(new My.SimpleStruct(3141));
            var valueA1 = Maybe.Of(new My.SimpleStruct(3141));
            var value = Maybe.Of(new My.SimpleStruct(1570));

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

            var value1 = Maybe.Of(new My.SimpleStruct(3141));
            var value2 = Maybe.Of(new My.SimpleStruct(3141));
            var value3 = Maybe.Of(new My.SimpleStruct(3141));

            var reference1 = Maybe.Of(new List<int>());
            var reference2 = Maybe.Of(new List<int>());
            var reference3 = Maybe.Of(new List<int>());

            // Act & Assert
            Assert.Equal(simple1.Equals(simple2) && simple2.Equals(simple3), simple1.Equals(simple3));
            Assert.Equal(value1.Equals(value2) && value2.Equals(value3), value1.Equals(value3));
            Assert.Equal(reference1.Equals(reference2) && reference2.Equals(reference3), reference1.Equals(reference3));
        }

        [Fact]
        public static void Equals_ReturnsFalse_IfNone_ForNull()
        {
            // Arrange
            var simple = Maybe<int>.None;
            var value = Maybe<My.SimpleStruct>.None;
            var reference = Maybe<List<int>>.None;

            // Act & Assert
            Assert.False(simple.Equals(null));
            Assert.False(value.Equals(null));
            Assert.False(reference.Equals((object)null));
        }

        ////[Fact]
        ////public static void Equals_ReturnsTrue_IfNone_ForNullOfUnderlyingType()
        ////{
        ////    // Arrange
        ////    var reference = Maybe<List<int>>.None;

        ////    // Act & Assert
        ////    Assert.True(reference.Equals((List<int>)null));
        ////    //Assert.True(reference.Equals((Maybe<List<int>>)null));
        ////}

        ////[Fact]
        ////public static void Equals_ReturnsTrue_ForOriginalValue()
        ////{
        ////    // Arrange
        ////    var simple = 3141;
        ////    var simpleOpt = Maybe.Of(simple);

        ////    var value = new My.SimpleStruct(3141);
        ////    var valueOpt = Maybe.Of(value);

        ////    var reference = new List<int>();
        ////    var referenceOpt = Maybe.Of(reference);

        ////    // Act & Assert
        ////    Assert.True(simpleOpt.Equals(simple));
        ////    Assert.True(valueOpt.Equals(value));
        ////    Assert.True(referenceOpt.Equals(reference));
        ////}

        ////[Fact]
        ////public static void Equals_ReturnsTrue_ForOriginalValueCastedToObject()
        ////{
        ////    // Arrange
        ////    var simple = 3141;
        ////    var simpleOpt = Maybe.Of(simple);

        ////    var value = new My.SimpleStruct(3141);
        ////    var valueOpt = Maybe.Of(value);

        ////    var reference = new List<int>();
        ////    var referenceOpt = Maybe.Of(reference);

        ////    // Act & Assert
        ////    Assert.True(simpleOpt.Equals((object)simple));
        ////    Assert.True(valueOpt.Equals((object)value));
        ////    Assert.True(referenceOpt.Equals((object)reference));
        ////}

        [Fact]
        public static void Equals_FollowsStructuralEqualityRules()
        {
            // Arrange
            var simpleA0 = Maybe.Of(3141);
            var simpleA1 = Maybe.Of(3141);

            var valueA0 = Maybe.Of(new My.SimpleStruct(3141));
            var valueA1 = Maybe.Of(new My.SimpleStruct(3141));

            var almostValueA0 = Maybe.Of(new My.EquatableObj("Une chaîne de caractère"));
            var almostValueA1 = Maybe.Of(new My.EquatableObj("Une chaîne de caractère"));

            // Act & Assert
            Assert.True(simpleA0.Equals(simpleA1));
            Assert.True(valueA0.Equals(valueA1));
            Assert.True(almostValueA0.Equals(almostValueA1));
        }

        [Fact]
        public static void Equals_FollowsStructuralEqualityRules_AfterBoxing()
        {
            // Arrange
            var simpleA0 = Maybe.Of(3141);
            var simpleA1 = Maybe.Of(3141);

            var valueA0 = Maybe.Of(new My.SimpleStruct(3141));
            var valueA1 = Maybe.Of(new My.SimpleStruct(3141));

            var almostValueA0 = Maybe.Of(new My.EquatableObj("Une chaîne de caractère"));
            var almostValueA1 = Maybe.Of(new My.EquatableObj("Une chaîne de caractère"));

            // Act & Assert
            Assert.True(simpleA0.Equals((object)simpleA1));
            Assert.True(valueA0.Equals((object)valueA1));
            Assert.True(almostValueA0.Equals((object)almostValueA1));
        }

        #endregion

        #region Of()

        [Fact]
        public static void Of_ReturnsSome_ForNonNull()
        {
            // Arrange
            var simple = 3141;
            var value = new My.SimpleStruct(3141);
            My.SimpleStruct? nullableValue = new My.SimpleStruct(3141);
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
            My.SimpleStruct? value = null;
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
        /// Maybe<T>.Bind(selector) returned null when selector returned null.
        /// The correct behaviour is to return Maybe<T>.None.
        /// </summary>
        ////[Fact, Issue]
        ////public static void Bind_ReturnsNone_IfSelectorReturnsNull()
        ////{
        ////    // Arrange
        ////    var source = Maybe.Of(1);
        ////    Func<int, Maybe<int>> selector = _ => null;

        ////    // Act
        ////    var m = source.Bind(selector);

        ////    // Assert
        ////    Assert.True(m != null);
        ////    Assert.False(m.IsSome);
        ////}

        #endregion

        #region Linq Operators

        ////[Fact]
        ////public static void Where_ThrowsArgumentNullException_ForNullObject()
        ////{
        ////    // Arrange
        ////    // REVIEW: Cast
        ////    var source = (Maybe<int>)null;
        ////    Func<int, bool> predicate = _ => _ == 1;

        ////    // Act & Assert
        ////    Assert.Throws<ArgumentNullException>(() => source.Where(predicate));
        ////    Assert.Throws<ArgumentNullException>(() => from _ in source where predicate(_) select _);
        ////}

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

        ////[Fact]
        ////public static void Select_ThrowsArgumentNullException_ForNullObject()
        ////{
        ////    // Arrange
        ////    // REVIEW: Cast
        ////    var source = (Maybe<int>)null;
        ////    Func<int, int> selector = _ => _;

        ////    // Act & Assert
        ////    // NB: Apply only if Select is provided by an extension method.
        ////    ////Assert.Throws<ArgumentNullException>(() => source.Select(selector));
        ////    ////Assert.Throws<ArgumentNullException>(() => from _ in source select selector(_));
        ////    Assert.Throws<NullReferenceException>(() => source.Select(selector));
        ////    Assert.Throws<NullReferenceException>(() => from _ in source select selector(_));
        ////}

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

        ////[Fact]
        ////public static void SelectMany_ThrowsArgumentNullException_ForNullObject()
        ////{
        ////    // Arrange
        ////    // REVIEW: Cast
        ////    var source = (Maybe<int>)null;
        ////    var middle = Maybe.Of(2);
        ////    Func<int, Maybe<int>> valueSelector = _ => middle;
        ////    Func<int, int, int> resultSelector = (i, j) => i + j;

        ////    // Act & Assert
        ////    Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
        ////    Assert.Throws<ArgumentNullException>(() => from i in source
        ////                                               from j in middle
        ////                                               select resultSelector(i, j));
        ////}

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
        public static void Join_ReturnsNone_IfJoinFailed()
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
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class MaybeFacts
    {
        [Fact]
        public static void Maybe_IsImmutable()
        {
            // Arrange
            var value = new My.ImmutableObj(1);
            var option = Maybe.Of(value);

            // Act
            value = null;

            // Assert
            Assert.True(option.IsSome);
            Assert.NotEqual(null, option.Value);
            Assert.Equal(1, option.Value.Value);
        }

        #region Unit

        [Fact]
        public static void Unit_IsSomeXXX()
        {
            // Act & Assert
            Assert.Equal(Unit.Default, Maybe.Unit.Value);
        }

        #endregion

        #region Value

        [Fact]
        public static void Value_ReturnsTheOriginalValue_IfSome()
        {
            // Arrange
            var simple = 3141;
            var value = new My.SimpleStruct(3141);
            My.SimpleStruct? nullableValue = new My.SimpleStruct(3141);
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
        public static void Bind_ReturnsSomeAndApplySelector_IfSourceIsSome()
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
        public static void Select_ReturnsSomeAndApplySelector_IfSourceIsSome()
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
        public static void Join_ReturnsSome_IfJoinSucceed()
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
