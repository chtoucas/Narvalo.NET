// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Cerbere
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    public static class ContractHelpers
    {
        /// <summary>
        /// Instructs code analysis tools to assume that the specified object is not
        /// null, even if it cannot be statically proven.
        /// IMPORTANT: See the warnings detailed in the remarks before using this class.
        /// </summary>
        /// <remarks>
        /// <para>THERE ARE AT LEAST THREE REASONS TO NOT USE THIS CLASS. First, it adds
        /// an extension method to ALL objects. Second, there is always a better of doing the
        /// same thing (use directly Contract.Assume()). Third, this adds an unecessary call.</para>
        /// <para>When dealing with external dependencies, CCCheck can not infer
        /// that the result of a method is not null. When we know for sure that
        /// the result is not null, this extension method is a useful alias
        /// to inform CCCheck not to worry of null values here.</para>
        /// <para>We can not use a conditional attribute here since the method has
        /// a return type. Inlining the method should remove any performance concern (on close
        /// inspection, I think this is not even true).</para>
        /// </remarks>
        /// <typeparam name="T">The underlying type of the object.</typeparam>
        /// <param name="this">The input object.</param>
        /// <returns>The untouched input.</returns>
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
