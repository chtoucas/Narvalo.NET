// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
#if DEBUG
    using System;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ReleaseOnlyFactAttribute : System.Attribute { }
#else
    public sealed class ReleaseOnlyFactAttribute : Xunit.FactAttribute { }
#endif
}
