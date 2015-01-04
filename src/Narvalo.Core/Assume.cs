// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [DebuggerStepThrough]
    public static class Assume
    {
        /// <summary>
        /// When dealing with external dependencies, CCCheck can not infer
        /// that the result of a method is not null. When we know for sure that
        /// the result is not null, this extension method is a useful alias 
        /// to inform CCCheck not to worry of <c>null</c> values here.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        [DebuggerHidden]
        public static T AssumeNotNull<T>(this T @this) where T : class
        {
            Contract.Ensures(Contract.Result<T>() == @this);
            Contract.Ensures(Contract.Result<T>() != null);
            Contract.Assume(@this != null);

            return @this;
        }

        /// <summary>
        /// According to its documentation, CCCheck only assumes and asserts
        /// object invariants for the "this" object. This method allows
        /// to state explicitly the object invariance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        [Conditional("CONTRACTS_FULL")]
        public static void Invariant<T>(T obj) where T : class { }
    }
}
