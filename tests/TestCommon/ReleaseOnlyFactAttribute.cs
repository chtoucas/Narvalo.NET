// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using Xunit;

    public sealed class ReleaseOnlyFactAttribute : FactAttribute
    {
        public ReleaseOnlyFactAttribute() : base()
        {
#if DEBUG
            Skip = "Debug symbol defined.";
#endif
        }
    }

}
