﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;

    [Serializable]
    public class ThrottleException : GuardException
    {
        public ThrottleException() : base() { }

        public ThrottleException(string message) : base(message) { }

        public ThrottleException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
