// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1064:ExceptionsShouldBePublic",
        Justification = "[Intentionally] This exception is expressly hidden on purpose. "
            + "We do not want to give the impression that it is catchable. "
            + "Such an exception is thrown when a supposedly impossible situation happened.")]
    internal sealed class FailedPromiseException : Exception
    {
        public FailedPromiseException(string message) : base(message) { }
    }
}
