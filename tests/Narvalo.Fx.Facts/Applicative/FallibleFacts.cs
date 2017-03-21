// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Runtime.ExceptionServices;

    using Xunit;

    public static partial class FallibleFacts
    {
        #region Unit

        [Fact]
        public static void Unit_IsSuccess() => Assert.True(Fallible.Unit.IsSuccess);

        #endregion

        #region Ok

        [Fact]
        public static void Ok_IsSuccess() => Assert.True(Fallible.Ok.IsSuccess);

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
        public static void FromError_ThrowArgumentNullException_ForNullEmpty()
            => Assert.Throws<ArgumentNullException>(() => Fallible.FromError(null));

        [Fact]
        public static void FromError_ReturnsError()
        {
            var result = Fallible.FromError(Error);

            Assert.True(result.IsError);
        }

        #endregion
    }

    public static partial class FallibleFacts
    {
        private static readonly Lazy<ExceptionDispatchInfo> s_Error
            = new Lazy<ExceptionDispatchInfo>(CreateExceptionDispatchInfo);

        internal static ExceptionDispatchInfo Error => s_Error.Value;

        private static ExceptionDispatchInfo CreateExceptionDispatchInfo()
        {
            try
            {
                throw new Exception("My message");
            }
            catch (Exception ex)
            {
                return ExceptionDispatchInfo.Capture(ex);
            }
        }
    }
}
