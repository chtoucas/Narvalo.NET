// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using Xunit;

    public static partial class ResultFacts
    {
        #region Void

        [Fact]
        public static void Void_IsSuccess() => Assert.True(Result.Void.IsSuccess);

        #endregion
    }
}
