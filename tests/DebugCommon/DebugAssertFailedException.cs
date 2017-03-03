// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class DebugAssertFailedException : Exception
    {
        public DebugAssertFailedException() { }

        public DebugAssertFailedException(string message) : base(message) { }

        public DebugAssertFailedException(string message, Exception inner)
            : base(message, inner) { }

        private DebugAssertFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
