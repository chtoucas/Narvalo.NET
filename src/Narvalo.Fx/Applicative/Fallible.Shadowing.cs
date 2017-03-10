﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    public partial struct Fallible<T>
    {
        public Fallible<TResult> ReplaceBy<TResult>(TResult other)
            => IsSuccess ? Fallible.Of(other) : Fallible<TResult>.FromError(Error);

        public Fallible<TResult> ContinueWith<TResult>(Fallible<TResult> other)
            => IsSuccess ? other : Fallible<TResult>.FromError(Error);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select", Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of this method.")]
        public Fallible<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            if (IsError) { Fallible<TResult>.FromError(Error); }

            try
            {
                return Fallible.Of(selector(Value));
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);
                return Fallible<TResult>.FromError(edi);
            }
        }
    }

    public static partial class FallibleExtensions
    {
        internal static Fallible<IEnumerable<TSource>> CollectImpl<TSource>(
            this IEnumerable<Fallible<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Fallible.Of(CollectAnyIterator(@this));
        }
    }
}

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    public static partial class Qperators
    {
        internal static Fallible<IEnumerable<TSource>> WhereByImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Fallible<bool>> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            return Fallible.Of(WhereAnyIterator(@this, predicate));
        }
    }
}
