// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    internal static class Tracer
    {
        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Info(object source, string format, params string[] messages)
        {
            Trace_(TraceLevel.Info, source, format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Info(Type sourceType, string format, params string[] messages)
        {
            Trace_(TraceLevel.Info, sourceType, format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Info(object source, string message)
        {
            Trace_(TraceLevel.Info, source, message);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Info(Type sourceType, string message)
        {
            Trace_(TraceLevel.Info, sourceType, message);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Warning(object source, string format, params string[] messages)
        {
            Trace_(TraceLevel.Warning, source, format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Warning(Type sourceType, string format, params string[] messages)
        {
            Trace_(TraceLevel.Warning, sourceType, format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Warning(object source, string message)
        {
            Trace_(TraceLevel.Warning, source, message);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Warning(Type sourceType, string message)
        {
            Trace_(TraceLevel.Warning, sourceType, message);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Error(object source, string format, params string[] messages)
        {
            Trace_(TraceLevel.Error, source, format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Error(Type sourceType, string format, params string[] messages)
        {
            Trace_(TraceLevel.Error, sourceType, format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Error(object source, string message)
        {
            Trace_(TraceLevel.Error, source, message);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Error(Type sourceType, string message)
        {
            Trace_(TraceLevel.Error, sourceType, message);
        }

        private static void Trace_(TraceLevel level, object source, string format, params string[] messages)
        {
            Trace_(level, source.GetType(), format, messages);
        }

        private static void Trace_(TraceLevel level, Type sourceType, string format, params string[] messages)
        {
            Trace_(level, sourceType, String.Format(CultureInfo.InvariantCulture, format, messages));
        }

        private static void Trace_(TraceLevel level, object source, string message)
        {
            Trace_(level, source.GetType(), message);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Private method only.")]
        private static void Trace_(TraceLevel level, Type sourceType, string message)
        {
            var msg = String.Format(
                CultureInfo.InvariantCulture,
                "[{0}] {1}",
                sourceType.Name,
                message);

            switch (level)
            {
                case TraceLevel.Error:
                    Trace.TraceError(msg);
                    break;

                case TraceLevel.Warning:
                    Trace.TraceWarning(msg);
                    break;

                case TraceLevel.Info:
                case TraceLevel.Verbose:
                default:
                    Trace.TraceInformation(msg);
                    break;
            }
        }
    }
}
