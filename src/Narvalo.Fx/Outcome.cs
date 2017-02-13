// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Outcome{T}"/>.
    /// </summary>
    public static partial class Outcome
    {
        public static Outcome<T> FromError<T>(ExceptionDispatchInfo exceptionInfo)
        {
            Expect.NotNull(exceptionInfo);
            Warrant.NotNull<Outcome<T>>();

            return Outcome<T>.η(exceptionInfo);
        }
    }
}
