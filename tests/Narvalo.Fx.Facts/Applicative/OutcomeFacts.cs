// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    using Xunit;

    public static partial class OutcomeFacts
    {
        #region Unit

        [Fact]
        public static void Unit_IsSuccess() => Assert.True(Outcome.Unit.IsSuccess);

        #endregion

        #region Ok

        [Fact]
        public static void Ok_IsSuccess() => Assert.True(Outcome.Ok.IsSuccess);

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
        public static void FromError_ThrowArgumentNullException_ForNullString()
            => Assert.Throws<ArgumentNullException>(() => Outcome.FromError(null));

        [Fact]
        public static void FromError_ThrowArgumentException_ForEmptyString()
            => Assert.Throws<ArgumentException>(() => Outcome.FromError(String.Empty));

        [Fact]
        public static void FromError_ReturnsError()
        {
            var result = Outcome.FromError("error");
            Assert.True(result.IsError);
        }

        #endregion
    }
}
