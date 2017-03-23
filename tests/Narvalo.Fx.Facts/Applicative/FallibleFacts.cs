// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Runtime.ExceptionServices;

    using Xunit;

    // Tests for Fallible.
    public static partial class FallibleFacts
    {
        #region Ok

        [Fact]
        public static void Ok_IsSuccess() => Assert.True(Fallible.Ok.IsSuccess);

        #endregion

        #region FromError()

        [Fact]
        public static void FromError_Guards()
            => Assert.Throws<ArgumentNullException>("error", () => Fallible.FromError(null));

        [Fact]
        public static void FromError_ReturnsError()
        {
            var result = Fallible.FromError(Error);

            Assert.True(result.IsError);
        }

        #endregion

        #region Bind()

        [Fact]
        public static void Bind_Guards()
        {
            Assert.Throws<ArgumentNullException>("binder", () => Fallible.Ok.Bind<string>(null));
            Assert.Throws<ArgumentNullException>("binder", () => Fallible.FromError(Error).Bind<string>(null));
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
