// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using Narvalo.Properties;

    /// <summary>
    /// Provides helper methods to perform argument validation in the form
    /// of if-then-throw code and Code Contracts preconditions.
    /// </summary>
    /// <remarks>
    /// <para>The methods will be recognized by FxCop as parameter validators
    /// against <see langword="null"/> value.</para>
    /// <para>The methods will be recognized as Contract Argument Validator methods.</para>
    /// <para>If a condition does not hold, a <see cref="ArgumentException"/> is thrown.</para>
    /// <para>Only useful if you are using the "Custom Parameter Validation" assembly mode.</para>
    /// </remarks>
    /// <seealso cref="Guard"/>
    [DebuggerStepThrough]
    public static partial class Require
    {
        [ContractArgumentValidator]
        public static void True(bool testCondition, string parameterName)
        {
            if (!testCondition)
            {
                throw new ArgumentException("XXX", parameterName);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void True(bool testCondition, string parameterName, string message)
        {
            if (!testCondition)
            {
                throw new ArgumentException(message, parameterName);
            }

            Contract.EndContractBlock();
        }

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
                throw new ArgumentNullException("this", Strings_Cerbere.Require_ObjectNull);
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
                throw new ArgumentNullException("value", Strings_Cerbere.Require_PropertyNull);
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
            Property(value);

            if (value.Length == 0)
            {
                throw new ArgumentException(Strings_Cerbere.Require_PropertyEmpty, "value");
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
            Property(value);

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(Strings_Cerbere.Require_PropertyWhiteSpace, "value");
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
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(
                    parameterName,
                    Format.Resource(Strings_Cerbere.Require_ArgumentNull_Format, parameterName));
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
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Length == 0)
            {
                throw new ArgumentException(
                    Format.Resource(Strings_Cerbere.Require_ArgumentEmpty_Format, parameterName),
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
        public static void NotNullOrWhiteSpace([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    Format.Resource(Strings_Cerbere.Require_ArgumentWhiteSpace_Format, parameterName),
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
        public static void InRange(int value, int minInclusive, int maxInclusive, string parameterName)
        {
            ValidateRange(minInclusive, maxInclusive);

            if (value < minInclusive || value > maxInclusive)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(
                        Strings_Cerbere.Enforce_NotInRange_Format,
                        parameterName,
                        minInclusive,
                        maxInclusive));
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
        public static void InRange(long value, long minInclusive, long maxInclusive, string parameterName)
        {
            ValidateRange(minInclusive, maxInclusive);

            if (value < minInclusive || value > maxInclusive)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(
                        Strings_Cerbere.Enforce_NotInRange_Format,
                        parameterName,
                        minInclusive,
                        maxInclusive));
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
        public static void GreaterThan(int value, int minValue, string parameterName)
        {
            if (value <= minValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotGreaterThan_Format, parameterName, minValue));
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
        public static void GreaterThan(long value, long minValue, string parameterName)
        {
            if (value <= minValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotGreaterThan_Format, parameterName, minValue));
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
        public static void GreaterThanOrEqualTo(int value, int minValue, string parameterName)
        {
            if (value < minValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotGreaterThanOrEqualTo_Format, parameterName, minValue));
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
        public static void GreaterThanOrEqualTo(long value, long minValue, string parameterName)
        {
            if (value < minValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotGreaterThanOrEqualTo_Format, parameterName, minValue));
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
        public static void LessThan(int value, int maxValue, string parameterName)
        {
            if (value >= maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotLessThan_Format, parameterName, maxValue));
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
        public static void LessThan(long value, long maxValue, string parameterName)
        {
            if (value >= maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotLessThan_Format, parameterName, maxValue));
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
        public static void LessThanOrEqualTo(int value, int maxValue, string parameterName)
        {
            if (value > maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotLessThanOrEqualTo_Format, parameterName, maxValue));
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
        public static void LessThanOrEqualTo(long value, long maxValue, string parameterName)
        {
            if (value > maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.Resource(Strings_Cerbere.Enforce_NotLessThanOrEqualTo_Format, parameterName, maxValue));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Checks that the minimum value is lower or equal to the maximum value of the range.
        /// </summary>
        /// <remarks>
        /// This method was created to silent a CC1025 error:
        ///  "After contract block, found use of local variable defined in contract block"
        /// We don't use Require.LessThanOrEqualTo because we want to throw an ArgumentException
        /// instead of an ArgumentOutOfRangeException.
        /// </remarks>
        /// <param name="minInclusive">The minimum integer value.</param>
        /// <param name="maxInclusive">The maximum integer value.</param>
        [ContractArgumentValidator]
        private static void ValidateRange(int minInclusive, int maxInclusive)
        {
            if (minInclusive > maxInclusive)
            {
                throw new ArgumentException(
                    Format.Resource(
                        Strings_Cerbere.Enforce_InvalidRange_Format,
                        minInclusive,
                        maxInclusive),
                    "minInclusive");
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Checks that the minimum value is lower or equal to the maximum value of the range.
        /// </summary>
        /// <remarks>
        /// This method was created to silent a CC1025 error:
        ///  "After contract block, found use of local variable defined in contract block"
        /// We don't use Require.LessThanOrEqualTo because we want to throw an ArgumentException
        /// instead of an ArgumentOutOfRangeException.
        /// </remarks>
        /// <param name="minInclusive">The minimum long value.</param>
        /// <param name="maxInclusive">The maximum long value.</param>
        [ContractArgumentValidator]
        private static void ValidateRange(long minInclusive, long maxInclusive)
        {
            if (minInclusive > maxInclusive)
            {
                throw new ArgumentException(
                    Format.Resource(
                        Strings_Cerbere.Enforce_InvalidRange_Format,
                        minInclusive,
                        maxInclusive),
                    "minInclusive");
            }

            Contract.EndContractBlock();
        }
    }

    public static partial class Require
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
                    parameterName);
            }

            if (value.CompareTo(minInclusive) < 0 || value.CompareTo(maxInclusive) > 0)
            {
                var message = Format.Resource(
                    Strings_Cerbere.Enforce_NotInRange_Format,
                    parameterName,
                    minInclusive,
                    maxInclusive);

                throw new ArgumentOutOfRangeException(parameterName, value, message);
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
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is greater than
        /// the maximum value.</exception>
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