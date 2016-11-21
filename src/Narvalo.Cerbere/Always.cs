// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Diagnostics;

    using Narvalo.Internal;

    [DebuggerStepThrough]
    public static class Always
    {
        [DebuggerHidden]
        public static void True(bool testCondition)
        {
            if (!testCondition)
            {
                throw new ProgrammingException();
            }
        }

        [DebuggerHidden]
        public static void False(bool testCondition) => True(!testCondition);

        [DebuggerHidden]
        public static void True(bool testCondition, string rationale)
        {
            if (!testCondition)
            {
                throw new ProgrammingException(rationale);
            }
        }

        [DebuggerHidden]
        public static void False(bool testCondition, string rationale)
            => True(!testCondition, rationale);

        [DebuggerHidden]
        public static void Unreached()
        {
            throw new ProgrammingException();
        }

        [DebuggerHidden]
        public static void Unreached(string rationale)
        {
            throw new ProgrammingException(rationale);
        }
    }
}