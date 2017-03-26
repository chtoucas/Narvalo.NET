// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;

    using Xunit;

    using static global::My;

    // Tests for Outcome<T>.
    public static partial class OutcomeTFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string message) : base(nameof(Outcome), message) { }
        }

        #region Unit

        [t("Unit is OK.")]
        public static void Unit1() {
            Assert.True(Outcome.Unit.IsSuccess);
            Assert.False(Outcome.Unit.IsError);
        }

        #endregion

        #region Of()

        [t("Of() returns OK.")]
        public static void Of1() {
            var result = Outcome.Of(1);

            Assert.True(result.IsSuccess);
            Assert.False(result.IsError);
        }

        #endregion

        #region FromError()

        [t("FromError() guards.")]
        public static void FromError0() {
            Assert.Throws<ArgumentNullException>("error", () => Outcome<int>.FromError(null));
            Assert.Throws<ArgumentException>("error", () => Outcome<int>.FromError(String.Empty));
        }

        [t("FromError() returns NOK.")]
        public static void FromError1() {
            var result = Outcome<int>.FromError("error");

            Assert.True(result.IsError);
            Assert.False(result.IsSuccess);
        }

        #endregion

        #region ValueOrDefault()

        [t("")]
        public static void ValueOrDefault_ReturnsValue_IfSuccess() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            Assert.Same(exp, ok.ValueOrDefault());
        }

        [t("")]
        public static void ValueOrDefault_ReturnsDefault_IfError() {
            var err = Outcome<Obj>.FromError("error");

            Assert.Same(default(Obj), err.ValueOrDefault());
        }

        #endregion

        #region ValueOrNone()

        [t("")]
        public static void ValueOrNone_ReturnsSome_IfSuccess() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            var maybe = ok.ValueOrNone();

            Assert.True(maybe.IsSome);
#if !NO_INTERNALS_VISIBLE_TO
            Assert.Same(exp, maybe.Value);
