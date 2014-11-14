// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class PresenterBindingException : MvpException
    {
        public PresenterBindingException() { }

        public PresenterBindingException(string message) : base(message) { }

        public PresenterBindingException(string message, Exception innerException)
            : base(message, innerException) { }

        protected PresenterBindingException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
