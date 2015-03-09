// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Logging
{
    using System;

    public abstract class LoggerBase : ILogger
    {
        readonly LoggerLevel _level;
        readonly string _name;

        protected LoggerBase(string name)
            : this(name, LoggerLevel.None) { }

        protected LoggerBase(string name, LoggerLevel level)
        {
            _name = name;
            _level = level;
        }

        protected abstract void LogCore(LoggerLevel level, string message, Exception exception);

        #region ILogger

        public LoggerLevel Level { get { return _level; } }
        public string Name { get { return _name; } }

        public virtual bool IsLevelEnabled(LoggerLevel level)
        {
            return Level.HasFlag(level);
        }

        public void Log(LoggerLevel level, string message)
        {
            if (!IsLevelEnabled(level)) { return; }

            LogCore(level, message, null /* exception */);
        }

        public void Log(LoggerLevel level, Exception exception)
        {
            if (!IsLevelEnabled(level)) { return; }

            Require.NotNull(exception, "exception");

            LogCore(level, exception.Message, exception);
        }

        public void Log(LoggerLevel level, Func<string> messageFactory)
        {
            if (!IsLevelEnabled(level)) { return; }

            Require.NotNull(messageFactory, "messageFactory");

            LogCore(level, messageFactory(), null /* exception */);
        }

        public void Log(LoggerLevel level, string message, Exception exception)
        {
            if (!IsLevelEnabled(level)) { return; }

            LogCore(level, message, exception);
        }

        public void Log(LoggerLevel level, Func<string> messageFactory, Exception exception)
        {
            if (!IsLevelEnabled(level)) { return; }

            Require.NotNull(messageFactory, "messageFactory");

            LogCore(level, messageFactory(), exception);
        }

        public void Log(
            LoggerLevel level,
            IFormatProvider formatProvider,
            string format,
            params object[] args)
        {
            if (!IsLevelEnabled(level)) { return; }

            LogCore(level, String.Format(formatProvider, format, args), null /* exception */);
        }

        public void Log(
            LoggerLevel level,
            Exception exception,
            IFormatProvider formatProvider,
            string format,
            params object[] args)
        {
            if (!IsLevelEnabled(level)) { return; }

            LogCore(level, String.Format(formatProvider, format, args), exception);
        }

        #endregion
    }
}
