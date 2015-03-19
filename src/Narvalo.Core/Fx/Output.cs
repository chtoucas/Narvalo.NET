// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Output{T}"/>.
    /// </summary>
    public static partial class Output
    {
        public static Output<T> Failure<T>(ExceptionDispatchInfo exceptionInfo)
        {
            Contract.Requires(exceptionInfo != null);
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            return Output<T>.Failure.η(exceptionInfo);
        }
    }
}
