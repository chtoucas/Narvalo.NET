// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Runtime.ExceptionServices;

    using static System.Diagnostics.Contracts.Contract;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Outcome{T}"/>.
    /// </summary>
    public static partial class Outcome
    {
        public static Outcome<T> Failure<T>(ExceptionDispatchInfo exceptionInfo)
        {
            Expect.NotNull(exceptionInfo);
            Ensures(Result<Outcome<T>>() != null);

            return Outcome<T>.η(exceptionInfo);
        }
    }
}
