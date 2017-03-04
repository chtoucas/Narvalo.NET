// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using Xunit;

    public static partial class ResultFacts
    {
        #region Success

        [Fact]
        public static void Success_IsSuccess() => Assert.True(Result.Success.IsSuccess);

        #endregion
    }
}
