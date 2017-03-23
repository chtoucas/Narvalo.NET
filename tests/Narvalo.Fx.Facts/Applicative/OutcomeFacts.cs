// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    using Xunit;

    // Tests for Outcome.
    public static partial class OutcomeFacts
    {
        #region Ok

        [Fact]
        public static void Ok_IsSuccess() => Assert.True(Outcome.Ok.IsSuccess);

        #endregion

        #region FromError()

        [Fact]
        public static void FromError_Guards()
        {
            Assert.Throws<ArgumentNullException>("error", () => Outcome.FromError(null));
            Assert.Throws<ArgumentException>("error", () => Outcome.FromError(String.Empty));
        }

        [Fact]
        public static void FromError_ReturnsError()
        {
            var result = Outcome.FromError("error");
            Assert.True(result.IsError);
        }

        #endregion

        #region Bind()

        [Fact]
        public static void Bind_Guards()
        {
            Assert.Throws<ArgumentNullException>("binder", () => Outcome.Ok.Bind<string>(null));
            Assert.Throws<ArgumentNullException>("binder", () => Outcome.FromError("error").Bind<string>(null));
        }

        #endregion
    }
}
