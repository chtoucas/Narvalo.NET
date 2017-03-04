// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    using Narvalo.Applicative;

    // Specialized version of IEither<Unit, TError>.
    internal interface IResult<TError>
    {
        // Result<Unit, TError>.Bind().
        Result<TResult, TError> Then<TResult>(Func<Result<TResult, TError>> func);

        // Result<Unit, TError>.Select().
        Result<TResult, TError> Then<TResult>(Func<TResult> func);

        // Result<Unit, TError>.Then().
        Result<TResult, TError> Then<TResult>(Result<TResult, TError> other);

        // Result<Unit, TError>.ReplaceBy().
        Result<TResult, TError> Then<TResult>(TResult result);

        TResult Match<TResult>(Func<TResult> caseSuccess, Func<TError, TResult> caseError);

        void Do(Action onSuccess, Action<TError> onError);

        void OnSuccess(Action action);

        void OnError(Action<TError> action);
    }
}
