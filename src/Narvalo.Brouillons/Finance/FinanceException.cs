// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class FinanceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinanceException"/>
        /// class with its message string set to a system-supplied message.
        /// </summary>
        public FinanceException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinanceException"/>
        /// class with the specified error message.
        /// </summary>
        /// <param name="message">The error message to display with this exception.</param>
        public FinanceException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinanceException"/>
        /// class with a specified error message and a reference to the inner exception
        /// that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message to display with this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. 
        /// If the innerException parameter is not a null reference, the current exception is raised 
        /// in a catch block that handles the inner exception.</param>
        public FinanceException(string message, Exception innerException) :
            base(message, innerException) { }

        protected FinanceException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
