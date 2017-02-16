// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class Capturing
    {
        private static readonly IExceptionCatcher s_ExceptionCatcher = new Internal.ExceptionCatcher();

        public static IExceptionCatcher AnyException() => s_ExceptionCatcher;

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IExceptionCatcher Only<TException>()
            where TException : Exception
            => new Internal.ExceptionCatcher<TException>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IExceptionCatcher Only<T1Exception, T2Exception>()
            where T1Exception : Exception
            where T2Exception : Exception
            => new Internal.ExceptionCatcher<T1Exception, T2Exception>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IExceptionCatcher Only<T1Exception, T2Exception, T3Exception>()
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            => new Internal.ExceptionCatcher<T1Exception, T2Exception, T3Exception>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IExceptionCatcher Only<T1Exception, T2Exception, T3Exception, T4Exception>()
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
            => new Internal.ExceptionCatcher<T1Exception, T2Exception, T3Exception, T4Exception>();
    }
}
