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
        public static void Write(string format, params string[] messages)
        {
            Trace.TraceInformation(String.Format(CultureInfo.InvariantCulture, format, messages));
        }
    }
}
