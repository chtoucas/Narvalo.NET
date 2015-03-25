// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1064:ExceptionsShouldBePublic",
        Justification = "[Intentionally] This is an unrecoverable exception, thrown when a supposedly impossible situation happened.")]
    [SuppressMessage("Gendarme.Rules.Exceptions", "MissingExceptionConstructorsRule",
        Justification = "[Intentionally] This exception can not be initialized outside this assembly.")]
    internal sealed class FailedPromiseException : Exception
    {
        public FailedPromiseException(string message) : base(message) { }
    }
}