#endif
        }

        [t("")]
        public static void ValueOrNone_ReturnsNone_IfError() {
            var err = Outcome<Obj>.FromError("error");

            Assert.True(err.ValueOrNone().IsNone);
        }

        #endregion

        #region ValueOrElse()

        [t("")]
        public static void ValueOrElse_Guards() {
            Assert.Throws<ArgumentNullException>("valueFactory", () => MySuccess.ValueOrElse((Func<Obj>)null));
            Assert.Throws<ArgumentNullException>("valueFactory", () => MyError.ValueOrElse((Func<Obj>)null));
        }

        [t("")]
        public static void ValueOrElse_ReturnsValue_IfSuccess() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);
            var other = new Obj("other");

            Assert.Same(exp, ok.ValueOrElse(other));
            Assert.Same(exp, ok.ValueOrElse(() => other));
        }

        [t("")]
        public static void ValueOrElse_ReturnsOther_IfError() {
            var err = Outcome<Obj>.FromError("error");
            var exp = new Obj();

            Assert.Same(exp, err.ValueOrElse(exp));
            Assert.Same(exp, err.ValueOrElse(() => exp));
        }

        #endregion

        #region ValueOrThrow()

        [t("")]
        public static void ValueOrThrow_Guards() {
            Assert.Throws<ArgumentNullException>("exceptionFactory", () => MySuccess.ValueOrThrow(null));
            Assert.Throws<ArgumentNullException>("exceptionFactory", () => MyError.ValueOrThrow(null));
        }

        [t("")]
        public static void ValueOrThrow_ReturnsValue_IfSuccess() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            Assert.Same(exp, ok.ValueOrThrow());
            Assert.Equal(exp, ok.ValueOrThrow(error => new SimpleException(error)));
        }

        [t("")]
        public static void ValueOrThrow_Throws_IfError() {
            var err = Outcome<Obj>.FromError("error");

            Action act = () => err.ValueOrThrow();
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
        }

        [t("")]
        public static void ValueOrThrow_ThrowsCustomException_IfError() {
            var message = "error";
            var err = Outcome<Obj>.FromError(message);

            Action act = () => err.ValueOrThrow(error => new SimpleException(error));
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<SimpleException>(ex);
            Assert.Equal(message, ex.Message);
        }

        #endregion

        #region ToValue()

        [t("")]
        public static void ToValue_Throws_IfError() {
            var message = "error";
            var err = Outcome<Obj>.FromError(message);

            Action act = () => err.ToValue();
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<InvalidCastException>(ex);
        }

        [t("")]
        public static void ToValue_ReturnsValue_IfSuccess() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            Assert.Same(exp, ok.ToValue());
        }

        #endregion

        #region ToMaybe()

        [t("")]
        public static void ToMaybe_ReturnsSome_IfSuccess() {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            var maybe = ok.ToMaybe();

            Assert.True(maybe.IsSome);
#if !NO_INTERNALS_VISIBLE_TO
            Assert.Same(exp, maybe.Value);
#endif
        }

        [t("")]
        public static void ToMaybe_ReturnsNone_IfError() {
            var err = Outcome<Obj>.FromError("error");

            Assert.True(err.ToMaybe().IsNone);
        }

        #endregion

        #region Equals()

        [t("")]
        public static void Equals_Guards() {
            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.Equals(MySuccess, null));
            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.Equals(MyError, null));

            Assert.Throws<ArgumentNullException>("comparer", () => MyError.Equals(MyError, null));
            Assert.Throws<ArgumentNullException>("comparer", () => MyError.Equals(MySuccess, null));
        }

        #endregion

        #region GetHashCode()

        [t("")]
        public static void GetHashCode_Guards() {
            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.GetHashCode(null));
            Assert.Throws<ArgumentNullException>("comparer", () => MyError.GetHashCode(null));
        }

        #endregion
    }

    public static partial class OutcomeTFacts {
        #region Contains()

        [t("")]
        public static void Contains_Guards() {
            var value = new Obj();

            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.Contains(value, null));
            Assert.Throws<ArgumentNullException>("comparer", () => MyError.Contains(value, null));
        }

        #endregion

        #region Match()

        [t("")]
        public static void Match_Guards() {
            Assert.Throws<ArgumentNullException>("caseSuccess", () => MySuccess.Match(null, _ => new Obj()));
            Assert.Throws<ArgumentNullException>("caseError", () => MySuccess.Match(val => val, null));

            Assert.Throws<ArgumentNullException>("caseSuccess", () => MyError.Match(null, _ => new Obj()));
            Assert.Throws<ArgumentNullException>("caseError", () => MyError.Match(val => val, null));
        }

        #endregion
    }

    // Tests for the monadic methods.
    public static partial class OutcomeTFacts {

        #region Bind()

        [t("")]
        public static void Bind_Guards() {
            Assert.Throws<ArgumentNullException>("binder", () => MySuccess.Bind<string>(null));
            Assert.Throws<ArgumentNullException>("binder", () => MyError.Bind<string>(null));
        }

        [t("")]
        public static void Bind_ReturnsError_IfError() {
            // Arrange
            var exp = "error";
            var err = Outcome<Obj>.FromError(exp);
            Func<Obj, Outcome<string>> binder = _ => Outcome.Of(_.Value);

            // Act
            var me = err.Bind(binder);

            // Assert
            Assert.True(me.IsError);
#if !NO_INTERNALS_VISIBLE_TO
            Assert.Equal(exp, me.Error);
#endif
        }

        [t("")]
        public static void Bind_ReturnsSuccess_IfSuccess() {
            // Arrange
            var exp = new Obj("My Value");
            var ok = Outcome.Of(exp);
            Func<Obj, Outcome<string>> binder = _ => Outcome.Of(_.Value);

            // Act
            var me = ok.Bind(binder);

            // Assert
            Assert.True(me.IsSuccess);
        }

        #endregion

        #region Flatten()

        [t("")]
        public static void Flatten_ReturnsError_IfError() {
            var err = Outcome<Outcome<Obj>>.FromError("error");

            Assert.True(err.IsError);
        }

        [t("")]
        public static void Flatten_ReturnsSuccess_IfSuccess() {
            var ok = Outcome.Of(Outcome.Of(new Obj()));

            Assert.True(ok.IsSuccess);
        }

        #endregion

    }

    public static partial class OutcomeTFacts {
        private static Outcome<Obj> MySuccess => Outcome.Of(new Obj());
        private static Outcome<Obj> MyError => Outcome<Obj>.FromError("error");
    }
}
