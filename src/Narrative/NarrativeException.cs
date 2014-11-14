// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narrative
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NarrativeException : Exception
    {
        public NarrativeException() : base() { ; }

        public NarrativeException(string message) : base(message) { ; }

        public NarrativeException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected NarrativeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
