// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BindingException : MvpException
    {
        public BindingException() : base() { ; }

        public BindingException(string message) : base(message) { ; }

        public BindingException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected BindingException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
