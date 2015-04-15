// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Properties;

    /// <summary>
    /// Provides helper methods to perform argument validation.
    /// </summary>
    /// <remarks>
    /// <para>The methods will be recognized as parameter validators by FxCop.</para>
    /// <para>The methods MUST appear after all Code Contracts.</para>
    /// <para>If a condition does not hold, an <see cref="ArgumentException"/> is thrown.</para>
    /// <para>This class exists because CCCheck does not seem to be able to comprehend a precondition 
    /// used in conjunction with <see cref="IComparable{T}"/>; otherwise these helpers would have 
    /// been alongside the others in <see cref="Require"/>.</para>
    /// <para>This class only accept generics of value type. Adding reference types would make
    /// each method check too many things at a time (null-checks).</para>
    /// </remarks>
    /// <seealso cref="Require"/>
    [DebuggerStepThrough]
    public static class Enforce
    {
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
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void InRange<T>(T value, T minInclusive, T maxInclusive, string parameterName)
            where T : struct, IComparable<T>
        {
            if (minInclusive.CompareTo(maxInclusive) > 0)
            {
                throw new ArgumentException(
                    Format.Resource(
                        Strings_Cerbere.Enforce_InvalidRange_Format,
                        minInclusive,
                        maxInclusive),
                    "minInclusive");
            }

            if (value.CompareTo(minInclusive) < 0 || value.CompareTo(maxInclusive) > 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotInRange_Format, parameterName, minInclusive, maxInclusive));
            }
        }

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
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void GreaterThan<T>(T value, T minValue, string parameterName)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(minValue) <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotGreaterThan_Format, parameterName, minValue));
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
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void GreaterThanOrEqualTo<T>(T value, T minValue, string parameterName)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(minValue) < 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotGreaterThanOrEqualTo_Format, parameterName, minValue));
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
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void LessThan<T>(T value, T maxValue, string parameterName)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(maxValue) >= 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotLessThan_Format, parameterName, maxValue));
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than
        /// the maximum value.</exception>
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void LessThanOrEqualTo<T>(T value, T maxValue, string parameterName)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(maxValue) > 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotLessThanOrEqualTo_Format, parameterName, maxValue));
            }
        }
    }
}