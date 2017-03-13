// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;

    using Narvalo.Properties;

    /// <summary>
    /// Provides helper methods to specify preconditions on a method.
    /// </summary>
    /// <remarks>
    /// <para>If a condition does not hold, an exception is thrown.</para>
    /// <para>The methods will be recognized by FxCop as guards against null value.</para>
    /// </remarks>
    [DebuggerStepThrough]
    public static partial class Require
    {
        public static void State(bool testCondition)
        {
            if (!testCondition)
            {
                throw new InvalidOperationException();
            }
        }

        public static void State(bool testCondition, string message)
        {
            if (!testCondition)
            {
                throw new InvalidOperationException(message);
            }
        }
    }

    // Methods to perform argument validation.
    public static partial class Require
    {
        public static void True(bool testCondition, string parameterName)
        {
            if (!testCondition)
            {
                throw new ArgumentException(Strings_Core.Argument_TestFailed, parameterName);
            }
        }

        public static void True(bool testCondition, string parameterName, string message)
        {
            if (!testCondition)
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        public static void Range(bool rangeCondition, string parameterName)
        {
            if (!rangeCondition)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void Range(bool rangeCondition, string parameterName, string message)
        {
            if (!rangeCondition)
            {
                throw new ArgumentOutOfRangeException(parameterName, message);
            }
        }

        /// <summary>
        /// Validates that the specified argument is not null.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// null.</exception>
        /// <seealso cref="Require.NotNullUnconstrained{T}(T, String)"/>
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Validates that the specified argument is not null.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// null.</exception>
        /// <seealso cref="Require.NotNull{T}(T, String)"/>
        public static void NotNullUnconstrained<T>([ValidatedNotNull]T value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Validates that the specified argument is not null or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty.</exception>
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Length == 0)
            {
                throw new ArgumentException(Strings_Core.Argument_EmptyString, parameterName);
            }
        }

        /// <summary>
        /// Validates that the specified argument is not null or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty.</exception>
        public static void NotNullOrWhiteSpace([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (IsWhiteSpace(value))
            {
                throw new ArgumentException(Strings_Core.Argument_WhiteSpaceString, parameterName);
            }
        }

        /// <summary>
        /// Returns a value indicating whether the specified value only consists of white-space
        /// characters.
        /// </summary>
        /// <remarks>This method returns false if <paramref name="value"/> is empty.</remarks>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the input only consists of white-space characters;
        /// otherwise false.</returns>
        private static bool IsWhiteSpace(string value)
        {
            Debug.Assert(value != null);

            if (value.Length == 0) { return false; }

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