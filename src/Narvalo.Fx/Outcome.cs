// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Outcome{T}"/>.
    /// </summary>
    public static partial class Outcome
    {
        public static Outcome<T> Failure<T>(ExceptionDispatchInfo exceptionInfo)
        {
            Contract.Requires(exceptionInfo != null);
            Contract.Ensures(Contract.Result<Outcome<T>>() != null);

            return Outcome<T>.η(exceptionInfo);
        }
    }
}
