// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class AssertFailedException : Exception
    {
        public AssertFailedException() { }

        public AssertFailedException(string message) : base(message) { }

        public AssertFailedException(string message, Exception inner)
            : base(message, inner) { }

        protected AssertFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
