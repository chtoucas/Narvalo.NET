// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
#if DEBUG
    public sealed class ReleaseOnlyFactAttribute : System.Attribute { }
#else
    public sealed class ReleaseOnlyFactAttribute : Xunit.FactAttribute { }
#endif
}
