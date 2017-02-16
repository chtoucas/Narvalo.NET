// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class Catching
    {
        private static readonly IExceptionCatcher<Exception> s_ExceptionCatcher = new ExceptionCatcher<Exception>();

        public static IExceptionCatcher<Exception> AnyException() => s_ExceptionCatcher;

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IExceptionCatcher<TException> Only<TException>()
            where TException : Exception
            => new ExceptionCatcher<TException>();
    }
}
