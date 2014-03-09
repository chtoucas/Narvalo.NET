// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using Serilog;
    using Serilog.Events;

    [CLSCompliant(false)]
    public interface ILoggerProvider
    {
        ILogger GetLogger(LogEventLevel minimumLevel);
    }
}