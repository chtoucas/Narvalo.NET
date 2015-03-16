// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using System.Diagnostics;

    [DebuggerStepThrough]
    internal static class Tracer
    {
        [DebuggerHidden]
        [Conditional("TRACE")]
        public static void Info(object source, string message)
        {
            Trace.TraceInformation(Format_(source.GetType(), message));
        }

        [DebuggerHidden]
        [Conditional("TRACE")]
        public static void Info(Type sourceType, string message)
        {
            Trace.TraceInformation(Format_(sourceType, message));
        }

        [DebuggerHidden]
        [Conditional("TRACE")]
        public static void Warning(object source, string message)
        {
            Trace.TraceWarning(Format_(source.GetType(), message));
        }

        [DebuggerHidden]
        [Conditional("TRACE")]
        public static void Warning(Type sourceType, string message)
        {
            Trace.TraceWarning(Format_(sourceType, message));
        }

        [DebuggerHidden]
        [Conditional("TRACE")]
        public static void Error(object source, string message)
        {
            Trace.TraceError(Format_(source.GetType(), message));
        }

        [DebuggerHidden]
        [Conditional("TRACE")]
        public static void Error(Type sourceType, string message)
        {
            Trace.TraceError(Format_(sourceType, message));
        }

        private static string Format_(Type sourceType, string message)
        {
            return "[" + sourceType.Name + "] " + message;
        }
    }
}
