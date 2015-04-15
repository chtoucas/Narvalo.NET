// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides helper methods pertaining to <see cref="Func{T1, T2}"/> instances.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the delegates that this class encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the delegates that this class encapsulates.</typeparam>
    public static class Stubs<T1, T2>
    {
        private static readonly Func<T1, T2> s_AlwaysDefault = _ => default(T2);

        /// <summary>
        /// Gets the function that always evaluates to the default value.
        /// </summary>
        /// <value>The function that always evaluates to the default value.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Func<T1, T2> AlwaysDefault
        {
            get
            {
                Contract.Ensures(Contract.Result<Func<T1, T2>>() != null);

                return s_AlwaysDefault;
            }
        }
    }
}
