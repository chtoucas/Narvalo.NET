// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;

#if DEBUG

    // To test debug asserts, we could also override the global listeners in the App.config:
    //<configuration>
    //  <system.diagnostics>
    //    <trace>
    //      <listeners>
    //        <clear />
    //        <!-- Throw when Debug.Assert fails. -->
    //        <add name = "Default" type= "Narvalo.UnitTestTraceListener, Narvalo.Facts" />
    //      </ listeners >
    //    </ trace >
    //  </ system.diagnostics>
    //</ configuration>
    // This has two drawbacks. First, it affects all tests. Second, it does not work reliably
    // with OpenCover (at least for me).
    public sealed class DebugAssertFixture : IDisposable
    {
        private TraceListener[] _listeners;

        public DebugAssertFixture()
        {
            _listeners = new TraceListener[Trace.Listeners.Count];
            Debug.Listeners.CopyTo(_listeners, 0);
            Debug.Listeners.Clear();
            Debug.Listeners.Add(new UnitTestTraceListener());
        }

        public void Dispose()
        {
            Debug.Listeners.Clear();
            Debug.Listeners.AddRange(_listeners);
        }
    }

#else

    public sealed class DebugAssertFixture { }

#endif
}
