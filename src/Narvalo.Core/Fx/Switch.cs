// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public static class Switch
    {
        [SuppressMessage("Gendarme.Rules.Design.Generic", "AvoidMethodWithUnusedGenericTypeRule",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static Switch<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
        {
            Contract.Ensures(Contract.Result<Switch<TLeft, TRight>>() != null);

            return Switch<TLeft, TRight>.η(value);
        }

        [SuppressMessage("Gendarme.Rules.Design.Generic", "AvoidMethodWithUnusedGenericTypeRule",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static Switch<TLeft, TRight> Right<TLeft, TRight>(TRight value)
        {
            Contract.Ensures(Contract.Result<Switch<TLeft, TRight>>() != null);

            return Switch<TLeft, TRight>.η(value);
        }
    }
}
