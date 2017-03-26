// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Runtime.ExceptionServices;

    using Xunit;

    using static global::My;

    // Tests for Fallible<T>.
    public static partial class FallibleTFacts {
        internal sealed class factAttribute : FactAttribute_ {
            public factAttribute(string message) : base(nameof(Fallible), message) { }
        }

        #region Unit

        [fact("")]
        public static void Unit_IsSuccess() => Assert.True(Fallible.Unit.IsSuccess);

        #endregion

        #region Of()

        [fact("")]
        public static void Of_ReturnsSuccess() {
            var result = Fallible.Of(1);

            Assert.True(result.IsSuccess);
        }

        #endregion

        #region FromError()

        [fact("")]
        public static void FromError_Guards()
            => Assert.Throws<ArgumentNullException>("error", () => Fallible<int>.FromError(null));

        [fact("")]
        public static void FromError_ReturnsError() {
            var result = Fallible<int>.FromError(Edi);

            Assert.True(result.IsError);
        }

        #endregion

        #region ThrowIfError()

        [fact("")]
        public static void ThrowIfError_DoesNotThrow_IfSuccess() {
            var ok = Fallible.Of(new Obj());

            ok.ThrowIfError();
        }

        [fact("")]
        public static void ThrowIfError_Throws_IfError() {
            var err = Fallible<Obj>.FromError(Edi);

            Action act = () => err.ThrowIfError();
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<SimpleException>(ex);
            Assert.Equal(s_EdiMessage, ex.Message);
        }

        #endregion

        #region ValueOrDefault()

        [fact("")]
        public static void ValueOrDefault_ReturnsValue_IfSuccess() {
            var exp = new Obj();
            var ok = Fallible.Of(exp);

            Assert.Same(exp, ok.ValueOrDefault());
        }

        [fact("")]
        public static void ValueOrDefault_ReturnsDefault_IfError() {
            var err = Fallible<Obj>.FromError(Edi);

            Assert.Same(default(Obj), err.ValueOrDefault());
        }

        #endregion

        #region ValueOrNone()

        [fact("")]
        public static void ValueOrNone_ReturnsSome_IfSuccess() {
            var ok = Fallible.Of(new Obj());

            Assert.True(ok.ValueOrNone().IsSome);
        }

        [fact("")]
        public static void ValueOrNone_ReturnsNone_IfError() {
            var err = Fallible<Obj>.FromError(Edi);

            Assert.True(err.ValueOrNone().IsNone);
        }

        #endregion

        #region ValueOrElse()

        [fact("")]
        public static void ValueOrElse_Guards() {
            Assert.Throws<ArgumentNullException>("valueFactory", () => MySuccess.ValueOrElse((Func<Obj>)null));
            Assert.Throws<ArgumentNullException>("valueFactory", () => MyError.ValueOrElse((Func<Obj>)null));
        }

        [fact("")]
        public static void ValueOrElse_ReturnsValue_IfSuccess() {
            var exp = new Obj();
            var ok = Fallible.Of(exp);
            var other = new Obj("other");

            Assert.Same(exp, ok.ValueOrElse(other));
            Assert.Same(exp, ok.ValueOrElse(() => other));
        }

        [fact("")]
        public static void ValueOrElse_ReturnsOther_IfError() {
            var err = Fallible<Obj>.FromError(Edi);
            var exp = new Obj();

            Assert.Same(exp, err.ValueOrElse(exp));
            Assert.Same(exp, err.ValueOrElse(() => exp));
        }

        #endregion

        #region ValueOrThrow()

        [fact("")]
        public static void ValueOrThrow_ReturnsValue_IfSuccess() {
            var exp = new Obj();
            var ok = Fallible.Of(exp);

            Assert.Same(exp, ok.ValueOrThrow());
        }

        [fact("")]
        public static void ValueOrThrow_Throws_IfError() {
            var err = Fallible<Obj>.FromError(Edi);

            Action act = () => err.ValueOrThrow();
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<SimpleException>(ex);
            Assert.Equal(s_EdiMessage, ex.Message);
        }

        #endregion

        #region Bind()

        [fact("")]
        public static void Bind_Guards() {
            Assert.Throws<ArgumentNullException>("binder", () => MySuccess.Bind<string>(null));
            Assert.Throws<ArgumentNullException>("binder", () => MyError.Bind<string>(null));
        }

        [fact("")]
        public static void Bind_ReturnsError_IfError() {
            // Arrange
            var err = Fallible<Obj>.FromError(Edi);
            Func<Obj, Fallible<string>> binder = _ => Fallible.Of(_.Value);

            // Act
            var me = err.Bind(binder);

            // Assert
            Assert.True(me.IsError);
#if !NO_INTERNALS_VISIBLE_TO
            Assert.Same(Edi, me.Error);
#endif
        }

        [fact("")]
        public static void Bind_ReturnsSuccess_IfSuccess() {
            // Arrange
            var exp = new Obj("My Value");
            var ok = Fallible.Of(exp);
            Func<Obj, Fallible<string>> binder = _ => Fallible.Of(_.Value);

            // Act
            var me = ok.Bind(binder);

            // Assert
            Assert.True(me.IsSuccess);
        }

        #endregion

        #region Flatten()

        [fact("")]
        public static void Flatten_ReturnsError_IfError() {
            var err = Fallible<Fallible<Obj>>.FromError(Edi);

            Assert.True(err.IsError);
        }

        [fact("")]
        public static void Flatten_ReturnsSuccess_IfSuccess() {
            var ok = Fallible.Of(Fallible.Of(new Obj()));

            Assert.True(ok.IsSuccess);
        }

        #endregion

        #region Contains()

        [fact("")]
        public static void Contains_Guards() {
            var value = new Obj();

            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.Contains(value, null));
            Assert.Throws<ArgumentNullException>("comparer", () => MyError.Contains(value, null));
        }

        #endregion

        #region Match()

        [fact("")]
        public static void Match_Guards() {
            Assert.Throws<ArgumentNullException>("caseSuccess", () => MySuccess.Match(null, _ => new Obj()));
            Assert.Throws<ArgumentNullException>("caseError", () => MySuccess.Match(val => val, null));

            Assert.Throws<ArgumentNullException>("caseSuccess", () => MyError.Match(null, _ => new Obj()));
            Assert.Throws<ArgumentNullException>("caseError", () => MyError.Match(val => val, null));
        }

        #endregion

        #region Equals()

        [fact("")]
        public static void Equals_Guards() {
            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.Equals(MySuccess, null));
            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.Equals(MyError, null));

            Assert.Throws<ArgumentNullException>("comparer", () => MyError.Equals(MyError, null));
            Assert.Throws<ArgumentNullException>("comparer", () => MyError.Equals(MySuccess, null));
        }

        #endregion

        #region GetHashCode()

        [fact("")]
        public static void GetHashCode_Guards() {
            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.GetHashCode(null));
            Assert.Throws<ArgumentNullException>("comparer", () => MyError.GetHashCode(null));
        }

        #endregion
    }

    public static partial class FallibleTFacts {
        private static readonly Lazy<ExceptionDispatchInfo> s_Edi
            = new Lazy<ExceptionDispatchInfo>(CreateExceptionDispatchInfo);

        private static ExceptionDispatchInfo Edi => s_Edi.Value;
        private static Fallible<Obj> MySuccess => Fallible.Of(new Obj());
        private static Fallible<Obj> MyError => Fallible<Obj>.FromError(Edi);

        private static readonly string s_EdiMessage = "My message";

        private static ExceptionDispatchInfo CreateExceptionDispatchInfo() {
            try {
                throw new SimpleException(s_EdiMessage);
            } catch (Exception ex) {
                return ExceptionDispatchInfo.Capture(ex);
            }
        }
    }
}
