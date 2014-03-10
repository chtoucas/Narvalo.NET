// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.ComponentModel.Composition;
    using Serilog;
    using Serilog.Events;

    [Export("Default", typeof(ILoggerProvider))]
    public sealed class LoggerProvider : ILoggerProvider
    {
        [CLSCompliant(false)]
        public ILogger GetLogger()
        {
            return new LoggerConfiguration()
               .MinimumLevel.Is(LogEventLevel.Verbose)
               .WriteTo.ColoredConsole()
               .CreateLogger();
        }
    }
}
