// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Fx.Internal;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Func{T}"/> and <see cref="Action"/>.
    /// </summary>
    public static partial class Func
    {
        public static IExceptionCatcher CaptureAny() => new ExceptionCatcher();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IExceptionCatcher Capture<TException>()
            where TException : Exception
            => new ExceptionCatcher<TException>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IExceptionCatcher Capture<T1Exception, T2Exception>()
            where T1Exception : Exception
            where T2Exception : Exception
            => new ExceptionCatcher<T1Exception, T2Exception>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IExceptionCatcher Capture<T1Exception, T2Exception, T3Exception>()
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            => new ExceptionCatcher<T1Exception, T2Exception, T3Exception>();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IExceptionCatcher Capture<T1Exception, T2Exception, T3Exception, T4Exception>()
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
            => new ExceptionCatcher<T1Exception, T2Exception, T3Exception, T4Exception>();
    }
}
