// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using static System.Diagnostics.Contracts.Contract;

    /// <summary>
    /// Provides helper methods pertaining to <see cref="Func{T}"/> and <see cref="Action{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the delegates that this class encapsulates.</typeparam>
    public static class Stubs<T>
    {
        private static readonly Func<T> s_AlwaysDefault = () => default(T);

        private static readonly Func<T, bool> s_AlwaysFalse = _ => false;

        private static readonly Func<T, bool> s_AlwaysTrue = _ => true;

        private static readonly Func<T, T> s_Identity = _ => _;

        private static readonly Action<T> s_Ignore = _ => { };

        /// <summary>
        /// Gets the function that always returns the default value.
        /// </summary>
        /// <value>The function that always returns the default value.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Func<T> AlwaysDefault
        {
            get
            {
                Ensures(Result<Func<T>>() != null);

                return s_AlwaysDefault;
            }
        }

        /// <summary>
        /// Gets the predicate that always evaluates to false.
        /// </summary>
        /// <value>The predicate that always evaluates to false.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Func<T, bool> AlwaysFalse
        {
            get
            {
                Ensures(Result<Func<T, bool>>() != null);

                return s_AlwaysFalse;
            }
        }

        /// <summary>
        /// Gets the predicate that always evaluates to true.
        /// </summary>
        /// <value>The predicate that always evaluates to true.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Func<T, bool> AlwaysTrue
        {
            get
            {
                Ensures(Result<Func<T, bool>>() != null);

                return s_AlwaysTrue;
            }
        }

        /// <summary>
        /// Gets the identity function.
        /// </summary>
        /// <value>The identity function.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Func<T, T> Identity
        {
            get
            {
                Ensures(Result<Func<T, T>>() != null);

                return s_Identity;
            }
        }

        /// <summary>
        /// Gets the action that will do nothing to its input.
        /// </summary>
        /// <value>The empty action.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Action<T> Ignore
        {
            get
            {
                Ensures(Result<Action<T>>() != null);

                return s_Ignore;
            }
        }
    }
}
