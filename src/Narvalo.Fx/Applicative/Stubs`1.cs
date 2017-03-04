// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides helper methods pertaining to <see cref="Func{TSource}"/> and <see cref="Action{TSource}"/> instances.
    /// </summary>
    /// <typeparam name="TSource">The type of the parameter of the delegates that this class encapsulates.</typeparam>
    public static class Stubs<TSource>
    {
        private static readonly Func<TSource> s_AlwaysDefault = () => default(TSource);
        private static readonly Func<TSource, bool> s_AlwaysFalse = _ => false;
        private static readonly Func<TSource, bool> s_AlwaysTrue = _ => true;
        private static readonly Func<TSource, TSource> s_Identity = _ => _;
        private static readonly Action<TSource> s_Ignore = _ => { };

        /// <summary>
        /// Gets the function that always returns the default value.
        /// </summary>
        /// <value>The function that always returns the default value.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Func<TSource> AlwaysDefault => s_AlwaysDefault;

        /// <summary>
        /// Gets the predicate that always evaluates to false.
        /// </summary>
        /// <value>The predicate that always evaluates to false.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Func<TSource, bool> AlwaysFalse => s_AlwaysFalse;

        /// <summary>
        /// Gets the predicate that always evaluates to true.
        /// </summary>
        /// <value>The predicate that always evaluates to true.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Func<TSource, bool> AlwaysTrue => s_AlwaysTrue;

        /// <summary>
        /// Gets the identity function.
        /// </summary>
        /// <value>The identity function.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Func<TSource, TSource> Identity => s_Identity;

        /// <summary>
        /// Gets the action that will do nothing to its input.
        /// </summary>
        /// <value>The empty action.</value>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Action<TSource> Ignore => s_Ignore;
    }
}
