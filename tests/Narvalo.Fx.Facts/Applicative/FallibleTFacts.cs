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
            => Assert.Throws<ArgumentNullException>(() => Fallible<int>.FromError(null));

        [Fact]
        public static void FromError_ReturnsError()
        {
            var result = Fallible<int>.FromError(Edi);

            Assert.True(result.IsError);
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

        #region ValueOrElse()

        [Fact]
        public static void ValueOrElse_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => MySuccess.ValueOrElse((Func<My.SimpleObj>)null));
            Assert.Throws<ArgumentNullException>(() => MyError.ValueOrElse((Func<My.SimpleObj>)null));
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
            => Assert.Throws<My.SimpleException>(() => MyError.ValueOrThrow());

        #endregion

        #region Bind()

        [Fact]
        public static void Bind_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => MySuccess.Bind<string>(null));
            Assert.Throws<ArgumentNullException>(() => MyError.Bind<string>(null));
        }

        #endregion

        #region Contains()

        [Fact]
        public static void Contains_Guards()
        {
            var value = new My.SimpleObj();

            Assert.Throws<ArgumentNullException>(() => MySuccess.Contains(value, null));
            Assert.Throws<ArgumentNullException>(() => MyError.Contains(value, null));
        }

        #endregion

        #region Match()

        [Fact]
        public static void Match_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => MySuccess.Match(null, _ => new My.SimpleObj()));
            Assert.Throws<ArgumentNullException>(() => MySuccess.Match(val => val, null));
            Assert.Throws<ArgumentNullException>(() => MyError.Match(null, _ => new My.SimpleObj()));
            Assert.Throws<ArgumentNullException>(() => MyError.Match(val => val, null));
        }

        #endregion

        #region Equals()

        [Fact]
        public static void Equals_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => MySuccess.Equals(MySuccess, null));
            Assert.Throws<ArgumentNullException>(() => MySuccess.Equals(MyError, null));
            Assert.Throws<ArgumentNullException>(() => MyError.Equals(MyError, null));
            Assert.Throws<ArgumentNullException>(() => MyError.Equals(MySuccess, null));
        }

        #endregion

        #region GetHashCode()

        [Fact]
        public static void GetHashCode_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => MySuccess.GetHashCode(null));
            Assert.Throws<ArgumentNullException>(() => MyError.GetHashCode(null));
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

        private static ExceptionDispatchInfo CreateExceptionDispatchInfo()
        {
            try
            {
                throw new My.SimpleException("My message");
            }
            catch (Exception ex)
            {
                return ExceptionDispatchInfo.Capture(ex);
            }
        }
    }
}
