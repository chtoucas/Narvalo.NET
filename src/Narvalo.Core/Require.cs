// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Narvalo.Internal;
    using Narvalo.Resources;

    /// <summary>
    /// Provides helper methods to perform argument validation compatible with Code Contracts.
    /// </summary>
    /// <remarks>
    /// <para>The methods will be recognized as parameter validators by FxCop.</para>
    /// <para>The methods will be recognized as Contract Argument Validator methods.</para>
    /// <para>If a condition does not hold, a <see cref="ArgumentException"/> is thrown.</para>
    /// <para>Only useful if you are using the "Custom Parameter Validation" assembly mode.</para>
    /// </remarks>
    /// <seealso cref="Enforce"/>
    [DebuggerStepThrough]
    public static class Require
    {
        /// <summary>
        /// Validates that the specified object is not <see langword="null"/>. 
        /// Meant to be used inside an extension method to validate the first parameter which 
        /// specifies which type the method operates on.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="this"/>.</typeparam>
        /// <param name="this">The object to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="this"/> is <see langword="null"/>.</exception>
        [ContractArgumentValidator]
        public static void Object<T>([ValidatedNotNull]T @this) where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this", Strings_Core.Require_ObjectNull);
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified property value is not <see langword="null"/>. 
        /// Meant to be used inside a property setter to validate the new value.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The property value to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        [ContractArgumentValidator]
        public static void Property<T>([ValidatedNotNull]T value) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", Strings_Core.Require_PropertyNull);
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified property value is not <see langword="null"/> or empty.
        /// Meant to be used inside a property setter to validate the new value.
        /// </summary>
        /// <param name="value">The property value to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is <see langword="null"/> or empty.</exception>
        [ContractArgumentValidator]
        public static void PropertyNotEmpty([ValidatedNotNull]string value)
        {
            Require.Property(value);

            if (value.Length == 0)
            {
                throw new ArgumentException(Strings_Core.Require_PropertyNullOrEmpty, "value");
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified property value is not <see langword="null"/> or empty,
        /// or does not consist only of white-space characters.
        /// Meant to be used inside a property setter to validate the new value.
        /// </summary>
        /// <param name="value">The property value to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is <see langword="null"/> 
        /// or empty, or does not consist only of white-space characters.</exception>
        [ContractArgumentValidator]
        public static void PropertyNotWhiteSpace([ValidatedNotNull]string value)
        {
            Require.Property(value);

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(Strings_Core.Require_PropertyNullOrWhiteSpace, "value");
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(
                    parameterName,
                    Format.CurrentCulture(Strings_Core.Require_ArgumentNull_Format, parameterName));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is <see langword="null"/> or empty.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string parameterName)
        {
            Require.NotNull(value, parameterName);

            if (value.Length == 0)
            {
                throw new ArgumentException(
                    Format.CurrentCulture(Strings_Core.Require_ArgumentNullOrEmpty_Format, parameterName),
                    parameterName);
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/> or empty,
        /// and does not consist only of white-space characters.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is <see langword="null"/> 
        /// or empty, or does not consist only of white-space characters.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void NotNullOrWhiteSpace([ValidatedNotNull]string value, string parameterName)
        {
            Require.NotNull(value, parameterName);

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    Format.CurrentCulture(Strings_Core.Require_ArgumentNullOrWhiteSpace_Format, parameterName),
                    parameterName);
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is in a given range of integers, range borders included.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="minInclusive">The minimum integer value (inclusive).</param>
        /// <param name="maxInclusive">The maximum integer value (inclusive).</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="minInclusive"/> is greater than 
        /// or equal to <paramref name="maxInclusive"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is outside
        /// the allowable range of values.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void InRange(int value, int minInclusive, int maxInclusive, string parameterName)
        {
            if (minInclusive > maxInclusive)
            {
                throw new ArgumentException(
                    Format.CurrentCulture(
                        Strings_Core.Require_InvalidRange_Format,
                        minInclusive,
                        maxInclusive),
                    "minInclusive");
            }

            if (value < minInclusive || value > maxInclusive)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotInRange_Format, parameterName, minInclusive, maxInclusive));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is in a given range of long integers, range borders included.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="minInclusive">The minimum long value (inclusive).</param>
        /// <param name="maxInclusive">The maximum long value (inclusive).</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="minInclusive"/> is greater than 
        /// or equal to <paramref name="maxInclusive"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is outside
        /// the allowable range of values.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void InRange(long value, long minInclusive, long maxInclusive, string parameterName)
        {
            if (minInclusive > maxInclusive)
            {
                throw new ArgumentException(
                    Format.CurrentCulture(
                        Strings_Core.Require_InvalidRange_Format,
                        minInclusive,   
                        maxInclusive),
                    "minInclusive");
            }

            if (value < minInclusive || value > maxInclusive)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotInRange_Format, parameterName, minInclusive, maxInclusive));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is greater than a minimum integer value.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="minValue">The minimum integer value.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than
        /// or equal to the minimum integer value.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void GreaterThan(int value, int minValue, string parameterName)
        {
            if (value <= minValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotGreaterThan_Format, parameterName, minValue));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is greater than a minimum long value.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="minValue">The minimum long value.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than
        /// or equal to the minimum long value.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void GreaterThan(long value, long minValue, string parameterName)
        {
            if (value <= minValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotGreaterThan_Format, parameterName, minValue));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is greater than or equal to a minimum integer value.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="minValue">The minimum integer value (inclusive).</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than
        /// the minimum integer value.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void GreaterThanOrEqualTo(int value, int minValue, string parameterName)
        {
            if (value < minValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotGreaterThanOrEqualTo_Format, parameterName, minValue));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is greater than or equal to a minimum long value.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="minValue">The minimum long value (inclusive).</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than
        /// the minimum long value.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void GreaterThanOrEqualTo(long value, long minValue, string parameterName)
        {
            if (value < minValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotGreaterThanOrEqualTo_Format, parameterName, minValue));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is less than a maximum integer value.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="maxValue">The maximum integer value.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than
        /// or equal the maximum integer value.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void LessThan(int value, int maxValue, string parameterName)
        {
            if (value >= maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotLessThan_Format, parameterName, maxValue));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is less than a maximum long value.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="maxValue">The maximum long value.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than
        /// or equal the maximum long value.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void LessThan(long value, long maxValue, string parameterName)
        {
            if (value >= maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotLessThan_Format, parameterName, maxValue));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is less or equal to a maximum integer value.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="maxValue">The maximum integer value (inclusive).</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than
        /// the maximum integer value.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void LessThanOrEqualTo(int value, int maxValue, string parameterName)
        {
            if (value > maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotLessThanOrEqualTo_Format, parameterName, maxValue));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is less or equal to a maximum long value.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="maxValue">The maximum long value (inclusive).</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than
        /// the maximum long value.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Gendarme.Rules.Exceptions", "InstantiateArgumentExceptionCorrectlyRule",
            Justification = "[Ignore] We do initialize the exceptions correctly, but Gendarme does not recognize that.")]
        public static void LessThanOrEqualTo(long value, long maxValue, string parameterName)
        {
            if (value > maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotLessThanOrEqualTo_Format, parameterName, maxValue));
            }

            Contract.EndContractBlock();
        }
    }
}