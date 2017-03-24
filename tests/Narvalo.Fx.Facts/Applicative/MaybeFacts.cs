// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    using static global::My;

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

        #region IsSome / IsNone

        [Fact]
        public static void IsSome_IsFalse_IfNone()
        {
            var o1 = Maybe<int>.None;
            var o2 = Maybe<Val>.None;
            var o3 = Maybe<Val?>.None;
            var o4 = Maybe<Obj>.None;

            Assert.False(o1.IsSome);
            Assert.False(o2.IsSome);
            Assert.False(o3.IsSome);
            Assert.False(o4.IsSome);
        }

        [Fact]
        public static void IsSome_IsTrue_IfSome()
        {
            var o1 = Maybe.Of(1);
            var o2 = Maybe.Of(new Val(1));
            var o3 = Maybe.Of((Val?)new Val(1));
            var o4 = Maybe.Of(new Obj());

            Assert.True(o1.IsSome);
            Assert.True(o2.IsSome);
            Assert.True(o3.IsSome);
            Assert.True(o4.IsSome);
        }

        [Fact]
        public static void IsSome_IsNotNone()
        {
            var o1 = Maybe<int>.None;
            var o2 = Maybe<Val>.None;
            var o3 = Maybe<Val?>.None;
            var o4 = Maybe<Obj>.None;
            var o5 = Maybe.Of(1);
            var o6 = Maybe.Of(new Val(1));
            var o7 = Maybe.Of((Val?)new Val(1));
            var o8 = Maybe.Of(new Obj());

            Assert.Equal(o1.IsSome, !o1.IsNone);
            Assert.Equal(o2.IsSome, !o2.IsNone);
            Assert.Equal(o3.IsSome, !o3.IsNone);
            Assert.Equal(o4.IsSome, !o4.IsNone);
            Assert.Equal(o5.IsSome, !o5.IsNone);
            Assert.Equal(o6.IsSome, !o6.IsNone);
            Assert.Equal(o7.IsSome, !o7.IsNone);
            Assert.Equal(o8.IsSome, !o8.IsNone);
        }

        [Fact]
        public static void IsSome_IsImmutable_OnceTrue()
        {
            // Arrange
            var obj = new Obj();
            var some = Maybe.Of(obj);

            Assert.True(some.IsSome);

            obj = null;

            Assert.True(some.IsSome);
        }

        [Fact]
        public static void IsNone_IsImmutable_OnceTrue()
        {
            Obj obj = null;
            var none = Maybe.Of(obj);

            Assert.True(none.IsNone);

            obj = new Obj();

            Assert.True(none.IsNone);
        }

        #endregion

        #region Of()

        [Fact]
        public static void Of_ReturnsNone_ForNull()
        {
            var o1 = Maybe.Of((Obj)null);
            var o2 = Maybe.Of((Val?)null);

            Assert.True(o1.IsNone);
            Assert.True(o2.IsNone);
        }

        [Fact]
        public static void Of_ReturnsSome_ForNonNull()
        {
            var o1 = Maybe.Of(1);
            var o2 = Maybe.Of(new Val(1));
            var o3 = Maybe.Of((Val?)new Val(1));
            var o4 = Maybe.Of(new Obj());

            Assert.True(o1.IsSome);
            Assert.True(o2.IsSome);
            Assert.True(o3.IsSome);
            Assert.True(o4.IsSome);
        }

        #endregion

        #region Cast to value

        [Fact]
        public static void CastToValue_Throws_IfNone()
        {
            var none = Maybe<Obj>.None;

            Action act = () => { var _ = (Obj)none; };
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<InvalidCastException>(ex);
        }

        [Fact]
        public static void CastToValue_ReturnsValue_IfSome()
        {
            var exp = new Obj();
            var some = Maybe.Of(exp);

            Assert.Same(exp, (Obj)some);
        }

        #endregion

        #region Cast from value

        [Fact]
        public static void CastFromValue_ReturnsNone_IfNull()
        {
            var maybe = (Maybe<Obj>)null;

            Assert.True(maybe.IsNone);
        }

        [Fact]
        public static void CastFromValue_ReturnsSome_IfNonNull()
        {
            var exp = new Obj();
            var maybe = (Maybe<Obj>)exp;

            Assert.True(maybe.IsSome);
            Assert.Same(exp, maybe.Value);
        }

        #endregion

        #region ValueOrDefault()

        [Fact]
        public static void ValueOrDefault_ReturnsValue_IfSome()
        {
            var exp = new Obj();
            var some = Maybe.Of(exp);

            Assert.Same(exp, some.ValueOrDefault());
        }

        [Fact]
        public static void ValueOrDefault_ReturnsDefault_IfNone()
        {
            var none = Maybe<Obj>.None;
            var exp = default(Obj);

            Assert.Same(exp, none.ValueOrDefault());
        }

        #endregion

        #region ValueOrElse()

        [Fact]
        public static void ValueOrElse_Guards()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("other", () => some.ValueOrElse((Obj)null));
            Assert.Throws<ArgumentNullException>("valueFactory", () => some.ValueOrElse((Func<Obj>)null));

            Assert.Throws<ArgumentNullException>("other", () => none.ValueOrElse((Obj)null));
            Assert.Throws<ArgumentNullException>("valueFactory", () => none.ValueOrElse((Func<Obj>)null));
        }

        [Fact]
        public static void ValueOrElse_ReturnsValue_IfSome()
        {
            var exp = new Obj();
            var some = Maybe.Of(exp);
            var other = new Obj("other");

            Assert.Same(exp, some.ValueOrElse(other));
            Assert.Same(exp, some.ValueOrElse(() => other));
        }

        [Fact]
        public static void ValueOrElse_ReturnsOther_IfNone()
        {
            var none = Maybe<Obj>.None;
            var exp = new Obj();

            Assert.Same(exp, none.ValueOrElse(exp));
            Assert.Same(exp, none.ValueOrElse(() => exp));
        }

        #endregion

        #region ValueOrThrow()

        [Fact]
        public static void ValueOrThrow_Guards()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("exceptionFactory", () => some.ValueOrThrow(null));
            Assert.Throws<ArgumentNullException>("exceptionFactory", () => none.ValueOrThrow(null));
        }

        [Fact]
        public static void ValueOrThrow_ReturnsValue_IfSome()
        {
            var exp = new Obj();
            var some = Maybe.Of(exp);

            Assert.Same(exp, some.ValueOrThrow());
            Assert.Same(exp, some.ValueOrThrow(() => new SimpleException()));
        }

        [Fact]
        public static void ValueOrThrow_Throws_IfNone()
        {
            var none = Maybe<Obj>.None;

            Assert.Throws<InvalidOperationException>(() => none.ValueOrThrow());
            Assert.Throws<SimpleException>(() => none.ValueOrThrow(() => new SimpleException()));
        }

        #endregion

        #region Bind()

        [Fact]
        public static void Bind_Guards()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("binder", () => some.Bind<string>(null));
            Assert.Throws<ArgumentNullException>("binder", () => none.Bind<string>(null));
        }

        [Fact]
        public static void Bind_ReturnsNone_IfNone()
        {
            // Arrange
            var none = Maybe<Obj>.None;
            Func<Obj, Maybe<string>> binder = val => Maybe.Of(val.Value);

            // Act
            var me = none.Bind(binder);

            // Assert
            Assert.True(me.IsNone);
        }

        [Fact]
        public static void Bind_ReturnsSome_IfSome()
        {
            // Arrange
            var exp = new Obj();
            var some = Maybe.Of(exp);
            Func<Obj, Maybe<string>> binder = val => Maybe.Of(val.Value);

            // Act
            var me = some.Bind(binder);

            // Assert
            Assert.True(me.IsSome);
        }

        #endregion

        #region Flatten()

        [Fact]
        public static void Flatten_ReturnsNone_IfNone()
        {
            var me = Maybe<Maybe<Obj>>.None.Flatten();

            Assert.True(me.IsNone);
        }

        [Fact]
        public static void Flatten_ReturnsSome_IfSome()
        {
            var me = Maybe.Of(Maybe.Of(new Obj())).Flatten();

            Assert.True(me.IsSome);
        }

        #endregion

        #region OrElse()

        [Fact]
        public static void OrElse_ReturnsOther_IfNone()
        {
            var none = Maybe<Obj>.None;
            var other = Maybe.Of(new Obj());

            var obj = none.OrElse(other);

            Assert.Equal(other, obj);
        }

        [Fact]
        public static void OrElse_ReturnsObj_IfSome()
        {
            var some = Maybe.Of(new Obj());
            var other = Maybe.Of(new Obj("other"));

            var obj = some.OrElse(other);

            Assert.Equal(some, obj);
        }

        #endregion

        #region ToEnumerable()

        [Fact]
        public static void ToEnumerable_ReturnsEmpty_IfNone()
        {
            var none = Maybe<Obj>.None;
            var seq = none.ToEnumerable();

            Assert.Empty(seq);
        }

        [Fact]
        public static void ToEnumerable_ReturnsSequenceWithUnderlyingValue_IfSome()
        {
            var obj = new Obj();
            var some = Maybe.Of(obj);
            var seq = some.ToEnumerable();

            Assert.Equal(Enumerable.Repeat(obj, 1), seq);
        }

        #endregion

        #region Contains()

        [Fact]
        public static void Contains_Guards()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());
            var value = new Obj();

            Assert.Throws<ArgumentNullException>("comparer", () => some.Contains(value, null));
            Assert.Throws<ArgumentNullException>("comparer", () => none.Contains(value, null));
        }

        [Fact]
        public static void Contains_ReturnsFalse_IfNone()
        {
            var n1 = Maybe<int>.None;
            var n2 = Maybe<Obj>.None;

            Assert.False(n1.Contains(1));
            Assert.False(n1.Contains(1, EqualityComparer<int>.Default));
            Assert.False(n2.Contains(new Obj()));
            Assert.False(n2.Contains(new Obj(), EqualityComparer<Obj>.Default));
        }

        [Fact]
        public static void Contains_ReturnsTrue_ForCorrectValue()
        {
            var value = new Obj();
            var s1 = Maybe.Of(1);
            var s2 = Maybe.Of(value);

            Assert.True(s1.Contains(1), "Value type.");
            Assert.True(s1.Contains(1, EqualityComparer<int>.Default), "Value type.");
            Assert.True(s2.Contains(value), "Reference type.");
            Assert.True(s2.Contains(value, EqualityComparer<Obj>.Default), "Reference type.");
        }

        [Fact]
        public static void Contains_ReturnsTrue_ForTypeImplementingStructuralEquality()
        {
            var some = Maybe.Of(Tuple.Create("value"));

            Assert.True(some.Contains(Tuple.Create("value")), "References differ but we have structural equality.");
            Assert.True(some.Contains(Tuple.Create("value"), EqualityComparer<Tuple<string>>.Default), "References differ but we have structural equality.");
        }

        [Fact]
        public static void Contains_ReturnsFalse_ForIncorrectValue()
        {
            var s1 = Maybe.Of(1);
            var s2 = Maybe.Of(new Obj());
            var other = new Obj("other");

            Assert.False(s1.Contains(2));
            Assert.False(s1.Contains(2, EqualityComparer<int>.Default));
            Assert.False(s2.Contains(other, EqualityComparer<Obj>.Default));
            Assert.False(s2.Contains(new Obj(), EqualityComparer<Obj>.Default), "References differ.");
        }

        #endregion

        #region Match()

        [Fact]
        public static void Match_Guards()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("caseSome", () => some.Match(null, new Obj()));
            Assert.Throws<ArgumentNullException>("caseSome", () => some.Match(null, () => new Obj()));
            Assert.Throws<ArgumentNullException>("caseNone", () => some.Match(val => val, (Func<Obj>)null));

            Assert.Throws<ArgumentNullException>("caseSome", () => none.Match(null, new Obj()));
            Assert.Throws<ArgumentNullException>("caseSome", () => none.Match(null, () => new Obj()));
            Assert.Throws<ArgumentNullException>("caseNone", () => none.Match(val => val, (Func<Obj>)null));
        }

        #endregion

        #region Coalesce()

        [Fact]
        public static void Coalesce_Guards()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("predicate", () => some.Coalesce(null, new Obj("this"), new Obj("that")));
            Assert.Throws<ArgumentNullException>("predicate", () => some.Coalesce(null, val => val, () => new Obj()));
            Assert.Throws<ArgumentNullException>("selector", () => some.Coalesce(val => true, null, () => new Obj()));
            Assert.Throws<ArgumentNullException>("otherwise", () => some.Coalesce(val => true, val => val, null));

            Assert.Throws<ArgumentNullException>("predicate", () => none.Coalesce(null, new Obj("this"), new Obj("that")));
            Assert.Throws<ArgumentNullException>("predicate", () => none.Coalesce(null, val => val, () => new Obj()));
            Assert.Throws<ArgumentNullException>("selector", () => none.Coalesce(val => true, null, () => new Obj()));
            Assert.Throws<ArgumentNullException>("otherwise", () => none.Coalesce(val => true, val => val, null));
        }

        #endregion

        #region When()

        [Fact]
        public static void When_Guards()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("predicate", () => some.When(null, val => { }));
            Assert.Throws<ArgumentNullException>("action", () => some.When(val => true, null));
            Assert.Throws<ArgumentNullException>("predicate", () => some.When(null, val => { }, () => { }));
            Assert.Throws<ArgumentNullException>("action", () => some.When(val => true, null, () => { }));
            Assert.Throws<ArgumentNullException>("otherwise", () => some.When(val => true, val => { }, null));

            Assert.Throws<ArgumentNullException>("predicate", () => none.When(null, val => { }));
            Assert.Throws<ArgumentNullException>("action", () => none.When(val => true, null));
            Assert.Throws<ArgumentNullException>("predicate", () => none.When(null, val => { }, () => { }));
            Assert.Throws<ArgumentNullException>("action", () => none.When(val => true, null, () => { }));
            Assert.Throws<ArgumentNullException>("otherwise", () => none.When(val => true, val => { }, null));
        }

        #endregion

        #region Do()

        [Fact]
        public static void Do_Guards()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("onSome", () => some.Do(null, () => { }));
            Assert.Throws<ArgumentNullException>("onNone", () => some.Do(val => { }, null));

            Assert.Throws<ArgumentNullException>("onSome", () => none.Do(null, () => { }));
            Assert.Throws<ArgumentNullException>("onNone", () => none.Do(val => { }, null));
        }

        [Fact]
        public static void Do_InvokesOnSome_IfSome()
        {
            // Arrange
            var some = Maybe.Of(new Obj());
            var onSomeWasCalled = false;
            var onNoneWasCalled = false;
            Action<Obj> onSome = _ => onSomeWasCalled = true;
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
            var none = Maybe<Obj>.None;
            var onSomeWasCalled = false;
            var onNoneWasCalled = false;
            Action<Obj> onSome = _ => onSomeWasCalled = true;
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
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("action", () => some.OnSome(null));
            Assert.Throws<ArgumentNullException>("action", () => none.OnSome(null));
        }

        [Fact]
        public static void OnSome_InvokesAction_IfSome()
        {
            // Arrange
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            Action<Obj> op = _ => wasCalled = true;

            // Act
            some.OnSome(op);

            // Assert
            Assert.True(wasCalled);
        }

        [Fact]
        public static void OnSome_DoesNotInvokeAction_IfNone()
        {
            // Arrange
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            Action<Obj> op = _ => wasCalled = true;

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
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("action", () => some.OnNone(null));
            Assert.Throws<ArgumentNullException>("action", () => none.OnNone(null));
        }

        [Fact]
        public static void OnNone_InvokesAction_IfNone()
        {
            // Arrange
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            Action act = () => wasCalled = true;

            // Act
            none.OnNone(act);

            // Assert
            Assert.True(wasCalled);
        }

        [Fact]
        public static void OnNone_DoesNotInvokeAction_IfSome()
        {
            // Arrange
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            Action act = () => wasCalled = true;

            // Act
            some.OnNone(act);

            // Assert
            Assert.False(wasCalled);
        }

        #endregion

        #region op_Equality() // op_Inequality()

        [Fact]
        public static void Equality_ReturnsTrue()
        {
            var o1a = Maybe.Of(1);
            var o1b = Maybe.Of(1);
            var o2a = Maybe.Of(new Val(1));
            var o2b = Maybe.Of(new Val(1));
            var o3a = Maybe.Of((Val?)new Val(1));
            var o3b = Maybe.Of((Val?)new Val(1));
            var o4a = Maybe.Of(Tuple.Create("1"));
            var o4b = Maybe.Of(Tuple.Create("1"));

            Assert.True(o1a == o1b);
            Assert.True(o2a == o2b);
            Assert.True(o3a == o3b);
            Assert.True(o4a == o4b);

            Assert.False(o1a != o1b);
            Assert.False(o2a != o2b);
            Assert.False(o3a != o3b);
            Assert.False(o4a != o4b);
        }

        [Fact]
        public static void Equality_ReturnsFalse()
        {
            var o1a = Maybe.Of(1);
            var o1b = Maybe.Of(2);
            var o2a = Maybe.Of(new Val(1));
            var o2b = Maybe.Of(new Val(2));
            var o3a = Maybe.Of((Val?)new Val(1));
            var o3b = Maybe.Of((Val?)new Val(2));
            var o4a = Maybe.Of(Tuple.Create("1"));
            var o4b = Maybe.Of(Tuple.Create("2"));
            var o5a = Maybe.Of(new Obj());
            var o5b = Maybe.Of(new Obj());

            Assert.False(o1a == o1b);
            Assert.False(o2a == o2b);
            Assert.False(o3a == o3b);
            Assert.False(o4a == o4b);
            Assert.False(o5a == o5b);

            Assert.True(o1a != o1b);
            Assert.True(o2a != o2b);
            Assert.True(o3a != o3b);
            Assert.True(o4a != o4b);
            Assert.True(o5a != o5b);
        }

        #endregion

        #region Equals()

        [Fact]
        public static void Equals_Guards()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("comparer", () => some.Equals(some, null));
            Assert.Throws<ArgumentNullException>("comparer", () => some.Equals(none, null));

            Assert.Throws<ArgumentNullException>("comparer", () => none.Equals(none, null));
            Assert.Throws<ArgumentNullException>("comparer", () => none.Equals(some, null));
        }

        [Fact]
        public static void Equals_IsReflexive()
        {
            var o1 = Maybe.Of(1);
            var o2 = Maybe.Of(new Val(1));
            var o3 = Maybe.Of((Val?)new Val(1));
            var o4 = Maybe.Of(new Obj());

            Assert.True(o1.Equals(o1));
            Assert.True(o2.Equals(o2));
            Assert.True(o3.Equals(o3));
            Assert.True(o4.Equals(o4));
            Assert.True(o1.Equals(o1, EqualityComparer<int>.Default));
            Assert.True(o2.Equals(o2, EqualityComparer<Val>.Default));
            Assert.True(o3.Equals(o3, EqualityComparer<Val>.Default));
            Assert.True(o4.Equals(o4, EqualityComparer<Obj>.Default));
        }

        [Fact]
        public static void Equals_IsAbelian()
        {
            var o1a = Maybe.Of(1);
            var o1b = Maybe.Of(1);
            var o1c = Maybe.Of(2);

            var o2a = Maybe.Of(new Val(1));
            var o2b = Maybe.Of(new Val(1));
            var o2c = Maybe.Of(new Val(2));

            var o3a = Maybe.Of((Val?)new Val(1));
            var o3b = Maybe.Of((Val?)new Val(1));
            var o3c = Maybe.Of((Val?)new Val(2));

            var o4a = Maybe.Of(new Obj());
            var o4b = Maybe.Of(new Obj());
            var o4c = Maybe.Of(new Obj("other"));

            Assert.Equal(o1a.Equals(o1b), o1b.Equals(o1a));
            Assert.Equal(o1a.Equals(o1c), o1c.Equals(o1a));
            Assert.Equal(o2a.Equals(o2b), o2b.Equals(o2a));
            Assert.Equal(o2a.Equals(o2c), o2c.Equals(o2a));
            Assert.Equal(o3a.Equals(o3b), o2b.Equals(o3a));
            Assert.Equal(o3a.Equals(o3c), o2c.Equals(o3a));
            Assert.Equal(o4a.Equals(o4b), o4b.Equals(o4a));
            Assert.Equal(o4a.Equals(o4c), o4c.Equals(o4a));
            Assert.Equal(o1a.Equals(o1b, EqualityComparer<int>.Default), o1b.Equals(o1a, EqualityComparer<int>.Default));
            Assert.Equal(o1a.Equals(o1c, EqualityComparer<int>.Default), o1c.Equals(o1a, EqualityComparer<int>.Default));
            Assert.Equal(o2a.Equals(o2b, EqualityComparer<Val>.Default), o2b.Equals(o2a, EqualityComparer<Val>.Default));
            Assert.Equal(o2a.Equals(o2c, EqualityComparer<Val>.Default), o2c.Equals(o2a, EqualityComparer<Val>.Default));
            Assert.Equal(o3a.Equals(o3b, EqualityComparer<Val>.Default), o2b.Equals(o3a, EqualityComparer<Val>.Default));
            Assert.Equal(o3a.Equals(o3c, EqualityComparer<Val>.Default), o2c.Equals(o3a, EqualityComparer<Val>.Default));
            Assert.Equal(o4a.Equals(o4b, EqualityComparer<Obj>.Default), o4b.Equals(o4a, EqualityComparer<Obj>.Default));
            Assert.Equal(o4a.Equals(o4c, EqualityComparer<Obj>.Default), o4c.Equals(o4a, EqualityComparer<Obj>.Default));
        }

        [Fact]
        public static void Equals_IsTransitive()
        {
            var o1a = Maybe.Of(1);
            var o1b = Maybe.Of(1);
            var o1c = Maybe.Of(1);

            var o2a = Maybe.Of(new Val(1));
            var o2b = Maybe.Of(new Val(1));
            var o2c = Maybe.Of(new Val(1));

            var o3a = Maybe.Of((Val?)new Val(1));
            var o3b = Maybe.Of((Val?)new Val(1));
            var o3c = Maybe.Of((Val?)new Val(1));

            var o4a = Maybe.Of(new Obj());
            var o4b = Maybe.Of(new Obj());
            var o4c = Maybe.Of(new Obj());

            Assert.Equal(o1a.Equals(o1b) && o1b.Equals(o1c), o1a.Equals(o1c));
            Assert.Equal(o2a.Equals(o2b) && o2b.Equals(o2c), o2a.Equals(o2c));
            Assert.Equal(o3a.Equals(o3b) && o3b.Equals(o3c), o2a.Equals(o3c));
            Assert.Equal(o4a.Equals(o4b) && o4b.Equals(o4c), o4a.Equals(o4c));
            Assert.Equal(o1a.Equals(o1b, EqualityComparer<int>.Default) && o1b.Equals(o1c, EqualityComparer<int>.Default), o1a.Equals(o1c, EqualityComparer<int>.Default));
            Assert.Equal(o2a.Equals(o2b, EqualityComparer<Val>.Default) && o2b.Equals(o2c, EqualityComparer<Val>.Default), o2a.Equals(o2c, EqualityComparer<Val>.Default));
            Assert.Equal(o3a.Equals(o3b, EqualityComparer<Val>.Default) && o3b.Equals(o3c, EqualityComparer<Val>.Default), o2a.Equals(o3c, EqualityComparer<Val>.Default));
            Assert.Equal(o4a.Equals(o4b, EqualityComparer<Obj>.Default) && o4b.Equals(o4c, EqualityComparer<Obj>.Default), o4a.Equals(o4c, EqualityComparer<Obj>.Default));
        }

        [Fact]
        public static void Equals_ReturnsFalse_IfNone_ForNull()
        {
            var o1 = Maybe<int>.None;
            var o2 = Maybe<Val>.None;
            var o3 = Maybe<Val?>.None;
            var o4 = Maybe<Obj>.None;

            Assert.False(o1.Equals(null));
            Assert.False(o2.Equals(null));
            Assert.False(o3.Equals(null));
            Assert.False(o4.Equals(null));
            Assert.False(o1.Equals(null, EqualityComparer<int>.Default));
            Assert.False(o2.Equals(null, EqualityComparer<Val>.Default));
            Assert.False(o3.Equals(null, EqualityComparer<Val?>.Default));
            Assert.False(o4.Equals(null, EqualityComparer<Obj>.Default));
        }

        [Fact]
        public static void Equals_ReturnsFalse_IfSome_ForNull()
        {
            var o1 = Maybe.Of(1);
            var o2 = Maybe.Of(new Val(1));
            var o3 = Maybe.Of((Val?)new Val(1));
            var o4 = Maybe.Of(new Obj());

            Assert.False(o1.Equals(null));
            Assert.False(o2.Equals(null));
            Assert.False(o3.Equals(null));
            Assert.False(o4.Equals(null));
            Assert.False(o1.Equals(null, EqualityComparer<int>.Default));
            Assert.False(o2.Equals(null, EqualityComparer<Val>.Default));
            Assert.False(o3.Equals(null, EqualityComparer<Val>.Default));
            Assert.False(o4.Equals(null, EqualityComparer<Obj>.Default));
        }

        [Fact]
        public static void Equals_FollowsStructuralEqualityRules()
        {
            var o1a = Maybe.Of(1);
            var o1b = Maybe.Of(1);
            var o2a = Maybe.Of(new Val(1));
            var o2b = Maybe.Of(new Val(1));
            var o3a = Maybe.Of(Tuple.Create("1"));
            var o3b = Maybe.Of(Tuple.Create("1"));

            Assert.True(o1a.Equals(o1b));
            Assert.True(o2a.Equals(o2b));
            Assert.True(o3a.Equals(o3b));
            Assert.True(o1a.Equals(o1b, EqualityComparer<int>.Default));
            Assert.True(o2a.Equals(o2b, EqualityComparer<Val>.Default));
            Assert.True(o3a.Equals(o3b, EqualityComparer<Tuple<string>>.Default));
        }

        [Fact]
        public static void Equals_FollowsStructuralEqualityRules_AfterBoxing()
        {
            var o1a = Maybe.Of(1);
            var o1b = Maybe.Of(1);
            var o2a = Maybe.Of(new Val(1));
            var o2b = Maybe.Of(new Val(1));
            var o3a = Maybe.Of(Tuple.Create("1"));
            var o3b = Maybe.Of(Tuple.Create("1"));

            Assert.True(o1a.Equals((object)o1b));
            Assert.True(o2a.Equals((object)o2b));
            Assert.True(o3a.Equals((object)o3b));
            Assert.True(o1a.Equals((object)o1b, EqualityComparer<int>.Default));
            Assert.True(o2a.Equals((object)o2b, EqualityComparer<Val>.Default));
            Assert.True(o3a.Equals((object)o3b, EqualityComparer<Tuple<string>>.Default));
        }

        #endregion

        #region GetHashCode()

        [Fact]
        public static void GetHashCode_Guards()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("comparer", () => some.GetHashCode(null));
            Assert.Throws<ArgumentNullException>("comparer", () => none.GetHashCode(null));
        }

        [Fact]
        public static void GetHashCode_ReturnsSameResult_WhenCalledRepeatedly()
        {
            var none = Maybe<Obj>.None;
            var someobj = Maybe.Of(new Obj());
            var someval = Maybe.Of(1);

            Assert.Equal(none.GetHashCode(), none.GetHashCode());
            Assert.Equal(someobj.GetHashCode(), someobj.GetHashCode());
            Assert.Equal(someval.GetHashCode(), someval.GetHashCode());
        }

        [Fact]
        public static void GetHashCode_ReturnsSameResult_ForEqualInstances()
        {
            var tuple1 = Tuple.Create("1");
            var tuple2 = Tuple.Create("1");

            Assert.NotSame(tuple1, tuple2);
            Assert.Equal(tuple1.GetHashCode(), tuple2.GetHashCode());
        }

        #endregion

        #region ToString()

        [Fact]
        public static void ToString_ContainsUnderlyingValue()
        {
            var exp = "My Value";
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(exp);

            Assert.Contains("None", none.ToString());
            Assert.Contains(exp, some.ToString());
        }

        #endregion

        #region Select()

        [Fact]
        public static void Select_ReturnsSome_IfSome()
        {
            // Arrange
            var some = Maybe.Of(1);
            Func<int, int> selector = val => 2 * val;

            // Act
            var m = some.Select(selector);
            var q = from item in some select selector(item);

            // Assert
            Assert.True(m.IsSome);
            Assert.True(q.IsSome);
        }

        [Fact]
        public static void Select_ReturnsNone_IfNone()
        {
            // Arrange
            var none = Maybe<int>.None;
            Func<int, int> selector = val => 2 * val;

            // Act
            var m = none.Select(selector);
            var q = from item in none select selector(item);

            // Assert
            Assert.True(m.IsNone);
            Assert.True(q.IsNone);
        }

        #endregion
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class MaybeFacts
    {
        #region η()

        [Fact]
        public static void η_ReturnsNone_ForNull()
        {
            var o1 = Maybe<Obj>.η(null);
            var o2 = Maybe<Val?>.η(null);

            Assert.True(o1.IsNone);
            Assert.True(o2.IsNone);
        }

        [Fact]
        public static void η_ReturnsSome_ForNonNull()
        {
            var o1 = Maybe<int>.η(1);
            var o2 = Maybe<Val>.η(new Val(1));
            var o3 = Maybe<Val?>.η(new Val(1));
            var o4 = Maybe<Obj>.η(new Obj());

            Assert.True(o1.IsSome);
            Assert.True(o2.IsSome);
            Assert.True(o3.IsSome);
            Assert.True(o4.IsSome);
        }

        #endregion

        #region Value

        [ReleaseOnlyFact]
        public static void Value_IsNullOrDefault_IfNone()
        {
            Assert.Equal(Maybe<int>.None.Value, default(int));
            Assert.Null(Maybe<string>.None.Value);
            Assert.Null(Maybe<Obj>.None.Value);
        }

        #endregion
    }

#endif

    public static partial class MaybeFacts
    {
        #region Equals()

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

        ////    var value = new SimpleStruct(3141);
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

        ////    var value = new SimpleStruct(3141);
        ////    var valueOpt = Maybe.Of(value);

        ////    var reference = new List<int>();
        ////    var referenceOpt = Maybe.Of(reference);

        ////    // Act & Assert
        ////    Assert.True(simpleOpt.Equals((object)simple));
        ////    Assert.True(valueOpt.Equals((object)value));
        ////    Assert.True(referenceOpt.Equals((object)reference));
        ////}

        #endregion

        #region Bind()

        ///// <summary>
        ///// Maybe<T>.Bind(selector) returned null when selector returned null.
        ///// The correct behaviour is to return Maybe<T>.None.
        ///// </summary>
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
            var value = new ImmutableObj(1);
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
            var value = new Val(3141);
            Val? nullableValue = new Val(3141);
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
