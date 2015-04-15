// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Reflection;

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
        [SuppressMessage("Gendarme.Rules.Portability", "MonoCompatibilityReviewRule",
            Justification = "[Intentionally] Method marked as MonoTODO.")]
        public static bool IsFlagsEnum(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return type != null && typeInfo.IsEnum && typeInfo.GetCustomAttribute<FlagsAttribute>(inherit: false) != null;
        }

        /// <summary>
        /// Returns a value indicating whether the specified value consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns><see langword="true"/> if the specified value consists only of white-space characters; 
        /// otherwise <see langword="false"/>.</returns>
        [Pure]
        public static bool IsWhiteSpace(string value)
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
