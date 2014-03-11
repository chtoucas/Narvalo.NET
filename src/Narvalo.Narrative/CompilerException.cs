// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CompilerException : Exception
    {
        public CompilerException() : base() { ; }

        public CompilerException(string message) : base(message) { ; }

        public CompilerException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected CompilerException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }

        public int Column { get; set; }

        public int Line { get; set; }
    }
}
