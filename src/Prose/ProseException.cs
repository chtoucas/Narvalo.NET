// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ProseException : Exception
    {
        public ProseException() : base() { }

        public ProseException(string message) : base(message) { }

        public ProseException(string message, Exception innerException)
            : base(message, innerException) { }

        protected ProseException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
