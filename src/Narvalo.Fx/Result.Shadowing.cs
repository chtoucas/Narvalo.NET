// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public partial class Result<T, TError>
    {
        public abstract Result<TResult, TError> Replace<TResult>(TResult value);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select", Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        public abstract Result<TResult, TError> Select<TResult>(Func<T, TResult> selector);

        public abstract Result<TResult, TError> ReplaceBy<TResult>(Result<TResult, TError> other);

        public abstract Result<IEnumerable<T>, TError> Repeat(int count);

        private partial class Success_
        {
            public override Result<TResult, TError> Select<TResult>(Func<T, TResult> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return Result.Of<TResult, TError>(selector.Invoke(Value));
            }

            public override Result<TResult, TError> ReplaceBy<TResult>(Result<TResult, TError> other)
                => other;

            public override Result<IEnumerable<T>, TError> Repeat(int count)
                => Result.Of<IEnumerable<T>, TError>(Enumerable.Repeat(Value, count));

            public override Result<TResult, TError> Replace<TResult>(TResult value)
                => Result.Of<TResult, TError>(value);
        }

        private partial class Error_
        {
            public override Result<TResult, TError> Select<TResult>(Func<T, TResult> selector)
                => Result.FromError<TResult, TError>(Error);

            public override Result<TResult, TError> ReplaceBy<TResult>(Result<TResult, TError> other)
                => Result.FromError<TResult, TError>(Error);

            public override Result<IEnumerable<T>, TError> Repeat(int count)
                => Result.FromError<IEnumerable<T>, TError>(Error);

            public override Result<TResult, TError> Replace<TResult>(TResult value)
                => Result.FromError<TResult, TError>(Error);
        }
    }

    public static partial class Result
    {
        internal static Result<IEnumerable<TSource>, TError> CollectImpl<TSource, TError>(
            this IEnumerable<Result<TSource, TError>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Result.Of<IEnumerable<TSource>, TError>(CollectAnyIterator(@this));
        }
    }
}
