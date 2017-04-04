// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    using static global::My;

    public static partial class MaybeFacts {
        [t("Unit is some.")]
        public static void Unit1() {
            Assert.True(Maybe.Unit.IsSome);
            Assert.False(Maybe.Unit.IsNone);
        }

        [t("None (static) is none.")]
        public static void None1() {
            Assert.True(Maybe.None.IsNone);
            Assert.False(Maybe.None.IsSome);
        }

        [t("None is none.")]
        public static void None2() {
            var m1 = Maybe<int>.None;
            Assert.True(m1.IsNone);
            Assert.False(m1.IsSome);

            var m2 = Maybe<Val>.None;
            Assert.True(m2.IsNone);
            Assert.False(m2.IsSome);

            var m3 = Maybe<Val?>.None;
            Assert.True(m3.IsNone);
            Assert.False(m3.IsSome);

            var m4 = Maybe<Obj>.None;
            Assert.True(m4.IsNone);
            Assert.False(m4.IsSome);
        }

        [t("IsSome, once true, stays true.")]
        public static void IsSome1() {
            var obj = new Obj();
            var some = Maybe.Of(obj);

            Assert.True(some.IsSome);
            Assert.False(some.IsNone);

            obj = null;

            Assert.True(some.IsSome);
            Assert.False(some.IsNone);
        }

        [t("IsNone, once true, stays true.")]
        public static void IsNone1() {
            Obj obj = null;
            var none = Maybe.Of(obj);

            Assert.True(none.IsNone);
            Assert.False(none.IsSome);

            obj = new Obj();

            Assert.True(none.IsNone);
            Assert.False(none.IsSome);
        }

        [t("Of(non-null) returns some.")]
        public static void Of1() {
            var m1 = Maybe.Of(1);
            Assert.True(m1.IsSome);
            Assert.False(m1.IsNone);

            var m2 = Maybe.Of(new Val(1));
            Assert.True(m2.IsSome);
            Assert.False(m2.IsNone);

            var m3 = Maybe.Of((Val?)new Val(1));
            Assert.True(m3.IsSome);
            Assert.False(m3.IsNone);

            var m4 = Maybe.Of(new Obj());
            Assert.True(m4.IsSome);
            Assert.False(m4.IsNone);
        }

        [t("Of(null) returns none.")]
        public static void Of2() {
            var m1 = Maybe.Of((Obj)null);
            Assert.True(m1.IsNone);
            Assert.False(m1.IsSome);

            var m2 = Maybe.Of((Val?)null);
            Assert.True(m2.IsNone);
            Assert.False(m2.IsSome);
        }

        [t("Deconstruct() if some.")]
        public static void Deconstruct1() {
            var exp = new Obj();
            var some = Maybe.Of(exp);
            var (isSome, value) = some;
            Assert.True(isSome);
            Assert.Same(exp, value);
        }

        [t("Deconstruct() if none.")]
        public static void Deconstruct2() {
            var none = Maybe<Obj>.None;
            var (isSome, value) = none;
            Assert.False(isSome);
            Assert.Null(value);
        }

        [t("Deconstruct() w/ a nullable type.")]
        public static void Deconstruct3() {
            var none = Maybe<int?>.None;
            var (isSome, value) = none;
            Assert.True(none.IsNone);
            Assert.False(isSome);
            Assert.Null(value);
        }

        [t("Deconstruct() w/ a nullable type.")]
        public static void Deconstruct4() {
            Maybe<int?> none = Maybe.Of(0).Select<int?>(x => null);
            var (isSome, value) = none;
            Assert.True(none.IsNone);
            Assert.False(isSome);
            Assert.Null(value);
        }

        [t("Deconstruct() w/ a nullable type.")]
        public static void Deconstruct5() {
            int exp = 1;
            Maybe<int?> some = Maybe.Of(0).Select<int?>(x => exp);
            var (isSome, value) = some;
            Assert.True(some.IsSome);
            Assert.True(isSome);
            Assert.Equal(exp, value);
        }

        [t("ValueOrDefault() returns Value if some.")]
        public static void ValueOrDefault1() {
            var exp = new Obj();
            var some = Maybe.Of(exp);

            Assert.Same(exp, some.ValueOrDefault());
        }

        [t("ValueOrDefault() returns default(T) if none.")]
        public static void ValueOrDefault2() {
            var exp = default(Obj);
            var none = Maybe<Obj>.None;

            Assert.Same(exp, none.ValueOrDefault());
        }

        [t("ValueOrElse() guards.")]
        public static void ValueOrElse0() {
            var some = Maybe.Of(new Obj());
            Assert.Throws<ArgumentNullException>("other", () => some.ValueOrElse(default(Obj)));
            Assert.Throws<ArgumentNullException>("valueFactory", () => some.ValueOrElse(default(Func<Obj>)));

            var none = Maybe<Obj>.None;
            Assert.Throws<ArgumentNullException>("other", () => none.ValueOrElse(default(Obj)));
            Assert.Throws<ArgumentNullException>("valueFactory", () => none.ValueOrElse(default(Func<Obj>)));
        }

        [t("ValueOrElse() returns Value if some.")]
        public static void ValueOrElse1() {
            var exp = new Obj();
            var some = Maybe.Of(exp);
            var other = new Obj("other");

            Assert.Same(exp, some.ValueOrElse(other));
            Assert.Same(exp, some.ValueOrElse(() => other));
        }

        [t("ValueOrElse(other) returns 'other' if none.")]
        public static void ValueOrElse2() {
            var none = Maybe<Obj>.None;
            var exp = new Obj();

            Assert.Same(exp, none.ValueOrElse(exp));
            Assert.Same(exp, none.ValueOrElse(() => exp));
        }

        [t("ValueOrThrow() guards.")]
        public static void ValueOrThrow0() {
            var some = Maybe.Of(new Obj());
            Assert.Throws<ArgumentNullException>("exceptionFactory", () => some.ValueOrThrow(null));

            var none = Maybe<Obj>.None;
            Assert.Throws<ArgumentNullException>("exceptionFactory", () => none.ValueOrThrow(null));
        }

        [t("ValueOrThrow() returns Value if some.")]
        public static void ValueOrThrow1() {
            var exp = new Obj();
            var some = Maybe.Of(exp);

            Assert.Same(exp, some.ValueOrThrow());
            Assert.Same(exp, some.ValueOrThrow(() => new SimpleException()));
        }

        [t("ValueOrThrow() throws if none.")]
        public static void ValueOrThrow2() {
            var none = Maybe<Obj>.None;

            Assert.Throws<InvalidOperationException>(() => none.ValueOrThrow());
            Assert.Throws<SimpleException>(() => none.ValueOrThrow(() => new SimpleException()));
        }

        [t("Casting (to T) throws if none.")]
        public static void cast1() {
            Assert.Throws<InvalidCastException>(() => (Obj)Maybe<Obj>.None);
        }

        [t("Casting (to T) returns Value if some.")]
        public static void cast2() {
            var exp = new Obj();
            var some = Maybe.Of(exp);

            Assert.Same(exp, (Obj)some);
        }

        [t("Casting (from) null returns none.")]
        public static void cast3() {
            var none = (Maybe<Obj>)null;

            Assert.True(none.IsNone);
        }

        [t("Casting (from) non-null returns some.")]
        public static void cast4() {
            var exp = new Obj();
            var some = (Maybe<Obj>)exp;

            Assert.True(some.IsSome);
        }

        [t("ToNullable() returns null if none and some if some (1).")]
        public static void ToNullable1() {
            int? exp = 1;
            var some = Maybe.Of(1);
            Assert.Equal(exp, some.ToNullable());

            var none = Maybe<int>.None;
            Assert.Null(none.ToNullable());
        }

        [t("ToNullable() returns null if none and some if some (2).")]
        public static void ToNullable2() {
            int? exp = 1;
            Maybe<int?> some = Maybe.Of(1).Select<int?>(i => 1);
            Assert.Equal(exp, some.ToNullable());

            Maybe<int?> none1 = Maybe.Of(1).Select<int?>(i => null);
            Assert.Null(none1.ToNullable());

            var none2 = Maybe<int?>.None;
            Assert.Null(none2.ToNullable());
        }

        [t("== and != when the Value's are equal.")]
        public static void Equality1() {
            var m1 = Maybe.Of(1);
            var m2 = Maybe.Of(1);
            Assert.True(m1 == m2);
            Assert.False(m1 != m2);

            var m3 = Maybe.Of(new Val(1));
            var m4 = Maybe.Of(new Val(1));
            Assert.True(m3 == m4);
            Assert.False(m3 != m4);

            var m5 = Maybe.Of((Val?)new Val(1));
            var m6 = Maybe.Of((Val?)new Val(1));
            Assert.True(m5 == m6);
            Assert.False(m5 != m6);

            var m7 = Maybe.Of(Tuple.Create("1"));
            var m8 = Maybe.Of(Tuple.Create("1"));
            Assert.True(m7 == m8);
            Assert.False(m7 != m8);
        }

        [t("== and != when the Value's are not equal.")]
        public static void Equality2() {
            var m1 = Maybe.Of(1);
            var m2 = Maybe.Of(2);
            Assert.False(m1 == m2);
            Assert.True(m1 != m2);

            var m3 = Maybe.Of(new Val(1));
            var m4 = Maybe.Of(new Val(2));
            Assert.False(m3 == m4);
            Assert.True(m3 != m4);

            var m5 = Maybe.Of((Val?)new Val(1));
            var m6 = Maybe.Of((Val?)new Val(2));
            Assert.False(m5 == m6);
            Assert.True(m5 != m6);

            var m7 = Maybe.Of(Tuple.Create("1"));
            var m8 = Maybe.Of(Tuple.Create("2"));
            Assert.False(m7 == m8);
            Assert.True(m7 != m8);

            var m9 = Maybe.Of(new Obj());
            var m10 = Maybe.Of(new Obj());
            Assert.False(m9 == m10);
            Assert.True(m9 != m10);
        }

        [t("!= when one of the sides is none.")]
        public static void Equality3() {
            var none = Maybe<int>.None;
            var some = Maybe.Of(1);

            Assert.True(none != some);
        }

        [t("Equals() guards.")]
        public static void Equals0() {
            var some = Maybe.Of(new Obj());
            var none = Maybe<Obj>.None;

            Assert.Throws<ArgumentNullException>("comparer", () => some.Equals(some, null));
            Assert.Throws<ArgumentNullException>("comparer", () => some.Equals(none, null));

            Assert.Throws<ArgumentNullException>("comparer", () => none.Equals(none, null));
            Assert.Throws<ArgumentNullException>("comparer", () => none.Equals(some, null));
        }

        [t("Equals() is reflexive.")]
        public static void Equals1() {
            var m1 = Maybe<int>.None;
            Assert.True(m1.Equals(m1));
            Assert.True(m1.Equals(m1, EqualityComparer<int>.Default));

            var m2 = Maybe.Of(1);
            Assert.True(m2.Equals(m2));
            Assert.True(m2.Equals(m2, EqualityComparer<int>.Default));

            var m3 = Maybe.Of(new Val(1));
            Assert.True(m3.Equals(m3));
            Assert.True(m3.Equals(m3, EqualityComparer<Val>.Default));

            var m4 = Maybe.Of((Val?)new Val(1));
            Assert.True(m4.Equals(m4));
            Assert.True(m4.Equals(m4, EqualityComparer<Val>.Default));

            var m5 = Maybe.Of(new Obj());
            Assert.True(m5.Equals(m5));
            Assert.True(m5.Equals(m5, EqualityComparer<Obj>.Default));
        }

        [t("Equals() is abelian.")]
        public static void Equals2() {
            var m1 = Maybe.Of(1);
            var m2 = Maybe.Of(1);
            var m3 = Maybe.Of(2);
            Assert.Equal(m1.Equals(m2), m2.Equals(m1));
            Assert.Equal(m1.Equals(m2, EqualityComparer<int>.Default), m2.Equals(m1, EqualityComparer<int>.Default));
            Assert.Equal(m1.Equals(m3), m3.Equals(m1));
            Assert.Equal(m1.Equals(m3, EqualityComparer<int>.Default), m3.Equals(m1, EqualityComparer<int>.Default));

            var m4 = Maybe.Of(new Val(1));
            var m5 = Maybe.Of(new Val(1));
            var m6 = Maybe.Of(new Val(2));
            Assert.Equal(m4.Equals(m5), m5.Equals(m4));
            Assert.Equal(m4.Equals(m5, EqualityComparer<Val>.Default), m5.Equals(m4, EqualityComparer<Val>.Default));
            Assert.Equal(m4.Equals(m6), m6.Equals(m4));
            Assert.Equal(m4.Equals(m6, EqualityComparer<Val>.Default), m6.Equals(m4, EqualityComparer<Val>.Default));

            var m7 = Maybe.Of((Val?)new Val(1));
            var m8 = Maybe.Of((Val?)new Val(1));
            var m9 = Maybe.Of((Val?)new Val(2));
            Assert.Equal(m7.Equals(m8), m5.Equals(m7));
            Assert.Equal(m7.Equals(m8, EqualityComparer<Val>.Default), m5.Equals(m7, EqualityComparer<Val>.Default));
            Assert.Equal(m7.Equals(m9), m6.Equals(m7));
            Assert.Equal(m7.Equals(m9, EqualityComparer<Val>.Default), m6.Equals(m7, EqualityComparer<Val>.Default));

            var m10 = Maybe.Of(new Obj());
            var m11 = Maybe.Of(new Obj());
            var m12 = Maybe.Of(new Obj("other"));
            Assert.Equal(m10.Equals(m11), m11.Equals(m10));
            Assert.Equal(m10.Equals(m11, EqualityComparer<Obj>.Default), m11.Equals(m10, EqualityComparer<Obj>.Default));
            Assert.Equal(m10.Equals(m12), m12.Equals(m10));
            Assert.Equal(m10.Equals(m12, EqualityComparer<Obj>.Default), m12.Equals(m10, EqualityComparer<Obj>.Default));
        }

        [t("Equals() is transitive.")]
        public static void Equals3() {
            var m1 = Maybe.Of(1);
            var m2 = Maybe.Of(1);
            var m3 = Maybe.Of(1);
            Assert.Equal(m1.Equals(m2) && m2.Equals(m3), m1.Equals(m3));
            Assert.Equal(m1.Equals(m2, EqualityComparer<int>.Default) && m2.Equals(m3, EqualityComparer<int>.Default), m1.Equals(m3, EqualityComparer<int>.Default));

            var m4 = Maybe.Of(new Val(1));
            var m5 = Maybe.Of(new Val(1));
            var m6 = Maybe.Of(new Val(1));
            Assert.Equal(m4.Equals(m5) && m5.Equals(m6), m4.Equals(m6));
            Assert.Equal(m4.Equals(m5, EqualityComparer<Val>.Default) && m5.Equals(m6, EqualityComparer<Val>.Default), m4.Equals(m6, EqualityComparer<Val>.Default));

            var m7 = Maybe.Of((Val?)new Val(1));
            var m8 = Maybe.Of((Val?)new Val(1));
            var m9 = Maybe.Of((Val?)new Val(1));
            Assert.Equal(m7.Equals(m8) && m8.Equals(m9), m4.Equals(m9));
            Assert.Equal(m7.Equals(m8, EqualityComparer<Val>.Default) && m8.Equals(m9, EqualityComparer<Val>.Default), m4.Equals(m9, EqualityComparer<Val>.Default));

            var m10 = Maybe.Of(new Obj());
            var m11 = Maybe.Of(new Obj());
            var m12 = Maybe.Of(new Obj());
            Assert.Equal(m10.Equals(m11) && m11.Equals(m12), m10.Equals(m12));
            Assert.Equal(m10.Equals(m11, EqualityComparer<Obj>.Default) && m11.Equals(m12, EqualityComparer<Obj>.Default), m10.Equals(m12, EqualityComparer<Obj>.Default));
        }

        [t("Equals(null) returns false if none.")]
        public static void Equals4() {
            var m1 = Maybe<int>.None;
            Assert.False(m1.Equals(null));
            Assert.False(m1.Equals(null, EqualityComparer<int>.Default));

            var m2 = Maybe<Val>.None;
            Assert.False(m2.Equals(null));
            Assert.False(m2.Equals(null, EqualityComparer<Val>.Default));

            var m3 = Maybe<Val?>.None;
            Assert.False(m3.Equals(null));
            Assert.False(m3.Equals(null, EqualityComparer<Val?>.Default));

            var m4 = Maybe<Obj>.None;
            Assert.False(m4.Equals(null));
            Assert.False(m4.Equals(null, EqualityComparer<Obj>.Default));
        }

        [t("Equals(null) returns false if some.")]
        public static void Equals5() {
            var m1 = Maybe.Of(1);
            Assert.False(m1.Equals(null));
            Assert.False(m1.Equals(null, EqualityComparer<int>.Default));

            var m2 = Maybe.Of(new Val(1));
            Assert.False(m2.Equals(null));
            Assert.False(m2.Equals(null, EqualityComparer<Val>.Default));

            var m3 = Maybe.Of((Val?)new Val(1));
            Assert.False(m3.Equals(null));
            Assert.False(m3.Equals(null, EqualityComparer<Val>.Default));

            var m4 = Maybe.Of(new Obj());
            Assert.False(m4.Equals(null));
            Assert.False(m4.Equals(null, EqualityComparer<Obj>.Default));
        }

        [t("Equals(obj) returns false if obj is not Maybe.")]
        public static void Equals6() {
            var some = Maybe.Of(1);
            Assert.False(some.Equals(new Obj()));

            var none = Maybe.Of(new Val(1));
            Assert.False(none.Equals(new Obj()));
        }

        [t("Equals() follows structural equality rules.")]
        public static void Equals7() {
            var m1 = Maybe.Of(1);
            var m2 = Maybe.Of(1);
            var m3 = Maybe.Of(2);
            Assert.True(m1.Equals(m2));
            Assert.True(m1.Equals(m2, EqualityComparer<int>.Default));
            Assert.False(m1.Equals(m3));
            Assert.False(m1.Equals(m3, EqualityComparer<int>.Default));

            var m4 = Maybe.Of(new Val(1));
            var m5 = Maybe.Of(new Val(1));
            var m6 = Maybe.Of(new Val(2));
            Assert.True(m4.Equals(m5));
            Assert.True(m4.Equals(m5, EqualityComparer<Val>.Default));
            Assert.False(m4.Equals(m6));
            Assert.False(m4.Equals(m6, EqualityComparer<Val>.Default));

            var m7 = Maybe.Of(Tuple.Create("1"));
            var m8 = Maybe.Of(Tuple.Create("1"));
            var m9 = Maybe.Of(Tuple.Create("2"));
            Assert.True(m7.Equals(m8));
            Assert.True(m7.Equals(m8, EqualityComparer<Tuple<string>>.Default));
            Assert.False(m7.Equals(m9));
            Assert.False(m7.Equals(m9, EqualityComparer<Tuple<string>>.Default));
        }

        [t("Equals() follows structural equality rules after boxing.")]
        public static void Equals8() {
            var m1 = Maybe.Of(1);
            var m2 = Maybe.Of(1);
            var m3 = Maybe.Of(2);
            Assert.True(m1.Equals((object)m2));
            Assert.True(m1.Equals((object)m2, EqualityComparer<int>.Default));
            Assert.False(m1.Equals((object)m3));
            Assert.False(m1.Equals((object)m3, EqualityComparer<int>.Default));

            var m4 = Maybe.Of(new Val(1));
            var m5 = Maybe.Of(new Val(1));
            var m6 = Maybe.Of(new Val(2));
            Assert.True(m4.Equals((object)m5));
            Assert.True(m4.Equals((object)m5, EqualityComparer<Val>.Default));
            Assert.False(m4.Equals((object)m6));
            Assert.False(m4.Equals((object)m6, EqualityComparer<Val>.Default));

            var m7 = Maybe.Of(Tuple.Create("1"));
            var m8 = Maybe.Of(Tuple.Create("1"));
            var m9 = Maybe.Of(Tuple.Create("2"));
            Assert.True(m7.Equals((object)m8));
            Assert.True(m7.Equals((object)m8, EqualityComparer<Tuple<string>>.Default));
            Assert.False(m7.Equals((object)m9));
            Assert.False(m7.Equals((object)m9, EqualityComparer<Tuple<string>>.Default));
        }

        [t("GetHashCode() guards.")]
        public static void GetHashCode0() {
            var some = Maybe.Of(new Obj());
            Assert.Throws<ArgumentNullException>("comparer", () => some.GetHashCode(null));

            var none = Maybe<Obj>.None;
            Assert.Throws<ArgumentNullException>("comparer", () => none.GetHashCode(null));
        }

        [t("GetHashCode() returns the same result when called repeatedly.")]
        public static void GetHashCode1() {
            var m1 = Maybe<Obj>.None;
            Assert.Equal(m1.GetHashCode(), m1.GetHashCode());
            Assert.Equal(m1.GetHashCode(EqualityComparer<Obj>.Default), m1.GetHashCode(EqualityComparer<Obj>.Default));

            var m2 = Maybe.Of(new Obj());
            Assert.Equal(m2.GetHashCode(), m2.GetHashCode());
            Assert.Equal(m2.GetHashCode(EqualityComparer<Obj>.Default), m2.GetHashCode(EqualityComparer<Obj>.Default));

            var m3 = Maybe.Of(1);
            Assert.Equal(m3.GetHashCode(), m3.GetHashCode());
            Assert.Equal(m3.GetHashCode(EqualityComparer<int>.Default), m3.GetHashCode(EqualityComparer<int>.Default));
        }

        [t("GetHashCode() returns the same result for equal instances.")]
        public static void GetHashCode2() {
            var m1 = Maybe.Of(Tuple.Create("1"));
            var m2 = Maybe.Of(Tuple.Create("1"));

            Assert.Equal(m1.GetHashCode(), m2.GetHashCode());
            Assert.Equal(m1.GetHashCode(EqualityComparer<Tuple<string>>.Default), m2.GetHashCode(EqualityComparer<Tuple<string>>.Default));
        }

        [t("GetHashCode() returns different results for non-equal instances.")]
        public static void GetHashCode3() {
            var m1 = Maybe.Of(1);
            var m2 = Maybe.Of(2);

            Assert.NotEqual(m1.GetHashCode(), m2.GetHashCode());
            Assert.NotEqual(m1.GetHashCode(EqualityComparer<int>.Default), m2.GetHashCode(EqualityComparer<int>.Default));

            var none = Maybe<int>.None;

            Assert.NotEqual(m1.GetHashCode(), none.GetHashCode());
            Assert.NotEqual(m1.GetHashCode(EqualityComparer<int>.Default), none.GetHashCode(EqualityComparer<int>.Default));
        }

        [t("ToString() result contains a string representation of the value if some, contains 'None' if none.")]
        public static void ToString1() {
            var none = Maybe<Obj>.None;
            Assert.Contains("None", none.ToString(), StringComparison.OrdinalIgnoreCase);

            var value = new Obj("My Value");
            var some = Maybe.Of(value);
            Assert.Contains(value.ToString(), some.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }

    // Tests for the Internal.IMaybe<T> interface.
    public static partial class MaybeFacts {
        [t("ToEnumerable() result is empty if none.")]
        public static void ToEnumerable1() {
            var none = Maybe<Obj>.None;
            var seq = none.ToEnumerable();

            Assert.Empty(seq);
        }

        [t("ToEnumerable() result is a sequence made of exactly one element if some.")]
        public static void ToEnumerable2() {
            var obj = new Obj();
            var some = Maybe.Of(obj);
            var seq = some.ToEnumerable();

            Assert.Equal(Enumerable.Repeat(obj, 1), seq);
        }

        [t("GetEnumerator() does not iterate if none.")]
        public static void GetEnumerator1() {
            var none = Maybe<Obj>.None;
            var count = 0;

            foreach (var x in none) { count++; }

            Assert.Equal(0, count);
        }

        [t("GetEnumerator() iterates only once if some.")]
        public static void GetEnumerator2() {
            var exp = new Obj();
            var some = Maybe.Of(exp);
            var count = 0;

            foreach (var x in some) { count++; Assert.Same(exp, x); }

            Assert.Equal(1, count);
        }

        [t("Contains() guards.")]
        public static void Contains0() {
            var value = new Obj();

            var some = Maybe.Of(new Obj());
            Assert.Throws<ArgumentNullException>("comparer", () => some.Contains(value, null));

            var none = Maybe<Obj>.None;
            Assert.Throws<ArgumentNullException>("comparer", () => none.Contains(value, null));
        }

        [t("Contains() returns false if none.")]
        public static void Contains1() {
            var m1 = Maybe<int>.None;
            Assert.False(m1.Contains(1));
            Assert.False(m1.Contains(1, EqualityComparer<int>.Default));

            var m2 = Maybe<Obj>.None;
            Assert.False(m2.Contains(new Obj()));
            Assert.False(m2.Contains(new Obj(), EqualityComparer<Obj>.Default));
        }

        [t("Contains(value) returns true if it does contain 'value'.")]
        public static void Contains2() {
            var value = new Obj();

            var m1 = Maybe.Of(1);
            Assert.True(m1.Contains(1), "Value type.");
            Assert.True(m1.Contains(1, EqualityComparer<int>.Default), "Value type.");

            var m2 = Maybe.Of(value);
            Assert.True(m2.Contains(value), "Reference type.");
            Assert.True(m2.Contains(value, EqualityComparer<Obj>.Default), "Reference type.");
        }

        [t("Contains() returns true if references are different but they are structurally equal.")]
        public static void Contains3() {
            var some = Maybe.Of(Tuple.Create("value"));

            Assert.True(some.Contains(Tuple.Create("value")), "References differ but we have structural equality.");
            Assert.True(some.Contains(Tuple.Create("value"), EqualityComparer<Tuple<string>>.Default), "References differ but we have structural equality.");
        }

        [t("Contains(value) returns false if it does not contain 'value'.")]
        public static void Contains4() {
            var other = new Obj("other");

            var m1 = Maybe.Of(1);
            Assert.False(m1.Contains(2));
            Assert.False(m1.Contains(2, EqualityComparer<int>.Default));

            var m2 = Maybe.Of(new Obj());
            Assert.False(m2.Contains(other, EqualityComparer<Obj>.Default));
            Assert.False(m2.Contains(new Obj(), EqualityComparer<Obj>.Default), "References differ.");
        }

        [t("Match() guards.")]
        public static void Match0() {
            var some = Maybe.Of(new Obj());
            Assert.Throws<ArgumentNullException>("caseSome", () => some.Match(null, new Obj()));
            Assert.Throws<ArgumentNullException>("caseSome", () => some.Match(null, () => new Obj()));
            Assert.Throws<ArgumentNullException>("caseNone", () => some.Match(x => x, default(Func<Obj>)));

            var none = Maybe<Obj>.None;
            Assert.Throws<ArgumentNullException>("caseSome", () => none.Match(null, new Obj()));
            Assert.Throws<ArgumentNullException>("caseSome", () => none.Match(null, () => new Obj()));
            Assert.Throws<ArgumentNullException>("caseNone", () => none.Match(x => x, default(Func<Obj>)));
        }

        [t("Match() calls 'caseSome' if some (1).")]
        public static void Match1() {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var notCalled = true;
            var exp = new Obj("caseSome");
            Func<Obj, Obj> caseSome = _ => { wasCalled = true; return exp; };
            Func<Obj> caseNone = () => { notCalled = false; return new Obj("caseNone"); };

            var result = some.Match(caseSome, caseNone);

            Assert.True(notCalled);
            Assert.True(wasCalled);
            Assert.Same(exp, result);
        }

        [t("Match() calls 'caseSome' if some (2).")]
        public static void Match2() {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var exp = new Obj("caseSome");
            Func<Obj, Obj> caseSome = _ => { wasCalled = true; return exp; };
            var caseNone = new Obj("caseNone");

            var result = some.Match(caseSome, caseNone);

            Assert.True(wasCalled);
            Assert.Same(exp, result);
        }

        [t("Match() calls 'caseNone' if none (1).")]
        public static void Match3() {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            var notCalled = true;
            var exp = new Obj("caseNone");
            Func<Obj, Obj> caseSome = _ => { notCalled = false; return new Obj("caseSome"); };
            Func<Obj> caseNone = () => { wasCalled = true; return exp; };

            var result = none.Match(caseSome, caseNone);

            Assert.True(notCalled);
            Assert.True(wasCalled);
            Assert.Same(exp, result);
        }

        [t("Match() calls 'caseNone' if none (2).")]
        public static void Match4() {
            var none = Maybe<Obj>.None;
            var notCalled = true;
            Func<Obj, Obj> caseSome = _ => { notCalled = false; return new Obj("caseSome"); };
            var caseNone = new Obj("caseNone");

            var result = none.Match(caseSome, caseNone);

            Assert.True(notCalled);
            Assert.Same(caseNone, result);
        }

        [t("Coalesce() guards.")]
        public static void Coalesce0() {
            var some = Maybe.Of(new Obj());
            Assert.Throws<ArgumentNullException>("predicate", () => some.Coalesce(null, new Obj("this"), new Obj("that")));
            Assert.Throws<ArgumentNullException>("predicate", () => some.Coalesce(null, x => x, () => new Obj()));
            Assert.Throws<ArgumentNullException>("selector", () => some.Coalesce(_ => true, null, () => new Obj()));
            Assert.Throws<ArgumentNullException>("otherwise", () => some.Coalesce(_ => true, x => x, null));

            var none = Maybe<Obj>.None;
            Assert.Throws<ArgumentNullException>("predicate", () => none.Coalesce(null, new Obj("this"), new Obj("that")));
            Assert.Throws<ArgumentNullException>("predicate", () => none.Coalesce(null, x => x, () => new Obj()));
            Assert.Throws<ArgumentNullException>("selector", () => none.Coalesce(_ => true, null, () => new Obj()));
            Assert.Throws<ArgumentNullException>("otherwise", () => none.Coalesce(_ => true, x => x, null));
        }

        [t("Coalesce() calls 'selector' if some and 'predicate' is true.")]
        public static void Coalesce1() {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var notCalled = true;
            var exp = new Obj("selector");
            Func<Obj, Obj> selector = _ => { wasCalled = true; return exp; };
            Func<Obj> otherwise = () => { notCalled = false; return new Obj("otherwise"); };

            var result = some.Coalesce(_ => true, selector, otherwise);

            Assert.True(notCalled);
            Assert.True(wasCalled);
            Assert.Same(exp, result);
        }

        [t("Coalesce() returns 'thenResult' if some and 'predicate' is true.")]
        public static void Coalesce2() {
            var some = Maybe.Of(new Obj());
            var thenResult = new Obj("then");
            var elseResult = new Obj("else");

            var result = some.Coalesce(_ => true, thenResult, elseResult);

            Assert.Same(thenResult, result);
        }

        [t("Coalesce() calls 'otherwise' if some and 'predicate' is false.")]
        public static void Coalesce3() {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var notCalled = true;
            var exp = new Obj("otherwise");
            Func<Obj, Obj> selector = _ => { notCalled = false; return new Obj("selector"); };
            Func<Obj> otherwise = () => { wasCalled = true; return exp; };

            var result = some.Coalesce(_ => false, selector, otherwise);

            Assert.True(notCalled);
            Assert.True(wasCalled);
            Assert.Same(exp, result);
        }

        [t("Coalesce() returns 'elseResult' if some and 'predicate' is false.")]
        public static void Coalesce4() {
            var some = Maybe.Of(new Obj());
            var thenResult = new Obj("then");
            var elseResult = new Obj("else");

            var result = some.Coalesce(_ => false, thenResult, elseResult);

            Assert.Same(elseResult, result);
        }

        [t("Coalesce() calls 'otherwise' if none and 'predicate' is true.")]
        public static void Coalesce5() {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            var notCalled = true;
            var exp = new Obj("otherwise");
            Func<Obj, Obj> selector = _ => { notCalled = false; return new Obj("selector"); };
            Func<Obj> otherwise = () => { wasCalled = true; return exp; };

            var result = none.Coalesce(_ => true, selector, otherwise);

            Assert.True(notCalled);
            Assert.True(wasCalled);
            Assert.Same(exp, result);
        }

        [t("Coalesce() calls 'otherwise' if none and 'predicate' is false.")]
        public static void Coalesce6() {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            var notCalled = true;
            var exp = new Obj("otherwise");
            Func<Obj, Obj> selector = _ => { notCalled = false; return new Obj("selector"); };
            Func<Obj> otherwise = () => { wasCalled = true; return exp; };

            var result = none.Coalesce(_ => false, selector, otherwise);

            Assert.True(notCalled);
            Assert.True(wasCalled);
            Assert.Same(exp, result);
        }

        [t("Coalesce() returns 'elseResult' if none and 'predicate' is true.")]
        public static void Coalesce7() {
            var none = Maybe<Obj>.None;
            var thenResult = new Obj("then");
            var elseResult = new Obj("else");

            var result = none.Coalesce(_ => true, thenResult, elseResult);

            Assert.Same(elseResult, result);
        }

        [t("Coalesce() returns 'elseResult' if none and 'predicate' is false.")]
        public static void Coalesce8() {
            var none = Maybe<Obj>.None;
            var thenResult = new Obj("then");
            var elseResult = new Obj("else");

            var result = none.Coalesce(_ => false, thenResult, elseResult);

            Assert.Same(elseResult, result);
        }

        [t("When() guards.")]
        public static void When0() {
            var some = Maybe.Of(new Obj());
            Assert.Throws<ArgumentNullException>("predicate", () => some.When(null, _ => { }));
            Assert.Throws<ArgumentNullException>("action", () => some.When(_ => true, null));
            Assert.Throws<ArgumentNullException>("predicate", () => some.When(null, _ => { }, () => { }));
            Assert.Throws<ArgumentNullException>("action", () => some.When(_ => true, null, () => { }));
            Assert.Throws<ArgumentNullException>("otherwise", () => some.When(_ => true, _ => { }, null));

            var none = Maybe<Obj>.None;
            Assert.Throws<ArgumentNullException>("predicate", () => none.When(null, _ => { }));
            Assert.Throws<ArgumentNullException>("action", () => none.When(_ => true, null));
            Assert.Throws<ArgumentNullException>("predicate", () => none.When(null, _ => { }, () => { }));
            Assert.Throws<ArgumentNullException>("action", () => none.When(_ => true, null, () => { }));
            Assert.Throws<ArgumentNullException>("otherwise", () => none.When(_ => true, _ => { }, null));
        }

        [t("When() calls 'action' if some and 'predicate' is true (1).")]
        public static void When1() {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var notCalled = true;
            Action<Obj> action = _ => wasCalled = true;
            Action otherwise = () => notCalled = false;

            some.When(_ => true, action, otherwise);

            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("When() calls 'action' if some and 'predicate' is true (2).")]
        public static void When2() {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            Action<Obj> action = _ => wasCalled = true;

            some.When(_ => true, action);

            Assert.True(wasCalled);
        }

        [t("When() calls 'otherwise' if some and 'predicate' is false.")]
        public static void When3() {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var notCalled = true;
            Action<Obj> action = _ => notCalled = false;
            Action otherwise = () => wasCalled = true;

            some.When(_ => false, action, otherwise);

            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("When() does not call 'action' if some and 'predicate' is false.")]
        public static void When4() {
            var some = Maybe.Of(new Obj());
            var notCalled = true;
            Action<Obj> action = _ => notCalled = false;

            some.When(_ => false, action);

            Assert.True(notCalled);
        }

        [t("When() calls 'otherwise' if none and 'predicate' is true.")]
        public static void When5() {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            var notCalled = true;
            Action<Obj> action = _ => notCalled = false;
            Action otherwise = () => wasCalled = true;

            none.When(_ => true, action, otherwise);

            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("When() does not call 'action' if none and 'predicate' is true.")]
        public static void When6() {
            var none = Maybe<Obj>.None;
            var notCalled = true;
            Action<Obj> action = _ => notCalled = false;

            none.When(_ => true, action);

            Assert.True(notCalled);
        }

        [t("When() calls 'otherwise' if none and 'predicate' is false.")]
        public static void When7() {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            var notCalled = true;
            Action<Obj> action = _ => notCalled = false;
            Action otherwise = () => wasCalled = true;

            none.When(_ => false, action, otherwise);

            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("When() does not call 'action' if none and 'predicate' is false.")]
        public static void When8() {
            var none = Maybe<Obj>.None;
            var notCalled = true;
            Action<Obj> action = _ => notCalled = false;

            none.When(_ => false, action);

            Assert.True(notCalled);
        }

        [t("Do() guards.")]
        public static void Do0() {
            var some = Maybe.Of(new Obj());
            Assert.Throws<ArgumentNullException>("onSome", () => some.Do(null, () => { }));
            Assert.Throws<ArgumentNullException>("onNone", () => some.Do(_ => { }, null));

            var none = Maybe<Obj>.None;
            Assert.Throws<ArgumentNullException>("onSome", () => none.Do(null, () => { }));
            Assert.Throws<ArgumentNullException>("onNone", () => none.Do(_ => { }, null));
        }

        [t("Do() calls 'onSome' if some.")]
        public static void Do1() {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var notCalled = true;
            Action<Obj> onSome = _ => wasCalled = true;
            Action onNone = () => notCalled = false;

            some.Do(onSome, onNone);

            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("Do() calls 'onNone' if none.")]
        public static void Do2() {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            var notCalled = true;
            Action<Obj> onSome = _ => notCalled = false;
            Action onNone = () => wasCalled = true;

            none.Do(onSome, onNone);

            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("OnSome() guards.")]
        public static void OnSome0() {
            var some = Maybe.Of(new Obj());
            Assert.Throws<ArgumentNullException>("action", () => some.OnSome(null));

            var none = Maybe<Obj>.None;
            Assert.Throws<ArgumentNullException>("action", () => none.OnSome(null));
        }

        [t("OnSome() calls 'action' if some.")]
        public static void OnSome1() {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            Action<Obj> act = _ => wasCalled = true;

            some.OnSome(act);

            Assert.True(wasCalled);
        }

        [t("OnSome() does not call 'action' if none.")]
        public static void OnSome2() {
            var none = Maybe<Obj>.None;
            var notCalled = true;
            Action<Obj> act = _ => notCalled = false;

            none.OnSome(act);

            Assert.True(notCalled);
        }

        [t("OnNone() guards.")]
        public static void OnNone0() {
            var some = Maybe.Of(new Obj());
            Assert.Throws<ArgumentNullException>("action", () => some.OnNone(null));

            var none = Maybe<Obj>.None;
            Assert.Throws<ArgumentNullException>("action", () => none.OnNone(null));
        }

        [t("OnNone() calls 'action' if none.")]
        public static void OnNone1() {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            Action act = () => wasCalled = true;

            none.OnNone(act);

            Assert.True(wasCalled);
        }

        [t("OnNone() does not call 'action' if some.")]
        public static void OnNone2() {
            var some = Maybe.Of(new Obj());
            var notCalled = true;
            Action act = () => notCalled = false;

            some.OnNone(act);

            Assert.True(notCalled);
        }
    }

    // Tests for the monadic methods.
    public static partial class MaybeFacts {
        [t("Bind() guards.")]
        public static void Bind0() {
            var some = Maybe.Of(new Obj());
            Assert.Throws<ArgumentNullException>("binder", () => some.Bind<string>(null));

            var none = Maybe<Obj>.None;
            Assert.Throws<ArgumentNullException>("binder", () => none.Bind<string>(null));
        }

        [t("Bind() returns none if none.")]
        public static void Bind1() {
            var none = Maybe<Obj>.None;
            Func<Obj, Maybe<string>> binder = x => Maybe.Of(x.Value);

            var m = none.Bind(binder);

            Assert.True(m.IsNone);
        }

        [t("Bind() returns some if some.")]
        public static void Bind2() {
            var exp = new Obj();
            var some = Maybe.Of(exp);
            Func<Obj, Maybe<string>> binder = x => Maybe.Of(x.Value);

            var m = some.Bind(binder);

            Assert.True(m.IsSome);
        }

        [t("Flatten() returns none if none.")]
        public static void Flatten1() {
            var none = Maybe<Maybe<Obj>>.None;
            var m = none.Flatten();
            Assert.True(m.IsNone);
        }

        [t("Flatten() returns some if some.")]
        public static void Flatten2() {
            var some = Maybe.Of(Maybe.Of(new Obj()));
            var m = some.Flatten();
            Assert.True(m.IsSome);
        }

        [t("Flatten() returns none if none for T? (1).")]
        public static void Flatten3() {
            var none = Maybe<int?>.None;
            var m = none.Flatten();
            Assert.True(m.IsNone);
        }

        [t("Flatten() returns none if none for T? (2).")]
        public static void Flatten4() {
            var none = Maybe.Of(1).Select<int?>(i => null);
            var m = none.Flatten();
            Assert.True(m.IsNone);
        }

        [t("Flatten() returns some if some for T?.")]
        public static void Flatten5() {
            var some = Maybe.Of(1).Select<int?>(i => 1);
            var m = some.Flatten();
            Assert.True(m.IsSome);
        }

        [t("OrElse(other) returns 'other' if none.")]
        public static void OrElse1() {
            var none = Maybe<Obj>.None;
            var other = Maybe.Of(new Obj());

            var obj = none.OrElse(other);

            Assert.Equal(other, obj);
        }

        [t("OrElse() returns 'this' if some.")]
        public static void OrElse2() {
            var some = Maybe.Of(new Obj());
            var other = Maybe.Of(new Obj("other"));

            var obj = some.OrElse(other);

            Assert.Equal(some, obj);
        }

        [t("ReplaceBy() returns some if some.")]
        public static void ReplaceBy1() {
            var some = Maybe.Of(1);
            var exp = 2;

            var m1 = some.ReplaceBy(exp);
            Assert.True(m1.IsSome);

            var m2 = Maybe.ReplaceBy(some, exp);
            Assert.True(m2.IsSome);
        }

        [t("ReplaceBy() returns none if none.")]
        public static void ReplaceBy2() {
            var none = Maybe<int>.None;
            var exp = 2;

            var m1 = none.ReplaceBy(exp);
            Assert.True(m1.IsNone);

            var m2 = Maybe.ReplaceBy(none, exp);
            Assert.True(m2.IsNone);
        }

        [t("ContinueWith(other) returns 'other' if some.")]
        public static void ContinueWith1() {
            var some = Maybe.Of(new Obj());
            var exp = Maybe.Of(1);

            var m1 = some.ContinueWith(exp);
            Assert.Equal(exp, m1);

            var m2 = Maybe.ContinueWith(some, exp);
            Assert.Equal(exp, m2);
        }

        [t("ContinueWith() returns none if none.")]
        public static void ContinueWith2() {
            var none = Maybe<Obj>.None;
            var other = Maybe.Of(1);

            var m1 = none.ContinueWith(other);
            Assert.True(m1.IsNone);

            var m2 = Maybe.ContinueWith(none, other);
            Assert.True(m2.IsNone);
        }

        [t("PassBy(other) returns 'this' if 'other' is some.")]
        public static void PassBy1() {
            var other = Maybe.Of(1);

            var some = Maybe.Of(new Obj());

            var m1 = some.PassBy(other);
            Assert.Equal(some, m1);

            var m2 = Maybe.PassBy(some, other);
            Assert.Equal(some, m2);

            var none = Maybe<Obj>.None;

            var m3 = none.PassBy(other);
            Assert.Equal(none, m3);

            var m4 = Maybe.PassBy(none, other);
            Assert.Equal(none, m4);
        }

        [t("PassBy(other) returns none if 'other' is none.")]
        public static void PassBy2() {
            var other = Maybe<int>.None;

            var some = Maybe.Of(new Obj());

            var m1 = some.PassBy(other);
            Assert.True(m1.IsNone);

            var m2 = Maybe.PassBy(some, other);
            Assert.True(m2.IsNone);

            var none = Maybe<Obj>.None;

            var m3 = none.PassBy(other);
            Assert.True(m3.IsNone);

            var m4 = Maybe.PassBy(none, other);
            Assert.True(m4.IsNone);
        }

        [t("Skip() returns 'Maybe.Unit' if some.")]
        public static void Skip1() {
            var some = Maybe.Of(new Obj());

            var m1 = some.Skip();
            Assert.Equal(Maybe.Unit, m1);

            var m2 = Maybe.Skip(some);
            Assert.Equal(Maybe.Unit, m2);
        }

        [t("Skip() returns 'Maybe.None' if none.")]
        public static void Skip2() {
            var none = Maybe<Obj>.None;

            var m1 = none.Skip();
            Assert.Equal(Maybe.None, m1);

            var m2 = Maybe.Skip(none);
            Assert.Equal(Maybe.None, m2);
        }

        [t("Select() returns some if some.")]
        public static void Select1() {
            var some = Maybe.Of(1);
            Func<int, int> selector = x => 2 * x;

            var m1 = some.Select(selector);
            Assert.True(m1.IsSome);

            var m2 = Maybe.Select(some, selector);
            Assert.True(m2.IsSome);

            var q = from item in some select selector(item);
            Assert.True(q.IsSome);
        }

        [t("Select() returns none if none.")]
        public static void Select2() {
            var none = Maybe<int>.None;
            Func<int, int> selector = x => 2 * x;

            var m1 = none.Select(selector);
            Assert.True(m1.IsNone);

            var m2 = Maybe.Select(none, selector);
            Assert.True(m2.IsNone);

            var q = from item in none select selector(item);
            Assert.True(q.IsNone);
        }

        [t("Where() returns none if some and 'predicate' is false.")]
        public static void Where1() {
            var some = Maybe.Of(1);
            Func<int, bool> predicate = _ => false;

            var m1 = some.Where(predicate);
            Assert.True(m1.IsNone);

            var m2 = Maybe.Where(some, predicate);
            Assert.True(m2.IsNone);

            var q = from _ in some where predicate(_) select _;
            Assert.True(q.IsNone);
        }

        [t("Where() returns some if some and 'predicate' is true.")]
        public static void Where2() {
            var some = Maybe.Of(1);
            Func<int, bool> predicate = _ => true;

            var m1 = some.Where(predicate);
            Assert.True(m1.IsSome);
            Assert.Equal(some, m1);

            var m2 = Maybe.Where(some, predicate);
            Assert.True(m2.IsSome);
            Assert.Equal(some, m2);

            var q = from _ in some where predicate(_) select _;
            Assert.True(q.IsSome);
            Assert.Equal(some, q);
        }

        [t("SelectMany() returns none if none and 'valueSelector' returns some.")]
        public static void SelectMany1() {
            var none = Maybe<int>.None;
            var some = Maybe.Of(2);
            Func<int, Maybe<int>> valueSelector = _ => some;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            var m1 = none.SelectMany(valueSelector, resultSelector);
            Assert.True(m1.IsNone);

            var m2 = Maybe.SelectMany(none, valueSelector, resultSelector);
            Assert.True(m2.IsNone);

            var q = from i in none
                    from j in some
                    select resultSelector(i, j);
            Assert.True(q.IsNone);
        }

        [t("SelectMany() returns none if none and 'valueSelector' returns none.")]
        public static void SelectMany2() {
            var none1 = Maybe<int>.None;
            var none2 = Maybe<int>.None;
            Func<int, Maybe<int>> valueSelector = _ => none2;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            var m1 = none1.SelectMany(valueSelector, resultSelector);
            Assert.True(m1.IsNone);

            var m2 = Maybe.SelectMany(none1, valueSelector, resultSelector);
            Assert.True(m2.IsNone);

            var q = from i in none1
                    from j in none2
                    select resultSelector(i, j);
            Assert.True(q.IsNone);
        }

        [t("SelectMany() returns none if some and 'valueSelector' returns none..")]
        public static void SelectMany3() {
            var some = Maybe.Of(1);
            var none = Maybe<int>.None;
            Func<int, Maybe<int>> valueSelector = _ => none;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            var m1 = some.SelectMany(valueSelector, resultSelector);
            Assert.True(m1.IsNone);

            var m2 = Maybe.SelectMany(some, valueSelector, resultSelector);
            Assert.True(m2.IsNone);

            var q = from i in some
                    from j in none
                    select resultSelector(i, j);
            Assert.True(q.IsNone);
        }

        [t("Join() returns none if join fails.")]
        public static void Join1() {
            var some1 = Maybe.Of(1);
            var some2 = Maybe.Of(2);

            var m1 = some1.Join(some2, i => i, i => i, (i, j) => i + j);
            Assert.True(m1.IsNone);

            var m2 = Maybe.Join(some1, some2, i => i, i => i, (i, j) => i + j);
            Assert.True(m2.IsNone);

            var q = from i in some1
                    join j in some2 on i equals j
                    select i + j;
            Assert.True(q.IsNone);
        }
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class MaybeFacts {
        [t("η(null) returns none.")]
        public static void η1() {
            var m1 = Maybe<Obj>.η(null);
            Assert.True(m1.IsNone);

            var m2 = Maybe<Val?>.η(null);
            Assert.True(m2.IsNone);
        }

        [t("η(non-null) returns some.")]
        public static void η2() {
            var m1 = Maybe<int>.η(1);
            Assert.True(m1.IsSome);

            var m2 = Maybe<Val>.η(new Val(1));
            Assert.True(m2.IsSome);

            var m3 = Maybe<Val?>.η(new Val(1));
            Assert.True(m3.IsSome);

            var m4 = Maybe<Obj>.η(new Obj());
            Assert.True(m4.IsSome);
        }

        [t("Casting (from) non-null returns some of Value.")]
        public static void cast5() {
            var exp = new Obj();
            var some = (Maybe<Obj>)exp;

            Assert.Same(exp, some.Value);
        }

        [t("Value is immutable.")]
        public static void Value1() {
            var v = new Obj();
            var m = Maybe.Of(v);

            v = null;

            Assert.NotNull(m.Value);
            Assert.NotSame(v, m.Value);
        }

        [t("Value contains the original value.")]
        public static void Value2() {
            var v1 = 1;
            var m1 = Maybe.Of(v1);
            Assert.Equal(v1, m1.Value);

            var v2 = new Val(1);
            var m2 = Maybe.Of(v2);
            Assert.Equal(v2, m2.Value);

            var v3 = (Val?)new Val(1);
            var m3 = Maybe.Of(v3);
            Assert.Equal(v3, m3.Value);

            var v4 = new Obj();
            var m4 = Maybe.Of(v4);
            Assert.Same(v4, m4.Value);
        }

        [ReleaseOnlyFact(DisplayName = nameof(Maybe) + " - Value contains default(T) if none.")]
        public static void Value3() {
            var m1 = Maybe<Val>.None;
            Assert.Equal(default(Val), m1.Value);

            var m2 = Maybe<Val?>.None;
            Assert.Null(m2.Value);

            var m3 = Maybe<Obj>.None;
            Assert.Null(m3.Value);
        }

        [t("ReplaceBy() replace Value with the new one if some.")]
        public static void ReplaceBy3() {
            var v = new Obj("other");
            var m = Maybe.Of(new Obj()).ReplaceBy(v);
            Assert.Same(v, m.Value);
        }

        [t("Bind() applies binder if some.")]
        public static void Bind3() {
            var some = Maybe.Of(1);
            Func<int, Maybe<int>> binder = x => Maybe.Of(2 * x);

            var m = some.Bind(binder);
            Assert.Equal(2, m.Value);
        }

        [t("Select() applies selector if some.")]
        public static void Select3() {
            var some = Maybe.Of(1);
            Func<int, int> selector = x => 2 * x;

            var m1 = some.Select(selector);
            Assert.Equal(2, m1.Value);

            var m2 = Maybe.Select(some, selector);
            Assert.Equal(2, m2.Value);

            var q = from item in some select selector(item);
            Assert.Equal(2, q.Value);
        }

        [t("SelectMany() applies selectors ands returns some if some.")]
        public static void SelectMany4() {
            var source = Maybe.Of(1);
            var middle = Maybe.Of(2);
            Func<int, Maybe<int>> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            var m1 = source.SelectMany(valueSelector, resultSelector);
            Assert.True(m1.IsSome);
            Assert.Equal(3, m1.Value);

            var m2 = Maybe.SelectMany(source, valueSelector, resultSelector);
            Assert.True(m2.IsSome);
            Assert.Equal(3, m2.Value);

            var q = from i in source
                    from j in middle
                    select resultSelector(i, j);
            Assert.True(q.IsSome);
            Assert.Equal(3, q.Value);
        }

        [t("Join() returns some if join succeeds.")]
        public static void Join2() {
            var source = Maybe.Of(1);
            var inner = Maybe.Of(2);

            var m1 = source.Join(inner, i => 2 * i, i => i, (i, j) => i + j);
            Assert.True(m1.IsSome);
            Assert.Equal(3, m1.Value);

            var m2 = Maybe.Join(source, inner, i => 2 * i, i => i, (i, j) => i + j);
            Assert.True(m2.IsSome);
            Assert.Equal(3, m2.Value);

            var q = from i in source
                    join j in inner on 2 * i equals j
                    select i + j;
            Assert.True(q.IsSome);
            Assert.Equal(3, q.Value);
        }
    }

#endif
}
