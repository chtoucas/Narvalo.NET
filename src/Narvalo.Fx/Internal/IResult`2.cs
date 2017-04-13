// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    // The only difference with IEither<TLeft, TRight> is in the parameter (and type) names.
    // **WARNING** If we update this interface, we should mirror the modifications in IEither<TLeft, TRight>.
    internal interface IResult<T, TError> : IContainer<T>, ISecondaryContainer<TError>
    {
        TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<TError, TResult> caseError);

        // Complements:
        // > IContainer<TLeft>.Do(Action<TLeft> action);
        // > ISecondaryContainer<TRight>.Do(Action<TRight> action);
        void Do(Action<T> onSuccess, Action<TError> onError);
    }
}
