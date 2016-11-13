// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.TestCommon
{
    using System;

#if DEBUG
    using Xunit;
#endif

    internal static class AssertExt
    {
        public static class DebugOnly
        {
            public static void Throws<T>(Action testCode) where T : Exception
            {
#if DEBUG
                Assert.Throws<T>(testCode);
#else
                testCode();
#endif
            }

            public static void ThrowsAny<T>(Action testCode) where T : Exception
            {
#if DEBUG
                Assert.ThrowsAny<T>(testCode);
#else
                testCode();
#endif
            }
        }
    }
}
