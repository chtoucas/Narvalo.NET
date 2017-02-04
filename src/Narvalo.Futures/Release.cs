// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;

    using Narvalo.Internal;

    [DebuggerStepThrough]
    public static class Release
    {
        [DebuggerHidden]
        public static void Assert(bool testCondition)
        {
            if (!testCondition)
            {
                Fail(new ProgrammingException());
            }
        }

        [DebuggerHidden]
        public static void Assert(bool testCondition, Exception ex)
        {
            if (!testCondition)
            {
                Fail(ex);
            }
        }

        [DebuggerHidden]
        public static void Assert(bool testCondition, string rationale)
        {
            if (!testCondition)
            {
                Fail(new ProgrammingException(rationale));
            }
        }

        private static void Fail(Exception ex)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            //else if (Debugger.Launch())
            //{
            //    throw ex;
            //}
            else
            {
                Environment.FailFast("XXX", ex);
            }
        }
    }
}