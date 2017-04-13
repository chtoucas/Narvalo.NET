// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics.CodeAnalysis;

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

            return IsError ? Fallible<TResult>.FromError(Error) : Fallible.Of(selector(Value));
        }
    }
}
