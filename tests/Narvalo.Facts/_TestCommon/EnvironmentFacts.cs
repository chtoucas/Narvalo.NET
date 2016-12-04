// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    public static partial class EnvironmentFacts
    {
#if DEBUG // Release must be understood as follows: the DEBUG symbol is not defined.
        [Xunit.Fact(Skip = "Release-only tests are disabled for this configuration.")]
        public static void ReleaseOnlyTestsAreHidden() { }
#else
        [Xunit.Fact(Skip = "Debug-only test are disabled for this configuration.")]
        public static void DebugOnlyTestsAreHidden() { }
#endif
    }
}
