// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides helper methods to help the Code Contracts tools recognize that 
    /// certain conditions are met.
    /// </summary>
    public static class Assume
    {
        /// <summary>
        /// Instructs code analysis tools to assume that the specified object is not null,
        /// even if it cannot be statically proven not to be <see langword="null"/>.
        /// </summary>
        /// <remarks>
        /// <para>When dealing with external dependencies, CCCheck can not infer
        /// that the result of a method is not null. When we know for sure that
        /// the result is not null, this extension method is a useful alias
        /// to inform CCCheck not to worry of <see langword="null"/> values here.</para>
        /// <para>We can not use a conditional attribute here since the method has 
        /// a return type. Inlining the method should remove any performance concern.</para>
        /// </remarks>
        /// <typeparam name="T">The underlying type of the object.</typeparam>
        /// <param name="this">The input object.</param>
        /// <returns>The untouched input.</returns>
        [Obsolete]
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T AssumeNotNull<T>(this T @this) where T : class
        {
            Contract.Ensures(Contract.Result<T>() == @this);
            Contract.Ensures(Contract.Result<T>() != null);
            Contract.Assume(@this != null);

            return @this;
        }
    }
}
