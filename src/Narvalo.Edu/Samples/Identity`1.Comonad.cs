// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Samples
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial class Identity<T>
    {
        public Identity<TResult> Extend<TResult>(Func<Identity<T>, TResult> fun)
        {
            Require.NotNull(fun, "fun");

            return new Identity<TResult>(fun.Invoke(this));
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics.")]
        internal static T ε(Identity<T> monad)
        {
            Require.NotNull(monad, "monad");

            return monad._value;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics.")]
        internal static Identity<Identity<T>> δ(Identity<T> monad)
        {
            return new Identity<Identity<T>>(monad);
        }
    }
}
