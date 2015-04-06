// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.TestCommon
{
    using System;

    using Xunit;

    public sealed class DebugOnlyAssert
    {
        public T Throws<T>(Action testCode) where T : Exception
        {
#if DEBUG
            return Assert.Throws<T>(testCode);
#else
            testCode();
            return default(T);
#endif
        }

        public T ThrowsAny<T>(Action testCode) where T : Exception
        {
#if DEBUG
            return Assert.ThrowsAny<T>(testCode);
#else
            testCode();
            return default(T);
#endif
        }
    }
}
