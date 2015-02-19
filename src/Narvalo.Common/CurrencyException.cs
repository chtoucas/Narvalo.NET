// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CurrencyException : Exception
    {
        public CurrencyException() { }

        public CurrencyException(string message)
            : base(message) { }

        public CurrencyException(string message, Exception innerException) :
            base(message, innerException) { }

        protected CurrencyException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
