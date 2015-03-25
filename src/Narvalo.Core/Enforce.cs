// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Internal;

    /// <summary>
    /// Provides helper methods to perform argument validation.
    /// </summary>
    /// <remarks>
    /// <para>WARNING: These methods MUST appear after all Code Contracts.</para>
    /// <para>The methods are recognized as parameter validators by the Code Analysis tool.</para>
    /// <para>This class exists because CCCheck does not seem to be able to comprehend a precondition 
    /// used in conjunction with <see cref="IComparable{T}"/>; otherwise these helpers would have 
    /// been alongside the others in <see cref="Require"/>.</para>
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
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is outside
        /// the allowable range of values.</exception>
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void InRange<T>([ValidatedNotNull]T value, T minInclusive, T maxInclusive, string parameterName)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(minInclusive) < 0 || value.CompareTo(maxInclusive) > 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotInRangeFormat, parameterName, minInclusive, maxInclusive));
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
        public static void GreaterThan<T>([ValidatedNotNull]T value, T minValue, string parameterName)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(minValue) <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotGreaterThanFormat, parameterName, minValue));
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
        public static void GreaterThanOrEqualTo<T>([ValidatedNotNull]T value, T minValue, string parameterName)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(minValue) < 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotGreaterThanOrEqualToFormat, parameterName, minValue));
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
        public static void LessThan<T>([ValidatedNotNull]T value, T maxValue, string parameterName)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(maxValue) >= 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotLessThanFormat, parameterName, maxValue));
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
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void LessThanOrEqualTo<T>([ValidatedNotNull]T value, T maxValue, string parameterName)
            where T : struct, IComparable<T>
        {
            if (value.CompareTo(maxValue) > 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotLessThanOrEqualToFormat, parameterName, maxValue));
            }
        }
    }
}