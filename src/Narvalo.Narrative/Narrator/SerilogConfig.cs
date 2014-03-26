// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Narrator
{
    using System;
    using Serilog;
    using Serilog.Events;

    public sealed class SerilogConfig
    {
        readonly LogEventLevel _level;

        [CLSCompliant(false)]
        public SerilogConfig(LogEventLevel level)
        {
            _level = level;
        }

        public void Configure()
        {
            Log.Logger = CreateLogger_(_level);
        }

        static ILogger CreateLogger_(LogEventLevel mimimumLevel)
        {
            return new LoggerConfiguration()
               .MinimumLevel.Is(mimimumLevel)
               .WriteTo.ColoredConsole()
               .CreateLogger();
        }
    }
}
