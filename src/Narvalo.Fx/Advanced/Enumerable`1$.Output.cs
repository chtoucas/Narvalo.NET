// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Advanced
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/> that depend on the <see cref="Output{T}"/> class.
    /// </summary>>
    [SuppressMessage("Gendarme.Rules.Smells", "AvoidSpeculativeGeneralityRule",
        Justification = "[Intentionally] Delegation is an unavoidable annoyance of fluent interfaces on delegates.")]
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
}
