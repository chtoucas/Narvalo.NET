// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MvpException : Exception
    {
        public MvpException() { }

        public MvpException(string message) : base(message) { }

        public MvpException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected MvpException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
