// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Runtime.ExceptionServices;

    using Xunit;

    // Tests for Fallible<T>.
    public static partial class FallibleTFacts
    {
        #region Unit

        [Fact]
        public static void Unit_IsSuccess() => Assert.True(Fallible.Unit.IsSuccess);

        #endregion

        #region Of()

        [Fact]
        public static void Of_ReturnsSuccess()
        {
            var result = Fallible.Of(1);

            Assert.True(result.IsSuccess);
        }

        #endregion

        #region FromError()

        [Fact]
        public static void FromError_Guards()
            => Assert.Throws<ArgumentNullException>("error", () => Fallible<int>.FromError(null));

        [Fact]
        public static void FromError_ReturnsError()
        {
            var result = Fallible<int>.FromError(Edi);

            Assert.True(result.IsError);
        }

        #endregion

        #region ThrowIfError()

        [Fact]
        public static void ThrowIfError_DoesNotThrow_IfSuccess()
        {
            var ok = Fallible.Of(new My.SimpleObj());

            ok.ThrowIfError();
        }

        [Fact]
        public static void ThrowIfError_Throws_IfError()
        {
            var err = Fallible<My.SimpleObj>.FromError(Edi);

            Action act = () => err.ThrowIfError();
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<My.SimpleException>(ex);
            Assert.Equal(s_EdiMessage, ex.Message);
        }

        #endregion

        #region ValueOrDefault()

        [Fact]
        public static void ValueOrDefault_ReturnsValue_IfSuccess()
        {
            var exp = new My.SimpleObj();
            var ok = Fallible.Of(exp);

            Assert.Same(exp, ok.ValueOrDefault());
        }

        [Fact]
        public static void ValueOrDefault_ReturnsDefault_IfError()
        {
            var err = Fallible<My.SimpleObj>.FromError(Edi);

            Assert.Same(default(My.SimpleObj), err.ValueOrDefault());
        }

        #endregion

        #region ValueOrNone()

        [Fact]
        public static void ValueOrNone_ReturnsSome_IfSuccess()
        {
            var ok = Fallible.Of(new My.SimpleObj());

            Assert.True(ok.ValueOrNone().IsSome);
        }

        [Fact]
        public static void ValueOrNone_ReturnsNone_IfError()
        {
            var err = Fallible<My.SimpleObj>.FromError(Edi);

            Assert.True(err.ValueOrNone().IsNone);
        }

        #endregion

        #region ValueOrElse()

        [Fact]
        public static void ValueOrElse_Guards()
        {
            Assert.Throws<ArgumentNullException>("valueFactory", () => MySuccess.ValueOrElse((Func<My.SimpleObj>)null));
            Assert.Throws<ArgumentNullException>("valueFactory", () => MyError.ValueOrElse((Func<My.SimpleObj>)null));
        }

        [Fact]
        public static void ValueOrElse_ReturnsValue_IfSuccess()
        {
            var exp = new My.SimpleObj();
            var ok = Fallible.Of(exp);
            var other = new My.SimpleObj("other");

            Assert.Same(exp, ok.ValueOrElse(other));
            Assert.Same(exp, ok.ValueOrElse(() => other));
        }

        [Fact]
        public static void ValueOrElse_ReturnsOther_IfError()
        {
            var err = Fallible<My.SimpleObj>.FromError(Edi);
            var exp = new My.SimpleObj();

            Assert.Same(exp, err.ValueOrElse(exp));
            Assert.Same(exp, err.ValueOrElse(() => exp));
        }

        #endregion

        #region ValueOrThrow()

        [Fact]
        public static void ValueOrThrow_ReturnsValue_IfSuccess()
        {
            var exp = new My.SimpleObj();
            var ok = Fallible.Of(exp);

            Assert.Same(exp, ok.ValueOrThrow());
        }

        [Fact]
        public static void ValueOrThrow_Throws_IfError()
        {
            var err = Fallible<My.SimpleObj>.FromError(Edi);

            Action act = () => err.ValueOrThrow();
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<My.SimpleException>(ex);
            Assert.Equal(s_EdiMessage, ex.Message);
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
            var err = Fallible<My.SimpleObj>.FromError(Edi);
            Func<My.SimpleObj, Fallible<string>> binder = _ => Fallible.Of(_.Value);

            // Act
            var me = err.Bind(binder);

            // Assert
            Assert.True(me.IsError);
#if !NO_INTERNALS_VISIBLE_TO
            Assert.Same(Edi, me.Error);
#endif
        }

        [Fact]
        public static void Bind_ReturnsSuccess_IfSuccess()
        {
            // Arrange
            var exp = new My.SimpleObj("My Value");
            var ok = Fallible.Of(exp);
            Func<My.SimpleObj, Fallible<string>> binder = _ => Fallible.Of(_.Value);

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
            var err = Fallible<Fallible<My.SimpleObj>>.FromError(Edi);

            Assert.True(err.IsError);
        }

        [Fact]
        public static void Flatten_ReturnsSuccess_IfSuccess()
        {
            var ok = Fallible.Of(Fallible.Of(new My.SimpleObj()));

            Assert.True(ok.IsSuccess);
        }

        #endregion

        #region Contains()

        [Fact]
        public static void Contains_Guards()
        {
            var value = new My.SimpleObj();

            Assert.Throws<ArgumentNullException>("comparer", () => MySuccess.Contains(value, null));
            Assert.Throws<ArgumentNullException>("comparer", () => MyError.Contains(value, null));
        }

        #endregion

        #region Match()

        [Fact]
        public static void Match_Guards()
        {
            Assert.Throws<ArgumentNullException>("caseSuccess", () => MySuccess.Match(null, _ => new My.SimpleObj()));
            Assert.Throws<ArgumentNullException>("caseError", () => MySuccess.Match(val => val, null));
            Assert.Throws<ArgumentNullException>("caseSuccess", () => MyError.Match(null, _ => new My.SimpleObj()));
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

    public static partial class FallibleTFacts
    {
        private static readonly Lazy<ExceptionDispatchInfo> s_Edi
            = new Lazy<ExceptionDispatchInfo>(CreateExceptionDispatchInfo);

        private static ExceptionDispatchInfo Edi => s_Edi.Value;
        private static Fallible<My.SimpleObj> MySuccess => Fallible.Of(new My.SimpleObj());
        private static Fallible<My.SimpleObj> MyError => Fallible<My.SimpleObj>.FromError(Edi);

        private static readonly string s_EdiMessage = "My message";

        private static ExceptionDispatchInfo CreateExceptionDispatchInfo()
        {
            try
            {
                throw new My.SimpleException(s_EdiMessage);
            }
            catch (Exception ex)
            {
                return ExceptionDispatchInfo.Capture(ex);
            }
        }
    }
}
