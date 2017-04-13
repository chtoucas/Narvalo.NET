// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    using static global::My;

    // Tests for Outcome<T>.
    public static partial class OutcomeTFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Outcome), description) { }
        }

        [t("Unit is OK.")]
        public static void Unit1() {
            Assert.True(Outcome.Unit.IsSuccess);
            Assert.False(Outcome.Unit.IsError);
        }

        [t("Of() returns OK.")]
        public static void Of1() {
            var ok = Outcome.Of(1);
            Assert.True(ok.IsSuccess);
            Assert.False(ok.IsError);
        }

        [t("FromError() guards.")]
        public static void FromError0() {
            Assert.Throws<ArgumentNullException>("error", () => Outcome<int>.FromError(null));
            Assert.Throws<ArgumentException>("error", () => Outcome<int>.FromError(String.Empty));
        }

        [t("FromError() returns NOK.")]
        public static void FromError1() {
            var nok = Outcome<int>.FromError("error");
            Assert.True(nok.IsError);
            Assert.False(nok.IsSuccess);
        }

        [t("Deconstruct() if OK.")]
        public static void Deconstruct1() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);
            var (succeed, value, err) = ok;
            Assert.True(succeed);
            Assert.Equal(String.Empty, err);
            Assert.Same(exp, value);
        }

        [t("Deconstruct() if NOK.")]
        public static void Deconstruct2() {
            var exp = "error";
            var nok = Outcome<Obj>.FromError(exp);
            var (succeed, value, err) = nok;
            Assert.False(succeed);
            Assert.Equal(exp, err);
            Assert.Null(value);
        }

        [t("ValueOrDefault() returns Value if OK.")]
        public static void ValueOrDefault1() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);
            Assert.Same(exp, ok.ValueOrDefault());
        }

        [t("ValueOrDefault() returns default(T) if NOK.")]
        public static void ValueOrDefault2() {
            var nok = Outcome<Obj>.FromError("error");
            Assert.Same(default(Obj), nok.ValueOrDefault());
        }

        [t("ValueOrNone() returns some if OK.")]
        public static void ValueOrNone1() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);
            var maybe = ok.ValueOrNone();
            Assert.True(maybe.IsSome);
        }

        [t("ValueOrNone() returns none if NOK.")]
        public static void ValueOrNone() {
            var nok = Outcome<Obj>.FromError("error");
            var maybe = nok.ValueOrNone();
            Assert.True(maybe.IsNone);
        }

        [t("ValueOrElse() guards.")]
        public static void ValueOrElse0() {
            var ok = Outcome.Of(new Obj());
            Assert.Throws<ArgumentNullException>("valueFactory", () => ok.ValueOrElse((Func<Obj>)null));

            var nok = Outcome<Obj>.FromError("error");
            Assert.Throws<ArgumentNullException>("valueFactory", () => nok.ValueOrElse((Func<Obj>)null));
        }

        [t("ValueOrElse() returns Value if OK.")]
        public static void ValueOrElse1() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);
            var other = new Obj("other");
            Assert.Same(exp, ok.ValueOrElse(other));
            Assert.Same(exp, ok.ValueOrElse(() => other));
        }

        [t("ValueOrElse(other) returns 'other' if NOK.")]
        public static void ValueOrElse2() {
            var nok = Outcome<Obj>.FromError("error");
            var exp = new Obj();
            Assert.Same(exp, nok.ValueOrElse(exp));
            Assert.Same(exp, nok.ValueOrElse(() => exp));
        }

        [t("ValueOrThrow() guards.")]
        public static void ValueOrThrow0() {
            var ok = Outcome.Of(new Obj());
            Assert.Throws<ArgumentNullException>("exceptionFactory", () => ok.ValueOrThrow(null));

            var nok = Outcome<Obj>.FromError("error");
            Assert.Throws<ArgumentNullException>("exceptionFactory", () => nok.ValueOrThrow(null));
        }

        [t("ValueOrThrow() returns Value if OK.")]
        public static void ValueOrThrow1() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);
            Assert.Same(exp, ok.ValueOrThrow());
            Assert.Equal(exp, ok.ValueOrThrow(error => new SimpleException(error)));
        }

        [t("ValueOrThrow() throws InvalidOperationException if NOK.")]
        public static void ValueOrThrow2() {
            var nok = Outcome<Obj>.FromError("error");
            Action act = () => nok.ValueOrThrow();
            Assert.Throws<InvalidOperationException>(act);
        }

        [t("ValueOrThrow() throws custom exception if NOK.")]
        public static void ValueOrThrow3() {
            var message = "error";
            var nok = Outcome<Obj>.FromError(message);

            Action act = () => nok.ValueOrThrow(err => new SimpleException(err));
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<SimpleException>(ex);
            Assert.Equal(message, ex.Message);
        }

        [t("ToValue() throws InvalidCastException if NOK.")]
        public static void ToValue1() {
            var message = "error";
            var err = Outcome<Obj>.FromError(message);

            Assert.Throws<InvalidCastException>(() => err.ToValue());
        }

        [t("ToValue() returns Value if OK.")]
        public static void ToValue2() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);
            Assert.Same(exp, ok.ToValue());
        }

        [t("ToMaybe() returns some if OK.")]
        public static void ToMaybe1() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            var maybe = ok.ToMaybe();
            Assert.True(maybe.IsSome);
        }

        [t("ToMaybe() returns none if OK.")]
        public static void ToMaybe2() {
            var nok = Outcome<Obj>.FromError("error");

            var maybe = nok.ToMaybe();
            Assert.True(maybe.IsNone);
        }

        [t("GetHashCode() returns the same result when called repeatedly.")]
        public static void GetHashCode1() {
            var nok = Outcome<Obj>.FromError("error");
            Assert.Equal(nok.GetHashCode(), nok.GetHashCode());
            Assert.Equal(nok.GetHashCode(EqualityComparer<Obj>.Default), nok.GetHashCode(EqualityComparer<Obj>.Default));

            var ok1 = Outcome.Of(new Obj());
            Assert.Equal(ok1.GetHashCode(), ok1.GetHashCode());
            Assert.Equal(ok1.GetHashCode(EqualityComparer<Obj>.Default), ok1.GetHashCode(EqualityComparer<Obj>.Default));

            var ok2 = Outcome.Of(1);
            Assert.Equal(ok2.GetHashCode(), ok2.GetHashCode());
            Assert.Equal(ok2.GetHashCode(EqualityComparer<int>.Default), ok2.GetHashCode(EqualityComparer<int>.Default));
        }

        [t("GetHashCode() returns the same result for equal instances.")]
        public static void GetHashCode2() {
            var nok1 = Outcome<Obj>.FromError("error");
            var nok2 = Outcome<Obj>.FromError("error");

            Assert.Equal(nok1.GetHashCode(), nok2.GetHashCode());
            Assert.Equal(nok1.GetHashCode(EqualityComparer<Obj>.Default), nok2.GetHashCode(EqualityComparer<Obj>.Default));

            var ok1 = Outcome.Of(Tuple.Create("1"));
            var ok2 = Outcome.Of(Tuple.Create("1"));

            Assert.Equal(ok1.GetHashCode(), ok2.GetHashCode());
            Assert.Equal(ok1.GetHashCode(EqualityComparer<Tuple<string>>.Default), ok2.GetHashCode(EqualityComparer<Tuple<string>>.Default));
        }

        [t("GetHashCode() returns different results for non-equal instances.")]
        public static void GetHashCode3() {
            var nok1 = Outcome<int>.FromError("error1");
            var nok2 = Outcome<int>.FromError("error2");

            Assert.NotEqual(nok1.GetHashCode(), nok2.GetHashCode());
            Assert.NotEqual(nok1.GetHashCode(EqualityComparer<int>.Default), nok2.GetHashCode(EqualityComparer<int>.Default));

            var ok1 = Outcome.Of(1);
            var ok2 = Outcome.Of(2);

            Assert.NotEqual(ok1.GetHashCode(), ok2.GetHashCode());
            Assert.NotEqual(ok1.GetHashCode(EqualityComparer<int>.Default), ok2.GetHashCode(EqualityComparer<int>.Default));

            Assert.NotEqual(ok1.GetHashCode(), nok1.GetHashCode());
            Assert.NotEqual(ok1.GetHashCode(EqualityComparer<int>.Default), nok1.GetHashCode(EqualityComparer<int>.Default));
        }

        [t("ToString() result contains a string representation of the value if OK, of the error if NOK.")]
        public static void ToString1() {
            var value = new Obj("My value");
            var ok = Outcome.Of(value);
            Assert.Contains(value.ToString(), ok.ToString(), StringComparison.OrdinalIgnoreCase);

            var error = "My error";
            var nok = Outcome<Obj>.FromError(error);
            Assert.Contains(error, nok.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }

    public static partial class OutcomeTFacts {
        [t("ToEnumerable() result is empty if NOK.")]
        public static void ToEnumerable1() {
            var nok = Outcome<Obj>.FromError("error");
            var seq = nok.ToEnumerable();
            Assert.Empty(seq);
        }

        [t("ToEnumerable() result is a sequence made of exactly one element if OK.")]
        public static void ToEnumerable2() {
            var obj = new Obj();
            var ok = Outcome.Of(obj);
            var seq = ok.ToEnumerable();
            Assert.Equal(Enumerable.Repeat(obj, 1), seq);
        }

        [t("GetEnumerator() does not iterate if NOK.")]
        public static void GetEnumerator1() {
            var nok = Outcome<Obj>.FromError("error");
            var count = 0;

            foreach (var x in nok) { count++; }

            Assert.Equal(0, count);
        }

        [t("GetEnumerator() iterates only once if OK.")]
        public static void GetEnumerator2() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);
            var count = 0;

            foreach (var x in ok) { count++; Assert.Same(exp, x); }

            Assert.Equal(1, count);
        }

        [t("Match() guards.")]
        public static void Match0() {
            var ok = Outcome.Of(new Obj());
            Assert.Throws<ArgumentNullException>("caseSuccess", () => ok.Match(null, _ => new Obj()));
            Assert.Throws<ArgumentNullException>("caseError", () => ok.Match(val => val, null));

            var nok = Outcome<Obj>.FromError("error");
            Assert.Throws<ArgumentNullException>("caseSuccess", () => nok.Match(null, _ => new Obj()));
            Assert.Throws<ArgumentNullException>("caseError", () => nok.Match(val => val, null));
        }
    }

    // Tests for the monadic methods.
    public static partial class OutcomeTFacts {
        [t("Bind() guards.")]
        public static void Bind0() {
            var ok = Outcome.Of(new Obj());
            Assert.Throws<ArgumentNullException>("binder", () => ok.Bind<string>(null));

            var nok = Outcome<Obj>.FromError("error");
            Assert.Throws<ArgumentNullException>("binder", () => nok.Bind<string>(null));
        }

        [t("Bind() returns NOK if NOK.")]
        public static void Bind1() {
            var nok = Outcome<Obj>.FromError("error");
            Func<Obj, Outcome<string>> binder = x => Outcome.Of(x.Value);

            var result = nok.Bind(binder);
            Assert.True(result.IsError);
        }

        [t("Bind() returns OK if OK.")]
        public static void Bind2() {
            var ok = Outcome.Of(new Obj("My Value"));
            Func<Obj, Outcome<string>> binder = x => Outcome.Of(x.Value.ToUpperInvariant());

            var result = ok.Bind(binder);
            Assert.True(result.IsSuccess);
        }

        [t("Flatten() returns NOK if NOK.")]
        public static void Flatten1() {
            var nok = Outcome<Outcome<Obj>>.FromError("error");
            var result = nok.Flatten();
            Assert.True(result.IsError);
        }

        [t("Flatten() returns OK if OK.")]
        public static void Flatten2() {
            var ok = Outcome.Of(Outcome.Of(new Obj()));
            var result = ok.Flatten();
            Assert.True(result.IsSuccess);
        }

        [t("Where() guards.")]
        public static void Where0() {
            var ok = Outcome.Of(new Obj());
            Assert.Throws<ArgumentNullException>("filter", () => ok.Where(null));

            var nok = Outcome<Obj>.FromError("error");
            Assert.Throws<ArgumentNullException>("filter", () => nok.Where(null));
        }

        [t("Where() returns NOK if OK and filter returns NOK.")]
        public static void Where1() {
            var ok = Outcome.Of(new Obj());
            Func<Obj, Outcome> filter = _ => Outcome.FromError("error");
            var result = ok.Where(filter);
            Assert.True(result.IsError);

            var q = from val in ok where filter(val) select val;
            Assert.True(q.IsError);
        }

        [t("Where() returns OK if OK and filter returns OK.")]
        public static void Where2() {
            var ok = Outcome.Of(new Obj());
            Func<Obj, Outcome> filter = _ => Outcome.Ok;
            var result = ok.Where(filter);
            Assert.True(result.IsSuccess);

            var q = from val in ok where filter(val) select val;
            Assert.True(q.IsSuccess);

        }

        [t("Where() returns NOK if NOK and filter returns NOK.")]
        public static void Where3() {
            var ok = Outcome<Obj>.FromError("error");
            Func<Obj, Outcome> filter = _ => Outcome.FromError("error");
            var result = ok.Where(filter);
            Assert.True(result.IsError);

            var q = from val in ok where filter(val) select val;
            Assert.True(q.IsError);
        }

        [t("Where() returns NOK if NOK and filter returns OK.")]
        public static void Where4() {
            var ok = Outcome<Obj>.FromError("error");
            Func<Obj, Outcome> filter = _ => Outcome.Ok;
            var result = ok.Where(filter);
            Assert.True(result.IsError);

            var q = from val in ok where filter(val) select val;
            Assert.True(q.IsError);
        }
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class OutcomeTFacts {
        [t("ValueOrNone() returns some for underlying value if OK.")]
        public static void ValueOrNone3() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            var maybe = ok.ValueOrNone();
            Assert.Same(exp, maybe.Value);
        }

        [t("ToMaybe() returns some for underlying value if OK.")]
        public static void ToMaybe3() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            var maybe = ok.ToMaybe();
            Assert.Same(exp, maybe.Value);
        }

        [t("Bind() transports error if NOK.")]
        public static void Bind3() {
            var exp = "error";
            var nok = Outcome<Obj>.FromError(exp);
            Func<Obj, Outcome<string>> binder = x => Outcome.Of(x.Value);

            var result = nok.Bind(binder);
            Assert.Equal(exp, result.Error);
        }

        [t("Bind() applies binder if OK.")]
        public static void Bind4() {
            var value = "My Value";
            var ok = Outcome.Of(new Obj(value));
            Func<Obj, Outcome<string>> binder = x => Outcome.Of(x.Value.ToUpperInvariant());

            var result = ok.Bind(binder);
            Assert.Equal(value.ToUpperInvariant(), result.Value);
        }
    }

#endif
}
