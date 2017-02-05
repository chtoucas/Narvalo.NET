// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides helper methods pertaining to <see cref="Func{TSource, TResult}"/> instances.
    /// </summary>
    /// <typeparam name="TSource">The type of the first parameter of the delegates that this class encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the second parameter of the delegates that this class encapsulates.</typeparam>
    public static class Stubs<TSource, TResult>
    {
        private static readonly Func<TSource, TResult> s_AlwaysDefault = _ => default(TResult);

        /// <summary>
        /// Gets the function that always evaluates to the default value.
        /// </summary>
        /// <value>The function that always evaluates to the default value.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Func<TSource, TResult> AlwaysDefault
        {
            get
            {
                Warrant.NotNull<Func<TSource, TResult>>();

                return s_AlwaysDefault;
            }
        }
    }
}
