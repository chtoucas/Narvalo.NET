// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.Internal;

    public static partial class Guard
    {
        #region Generic range guards.

        // Methods below only accept generics of value type. Adding reference types would make
        // each method check too many things at a time (null-checks).

        /// <summary>
        /// Validates that the specified argument is greater than a minimum value.
        /// This method does not enforce any Code Contracts specification.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than
        /// or equal to the minimum value.</exception>
        public static void GreaterThan<T>(T value, T minValue, string parameterName)
            where T : struct, IComparable<T>
        {
            Expect.NotNullOrEmpty(parameterName);

            if (value.CompareTo(minValue) <= 0)
            {
                throw Failure.NotGreaterThan(value, minValue, parameterName);
            }
        }

        /// <summary>
        /// Validates that the specified argument is greater than or equal to a minimum value.
        /// This method does not enforce any Code Contracts specification.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="minValue">The minimum value (inclusive).</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than
        /// the minimum value.</exception>
        public static void GreaterThanOrEqualTo<T>(T value, T minValue, string parameterName)
            where T : struct, IComparable<T>
        {
            Expect.NotNullOrEmpty(parameterName);

            if (value.CompareTo(minValue) < 0)
            {
                throw Failure.NotGreaterThanOrEqualTo(value, minValue, parameterName);
            }
        }

        /// <summary>
        /// Validates that the specified argument is less than a maximum value.
        /// This method does not enforce any Code Contracts specification.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than
        /// or equal the maximum value.</exception>
        public static void LessThan<T>(T value, T maxValue, string parameterName)
            where T : struct, IComparable<T>
        {
            Expect.NotNullOrEmpty(parameterName);

            if (value.CompareTo(maxValue) >= 0)
            {
                throw Failure.NotLessThan(value, maxValue, parameterName);
            }
        }

        /// <summary>
        /// Validates that the specified argument is less or equal to a maximum value.
        /// This method does not enforce any Code Contracts specification.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="maxValue">The maximum value (inclusive).</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than
        /// the maximum value.</exception>
        public static void LessThanOrEqualTo<T>(T value, T maxValue, string parameterName)
            where T : struct, IComparable<T>
        {
            Expect.NotNullOrEmpty(parameterName);

            if (value.CompareTo(maxValue) > 0)
            {
                throw Failure.NotLessThanOrEqualTo(value, maxValue, parameterName);
            }
        }

        #endregion
    }
}