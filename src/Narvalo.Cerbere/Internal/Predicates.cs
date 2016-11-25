// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    /// <summary>
    /// Provides predicate methods.
    /// </summary>
    // All members mentioned in a contract must be at least as visible as the method in which they
    // appear. Failing to do so will produce a CC1038 error.
    public static class Predicates
    {
        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="type"/> is a flags enumeration.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="type"/> is a flags enumeration;
        /// otherwise <see langword="false"/>.</returns>
        [Pure]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags",
            Justification = "[Intentionally] The rule does not apply here.")]
        public static bool IsFlagsEnum(Type type)
        {
            if (type == null)
            {
                return false;
            }

            TypeInfo typeInfo = type.GetTypeInfo();

            return typeInfo != null
                && typeInfo.IsEnum
                && typeInfo.GetCustomAttribute<FlagsAttribute>(inherit: false) != null;
        }

        /// <summary>
        /// Returns a value indicating whether the specified value consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns><see langword="true"/> if the specified value consists only of white-space characters;
        /// otherwise <see langword="false"/>.</returns>
        [Pure]
        public static bool IsEmptyOrWhiteSpace(string value)
        {
            if (value == null)
            {
                return false;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
