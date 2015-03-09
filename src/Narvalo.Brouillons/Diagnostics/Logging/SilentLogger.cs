// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Logging
{
    using System;

    public class SilentLogger : LoggerBase
    {
        public SilentLogger(string name)
            : base(name) { }

        public SilentLogger(string name, LoggerLevel level)
            : base(name, level) { }

        public override bool IsLevelEnabled(LoggerLevel level)
        {
            return false;
        }

        protected override void LogCore(LoggerLevel level, string message, Exception exception)
        {
            ;
        }
    }
}
