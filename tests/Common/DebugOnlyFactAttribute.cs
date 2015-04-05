// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.TestCommon
{
    using System;

    using Xunit;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class DebugOnlyFactAttribute : FactAttribute
    {
        public DebugOnlyFactAttribute()
            : base()
        {
#if !DEBUG // Debug only test.
            Skip = "Debug only test.";
#endif
        }
    }
}
