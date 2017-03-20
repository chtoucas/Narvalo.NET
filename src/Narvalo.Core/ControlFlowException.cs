// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.Properties;

    /// <summary>
    /// The exception that is thrown when the control flow path reached a section
    /// of the code that should have been unreachable under any circumstances,
    /// like a missing case in a switch.
    /// </summary>
    public class ControlFlowException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlFlowException"/> class.
        /// </summary>
        public ControlFlowException() : base(Strings.ControlFlowException_DefaultMessage) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlFlowException"/> class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">A string that describes the error.</param>
        public ControlFlowException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlFlowException"/> class with
        /// a specified error message and a reference to the inner exception that is
        /// the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.
        /// If the <paramref name="innerException"/> parameter is not a null reference, the current
        /// exception is raised in a catch block that handles the inner exception.</param>
        public ControlFlowException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
