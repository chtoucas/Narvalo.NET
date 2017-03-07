// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using Xunit;

    public static partial class ResultFacts
    {
        #region Ok

        [Fact]
        public static void Ok_IsSuccess() => Assert.True(Fallible.Ok.IsSuccess);

        #endregion
    }
}
