// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<TResult> MapAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Output<TResult>> funM)
        {
            Require.Object(@this);
            Require.NotNull(funM, "funM");

            throw new NotImplementedException();

            //return from _ in @this
            //       let m = funM.Invoke(_)
            //       where m.IsSuccess
            //       select m.Value;
        }
    }

    public static partial class EnumerableOutputExtensions
    {
        // Custom version of CollectCore.
        internal static Output<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Output<TSource>> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Output<IEnumerable<TSource>>>() != null);

            throw new NotImplementedException();

            //var list = new List<TSource>();

            //foreach (var m in @this)
            //{
            //    // REVIEW: Is this the correct behaviour when m is null?
            //    if (m == null)
            //    {
            //        continue;
            //    }

            //    if (m.IsFailure)
            //    {
            //        return Output.Failure<IEnumerable<TSource>>(m.ExceptionInfo);
            //    }

            //    list.Add(m.Value);
            //}

            //return Output.Success(list.AsEnumerable());
        }
    }
}
