// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class Catching
    {
        private static readonly ITryCatch<Exception> s_CatchAnyException = new TryCatch<Exception>();

        public static ITryCatch<Exception> AnyException() => s_CatchAnyException;

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static ITryCatch<TException> Only<TException>()
            where TException : Exception
            => new TryCatch<TException>();
    }
}
