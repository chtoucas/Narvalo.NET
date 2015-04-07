// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.TestCommon
{
    using Xunit;

    public static class EnvironmentFacts
    {
#if NO_INTERNALS_VISIBLE_TO // White-box testing.

        [Fact(Skip = "White-box tests are disabled for this configuration.")]
        public static void InternalsAreHidden() { }

#endif
    }
}
