// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    using Xunit;

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
            Assert.Throws<ArgumentNullException>(() => Outcome<int>.FromError(null));
            Assert.Throws<ArgumentException>(() => Outcome<int>.FromError(String.Empty));
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
            var exp = new My.SimpleObj();
            var ok = Outcome.Of(exp);

            Assert.Same(exp, ok.ValueOrDefault());
        }

        [Fact]
        public static void ValueOrDefault_ReturnsDefault_IfError()
        {
            var err = Outcome<My.SimpleObj>.FromError("error");

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
            var ok = Outcome.Of(exp);
            var other = new My.SimpleObj("other");

            Assert.Same(exp, ok.ValueOrElse(other));
            Assert.Same(exp, ok.ValueOrElse(() => other));
        }

        [Fact]
        public static void ValueOrElse_ReturnsOther_IfError()
        {
            var err = Outcome<My.SimpleObj>.FromError("error");
            var exp = new My.SimpleObj();

            Assert.Same(exp, err.ValueOrElse(exp));
            Assert.Same(exp, err.ValueOrElse(() => exp));
        }

        #endregion

        #region ValueOrThrow()

        [Fact]
        public static void ValueOrThrow_Guards()
        {
            Assert.Throws<ArgumentNullException>(() => MySuccess.ValueOrThrow(null));
            Assert.Throws<ArgumentNullException>(() => MyError.ValueOrThrow(null));
        }

        [Fact]
        public static void ValueOrThrow_ReturnsValue_IfSome()
        {
            var exp = new My.SimpleObj();
            var ok = Outcome.Of(exp);

            Assert.Same(exp, ok.ValueOrThrow());
            Assert.Equal(exp, ok.ValueOrThrow(error => new My.SimpleException(error)));
        }

        [Fact]
        public static void ValueOrThrow_Throws_IfNone()
        {
            var err = Outcome<My.SimpleObj>.FromError("error");

            Assert.Throws<InvalidOperationException>(() => err.ValueOrThrow());
            Assert.Throws<My.SimpleException>(() => err.ValueOrThrow(error => new My.SimpleException(error)));
        }

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

    public static partial class OutcomeTFacts
    {
        private static Outcome<My.SimpleObj> MySuccess => Outcome.Of(new My.SimpleObj());
        private static Outcome<My.SimpleObj> MyError => Outcome<My.SimpleObj>.FromError("error");
    }
}
