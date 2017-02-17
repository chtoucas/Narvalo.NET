// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class Capturing
    {
        private static readonly ITryCaptureExceptionInfo s_CaptureAnyException = new TryCaptureAnyException();

        public static ITryCaptureExceptionInfo AnyException() => s_CaptureAnyException;

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static ITryCaptureExceptionInfo Only<TException>()
            where TException : Exception
            => new TryCapture<TException>();

        [Obsolete("BAD IDEA")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static ITryCaptureExceptionInfo Only<T1Exception, T2Exception>()
            where T1Exception : Exception
            where T2Exception : Exception
            => new TryCapture<T1Exception, T2Exception>();

        [Obsolete("BAD IDEA")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static ITryCaptureExceptionInfo Only<T1Exception, T2Exception, T3Exception>()
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            => new TryCapture<T1Exception, T2Exception, T3Exception>();

        [Obsolete("BAD IDEA")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static ITryCaptureExceptionInfo Only<T1Exception, T2Exception, T3Exception, T4Exception>()
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
            => new TryCapture<T1Exception, T2Exception, T3Exception, T4Exception>();
    }
}
