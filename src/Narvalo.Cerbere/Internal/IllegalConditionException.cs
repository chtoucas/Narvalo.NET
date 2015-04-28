// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The exception that is thrown when an illegal condition is met.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1064:ExceptionsShouldBePublic",
        Justification = "[Intentionally] This is an unrecoverable exception, thrown when a supposedly impossible situation happened.")]
    internal sealed class IllegalConditionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalConditionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public IllegalConditionException(string message) : base(message) { }
    }
}
