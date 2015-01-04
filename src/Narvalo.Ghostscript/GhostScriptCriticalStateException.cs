// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class GhostScriptCriticalStateException : Exception
    {
        public GhostScriptCriticalStateException() : base() { }

        public GhostScriptCriticalStateException(string message) : base(message) { ; }

        public GhostScriptCriticalStateException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected GhostScriptCriticalStateException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
