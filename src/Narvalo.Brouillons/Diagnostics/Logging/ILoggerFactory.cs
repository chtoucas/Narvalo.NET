// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Logging
{
    using System;

    public interface ILoggerFactory 
    {
        ILogger CreateLogger(Type type);

        ILogger CreateLogger(string name);
    }
}
