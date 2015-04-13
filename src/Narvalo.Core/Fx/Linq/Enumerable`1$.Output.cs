// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/> that depend on the <see cref="Output{T}"/> class.
    /// </summary>>
    public static partial class EnumerableOutputExtensions
    {
        public static IEnumerable<TResult> MapAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Output<TResult>> funM)
        {
            Require.Object(@this);
            Require.NotNull(funM, "funM");

            // We could use a LINQ query expression but it then won't be possible 
            // to add the Code Contracts workarounds.
            //  return from _ in @this
            //      let m = funM.Invoke(_)
            //      where m.IsSuccess
            //      select m.Value;
            return @this
                .Select(_ => funM.Invoke(_)).AssumeNotNull()
                .Where(_ => _.IsSuccess)
                .Select(_ => _.ToValue()).AssumeNotNull();
        }
    }

    /// <content>
    /// Provides overrides for a bunch of auto-generated (extension) methods (see Output.g.cs).
    /// </content>
    public static partial class EnumerableOutputExtensions
    {
        // Custom version of CollectCore.
        internal static Output<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Output<TSource>> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Output<IEnumerable<TSource>>>() != null);

            var list = new List<TSource>();

            foreach (var m in @this)
            {
                // REVIEW: Is this the correct behaviour when m is null?
                if (m == null)
                {
                    continue;
                }

                if (!m.IsSuccess)
                {
                    return Output.Failure<IEnumerable<TSource>>(m.ToExceptionDispatchInfo());
                }

                list.Add(m.ToValue());
            }

            return Output.Success(list.AsEnumerable());
        }
    }
}
