// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;

    public class AggregateGuardException : GuardException
    {
        public AggregateGuardException() : base() { }

        public AggregateGuardException(string message) : base(message) { }

        public AggregateGuardException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
