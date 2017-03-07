// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Samples
{
    using System;

    using Narvalo;

    /// <remarks>
    /// <para>For the Query Expression Pattern, <see cref="Narvalo.Applicative.Nullable"/>.</para>
    /// <list type="bullet">
    /// <listheader>What's not to be found here:</listheader>
    /// <item><description><c>Pure</c> is casting: <c>(T?)value</c>.</description></item>
    /// <item><description><c>Nullable</c> does not support the <c>Flatten</c> operation;
    /// there is no <c>Nullable&lt;Nullable&lt;T&gt;&gt;</c>.</description></item>
    /// <item><description><c>Zero</c> is null.</description></item>
    /// </list>
    /// </remarks>
    public static class Nullable
    {
        public static TResult? Bind<TSource, TResult>(this TSource? @this, Func<TSource, TResult?> binder)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(binder, nameof(binder));

            return @this.HasValue ? binder(@this.Value) : null;
        }
    }
}
