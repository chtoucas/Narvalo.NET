// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.ExceptionServices;

    using Xunit;

    using static global::My;

    using Assert = Narvalo.AssertExtended;

    // Tests for Fallible<T>.
    public static partial class FallibleTFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Fallible), description) { }
        }

        [t("default(Fallible<T>) is OK.")]
        public static void Default1() {
            var result = default(Fallible<int>);

            Assert.True(result.IsSuccess);
        }

        [t("Unit is OK.")]
        public static void Unit1() {
            Assert.True(Fallible.Unit.IsSuccess);
            Assert.False(Fallible.Unit.IsError);
        }

        [t("Unit is default(Fallible<Unit>).")]
        public static void Unit2() {
            Assert.Equal(default(Fallible<Unit>), Fallible.Unit);
        }

        [t("Of() returns OK.")]
        public static void Of1() {
            var result = Fallible.Of(1);
            Assert.True(result.IsSuccess);
        }

        [t("FromError() guards.")]
        public static void FromError0()
            => Assert.Throws<ArgumentNullException>("error", () => Fallible<int>.FromError(null));

        [t("FromError() returns NOK.")]
        public static void FromError1() {
            var result = Fallible<int>.FromError(Error);
            Assert.True(result.IsError);
        }

        [t("Deconstruct() if OK.")]
        public static void Deconstruct1() {
            var exp = new Obj();
            var ok = Fallible.Of(exp);
            var (succeed, value, err) = ok;
            Assert.True(succeed);
            Assert.Null(err);
            Assert.Same(exp, value);
        }

        [t("Deconstruct() if NOK.")]
        public static void Deconstruct2() {
            var nok = Fallible<Obj>.FromError(Error);
            var (succeed, value, err) = nok;
            Assert.False(succeed);
            Assert.Same(Error, err);
            Assert.Null(value);
        }

        [t("ThrowIfError() does not throw if OK.")]
        public static void ThrowIfError1() {
            var ok = Fallible.Of(new Obj());
            Assert.DoesNotThrow(() => ok.ThrowIfError());
        }

        [t("ThrowIfError() throws if NOK.")]
        public static void ThrowIfError2() {
            var nok = Fallible<Obj>.FromError(Error);

            Action act = () => nok.ThrowIfError();
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<SimpleException>(ex);
            Assert.Equal(ErrorMessage, ex.Message);
        }

        [t("")]
        public static void ValueOrDefault_ReturnsValue_IfSuccess() {
            var exp = new Obj();
            var ok = Fallible.Of(exp);
            Assert.Same(exp, ok.ValueOrDefault());
        }

        [t("")]
        public static void ValueOrDefault_ReturnsDefault_IfError() {
            var nok = Fallible<Obj>.FromError(Error);
            Assert.Same(default(Obj), nok.ValueOrDefault());
        }

        [t("")]
        public static void ValueOrNone_ReturnsSome_IfSuccess() {
            var ok = Fallible.Of(new Obj());
            Assert.True(ok.ValueOrNone().IsSome);
        }

        [t("")]
        public static void ValueOrNone_ReturnsNone_IfError() {
            var nok = Fallible<Obj>.FromError(Error);
            Assert.True(nok.ValueOrNone().IsNone);
        }

        [t("ValueOrElse() guards.")]
        public static void ValueOrElse0() {
            var ok = Fallible.Of(new Obj());
            Assert.Throws<ArgumentNullException>("valueFactory", () => ok.ValueOrElse((Func<Obj>)null));

            var nok = Fallible<Obj>.FromError(Error);
            Assert.Throws<ArgumentNullException>("valueFactory", () => nok.ValueOrElse((Func<Obj>)null));
        }

        [t("")]
        public static void ValueOrElse_ReturnsValue_IfSuccess() {
            var exp = new Obj();
            var ok = Fallible.Of(exp);
            var other = new Obj("other");

            Assert.Same(exp, ok.ValueOrElse(other));
            Assert.Same(exp, ok.ValueOrElse(() => other));
        }

        [t("")]
        public static void ValueOrElse_ReturnsOther_IfError() {
            var nok = Fallible<Obj>.FromError(Error);
            var exp = new Obj();

            Assert.Same(exp, nok.ValueOrElse(exp));
            Assert.Same(exp, nok.ValueOrElse(() => exp));
        }

        [t("")]
        public static void ValueOrThrow_ReturnsValue_IfSuccess() {
            var exp = new Obj();
            var ok = Fallible.Of(exp);
            Assert.Same(exp, ok.ValueOrThrow());
        }

        [t("")]
        public static void ValueOrThrow_Throws_IfError() {
            var nok = Fallible<Obj>.FromError(Error);

            Action act = () => nok.ValueOrThrow();
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<SimpleException>(ex);
            Assert.Equal(ErrorMessage, ex.Message);
        }

        [t("GetHashCode() returns the same result when called repeatedly.")]
        public static void GetHashCode1() {
            var nok = Fallible<Obj>.FromError(Error);
            Assert.Equal(nok.GetHashCode(), nok.GetHashCode());
            Assert.Equal(nok.GetHashCode(EqualityComparer<Obj>.Default), nok.GetHashCode(EqualityComparer<Obj>.Default));

            var ok1 = Fallible.Of(new Obj());
            Assert.Equal(ok1.GetHashCode(), ok1.GetHashCode());
            Assert.Equal(ok1.GetHashCode(EqualityComparer<Obj>.Default), ok1.GetHashCode(EqualityComparer<Obj>.Default));

            var ok2 = Fallible.Of(1);
            Assert.Equal(ok2.GetHashCode(), ok2.GetHashCode());
            Assert.Equal(ok2.GetHashCode(EqualityComparer<int>.Default), ok2.GetHashCode(EqualityComparer<int>.Default));
        }

        [t("GetHashCode() returns the same result for equal instances.")]
        public static void GetHashCode2() {
            var nok1 = Fallible<Obj>.FromError(Error);
            var nok2 = Fallible<Obj>.FromError(Error);

            Assert.Equal(nok1.GetHashCode(), nok2.GetHashCode());
            Assert.Equal(nok1.GetHashCode(EqualityComparer<Obj>.Default), nok2.GetHashCode(EqualityComparer<Obj>.Default));

            var ok1 = Fallible.Of(Tuple.Create("1"));
            var ok2 = Fallible.Of(Tuple.Create("1"));

            Assert.Equal(ok1.GetHashCode(), ok2.GetHashCode());
            Assert.Equal(ok1.GetHashCode(EqualityComparer<Tuple<string>>.Default), ok2.GetHashCode(EqualityComparer<Tuple<string>>.Default));
        }

        [t("GetHashCode() returns different results for non-equal instances.")]
        public static void GetHashCode3() {
            var nok1 = Fallible<int>.FromError(Error);
            var nok2 = Fallible<int>.FromError(Error1);

            Assert.NotEqual(nok1.GetHashCode(), nok2.GetHashCode());
            Assert.NotEqual(nok1.GetHashCode(EqualityComparer<int>.Default), nok2.GetHashCode(EqualityComparer<int>.Default));

            var ok1 = Fallible.Of(1);
            var ok2 = Fallible.Of(2);

            Assert.NotEqual(ok1.GetHashCode(), ok2.GetHashCode());
            Assert.NotEqual(ok1.GetHashCode(EqualityComparer<int>.Default), ok2.GetHashCode(EqualityComparer<int>.Default));

            Assert.NotEqual(ok1.GetHashCode(), nok1.GetHashCode());
            Assert.NotEqual(ok1.GetHashCode(EqualityComparer<int>.Default), nok1.GetHashCode(EqualityComparer<int>.Default));
        }

        [t("ToString() result contains a string representation of the value if OK, of the error if NOK.")]
        public static void ToString1() {
            var value = new Obj("My value");
            var ok = Fallible.Of(value);
            Assert.Contains(value.ToString(), ok.ToString(), StringComparison.OrdinalIgnoreCase);

            var nok = Fallible<Obj>.FromError(Error);
            Assert.Contains(Error.ToString(), nok.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }

    public static partial class FallibleTFacts {
        [t("ToEnumerable() result is empty if NOK.")]
        public static void ToEnumerable1() {
            var nok = Fallible<Obj>.FromError(Error);
            var seq = nok.ToEnumerable();
            Assert.Empty(seq);
        }

        [t("ToEnumerable() result is a sequence made of exactly one element if OK.")]
        public static void ToEnumerable2() {
            var obj = new Obj();
            var ok = Fallible.Of(obj);
            var seq = ok.ToEnumerable();
            Assert.Equal(Enumerable.Repeat(obj, 1), seq);
        }

        [t("GetEnumerator() does not iterate if NOK.")]
        public static void GetEnumerator1() {
            var nok = Fallible<Obj>.FromError(Error);
            var count = 0;

            foreach (var x in nok) { count++; }

            Assert.Equal(0, count);
        }

        [t("GetEnumerator() iterates only once if OK.")]
        public static void GetEnumerator2() {
            var exp = new Obj();
            var ok = Fallible.Of(exp);
            var count = 0;

            foreach (var x in ok) { count++; Assert.Same(exp, x); }

            Assert.Equal(1, count);
        }

        [t("Match() guards.")]
        public static void Match0() {
            var ok = Fallible.Of(new Obj());
            Assert.Throws<ArgumentNullException>("caseSuccess", () => ok.Match(null, _ => new Obj()));
            Assert.Throws<ArgumentNullException>("caseError", () => ok.Match(val => val, null));

            var nok = Fallible<Obj>.FromError(Error);
            Assert.Throws<ArgumentNullException>("caseSuccess", () => nok.Match(null, _ => new Obj()));
            Assert.Throws<ArgumentNullException>("caseError", () => nok.Match(val => val, null));
        }
    }

    // Tests for the monadic methods.
    public static partial class FallibleTFacts {
        [t("Bind() guards.")]
        public static void Bind0() {
            var ok = Fallible.Of(new Obj());
            Assert.Throws<ArgumentNullException>("binder", () => ok.Bind<string>(null));

            var nok = Fallible<Obj>.FromError(Error);
            Assert.Throws<ArgumentNullException>("binder", () => nok.Bind<string>(null));
        }

        [t("Bind() returns NOK if NOK.")]
        public static void Bind1() {
            var nok = Fallible<Obj>.FromError(Error);
            Func<Obj, Fallible<string>> binder = x => Fallible.Of(x.Value);

            var result = nok.Bind(binder);
            Assert.True(result.IsError);
        }

        [t("Bind() returns OK if OK.")]
        public static void Bind2() {
            var ok = Fallible.Of(new Obj("My Value"));
            Func<Obj, Fallible<string>> binder = x => Fallible.Of(x.Value);

            var result = ok.Bind(binder);
            Assert.True(result.IsSuccess);
        }

        [t("Flatten() returns NOK if NOK.")]
        public static void Flatten1() {
            var nok = Fallible<Fallible<Obj>>.FromError(Error);
            var result = nok.Flatten();
            Assert.True(result.IsError);
        }

        [t("Flatten() returns OK if OK.")]
        public static void Flatten2() {
            var ok = Fallible.Of(Fallible.Of(new Obj()));
            var result = ok.Flatten();
            Assert.True(result.IsSuccess);
        }
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class FallibleTFacts {
        [t("default(Fallible<T>) contains default(T).")]
        public static void Default2() {
            var result1 = default(Fallible<int>);
            var result2 = default(Fallible<string>);

            Assert.Equal(default(int), result1.Value);
            Assert.Equal(default(string), result2.Value);
        }

        [t("Bind() transports error if NOK.")]
        public static void Bind3() {
            var nok = Fallible<Obj>.FromError(Error);
            Func<Obj, Fallible<string>> binder = x => Fallible.Of(x.Value);

            var result = nok.Bind(binder);
            Assert.Same(Error, result.Error);
        }

        [t("Bind() applies binder if OK.")]
        public static void Bind4() {
            var value = "My Value";
            var ok = Fallible.Of(new Obj(value));
            Func<Obj, Fallible<string>> binder = x => Fallible.Of(x.Value.ToUpperInvariant());

            var result = ok.Bind(binder);
            Assert.Equal(value.ToUpperInvariant(), result.Value);
        }
    }

#endif

    public static partial class FallibleTFacts {
        private static readonly Lazy<ExceptionDispatchInfo> s_Error
            = new Lazy<ExceptionDispatchInfo>(CreateExceptionDispatchInfo);
        private static readonly Lazy<ExceptionDispatchInfo> s_Error1
            = new Lazy<ExceptionDispatchInfo>(CreateExceptionDispatchInfo);

        private static ExceptionDispatchInfo Error => s_Error.Value;
        private static ExceptionDispatchInfo Error1 => s_Error1.Value;

        private static string ErrorMessage => "My error";

        private static ExceptionDispatchInfo CreateExceptionDispatchInfo() {
            try {
                throw new SimpleException(ErrorMessage);
            } catch (Exception ex) {
                return ExceptionDispatchInfo.Capture(ex);
            }
        }
    }
}
