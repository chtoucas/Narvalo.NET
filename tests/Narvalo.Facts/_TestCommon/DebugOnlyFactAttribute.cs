// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
#if DEBUG
    public sealed class DebugOnlyFactAttribute : Xunit.FactAttribute { }
#else
    public sealed class DebugOnlyFactAttribute : System.Attribute { }
#endif
}
