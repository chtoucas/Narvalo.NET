// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using Xunit;

    public sealed class DebugOnlyFactAttribute : FactAttribute
    {
        public DebugOnlyFactAttribute() : base()
        {
#if !DEBUG
            Skip = "Debug-only test.";
#endif
        }
    }
}
