// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;

    [DebuggerStepThrough]
    public static partial class Demand
    {
        [Conditional("DEBUG")]
        public static void State(bool testCondition) => True(testCondition);

        [Conditional("DEBUG")]
        public static void True(bool testCondition) => Debug.Assert(testCondition);

        [Conditional("DEBUG")]
        public static void Range(bool rangeCondition) => True(rangeCondition);

        [Conditional("DEBUG")]
        public static void NotNull<T>(T value) where T : class => True(value != null);

        [Conditional("DEBUG")]
        public static void NotNullUnconstrained<T>(T value) => True(value != null);

        [Conditional("DEBUG")]
        public static void NotNullOrEmpty(string value) => True(!String.IsNullOrEmpty(value));
    }
}
