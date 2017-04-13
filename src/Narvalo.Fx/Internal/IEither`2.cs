// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    /// <summary>
    /// Represents the disjoint union of <typeparamref name="TLeft"/> and <typeparamref name="TRight"/>.
    /// <para>A disjoint union is also called a discriminated union, a variant, a sum type...</para>
    /// </summary>
    /// <typeparam name="TLeft">The type of the underlying left value of the sum.</typeparam>
    /// <typeparam name="TRight">The type of the underlying right value of the sum.</typeparam>
    internal interface IEither<TLeft, TRight> : IContainer<TLeft>, ISecondaryContainer<TRight>
    {
        TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight);

        // Complements:
        // > IContainer<TLeft>.Do(Action<TLeft> action);
        // > ISecondaryContainer<TRight>.Do(Action<TRight> action);
        void Do(Action<TLeft> onLeft, Action<TRight> onRight);
    }
}
