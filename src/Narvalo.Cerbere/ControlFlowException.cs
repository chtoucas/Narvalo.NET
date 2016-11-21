// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    public class ControlFlowException : Exception
    {
        public ControlFlowException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlFlowException"/> class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ControlFlowException(string message) : base(message) { }

        public ControlFlowException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
