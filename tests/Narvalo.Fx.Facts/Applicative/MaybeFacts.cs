// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    using static global::My;

    public static partial class MaybeFacts {
        #region Unit

        [t("Unit is some.")]
        public static void Unit1() {
            Assert.True(Maybe.Unit.IsSome);
            Assert.False(Maybe.Unit.IsNone);
        }

        #endregion

        #region None

        [t("None (static) is none.")]
        public static void None1() {
            Assert.True(Maybe.None.IsNone);
            Assert.False(Maybe.None.IsSome);
        }

        [t("None is none.")]
        public static void None2() {
            var o1 = Maybe<int>.None;
            var o2 = Maybe<Val>.None;
            var o3 = Maybe<Val?>.None;
            var o4 = Maybe<Obj>.None;

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

        #endregion

        #region Of()

        [t("Of(non-null) returns some.")]
        public static void Of1() {
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

        [t("Of(null) returns none.")]
        public static void Of2() {
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

        [t("Casting (to T) throws if none.")]
        public static void cast1() {
            Action act = () => { var _ = (Obj)Maybe<Obj>.None; };
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<InvalidCastException>(ex);
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
            Assert.Same(exp, some.Value);
        }

        #endregion

        #region ValueOrDefault()

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

        #endregion

        #region ValueOrElse()

        [t("ValueOrElse() guards.")]
        public static void ValueOrElse0() {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("other", () => some.ValueOrElse(default(Obj)));
            Assert.Throws<ArgumentNullException>("valueFactory", () => some.ValueOrElse(default(Func<Obj>)));

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

        #endregion

        #region ValueOrThrow()

        [t("ValueOrThrow() guards.")]
        public static void ValueOrThrow0() {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("exceptionFactory", () => some.ValueOrThrow(null));
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

        #endregion

        #region op_Equality() & op_Inequality()

        [t("== and != when the Value's are equals.")]
        public static void Equality1() {
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

        [t("== and != when the Value's are not equals.")]
        public static void Equality2() {
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

        [t("Equals() guards.")]
        public static void Equals0() {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("comparer", () => some.Equals(some, null));
            Assert.Throws<ArgumentNullException>("comparer", () => some.Equals(none, null));

            Assert.Throws<ArgumentNullException>("comparer", () => none.Equals(none, null));
            Assert.Throws<ArgumentNullException>("comparer", () => none.Equals(some, null));
        }

        [t("Equals() is reflexive.")]
        public static void Equals1() {
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

        [t("Equals() is abelian.")]
        public static void Equals2() {
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

        [t("Equals() is transitive.")]
        public static void Equals3() {
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

        [t("Equals(null) returns false if none.")]
        public static void Equals4() {
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

        [t("Equals(null) returns false if some.")]
        public static void Equals5() {
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

        [t("Equals() follows structural equality rules.")]
        public static void Equals6() {
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

        [t("Equals() follows structural equality rules after boxing.")]
        public static void Equals7() {
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

        [t("GetHashCode() guards.")]
        public static void GetHashCode0() {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("comparer", () => some.GetHashCode(null));
            Assert.Throws<ArgumentNullException>("comparer", () => none.GetHashCode(null));
        }

        [t("GetHashCode() returns the same result when called repeatedly.")]
        public static void GetHashCode1() {
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

        [t("GetHashCode() returns the same result for equal instances.")]
        public static void GetHashCode2() {
            var tuple1 = Maybe.Of(Tuple.Create("1"));
            var tuple2 = Maybe.Of(Tuple.Create("1"));

            Assert.NotSame(tuple1, tuple2);
            Assert.Equal(tuple1.GetHashCode(), tuple2.GetHashCode());
            Assert.Equal(tuple1.GetHashCode(EqualityComparer<Tuple<string>>.Default), tuple2.GetHashCode(EqualityComparer<Tuple<string>>.Default));
        }

        #endregion

        #region ToString()

        [t("ToString() result contains a string representation of the value if some, contains 'None' if none.")]
        public static void ToString1() {
            var exp = "My Value";
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(exp);

            Assert.Contains("None", none.ToString());
            Assert.Contains(exp, some.ToString());
        }

        #endregion
    }

    // Tests for the Internal.IMaybe<T> interface.
    public static partial class MaybeFacts {
        #region ToEnumerable()

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

        #endregion

        #region GetEnumerator()

        [t("GetEnumerator() does not iterate.")]
        public static void GetEnumerator1() {
            var none = Maybe<Obj>.None;
            var count = 0;

            foreach (var x in none) { count++; }

            Assert.Equal(0, count);
        }

        [t("GetEnumerator() iterates only once.")]
        public static void GetEnumerator2() {
            var exp = new Obj();
            var some = Maybe.Of(exp);
            var count = 0;

            foreach (var x in some) { count++; Assert.Same(exp, x); }

            Assert.Equal(1, count);
        }

        #endregion

        #region Contains()

        [t("Contains() guards.")]
        public static void Contains0() {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());
            var value = new Obj();

            Assert.Throws<ArgumentNullException>("comparer", () => some.Contains(value, null));
            Assert.Throws<ArgumentNullException>("comparer", () => none.Contains(value, null));
        }

        [t("Contains() returns false if none.")]
        public static void Contains1() {
            var n1 = Maybe<int>.None;
            var n2 = Maybe<Obj>.None;

            Assert.False(n1.Contains(1));
            Assert.False(n1.Contains(1, EqualityComparer<int>.Default));
            Assert.False(n2.Contains(new Obj()));
            Assert.False(n2.Contains(new Obj(), EqualityComparer<Obj>.Default));
        }

        [t("Contains(value) returns true if it does contain 'value'.")]
        public static void Contains2() {
            var value = new Obj();
            var s1 = Maybe.Of(1);
            var s2 = Maybe.Of(value);

            Assert.True(s1.Contains(1), "Value type.");
            Assert.True(s1.Contains(1, EqualityComparer<int>.Default), "Value type.");
            Assert.True(s2.Contains(value), "Reference type.");
            Assert.True(s2.Contains(value, EqualityComparer<Obj>.Default), "Reference type.");
        }

        [t("Contains() returns true if references are different but they are structurally equals.")]
        public static void Contains3() {
            var some = Maybe.Of(Tuple.Create("value"));

            Assert.True(some.Contains(Tuple.Create("value")), "References differ but we have structural equality.");
            Assert.True(some.Contains(Tuple.Create("value"), EqualityComparer<Tuple<string>>.Default), "References differ but we have structural equality.");
        }

        [t("Contains(value) returns false if it does not contain 'value'.")]
        public static void Contains4() {
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

        [t("Match() guards.")]
        public static void Match0() {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("caseSome", () => some.Match(null, new Obj()));
            Assert.Throws<ArgumentNullException>("caseSome", () => some.Match(null, () => new Obj()));
            Assert.Throws<ArgumentNullException>("caseNone", () => some.Match(x => x, default(Func<Obj>)));

            Assert.Throws<ArgumentNullException>("caseSome", () => none.Match(null, new Obj()));
            Assert.Throws<ArgumentNullException>("caseSome", () => none.Match(null, () => new Obj()));
            Assert.Throws<ArgumentNullException>("caseNone", () => none.Match(x => x, default(Func<Obj>)));
        }

        [t("Match() calls 'caseSome' if some (1).")]
        public static void Match1() {
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

        [t("Match() calls 'caseSome' if some (2).")]
        public static void Match2() {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            var exp = new Obj("caseSome");
            Func<Obj, Obj> caseSome = _ => { wasCalled = true; return exp; };
            var caseNone = new Obj("caseNone");

            var rs = some.Match(caseSome, caseNone);

            Assert.True(wasCalled);
            Assert.Same(exp, rs);
        }

        [t("Match() calls 'caseNone' if none (1).")]
        public static void Match3() {
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

        [t("Match() calls 'caseNone' if none (2).")]
        public static void Match4() {
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

        [t("Coalesce() guards.")]
        public static void Coalesce0() {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("predicate", () => some.Coalesce(null, new Obj("this"), new Obj("that")));
            Assert.Throws<ArgumentNullException>("predicate", () => some.Coalesce(null, x => x, () => new Obj()));
            Assert.Throws<ArgumentNullException>("selector", () => some.Coalesce(_ => true, null, () => new Obj()));
            Assert.Throws<ArgumentNullException>("otherwise", () => some.Coalesce(_ => true, x => x, null));

            Assert.Throws<ArgumentNullException>("predicate", () => none.Coalesce(null, new Obj("this"), new Obj("that")));
            Assert.Throws<ArgumentNullException>("predicate", () => none.Coalesce(null, x => x, () => new Obj()));
            Assert.Throws<ArgumentNullException>("selector", () => none.Coalesce(_ => true, null, () => new Obj()));
            Assert.Throws<ArgumentNullException>("otherwise", () => none.Coalesce(_ => true, x => x, null));
        }

        [t("Coalesce() calls 'selector' if some and 'predicate' is true.")]
        public static void Coalesce1() {
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

        [t("Coalesce() returns 'thenResult' if some and 'predicate' is true.")]
        public static void Coalesce2() {
            var some = Maybe.Of(new Obj());
            var thenResult = new Obj("then");
            var elseResult = new Obj("else");

            var rs = some.Coalesce(_ => true, thenResult, elseResult);

            Assert.Same(thenResult, rs);
        }

        [t("Coalesce() calls 'otherwise' if some and 'predicate' is false.")]
        public static void Coalesce3() {
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

        [t("Coalesce() returns 'elseResult' if some and 'predicate' is false.")]
        public static void Coalesce4() {
            var some = Maybe.Of(new Obj());
            var thenResult = new Obj("then");
            var elseResult = new Obj("else");

            var rs = some.Coalesce(_ => false, thenResult, elseResult);

            Assert.Same(elseResult, rs);
        }

        [t("Coalesce() calls 'otherwise' if none and 'predicate' is true.")]
        public static void Coalesce5() {
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

        [t("Coalesce() calls 'otherwise' if none and 'predicate' is false.")]
        public static void Coalesce6() {
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

        [t("Coalesce() returns 'elseResult' if none and 'predicate' is true.")]
        public static void Coalesce7() {
            var none = Maybe<Obj>.None;
            var thenResult = new Obj("then");
            var elseResult = new Obj("else");

            var rs = none.Coalesce(_ => true, thenResult, elseResult);

            Assert.Same(elseResult, rs);
        }

        [t("Coalesce() returns 'elseResult' if none and 'predicate' is false.")]
        public static void Coalesce8() {
            var none = Maybe<Obj>.None;
            var thenResult = new Obj("then");
            var elseResult = new Obj("else");

            var rs = none.Coalesce(_ => false, thenResult, elseResult);

            Assert.Same(elseResult, rs);
        }

        #endregion

        #region When()

        [t("When() guards.")]
        public static void When0() {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("predicate", () => some.When(null, _ => { }));
            Assert.Throws<ArgumentNullException>("action", () => some.When(_ => true, null));
            Assert.Throws<ArgumentNullException>("predicate", () => some.When(null, _ => { }, () => { }));
            Assert.Throws<ArgumentNullException>("action", () => some.When(_ => true, null, () => { }));
            Assert.Throws<ArgumentNullException>("otherwise", () => some.When(_ => true, _ => { }, null));

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
            var otherWasCalled = false;
            Action<Obj> action = _ => wasCalled = true;
            Action otherwise = () => otherWasCalled = true;

            some.When(_ => true, action, otherwise);

            Assert.True(wasCalled);
            Assert.False(otherWasCalled);
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
            var otherWasCalled = false;
            Action<Obj> action = _ => wasCalled = true;
            Action otherwise = () => otherWasCalled = true;

            some.When(_ => false, action, otherwise);

            Assert.False(wasCalled);
            Assert.True(otherWasCalled);
        }

        [t("When() does not call 'action' if some and 'predicate' is false.")]
        public static void When4() {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            Action<Obj> action = _ => wasCalled = true;

            some.When(_ => false, action);

            Assert.False(wasCalled);
        }

        [t("When() calls 'otherwise' if none and 'predicate' is true.")]
        public static void When5() {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            var otherWasCalled = false;
            Action<Obj> action = _ => wasCalled = true;
            Action otherwise = () => otherWasCalled = true;

            none.When(_ => true, action, otherwise);

            Assert.False(wasCalled);
            Assert.True(otherWasCalled);
        }

        [t("When() does not call 'action' if none and 'predicate' is true.")]
        public static void When6() {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            Action<Obj> action = _ => wasCalled = true;

            none.When(_ => true, action);

            Assert.False(wasCalled);
        }

        [t("When() calls 'otherwise' if none and 'predicate' is false.")]
        public static void When7() {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            var otherWasCalled = false;
            Action<Obj> action = _ => wasCalled = true;
            Action otherwise = () => otherWasCalled = true;

            none.When(_ => false, action, otherwise);

            Assert.False(wasCalled);
            Assert.True(otherWasCalled);
        }

        [t("When() does not call 'action' if none and 'predicate' is false.")]
        public static void When8() {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            Action<Obj> action = _ => wasCalled = true;

            none.When(_ => false, action);

            Assert.False(wasCalled);
        }

        #endregion

        #region Do()

        [t("Do() guards.")]
        public static void Do0() {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("onSome", () => some.Do(null, () => { }));
            Assert.Throws<ArgumentNullException>("onNone", () => some.Do(_ => { }, null));

            Assert.Throws<ArgumentNullException>("onSome", () => none.Do(null, () => { }));
            Assert.Throws<ArgumentNullException>("onNone", () => none.Do(_ => { }, null));
        }

        [t("Do() calls 'onSome' if some.")]
        public static void Do1() {
            var some = Maybe.Of(new Obj());
            var onSomeWasCalled = false;
            var onNoneWasCalled = false;
            Action<Obj> onSome = _ => onSomeWasCalled = true;
            Action onNone = () => onNoneWasCalled = true;

            some.Do(onSome, onNone);

            Assert.True(onSomeWasCalled);
            Assert.False(onNoneWasCalled);
        }

        [t("Do() calls 'onNone' if none.")]
        public static void Do2() {
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

        [t("OnSome() guards.")]
        public static void OnSome0() {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("action", () => some.OnSome(null));
            Assert.Throws<ArgumentNullException>("action", () => none.OnSome(null));
        }

        [t("OnSome() calls 'action' if some.")]
        public static void OnSome1() {
            var some = Maybe.Of(new Obj());
            var wasCalled = false;
            Action<Obj> op = _ => wasCalled = true;

            some.OnSome(op);

            Assert.True(wasCalled);
        }

        [t("OnSome() does not call 'action' if none.")]
        public static void OnSome2() {
            var none = Maybe<Obj>.None;
            var wasCalled = false;
            Action<Obj> op = _ => wasCalled = true;

            none.OnSome(op);

            Assert.False(wasCalled);
        }

        #endregion

        #region OnNone()

        [t("OnNone() guards.")]
        public static void OnNone0() {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("action", () => some.OnNone(null));
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
            var wasCalled = false;
            Action act = () => wasCalled = true;

            some.OnNone(act);

            Assert.False(wasCalled);
        }

        #endregion
    }

    // Tests for the monadic methods.
    public static partial class MaybeFacts {
        #region Bind()

        [t("Bind() guards.")]
        public static void Bind0() {
            var none = Maybe<Obj>.None;
            var some = Maybe.Of(new Obj());

            Assert.Throws<ArgumentNullException>("binder", () => some.Bind<string>(null));
            Assert.Throws<ArgumentNullException>("binder", () => none.Bind<string>(null));
        }

        [t("Bind() returns none if none.")]
        public static void Bind1() {
            var none = Maybe<Obj>.None;
            Func<Obj, Maybe<string>> binder = x => Maybe.Of(x.Value);

            var me = none.Bind(binder);

            Assert.True(me.IsNone);
        }

        [t("Bind() returns some if some.")]
        public static void Bind2() {
            var exp = new Obj();
            var some = Maybe.Of(exp);
            Func<Obj, Maybe<string>> binder = x => Maybe.Of(x.Value);

            var me = some.Bind(binder);

            Assert.True(me.IsSome);
        }

        #endregion

        #region Flatten()

        [t("Flatten() returns none if none.")]
        public static void Flatten1() {
            var me = Maybe<Maybe<Obj>>.None.Flatten();

            Assert.True(me.IsNone);
        }

        [t("Flatten() returns some if some.")]
        public static void Flatten2() {
            var me = Maybe.Of(Maybe.Of(new Obj())).Flatten();

            Assert.True(me.IsSome);
        }

        #endregion

        #region OrElse()

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

        #endregion

        #region Select()

        [t("Select() returns some if some.")]
        public static void Select1() {
            var some = Maybe.Of(1);
            Func<int, int> selector = x => 2 * x;

            var m1 = some.Select(selector);
            var m2 = Maybe.Select(some, selector);
            var q = from item in some select selector(item);

            Assert.True(m1.IsSome);
            Assert.True(m2.IsSome);
            Assert.True(q.IsSome);
        }

        [t("Select() returns none if none.")]
        public static void Select2() {
            var none = Maybe<int>.None;
            Func<int, int> selector = x => 2 * x;

            var m1 = none.Select(selector);
            var m2 = Maybe.Select(none, selector);
            var q = from item in none select selector(item);

            Assert.True(m1.IsNone);
            Assert.True(m2.IsNone);
            Assert.True(q.IsNone);
        }

        #endregion

        #region ReplaceBy()

        [t("ReplaceBy() returns some if some.")]
        public static void ReplaceBy1() {
            var exp = 2;

            var m1 = Maybe.Of(1).ReplaceBy(exp);
            var m2 = Maybe.ReplaceBy(Maybe.Of(1), exp);

            Assert.True(m1.IsSome);
            Assert.True(m2.IsSome);
        }

        [t("ReplaceBy() returns none if none.")]
        public static void ReplaceBy2() {
            var exp = 2;

            var m1 = Maybe<int>.None.ReplaceBy(exp);
            var m2 = Maybe.ReplaceBy(Maybe<int>.None, exp);

            Assert.True(m1.IsNone);
            Assert.True(m2.IsNone);
        }

        #endregion

        #region ContinueWith()

        [t("ContinueWith(other) returns 'other' if some.")]
        public static void ContinueWith1() {
            var exp = Maybe.Of(1);

            var m1 = Maybe.Of(new Obj()).ContinueWith(exp);
            var m2 = Maybe.ContinueWith(Maybe.Of(new Obj()), exp);

            Assert.Equal(exp, m1);
            Assert.Equal(exp, m2);
        }

        [t("ContinueWith() returns none if none.")]
        public static void ContinueWith2() {
            var other = Maybe.Of(1);

            var m1 = Maybe<Obj>.None.ContinueWith(other);
            var m2 = Maybe.ContinueWith(Maybe<Obj>.None, other);

            Assert.True(m1.IsNone);
            Assert.True(m2.IsNone);
        }

        #endregion

        #region PassBy()

        [t("PassBy(other) returns 'this' if 'other' is some.")]
        public static void PassBy1() {
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

        [t("PassBy(other) returns none if 'other' is none.")]
        public static void PassBy2() {
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

        [t("Skip() returns 'Maybe.Unit' if some.")]
        public static void Skip1() {
            var m1 = Maybe.Of(new Obj()).Skip();
            var m2 = Maybe.Skip(Maybe.Of(new Obj()));

            Assert.Equal(Maybe.Unit, m1);
            Assert.Equal(Maybe.Unit, m2);
        }

        [t("Skip() returns 'Maybe.None' if none.")]
        public static void Skip2() {
            var m1 = Maybe<Obj>.None.Skip();
            var m2 = Maybe.Skip(Maybe<Obj>.None);

            Assert.Equal(Maybe.None, m1);
            Assert.Equal(Maybe.None, m2);
        }

        #endregion
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class MaybeFacts {
        #region η()

        [t("η(null) returns none.")]
        public static void η1() {
            var o1 = Maybe<Obj>.η(null);
            var o2 = Maybe<Val?>.η(null);

            Assert.True(o1.IsNone);
            Assert.True(o2.IsNone);
        }

        [t("η(non-null) returns some.")]
        public static void η2() {
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

        [t("Value is immutable.")]
        public static void Value1() {
            var exp = new Obj();
            var some = Maybe.Of(exp);

            exp = null;

            Assert.NotNull(some.Value);
            Assert.NotEqual(exp, some.Value);
        }

        [t("Value contains the original value.")]
        public static void Value2() {
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
        public static void Value3() {
            Assert.Equal(Maybe<int>.None.Value, default(int));
            Assert.Null(Maybe<string>.None.Value);
            Assert.Null(Maybe<Obj>.None.Value);
        }

        #endregion

        #region ReplaceBy()

        [t("ReplaceBy() replace Value with the new one if some.")]
        public static void ReplaceBy3() {
            var exp = new Obj("other");

            var m = Maybe.Of(new Obj()).ReplaceBy(exp);

            Assert.Same(exp, m.Value);
        }

        #endregion
    }

#endif

    public static partial class MaybeFacts {
        #region Linq Operators

        [t("Where() returns none if predicate is false.")]
        public static void Where1() {
            var some = Maybe.Of(1);
            Func<int, bool> predicate = _ => false;

            var m1 = some.Where(predicate);
            var m2 = Maybe.Where(some, predicate);
            var q = from _ in some where predicate(_) select _;

            Assert.False(m1.IsSome);
            Assert.False(m2.IsSome);
            Assert.False(q.IsSome);
        }

        [t("SelectMany() returns none if none and 'valueSelector' returns some.")]
        public static void SelectMany1() {
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

        [t("SelectMany() returns none if none and 'valueSelector' returns none.")]
        public static void SelectMany2() {
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

        [t("SelectMany() returns none if some and 'valueSelector' returns none..")]
        public static void SelectMany3() {
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

        [t("Join() returns none if join fails.")]
        public static void Join1() {
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

    public static partial class MaybeFacts {
        #region Linq Operators

        [t("Where() returns some if 'predicate' is true.")]
        public static void Where2() {
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

        [t("SelectMany() applies selectors ands returns some if some.")]
        public static void SelectMany4() {
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

        [t("Join() returns some if join succeeds.")]
        public static void Join2() {
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
