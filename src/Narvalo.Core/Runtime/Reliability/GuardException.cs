// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;

    public class GuardException : Exception
    {
        public GuardException() : base() { }

        public GuardException(string message) : base(message) { }

        public GuardException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
