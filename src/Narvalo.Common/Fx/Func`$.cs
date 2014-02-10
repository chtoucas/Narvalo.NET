// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class FuncExtensions
    {
        //// Compose (Nullable)

        public static Func<TSource, TResult?> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, TMiddle?> @this,
            Func<TMiddle, TResult?> kun)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.Object(@this);

            return _ => @this.Invoke(_).Bind(kun);
        }

        //// Compose (Maybe)

        public static Func<TSource, Maybe<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, Maybe<TMiddle>> @this,
            Func<TMiddle, Maybe<TResult>> kun)
        {
            Require.Object(@this);

            return _ => @this.Invoke(_).Bind(kun);
        }

        //// Compose (Output)

        public static Func<TSource, Output<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, Output<TMiddle>> @this,
            Func<TMiddle, Output<TResult>> kun)
        {
            Require.Object(@this);

            return _ => @this.Invoke(_).Bind(kun);
        }
    }
}
