// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
#if !CONTRACTS_FULL
    using System.Runtime.CompilerServices;
#endif

    [DebuggerStepThrough]
    public static class Assume
    {
#if CONTRACTS_FULL

        /// <summary>
        /// When dealing with external dependencies, CCCheck can not infer
        /// that the result of a method is not null. When we know for sure that
        /// the result is not null, this extension method is a useful alias
        /// to inform CCCheck not to worry of <see langword="null"/> values here.
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

#else

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T AssumeNotNull<T>(this T @this) where T : class
        {
            return @this;
        }

#endif

        /// <summary>
        /// According to its documentation, CCCheck only assumes and asserts
        /// object invariants for the "this" object. This method allows
        /// to state explicitly the object invariance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        [DebuggerHidden]
        [Conditional("CONTRACTS_FULL")]
        public static void Invariant<T>(T obj) where T : class { }
    }
}
