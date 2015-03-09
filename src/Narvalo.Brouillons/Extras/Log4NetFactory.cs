// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Logging
{
    using System;
    using log4net;
    using Narvalo;

    public class Log4NetFactory : ILoggerFactory
    {
        #region ILoggerFactory

        public ILogger CreateLogger(Type type)
        {
            Require.NotNull(type, "type");

            return Log4NetLogger.Create(LogManager.GetLogger(type));
        }

        public ILogger CreateLogger(string name)
        {
            Require.NotNullOrEmpty(name, "name");

            return Log4NetLogger.Create(LogManager.GetLogger(name));
        }

        #endregion
    }
}
