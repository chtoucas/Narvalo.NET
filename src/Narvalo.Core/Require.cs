// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;

    using Narvalo.Properties;

    /// <summary>
    /// Provides helper methods to specify preconditions on a method.
    /// </summary>
    /// <remarks>Guards against null values will be recognized by FxCop.</remarks>
    [DebuggerStepThrough]
    public static partial class Require
    {
        /// <summary>
        /// Validates a condition on the object's current state.
        /// </summary>
        /// <param name="testCondition">The conditional expression to evaluate.</param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="testCondition"/>
        /// is false.</exception>
        public static void State(bool testCondition)
        {
            if (!testCondition)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Validates a condition on the object's current state.
        /// </summary>
        /// <param name="testCondition">The conditional expression to evaluate.</param>
        /// <param name="message">The name of the parameter.</param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="testCondition"/>
        /// is false.</exception>
        public static void State(bool testCondition, string message)
        {
            if (!testCondition)
            {
                throw new InvalidOperationException(message);
            }
        }
    }

    // Methods to perform parameter validation.
    public static partial class Require
    {
        /// <summary>
        /// Validates a condition on a parameter.
        /// </summary>
        /// <param name="testCondition">The conditional expression to evaluate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="testCondition"/>
        /// is false.</exception>
        public static void True(bool testCondition, string parameterName)
        {
            if (!testCondition)
            {
                throw new ArgumentException(Strings.Argument_UnmetPrecondition, parameterName);
            }
        }

        /// <summary>
        /// Validates a condition on a parameter.
        /// </summary>
        /// <param name="testCondition">The conditional expression to evaluate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">A string that describes the error when
        /// <paramref name="testCondition"/> is false.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="testCondition"/>
        /// is false.</exception>
        public static void True(bool testCondition, string parameterName, string message)
        {
            if (!testCondition)
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        /// <summary>
        /// Validates a range condition on a parameter.
        /// </summary>
        /// <param name="rangeCondition">The range condition to evaluate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rangeCondition"/>
        /// is false.</exception>
        public static void Range(bool rangeCondition, string parameterName)
        {
            if (!rangeCondition)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        /// <summary>
        /// Validates a range condition on a parameter.
        /// </summary>
        /// <param name="rangeCondition">The range condition to evaluate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">A string that describes the error when
        /// <paramref name="rangeCondition"/> is false.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rangeCondition"/>
        /// is false.</exception>
        public static void Range(bool rangeCondition, string parameterName, string message)
        {
            if (!rangeCondition)
            {
                throw new ArgumentOutOfRangeException(parameterName, message);
            }
        }

        /// <summary>
        /// Validates that the specified parameter is not null.
        /// </summary>
        /// <remarks>For a method without a reference constraint on <paramref name="value"/>,
        /// see <see cref="NotNullUnconstrained{T}(T, String)"/>.</remarks>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The parameter to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// null.</exception>
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Validates that the specified parameter is not null.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The parameter to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// null.</exception>
        public static void NotNullUnconstrained<T>([ValidatedNotNull]T value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Validates that the specified parameter is not null or empty.
        /// </summary>
        /// <param name="value">The parameter to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty.</exception>
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Length == 0)
            {
                throw new ArgumentException(Strings.Argument_EmptyString, parameterName);
            }
        }

        /// <summary>
        /// Validates that the specified parameter is not null or empty, and does not
        /// consist only of white-space characters.
        /// </summary>
        /// <param name="value">The parameter to validate.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty,
        /// or consists only of white-space characters.</exception>
        public static void NotNullOrWhiteSpace([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (IsEmptyOrWhiteSpace(value))
            {
                throw new ArgumentException(Strings.Argument_WhiteSpaceString, parameterName);
            }
        }

        /// <summary>
        /// Returns a value indicating whether the specified value only consists of white-space
        /// characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the input is empty or consists only of white-space characters;
        /// otherwise false.</returns>
        private static bool IsEmptyOrWhiteSpace(string value)
        {
            Debug.Assert(value != null);

            if (value.Length == 0) { return true; }

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