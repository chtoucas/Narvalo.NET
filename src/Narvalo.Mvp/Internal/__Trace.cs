// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    internal static class __Trace
    {
        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Info(object source, string format, params string[] messages)
        {
            Info(source.GetType(), format, messages);
        }

        [DebuggerStepThrough]
        [Conditional("TRACE")]
        public static void Info(Type sourceType, string format, params string[] messages)
        {
            Trace.WriteLine("[" + sourceType.Name + "] " 
                + String.Format(CultureInfo.InvariantCulture, format, messages));
        }
    }
}
