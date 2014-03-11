// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Serilog;

    [CLSCompliant(false)]
    public interface ILoggerProvider
    {
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ILogger GetLogger();
    }
}