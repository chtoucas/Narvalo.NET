// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The exception that is thrown to signal a programming (fatal) error.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1064:ExceptionsShouldBePublic",
        Justification = "[Intentionally] This is an unrecoverable exception, thrown when a supposedly impossible situation happened.")]
    internal sealed class ProgrammingException : Exception
    {
        public ProgrammingException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgrammingException"/> class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ProgrammingException(string message) : base(message) { }
    }
}
