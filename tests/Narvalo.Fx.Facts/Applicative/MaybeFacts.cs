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

        [Fact(DisplayName = "Maybe.Unit is some.")]
        public static void Unit()
        {
            Assert.True(Maybe.Unit.IsSome);
            Assert.False(Maybe.Unit.IsNone, "IsSome != IsNone");
        }

        #endregion

        #region None

        [Fact(DisplayName = "Maybe.None is none.")]
        public static void None1()
        {
            Assert.True(Maybe.None.IsNone);
            Assert.False(Maybe.None.IsSome, "IsSome != IsNone");
        }

        [Fact(DisplayName = "None is none.")]
        public static void None2()
        {
            var o1 = Maybe<int>.None;               // Core value type
            var o2 = Maybe<Val>.None;               // User-defined value type
            var o3 = Maybe<Val?>.None;              // Nullable value type
            var o4 = Maybe<Obj>.None;               // Reference type

            Assert.True(o1.IsNone);
            Assert.True(o2.IsNone);
            Assert.True(o3.IsNone);
            Assert.True(o4.IsNone);
            // IsSome == !IsNone
            Assert.False(o1.IsSome);
            Assert.False(o2.IsSome);
            Assert.False(o3.IsSome);
            Assert.False(o4.IsSome);
        }

        #endregion

        #region IsSome & IsNone

        [Fact(DisplayName = "IsSome, once true, stays true.")]
        public static void IsSome()
        {
            var obj = new Obj();
            var some = Maybe.Of(obj);

            Assert.True(some.IsSome);
            Assert.False(some.IsNone, "IsSome != IsNone");

            obj = null;

            Assert.True(some.IsSome);
            Assert.False(some.IsNone, "IsSome != IsNone");
        }

        [Fact(DisplayName = "IsNone, once true, stays true.")]
        public static void IsNone()
        {
            Obj obj = null;
            var none = Maybe.Of(obj);

            Assert.True(none.IsNone);
            Assert.False(none.IsSome, "IsSome != IsNone");

            obj = new Obj();

            Assert.True(none.IsNone);
            Assert.False(none.IsSome, "IsSome != IsNone");
        }

        #endregion

        #region Of()

        [Fact(DisplayName = "Maybe.Of() returns some for non-null.")]
        public static void Of1()
        {
            var o1 = Maybe.Of(1);
            var o2 = Maybe.Of(new Val(1));
            var o3 = Maybe.Of((Val?)new Val(1));
            var o4 = Maybe.Of(new Obj());

            Assert.True(o1.IsSome);
            Assert.True(o2.IsSome);
            Assert.True(o3.IsSome);
            Assert.True(o4.IsSome);
            // IsSome == !IsNone
            Assert.False(o1.IsNone);
            Assert.False(o2.IsNone);
            Assert.False(o3.IsNone);
            Assert.False(o4.IsNone);
        }

        [Fact(DisplayName = "Maybe.Of() returns none for null.")]
        public static void Of2()
        {
            var o1 = Maybe.Of((Obj)null);
            var o2 = Maybe.Of((Val?)null);

            Assert.True(o1.IsNone);
            Assert.True(o2.IsNone);
            // IsSome == !IsNone
            Assert.False(o1.IsSome);
            Assert.False(o2.IsSome);
        }

        #endregion

        #region Cast to/from value

        [Fact(DisplayName = "Casting to value throws if none.")]
        public static void CastToValue1()
        {
            var none = Maybe<Obj>.None;

            Action act = () => { var _ = (Obj)none; };
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<InvalidCastException>(ex);
        }

        [Fact(DisplayName = "Casting to value returns value if some.")]
        public static void CastToValue2()
        {
            var exp = new Obj();
            var some = Maybe.Of(exp);

            Assert.Same(exp, (Obj)some);
        }

        [Fact(DisplayName = "Casting from value returns none for null.")]
        public static void CastFromValue1()
        {
            var none = (Maybe<Obj>)null;

            Assert.True(none.IsNone);
        }

        [Fact(DisplayName = "Casting from value returns some for non-null.")]
        public static void CastFromValue2()
        {
            var exp = new Obj();
            var some = (Maybe<Obj>)exp;

            Assert.True(some.IsSome);
            Assert.Same(exp, some.Value);
        }

        #endregion

        #region ValueOrDefault()

        [Fact(DisplayName = "ValueOrDefault() returns value if some.")]
        public static void ValueOrDefault1()
        {
            var exp = new Obj();
            var some = Maybe.Of(exp);

            Assert.Same(exp, some.ValueOrDefault());
        }

        [Fact(DisplayName = "ValueOrDefault() returns default value if none.")]
        public static void ValueOrDefault2()
        {
            var none = Maybe<Obj>.None;
            var exp = default(Obj);

            Assert.Same(exp, none.ValueOrDefault());
        }

        #endregion

        #region ValueOrElse()

        [Fact(DisplayName = "ValueOrElse() guards.")]
        public static void ValueOrElse1()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("other", () => some.ValueOrElse((Obj)null));
            Assert.Throws<ArgumentNullException>("valueFactory", () => some.ValueOrElse((Func<Obj>)null));

            Assert.Throws<ArgumentNullException>("other", () => none.ValueOrElse((Obj)null));
            Assert.Throws<ArgumentNullException>("valueFactory", () => none.ValueOrElse((Func<Obj>)null));
        }

        [Fact(DisplayName = "ValueOrElse() returns value if some.")]
        public static void ValueOrElse2()
        {
            var exp = new Obj();
            var some = Maybe.Of(exp);
            var other = new Obj("other");

            Assert.Same(exp, some.ValueOrElse(other));
            Assert.Same(exp, some.ValueOrElse(() => other));
        }

        [Fact(DisplayName = "ValueOrElse(other) returns other if none.")]
        public static void ValueOrElse3()
        {
            var none = Maybe<Obj>.None;
            var exp = new Obj();

            Assert.Same(exp, none.ValueOrElse(exp));
            Assert.Same(exp, none.ValueOrElse(() => exp));
        }

        #endregion

        #region ValueOrThrow()

        [Fact(DisplayName = "ValueOrThrow(other) guards.")]
        public static void ValueOrThrow1()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("exceptionFactory", () => some.ValueOrThrow(null));
            Assert.Throws<ArgumentNullException>("exceptionFactory", () => none.ValueOrThrow(null));
        }

        [Fact(DisplayName = "ValueOrThrow() returns value if some.")]
        public static void ValueOrThrow2()
        {
            var exp = new Obj();
            var some = Maybe.Of(exp);

            Assert.Same(exp, some.ValueOrThrow());
            Assert.Same(exp, some.ValueOrThrow(() => new SimpleException()));
        }

        [Fact(DisplayName = "ValueOrThrow() throws if none.")]
        public static void ValueOrThrow3()
        {
            var none = Maybe<Obj>.None;

            Assert.Throws<InvalidOperationException>(() => none.ValueOrThrow());
            Assert.Throws<SimpleException>(() => none.ValueOrThrow(() => new SimpleException()));
        }

        #endregion

        #region op_Equality() & op_Inequality()

        [Fact(DisplayName = "== and != for the same underlying values.")]
        public static void Equality1()
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

        [Fact(DisplayName = "== and != for different underlying values.")]
        public static void Equality2()
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

        [Fact(DisplayName = "Equals() guards.")]
        public static void Equals1()
        {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("comparer", () => some.Equals(some, null));
            Assert.Throws<ArgumentNullException>("comparer", () => some.Equals(none, null));

            Assert.Throws<ArgumentNullException>("comparer", () => none.Equals(none, null));
            Assert.Throws<ArgumentNullException>("comparer", () => none.Equals(some, null));
        }

        [Fact(DisplayName = "Equals() is reflexive.")]
        public static void Equals2()
        {
            var n = Maybe<int>.None;
            var o1 = Maybe.Of(1);
            var o2 = Maybe.Of(new Val(1));
            var o3 = Maybe.Of((Val?)new Val(1));
            var o4 = Maybe.Of(new Obj());

            Assert.True(n.Equals(n));
            Assert.True(o1.Equals(o1));
            Assert.True(o2.Equals(o2));
            Assert.True(o3.Equals(o3));
            Assert.True(o4.Equals(o4));
            Assert.True(n.Equals(n, EqualityComparer<int>.Default));
            Assert.True(o1.Equals(o1, EqualityComparer<int>.Default));
            Assert.True(o2.Equals(o2, EqualityComparer<Val>.Default));
            Assert.True(o3.Equals(o3, EqualityComparer<Val>.Default));
            Assert.True(o4.Equals(o4, EqualityComparer<Obj>.Default));
        }

        [Fact(DisplayName = "Equals() is abelian.")]
        public static void Equals3()
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

        [Fact(DisplayName = "Equals() is transitive.")]
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

        [Fact(DisplayName = "Equals() returns false if none for null.")]
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

        [Fact(DisplayName = "Equals() returns false if some for null.")]
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

        [Fact(DisplayName = "Equals() follows structural equality rules.")]
        public static void Equals_FollowsStructuralEqualityRules()
        {
            var o1a = Maybe.Of(1);
            var o1b = Maybe.Of(1);
            var o1c = Maybe.Of(2);
            var o2a = Maybe.Of(new Val(1));
            var o2b = Maybe.Of(new Val(1));
            var o2c = Maybe.Of(new Val(2));
            var o3a = Maybe.Of(Tuple.Create("1"));
            var o3b = Maybe.Of(Tuple.Create("1"));
            var o3c = Maybe.Of(Tuple.Create("2"));

            Assert.True(o1a.Equals(o1b));
            Assert.True(o2a.Equals(o2b));
            Assert.True(o3a.Equals(o3b));
            Assert.True(o1a.Equals(o1b, EqualityComparer<int>.Default));
            Assert.True(o2a.Equals(o2b, EqualityComparer<Val>.Default));
            Assert.True(o3a.Equals(o3b, EqualityComparer<Tuple<string>>.Default));

            Assert.False(o1a.Equals(o1c));
            Assert.False(o2a.Equals(o2c));
            Assert.False(o3a.Equals(o3c));
            Assert.False(o1a.Equals(o1c, EqualityComparer<int>.Default));
            Assert.False(o2a.Equals(o2c, EqualityComparer<Val>.Default));
            Assert.False(o3a.Equals(o3c, EqualityComparer<Tuple<string>>.Default));
        }

        [Fact(DisplayName = "Equals() follows structural equality rules after boxing.")]
        public static void Equals_FollowsStructuralEqualityRules_AfterBoxing()
        {
            var o1a = Maybe.Of(1);
            var o1b = Maybe.Of(1);
            var o1c = Maybe.Of(2);
            var o2a = Maybe.Of(new Val(1));
            var o2b = Maybe.Of(new Val(1));
            var o2c = Maybe.Of(new Val(2));
            var o3a = Maybe.Of(Tuple.Create("1"));
            var o3b = Maybe.Of(Tuple.Create("1"));
            var o3c = Maybe.Of(Tuple.Create("2"));

            Assert.True(o1a.Equals((object)o1b));
            Assert.True(o2a.Equals((object)o2b));
            Assert.True(o3a.Equals((object)o3b));
            Assert.True(o1a.Equals((object)o1b, EqualityComparer<int>.Default));
            Assert.True(o2a.Equals((object)o2b, EqualityComparer<Val>.Default));
            Assert.True(o3a.Equals((object)o3b, EqualityComparer<Tuple<string>>.Default));

            Assert.False(o1a.Equals((object)o1c));
            Assert.False(o2a.Equals((object)o2c));
            Assert.False(o3a.Equals((object)o3c));
            Assert.False(o1a.Equals((object)o1c, EqualityComparer<int>.Default));
            Assert.False(o2a.Equals((object)o2c, EqualityComparer<Val>.Default));
            Assert.False(o3a.Equals((object)o3c, EqualityComparer<Tuple<string>>.Default));
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
            Assert.Equal(none.GetHashCode(EqualityComparer<Obj>.Default), none.GetHashCode(EqualityComparer<Obj>.Default));
            Assert.Equal(someobj.GetHashCode(EqualityComparer<Obj>.Default), someobj.GetHashCode(EqualityComparer<Obj>.Default));
            Assert.Equal(someval.GetHashCode(EqualityComparer<int>.Default), someval.GetHashCode(EqualityComparer<int>.Default));
        }

        [Fact]
        public static void GetHashCode_ReturnsSameResult_ForEqualInstances()
        {
            var tuple1 = Maybe.Of(Tuple.Create("1"));
            var tuple2 = Maybe.Of(Tuple.Create("1"));

            Assert.NotSame(tuple1, tuple2);
            Assert.Equal(tuple1.GetHashCode(), tuple2.GetHashCode());
            Assert.Equal(tuple1.GetHashCode(EqualityComparer<Tuple<string>>.Default), tuple2.GetHashCode(EqualityComparer<Tuple<string>>.Default));
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
    }

    // Tests for the Internal.IMaybe<T> interface.
    public static partial class MaybeFacts
    {
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

        #region GetEnumerator()

        [Fact]
        public static void GetEnumerator_IfNone()
        {
            var none = Maybe<Obj>.None;
            var count = 0;

            foreach (var x in none) { count++; }

            Assert.Equal(0, count);
        }

        [Fact]
        public static void GetEnumerator_IfSome()
        {
            var exp = new Obj();
            var some = Maybe.Of(exp);
            var count = 0;

            foreach (var x in some) { count++; Assert.Same(exp, x); }

            Assert.Equal(1, count);
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

        [Fact]
        public static void Match_InvokesCaseSome_IfSome1()
        {
            var some = Maybe.Of(new Obj());
            var caseSomeWasCalled = false;
            var caseNoneWasCalled = false;
            var exp = new Obj("caseSome");
            Func<Obj, Obj> caseSome = _ => { caseSomeWasCalled = true; return exp; };
            Func<Obj> caseNone = () => { caseNoneWasCalled = true; return new Obj("caseNone"); };

            var rs = some.Match(caseSome, caseNone);

            Assert.True(caseSomeWasCalled);
            Assert.Same(exp, rs);
            Assert.False(caseNoneWasCalled);
        }

        [Fact]
        public static void Match_InvokesCaseSome_IfSome2()
        {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var exp = new Obj("caseSome");
            Func<Obj, Obj> caseSome = _ => { wasCalled = true; return exp; };
            var caseNone = new Obj("caseNone");

            var rs = some.Match(caseSome, caseNone);

            Assert.True(wasCalled);
            Assert.Same(exp, rs);
        }

        [Fact]
        public static void Match_InvokesCaseNone_IfNone1()
        {
            var none = Maybe<Obj>.None;
            var caseSomeWasCalled = false;
            var caseNoneWasCalled = false;
            var exp = new Obj("caseNone");
            Func<Obj, Obj> caseSome = _ => { caseSomeWasCalled = true; return new Obj("caseSome"); };
            Func<Obj> caseNone = () => { caseNoneWasCalled = true; return exp; };

            var rs = none.Match(caseSome, caseNone);

            Assert.True(caseNoneWasCalled);
            Assert.Same(exp, rs);
            Assert.False(caseSomeWasCalled);
        }

        [Fact]
        public static void Match_InvokesCaseNone_IfNone2()
        {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            Func<Obj, Obj> caseSome = _ => { wasCalled = true; return new Obj("caseSome"); };
            var caseNone = new Obj("caseNone");

            var rs = none.Match(caseSome, caseNone);

            Assert.False(wasCalled);
            Assert.Same(caseNone, rs);
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

        [Fact]
        public static void Coalesce_InvokesSelector_IfSome_ForTrue()
        {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var otherWasCalled = false;
            var exp = new Obj("selector");
            Func<Obj, Obj> selector = _ => { wasCalled = true; return exp; };
            Func<Obj> otherwise = () => { otherWasCalled = true; return new Obj("otherwise"); };

            var rs = some.Coalesce(_ => true, selector, otherwise);

            Assert.True(wasCalled);
            Assert.Same(exp, rs);
            Assert.False(otherWasCalled);
        }

        [Fact]
        public static void Coalesce_ReturnsThenResult_IfSome_ForTrue()
        {
            var some = Maybe.Of(new Obj());
            var thenResult = new Obj("then");
            var elseResult = new Obj("else");

            var rs = some.Coalesce(_ => true, thenResult, elseResult);

            Assert.Same(thenResult, rs);
        }

        [Fact]
        public static void Coalesce_InvokesOtherwise_IfSome_ForFalse()
        {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var otherWasCalled = false;
            var exp = new Obj("otherwise");
            Func<Obj, Obj> selector = _ => { wasCalled = true; return new Obj("selector"); };
            Func<Obj> otherwise = () => { otherWasCalled = true; return exp; };

            var rs = some.Coalesce(_ => false, selector, otherwise);

            Assert.True(otherWasCalled);
            Assert.Same(exp, rs);
            Assert.False(wasCalled);
        }

        [Fact]
        public static void Coalesce_ReturnsElseResult_IfSome_ForFalse()
        {
            var some = Maybe.Of(new Obj());
            var thenResult = new Obj("then");
            var elseResult = new Obj("else");

            var rs = some.Coalesce(_ => false, thenResult, elseResult);

            Assert.Same(elseResult, rs);
        }

        [Fact]
        public static void Coalesce_InvokesOtherwise_IfNone_ForTrue()
        {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            var otherWasCalled = false;
            var exp = new Obj("otherwise");
            Func<Obj, Obj> selector = _ => { wasCalled = true; return new Obj("selector"); };
            Func<Obj> otherwise = () => { otherWasCalled = true; return exp; };

            var rs = none.Coalesce(_ => true, selector, otherwise);

            Assert.True(otherWasCalled);
            Assert.Same(exp, rs);
            Assert.False(wasCalled);
        }

        [Fact]
        public static void Coalesce_InvokesOtherwise_IfNone_ForFalse()
        {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            var otherWasCalled = false;
            var exp = new Obj("otherwise");
            Func<Obj, Obj> selector = _ => { wasCalled = true; return new Obj("selector"); };
            Func<Obj> otherwise = () => { otherWasCalled = true; return exp; };

            var rs = none.Coalesce(_ => false, selector, otherwise);

            Assert.True(otherWasCalled);
            Assert.Same(exp, rs);
            Assert.False(wasCalled);
        }

        [Fact]
        public static void Coalesce_ReturnsElseResult_IfNone_ForTrue()
        {
            var none = Maybe<Obj>.None;
            var thenResult = new Obj("then");
            var elseResult = new Obj("else");

            var rs = none.Coalesce(_ => true, thenResult, elseResult);

            Assert.Same(elseResult, rs);
        }

        [Fact]
        public static void Coalesce_ReturnsElseResult_IfNone_ForFalse()
        {
            var none = Maybe<Obj>.None;
            var thenResult = new Obj("then");
            var elseResult = new Obj("else");

            var rs = none.Coalesce(_ => false, thenResult, elseResult);

            Assert.Same(elseResult, rs);
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

        [Fact]
        public static void When_InvokesAction_IfSome_ForTrue1()
        {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var otherWasCalled = false;
            Action<Obj> action = _ => wasCalled = true;
            Action otherwise = () => otherWasCalled = true;

            some.When(_ => true, action, otherwise);

            Assert.True(wasCalled);
            Assert.False(otherWasCalled);
        }

        [Fact]
        public static void When_InvokesAction_IfSome_ForTrue2()
        {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            Action<Obj> action = _ => wasCalled = true;

            some.When(_ => true, action);

            Assert.True(wasCalled);
        }

        [Fact]
        public static void When_InvokesOtherwise_IfSome_ForFalse()
        {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var otherWasCalled = false;
            Action<Obj> action = _ => wasCalled = true;
            Action otherwise = () => otherWasCalled = true;

            some.When(_ => false, action, otherwise);

            Assert.False(wasCalled);
            Assert.True(otherWasCalled);
        }

        [Fact]
        public static void When_DoesNotInvokesAction_IfSome_ForFalse()
        {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            Action<Obj> action = _ => wasCalled = true;

            some.When(_ => false, action);

            Assert.False(wasCalled);
        }

        [Fact]
        public static void When_InvokesOtherwise_IfNone_ForTrue()
        {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            var otherWasCalled = false;
            Action<Obj> action = _ => wasCalled = true;
            Action otherwise = () => otherWasCalled = true;

            none.When(_ => true, action, otherwise);

            Assert.False(wasCalled);
            Assert.True(otherWasCalled);
        }

        [Fact]
        public static void When_DoesNotInvokeAction_IfNone_ForTrue()
        {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            Action<Obj> action = _ => wasCalled = true;

            none.When(_ => true, action);

            Assert.False(wasCalled);
        }

        [Fact]
        public static void When_InvokesOtherwise_IfNone_ForFalse()
        {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            var otherWasCalled = false;
            Action<Obj> action = _ => wasCalled = true;
            Action otherwise = () => otherWasCalled = true;

            none.When(_ => false, action, otherwise);

            Assert.False(wasCalled);
            Assert.True(otherWasCalled);
        }

        [Fact]
        public static void When_DoesNotInvokeAction_IfNone_ForFalse()
        {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            Action<Obj> action = _ => wasCalled = true;

            none.When(_ => false, action);

            Assert.False(wasCalled);
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
            var some = Maybe.Of(new Obj());
            var onSomeWasCalled = false;
            var onNoneWasCalled = false;
            Action<Obj> onSome = _ => onSomeWasCalled = true;
            Action onNone = () => onNoneWasCalled = true;

            some.Do(onSome, onNone);

            Assert.True(onSomeWasCalled);
            Assert.False(onNoneWasCalled);
        }

        [Fact]
        public static void Do_InvokesOnNone_IfNone()
        {
            var none = Maybe<Obj>.None;
            var onSomeWasCalled = false;
            var onNoneWasCalled = false;
            Action<Obj> onSome = _ => onSomeWasCalled = true;
            Action onNone = () => onNoneWasCalled = true;

            none.Do(onSome, onNone);

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
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            Action<Obj> op = _ => wasCalled = true;

            some.OnSome(op);

            Assert.True(wasCalled);
        }

        [Fact]
        public static void OnSome_DoesNotInvokeAction_IfNone()
        {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            Action<Obj> op = _ => wasCalled = true;

            none.OnSome(op);

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
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            Action act = () => wasCalled = true;

            none.OnNone(act);

            Assert.True(wasCalled);
        }

        [Fact]
        public static void OnNone_DoesNotInvokeAction_IfSome()
        {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            Action act = () => wasCalled = true;

            some.OnNone(act);

            Assert.False(wasCalled);
        }

        #endregion
    }

    // Tests for the monadic methods.
    public static partial class MaybeFacts
    {
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
            var none = Maybe<Obj>.None;
            Func<Obj, Maybe<string>> binder = val => Maybe.Of(val.Value);

            var me = none.Bind(binder);

            Assert.True(me.IsNone);
        }

        [Fact]
        public static void Bind_ReturnsSome_IfSome()
        {
            var exp = new Obj();
            var some = Maybe.Of(exp);
            Func<Obj, Maybe<string>> binder = val => Maybe.Of(val.Value);

            var me = some.Bind(binder);

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

        #region Select()

        [Fact]
        public static void Select_ReturnsSome_IfSome()
        {
            var some = Maybe.Of(1);
            Func<int, int> selector = val => 2 * val;

            var m1 = some.Select(selector);
            var m2 = Maybe.Select(some, selector);
            var q = from item in some select selector(item);

            Assert.True(m1.IsSome);
            Assert.True(m2.IsSome);
            Assert.True(q.IsSome);
        }

        [Fact]
        public static void Select_ReturnsNone_IfNone()
        {
            var none = Maybe<int>.None;
            Func<int, int> selector = val => 2 * val;

            var m1 = none.Select(selector);
            var m2 = Maybe.Select(none, selector);
            var q = from item in none select selector(item);

            Assert.True(m1.IsNone);
            Assert.True(m2.IsNone);
            Assert.True(q.IsNone);
        }

        #endregion

        #region ReplaceBy()

        [Fact]
        public static void ReplaceBy_ReturnsSome_IfSome()
        {
            var exp = 2;

            var m1 = Maybe.Of(1).ReplaceBy(exp);
            var m2 = Maybe.ReplaceBy(Maybe.Of(1), exp);

            Assert.True(m1.IsSome);
            Assert.True(m2.IsSome);
        }

        [Fact]
        public static void ReplaceBy_ReturnsNone_IfNone()
        {
            var exp = 2;

            var m1 = Maybe<int>.None.ReplaceBy(exp);
            var m2 = Maybe.ReplaceBy(Maybe<int>.None, exp);

            Assert.True(m1.IsNone);
            Assert.True(m2.IsNone);
        }

        #endregion

        #region ContinueWith()

        [Fact]
        public static void ContinueWith_ReturnsOther_IfSome()
        {
            var exp = Maybe.Of(1);

            var m1 = Maybe.Of(new Obj()).ContinueWith(exp);
            var m2 = Maybe.ContinueWith(Maybe.Of(new Obj()), exp);

            Assert.Equal(exp, m1);
            Assert.Equal(exp, m2);
        }

        [Fact]
        public static void ContinueWith_ReturnsNone_IfNone()
        {
            var other = Maybe.Of(1);

            var m1 = Maybe<Obj>.None.ContinueWith(other);
            var m2 = Maybe.ContinueWith(Maybe<Obj>.None, other);

            Assert.True(m1.IsNone);
            Assert.True(m2.IsNone);
        }

        #endregion

        #region PassBy()

        [Fact]
        public static void PassBy_ReturnsObj_ForSome()
        {
            var some = Maybe.Of(new Obj());
            var none = Maybe<Obj>.None;

            var other = Maybe.Of(1);

            var m1a = some.PassBy(other);
            var m1b = Maybe.PassBy(some, other);
            var m2a = none.PassBy(other);
            var m2b = Maybe.PassBy(none, other);

            Assert.Equal(some, m1a);
            Assert.Equal(some, m1b);
            Assert.Equal(none, m2a);
            Assert.Equal(none, m2b);
        }

        [Fact]
        public static void PassBy_ReturnsNone_ForNone()
        {
            var some = Maybe.Of(new Obj());
            var none = Maybe<Obj>.None;

            var m1a = some.PassBy(Maybe<int>.None);
            var m1b = Maybe.PassBy(some, Maybe<int>.None);
            var m2a = none.PassBy(Maybe<int>.None);
            var m2b = Maybe.PassBy(none, Maybe<int>.None);

            Assert.True(m1a.IsNone);
            Assert.True(m1b.IsNone);
            Assert.True(m2a.IsNone);
            Assert.True(m2b.IsNone);
        }

        #endregion

        #region Skip()

        [Fact]
        public static void Skip_ReturnsUnit_IfSome()
        {
            var m1 = Maybe.Of(new Obj()).Skip();
            var m2 = Maybe.Skip(Maybe.Of(new Obj()));

            Assert.Equal(Maybe.Unit, m1);
            Assert.Equal(Maybe.Unit, m2);
        }

        [Fact]
        public static void Skip_ReturnsNone_IfNone()
        {
            var m1 = Maybe<Obj>.None.Skip();
            var m2 = Maybe.Skip(Maybe<Obj>.None);

            Assert.Equal(Maybe.None, m1);
            Assert.Equal(Maybe.None, m2);
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

        [Fact]
        public static void Value_IsImmutable()
        {
            var exp = new Obj();
            var some = Maybe.Of(exp);

            exp = null;

            Assert.NotNull(some.Value);
            Assert.NotEqual(exp, some.Value);
        }

        [Fact]
        public static void Value_IsOriginalValue_IfSome()
        {
            var exp1 = 1;
            var exp2 = new Val(1);
            var exp3 = (Val?)new Val(1);
            var exp4 = new Obj();

            var o1 = Maybe.Of(exp1);
            var o2 = Maybe.Of(exp2);
            var o3 = Maybe.Of(exp3);
            var o4 = Maybe.Of(exp4);

            Assert.Equal(exp1, o1.Value);
            Assert.Equal(exp2, o2.Value);
            Assert.Equal(exp3, o3.Value);
            Assert.Equal(exp4, o4.Value);
            Assert.Same(exp4, o4.Value);
        }

        [ReleaseOnlyFact]
        public static void Value_IsNullOrDefault_IfNone()
        {
            Assert.Equal(Maybe<int>.None.Value, default(int));
            Assert.Null(Maybe<string>.None.Value);
            Assert.Null(Maybe<Obj>.None.Value);
        }

        #endregion

        #region ReplaceBy()

        [Fact]
        public static void ReplaceBy_ContainsValue_IfSome()
        {
            var exp = new Obj("other");

            var m = Maybe.Of(new Obj()).ReplaceBy(exp);

            Assert.Same(exp, m.Value);
        }

        #endregion
    }

#endif

    public static partial class MaybeFacts
    {
        #region Linq Operators

        [Fact]
        public static void Where_ReturnsNone_ForUnsuccessfulPredicate()
        {
            var some = Maybe.Of(1);
            Func<int, bool> predicate = _ => _ == 2;

            var m1 = some.Where(predicate);
            var m2 = Maybe.Where(some, predicate);
            var q = from _ in some where predicate(_) select _;

            Assert.False(m1.IsSome);
            Assert.False(m2.IsSome);
            Assert.False(q.IsSome);
        }

        [Fact]
        public static void SelectMany_ReturnsNone_IfNone()
        {
            var none = Maybe<int>.None;
            var some = Maybe.Of(2);
            Func<int, Maybe<int>> valueSelector = _ => some;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            var m1 = none.SelectMany(valueSelector, resultSelector);
            var m2 = Maybe.SelectMany(none, valueSelector, resultSelector);
            var q = from i in none
                    from j in some
                    select resultSelector(i, j);

            Assert.False(m1.IsSome);
            Assert.False(m2.IsSome);
            Assert.False(q.IsSome);
        }

        [Fact]
        public static void SelectMany_ReturnsNone_ForMiddleIsNone()
        {
            var some = Maybe.Of(1);
            var none = Maybe<int>.None;
            Func<int, Maybe<int>> valueSelector = _ => none;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            var m1 = some.SelectMany(valueSelector, resultSelector);
            var m2 = Maybe.SelectMany(some, valueSelector, resultSelector);
            var q = from i in some
                    from j in none
                    select resultSelector(i, j);

            Assert.False(m1.IsSome);
            Assert.False(m2.IsSome);
            Assert.False(q.IsSome);
        }

        [Fact]
        public static void SelectMany_ReturnsNone_ForNoneObjectAndNoneMiddle()
        {
            var none1 = Maybe<int>.None;
            var none2 = Maybe<int>.None;
            Func<int, Maybe<int>> valueSelector = _ => none2;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            var m1 = none1.SelectMany(valueSelector, resultSelector);
            var m2 = Maybe.SelectMany(none1, valueSelector, resultSelector);
            var q = from i in none1
                    from j in none2
                    select resultSelector(i, j);

            Assert.False(m1.IsSome);
            Assert.False(m2.IsSome);
            Assert.False(q.IsSome);
        }

        [Fact]
        public static void Join_ReturnsNone_IfJoinFailed()
        {
            var some1 = Maybe.Of(1);
            var some2 = Maybe.Of(2);

            var m1 = some1.Join(some2, _ => _, _ => _, (i, j) => i + j);
            var m2 = Maybe.Join(some1, some2, _ => _, _ => _, (i, j) => i + j);
            var q = from i in some1
                    join j in some2 on i equals j
                    select i + j;

            Assert.False(m1.IsSome);
            Assert.False(m2.IsSome);
            Assert.False(q.IsSome);
        }

        #endregion
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class MaybeFacts
    {
        #region Linq Operators

        [Fact]
        public static void Where_ReturnsSome_ForSuccessfulPredicate()
        {
            var source = Maybe.Of(1);
            Func<int, bool> predicate = _ => _ == 1;

            var m1 = source.Where(predicate);
            var m2 = Maybe.Where(source, predicate);
            var q = from _ in source where predicate(_) select _;

            Assert.True(m1.IsSome);
            Assert.True(m2.IsSome);
            Assert.True(q.IsSome);
            Assert.Equal(1, m1.Value);
            Assert.Equal(1, m2.Value);
            Assert.Equal(1, q.Value);
        }

        [Fact]
        public static void SelectMany_ReturnsSomeAndApplySelector()
        {
            var source = Maybe.Of(1);
            var middle = Maybe.Of(2);
            Func<int, Maybe<int>> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            var m1 = source.SelectMany(valueSelector, resultSelector);
            var m2 = Maybe.SelectMany(source, valueSelector, resultSelector);
            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);

            Assert.True(m1.IsSome);
            Assert.True(m2.IsSome);
            Assert.True(q.IsSome);
            Assert.Equal(3, m1.Value);
            Assert.Equal(3, m2.Value);
            Assert.Equal(3, q.Value);
        }

        [Fact]
        public static void Join_ReturnsSome_IfJoinSucceed()
        {
            var source = Maybe.Of(1);
            var inner = Maybe.Of(2);

            var m1 = source.Join(inner, _ => 2 * _, _ => _, (i, j) => i + j);
            var m2 = Maybe.Join(source, inner, _ => 2 * _, _ => _, (i, j) => i + j);
            var q = from i in source
                    join j in inner on 2 * i equals j
                    select i + j;

            Assert.True(m1.IsSome);
            Assert.True(m2.IsSome);
            Assert.True(q.IsSome);
            Assert.Equal(3, m1.Value);
            Assert.Equal(3, m2.Value);
            Assert.Equal(3, q.Value);
        }

        #endregion
    }

#endif
}
