// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace DocuMaker
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DocuMakerException : Exception
    {
        public DocuMakerException() : base() { }

        public DocuMakerException(string message) : base(message) { }

        public DocuMakerException(string message, Exception innerException)
            : base(message, innerException) { }

        protected DocuMakerException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
