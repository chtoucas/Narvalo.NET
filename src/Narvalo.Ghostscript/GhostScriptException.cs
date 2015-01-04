// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class GhostScriptException : Exception
    {
        public GhostScriptException() : base() { }

        public GhostScriptException(string message) : base(message) { ; }

        public GhostScriptException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected GhostScriptException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
