// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static class Error
    {
        public static void Throw<TException>(Error<TException> @this) where TException : Exception
        {
            throw @this.Message;
        }
    }
}
