// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;

    [Serializable]
    public class AggregateGuardException : GuardException
    {
        public AggregateGuardException(string message) : base(message) { }

        public AggregateGuardException(string message, AggregateException innerException)
            : base(message, innerException) { }
    }
}
