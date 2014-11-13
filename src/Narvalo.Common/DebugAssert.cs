// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;

    public static class DebugAssert
    {
        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void IsEnum(Type type)
        {
            DebugCheck.NotNull(type);
            Debug.Assert(type.IsEnum, type.FullName, SR.DebugAssert_IsNotEnum);
        }

        [DebuggerStepThrough]
        [Conditional("DEBUG")]
        public static void IsValueType(Type type)
        {
            DebugCheck.NotNull(type);
            Debug.Assert(type.IsValueType, type.FullName, SR.DebugAssert_IsNotValueType);
        }
    }
}
