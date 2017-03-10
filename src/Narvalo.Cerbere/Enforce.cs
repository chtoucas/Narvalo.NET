// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Narvalo.Properties;

    /// <summary>
    /// Provides helper methods to specify preconditions which proved to be too complicated
    /// for the Code Contracts tools.
    /// </summary>
    /// <remarks>These methods MUST appear after all preconditions using Code Contracts.
    /// Failing to do so would result in a compilation error (CC1027).</remarks>
    public static partial class Enforce
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

        public static void True(bool testCondition, string parameterName)
        {
            Expect.NotNullOrEmpty(parameterName);

            if (!testCondition)
            {
                throw new ArgumentException(Strings_Cerbere.Argument_TestFailed, parameterName);
            }
        }

        public static void True(bool testCondition, string parameterName, string message)
        {
            Expect.NotNullOrEmpty(parameterName);

            if (!testCondition)
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        public static void Range(bool rangeCondition, string parameterName)
        {
            Expect.NotNullOrEmpty(parameterName);

            if (!rangeCondition)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void Range(bool rangeCondition, string parameterName, string message)
        {
            Expect.NotNullOrEmpty(parameterName);

            if (!rangeCondition)
            {
                throw new ArgumentOutOfRangeException(parameterName, message);
            }
        }

        /// <summary>
        /// Validates that the specified argument is in a given range, range borders included.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="minInclusive">The minimum value (inclusive).</param>
        /// <param name="maxInclusive">The maximum value (inclusive).</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="minInclusive"/> is greater than
        /// or equal to <paramref name="maxInclusive"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is outside
        /// the allowable range of values.</exception>
        public static void Range<T>(T value, T minInclusive, T maxInclusive, string parameterName)
            where T : struct, IComparable<T>
        {
            // We only accept generics of value type; adding reference types would make
            // each method check too many things at a time (null-checks).
            Expect.True(minInclusive.CompareTo(maxInclusive) <= 0);
            Expect.NotNullOrEmpty(parameterName);

            if (value.CompareTo(minInclusive) < 0 || value.CompareTo(maxInclusive) > 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Current(
                        Strings_Cerbere.ArgumentOutOfRange,
                        parameterName,
                        minInclusive,
                        maxInclusive));
            }
        }

        public static void NotWhiteSpace(string value, string parameterName)
        {
            Expect.NotNullOrEmpty(parameterName);

            if (Check.IsWhiteSpace(value))
            {
                throw new ArgumentException(Strings_Cerbere.Argument_WhiteSpaceString, parameterName);
            }
        }
    }
}
