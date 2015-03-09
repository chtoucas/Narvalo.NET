// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Logging
{
    using System;

    public interface ILogger
    {
        LoggerLevel Level { get; }
        string Name { get; }

        bool IsLevelEnabled(LoggerLevel level);

        void Log(LoggerLevel level, string message);
        void Log(LoggerLevel level, Exception exception);
        void Log(LoggerLevel level, Func<string> messageFactory);
        void Log(LoggerLevel level, string message, Exception exception);
        void Log(LoggerLevel level, Func<string> messageFactory, Exception exception);
        void Log(LoggerLevel level, IFormatProvider formatProvider, string format, params object[] args);
        void Log(LoggerLevel level, Exception exception, IFormatProvider formatProvider, string format, params object[] args);
    }
}
