// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    using Xunit;

    using static global::My;

    // Tests for Outcome<T>.
    public static partial class OutcomeTFacts
    {
        #region Unit

        [Fact]
        public static void Unit_IsSuccess() => Assert.True(Outcome.Unit.IsSuccess);

        #endregion

        #region Of()

        [Fact]
        public static void Of_ReturnsSuccess()
        {
            var result = Outcome.Of(1);

            Assert.True(result.IsSuccess);
        }

        #endregion

        #region FromError()

        [Fact]
        public static void FromError_Guards()
        {
            Assert.Throws<ArgumentNullException>("error", () => Outcome<int>.FromError(null));
            Assert.Throws<ArgumentException>("error", () => Outcome<int>.FromError(String.Empty));
        }

        [Fact]
        public static void FromError_ReturnsError()
        {
            var result = Outcome<int>.FromError("error");

            Assert.True(result.IsError);
        }

        #endregion

        #region ValueOrDefault()

        [Fact]
        public static void ValueOrDefault_ReturnsValue_IfSuccess()
        {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            Assert.Same(exp, ok.ValueOrDefault());
        }

        [Fact]
        public static void ValueOrDefault_ReturnsDefault_IfError()
        {
            var err = Outcome<Obj>.FromError("error");

            Assert.Same(default(Obj), err.ValueOrDefault());
        }

        #endregion

        #region ValueOrNone()

        [Fact]
        public static void ValueOrNone_ReturnsSome_IfSuccess()
        {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            var maybe = ok.ValueOrNone();

            Assert.True(maybe.IsSome);
#if !NO_INTERNALS_VISIBLE_TO
            Assert.Same(exp, maybe.Value);
#endif
        }

        [Fact]
        public static void ValueOrNone_ReturnsNone_IfError()
        {
            var err = Outcome<Obj>.FromError("error");

            Assert.True(err.ValueOrNone().IsNone);
        }

        #endregion

        #region ValueOrElse()

        [Fact]
        public static void ValueOrElse_Guards()
        {
            Assert.Throws<ArgumentNullException>("valueFactory", () => MySuccess.ValueOrElse((Func<Obj>)null));
            Assert.Throws<ArgumentNullException>("valueFactory", () => MyError.ValueOrElse((Func<Obj>)null));
        }

        [Fact]
        public static void ValueOrElse_ReturnsValue_IfSuccess()
        {
            var exp = new Obj();
            var ok = Outcome.Of(exp);
            var other = new Obj("other");

            Assert.Same(exp, ok.ValueOrElse(other));
            Assert.Same(exp, ok.ValueOrElse(() => other));
        }

        [Fact]
        public static void ValueOrElse_ReturnsOther_IfError()
        {
            var err = Outcome<Obj>.FromError("error");
            var exp = new Obj();

            Assert.Same(exp, err.ValueOrElse(exp));
            Assert.Same(exp, err.ValueOrElse(() => exp));
        }

        #endregion

        #region ValueOrThrow()

        [Fact]
        public static void ValueOrThrow_Guards()
        {
            Assert.Throws<ArgumentNullException>("exceptionFactory", () => MySuccess.ValueOrThrow(null));
            Assert.Throws<ArgumentNullException>("exceptionFactory", () => MyError.ValueOrThrow(null));
        }

        [Fact]
        public static void ValueOrThrow_ReturnsValue_IfSuccess()
        {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            Assert.Same(exp, ok.ValueOrThrow());
            Assert.Equal(exp, ok.ValueOrThrow(error => new SimpleException(error)));
        }

        [Fact]
        public static void ValueOrThrow_Throws_IfError()
        {
            var err = Outcome<Obj>.FromError("error");

            Action act = () => err.ValueOrThrow();
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public static void ValueOrThrow_ThrowsCustomException_IfError()
        {
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

        [Fact]
        public static void ToValue_Throws_IfError()
        {
            var message = "error";
            var err = Outcome<Obj>.FromError(message);

            Action act = () => err.ToValue();
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<InvalidCastException>(ex);
        }

        [Fact]
        public static void ToValue_ReturnsValue_IfSuccess()
        {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            Assert.Same(exp, ok.ToValue());
        }

        #endregion

        #region ToMaybe()

        [Fact]
        public static void ToMaybe_ReturnsSome_IfSuccess()
        {
            var exp = new Obj();
            var ok = Outcome.Of(exp);

            var maybe = ok.ToMaybe();

            Assert.True(maybe.IsSome);
#if !NO_INTERNALS_VISIBLE_TO
            Assert.Same(exp, maybe.Value);
#endif
        }

        [Fact]
        public static void ToMaybe_ReturnsNone_IfError()
        {
            var err = Outcome<Obj>.FromError("error");

            Assert.True(err.ToMaybe().IsNone);
        }

        #endregion

        #region Bind()

        [Fact]
        public static void Bind_Guards()
        {
            Assert.Throws<ArgumentNullException>("binder", () => MySuccess.Bind<string>(null));
            Assert.Throws<ArgumentNullException>("binder", () => MyError.Bind<string>(null));
        }

        [Fact]
        public static void Bind_ReturnsError_IfError()
        {
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

        [Fact]
        public static void Bind_ReturnsSuccess_IfSuccess()
        {
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

        [Fact]
        public static void Flatten_ReturnsError_IfError()
        {
            var err = Outcome<Outcome<Obj>>.FromError("error");

            Assert.True(err.IsError);
        }

        [Fact]
        public static void Flatten_ReturnsSuccess_IfSuccess()
        {
            var ok = Outcome.Of(Outcome.Of(new Obj()));

            Assert.True(ok.IsSuccess);
        }

        #endregion

        #region Contains()

        [Fact]
        public static void Contains_Guards()
        {
            var value = new Obj();

            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.Contains(value, null));
            Assert.Throws<ArgumentNullException>("comparer", () => MyError.Contains(value, null));
        }

        #endregion

        #region Match()

        [Fact]
        public static void Match_Guards()
        {
            Assert.Throws<ArgumentNullException>("caseSuccess", () => MySuccess.Match(null, _ => new Obj()));
            Assert.Throws<ArgumentNullException>("caseError", () => MySuccess.Match(val => val, null));

            Assert.Throws<ArgumentNullException>("caseSuccess", () => MyError.Match(null, _ => new Obj()));
            Assert.Throws<ArgumentNullException>("caseError", () => MyError.Match(val => val, null));
        }

        #endregion

        #region Equals()

        [Fact]
        public static void Equals_Guards()
        {
            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.Equals(MySuccess, null));
            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.Equals(MyError, null));

            Assert.Throws<ArgumentNullException>("comparer", () => MyError.Equals(MyError, null));
            Assert.Throws<ArgumentNullException>("comparer", () => MyError.Equals(MySuccess, null));
        }

        #endregion

        #region GetHashCode()

        [Fact]
        public static void GetHashCode_Guards()
        {
            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.GetHashCode(null));
            Assert.Throws<ArgumentNullException>("comparer", () => MyError.GetHashCode(null));
        }

        #endregion
    }

    public static partial class OutcomeTFacts
    {
        private static Outcome<Obj> MySuccess => Outcome.Of(new Obj());
        private static Outcome<Obj> MyError => Outcome<Obj>.FromError("error");
    }
}
