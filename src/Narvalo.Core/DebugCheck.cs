// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Diagnostics;

    [DebuggerStepThrough]
    public static class DebugCheck
    {
        [Conditional("DEBUG")]
        public static void NotNull<T>(T value)
        {
            Debug.Assert(value != null, SR.DebugCheck_IsNull);
        }

        [Conditional("DEBUG")]
        public static void NotNullOrEmpty(string value)
        {
            NotNull(value);
            Debug.Assert(value.Length != 0, SR.DebugCheck_IsEmpty);
        }
    }
}
