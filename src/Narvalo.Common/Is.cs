// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Internal;

    public static class Is
    {
        /// <summary>
        /// Returns a value indicating whether the specified type parameter is an enumeration.
        /// </summary>
        /// <typeparam name="T">The type to test.</typeparam>
        /// <returns><c>true</c> if the specified type parameter is an enumeration; 
        /// otherwise <c>false</c>.</returns>
        public static bool Enum<T>() where T : struct
        {
            return typeof(T).IsEnum;
        }

        /// <summary>
        /// Returns a value indicating whether the specified type parameter is a flags enumeration.
        /// </summary>
        /// <typeparam name="T">The type to test.</typeparam>
        /// <returns><c>true</c> if the specified type parameter is a flags enumeration; 
        /// otherwise <c>false</c>.</returns>
        public static bool FlagsEnum<T>() where T : struct
        {
            var type = typeof(T);

            return type.IsEnum && type.HasFlagsAttribute();
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="type"/> is a flags enumeration.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <returns><c>true</c> if the specified <paramref name="type"/> is a flags enumeration; 
        /// otherwise <c>false</c>.</returns>
        [Pure]
        public static bool FlagsEnum(Type type)
        {
            return type != null && type.IsEnum && type.HasFlagsAttribute();
        }

        /// <summary>
        /// Returns a value indicating whether the specified type parameter is a value type.
        /// </summary>
        /// <typeparam name="T">The type to test.</typeparam>
        /// <returns><c>true</c> if the specified type parameter is a value type; 
        /// otherwise <c>false</c>.</returns>
        public static bool ValueType<T>()
        {
            return typeof(T).IsValueType;
        }

        /// <summary>
        /// Returns a value indicating whether the specified value consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns><c>true</c> if the specified value consists only of white-space characters; 
        /// otherwise <c>false</c>.</returns>
        [Pure]
        public static bool WhiteSpace(string value)
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
