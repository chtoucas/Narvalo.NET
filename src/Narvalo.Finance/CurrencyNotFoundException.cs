// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    /// <summary>
    /// The exception thrown when a method is invoked which attempts to construct
    /// a currency that is not available.
    /// </summary>
    public class CurrencyNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyNotFoundException"/>
        /// class with its message string set to a system-supplied message.
        /// </summary>
        public CurrencyNotFoundException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyNotFoundException"/>
        /// class with the specified error message.
        /// </summary>
        /// <param name="message">The error message to display with this exception.</param>
        public CurrencyNotFoundException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyNotFoundException"/>
        /// class with a specified error message and a reference to the inner exception
        /// that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message to display with this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference, the current exception is raised
        /// in a catch block that handles the inner exception.</param>
        public CurrencyNotFoundException(string message, Exception innerException) :
            base(message, innerException) { }
    }
}
