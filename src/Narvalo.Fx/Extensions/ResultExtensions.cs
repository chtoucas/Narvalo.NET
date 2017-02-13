// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Extensions
{
    using System;

    public static class ResultExtensions
    {
        #region Applicative

        public static Result<TResult, TError> Gather<TSource, TResult, TError>(
            this Result<TSource, TError> @this,
            Result<Func<TSource, TResult>, TError> applicative)
        {
            return applicative.Apply(@this);
        }

        public static Result<Tuple<TSource, TOther>, TError> Merge<TSource, TOther, TError>(
            this Result<TSource, TError> @this,
            Result<TOther, TError> other)
        {
            return @this.Zip(other, Tuple.Create);
        }

        #endregion
    }
}
