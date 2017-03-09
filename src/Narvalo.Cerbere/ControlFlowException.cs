// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.Properties;

    public class ControlFlowException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlFlowException"/> class.
        /// </summary>
        public ControlFlowException() : base(Strings_Cerbere.ControlFlowException_DefaultMessage) { }

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
