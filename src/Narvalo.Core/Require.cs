// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using Narvalo.Internal;

    /// <summary>
    /// Provides helper methods to perform argument validation.
    /// If Code Contracts is enabled, these methods will be understood as Preconditions.
    /// </summary>
    [DebuggerStepThrough]
    public static class Require
    {
        [ContractArgumentValidator]
        public static void Object<T>([ValidatedNotNull]T @this) where T : class
        {
            if (@this == null) {
                throw new ArgumentNullException("this", Strings_Core.Require_ObjectNull);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void Object<T>([ValidatedNotNull]T? @this) where T : struct
        {
            if (@this == null) {
                throw new ArgumentNullException("this", Strings_Core.Require_ObjectNull);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void Property<T>([ValidatedNotNull]T value) where T : class
        {
            if (value == null) {
                throw new ArgumentNullException("value", Strings_Core.Require_PropertyNull);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void Property<T>([ValidatedNotNull]T? value) where T : struct
        {
            if (value == null) {
                throw new ArgumentNullException("value", Strings_Core.Require_PropertyNull);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void PropertyNotEmpty([ValidatedNotNull]string value)
        {
            Property(value);

            if (value.Length == 0) {
                throw new ArgumentException(Strings_Core.Require_PropertyEmpty, "value");
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName) where T : class
        {
            if (value == null) {
                throw ExceptionFactory.ArgumentNull(parameterName);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void NotNull<T>([ValidatedNotNull]T? value, string parameterName) where T : struct
        {
            if (value == null) {
                throw ExceptionFactory.ArgumentNull(parameterName);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Length == 0) {
                throw new ArgumentException(
                    Format.CurrentCulture(Strings_Core.Require_ArgumentEmptyFormat, parameterName),
                    parameterName);
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Check that an argument is in a given range of integers.
        /// </summary>
        /// <remarks>Range borders are included in the comparison.</remarks>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="parameterName"></param>
        [ContractArgumentValidator]
        public static void InRange(int value, int minValue, int maxValue, string parameterName)
        {
            if (value < minValue || value > maxValue) {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotInRangeFormat, minValue, maxValue));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Check that an argument is in a given range of long integers.
        /// </summary>
        /// <remarks>Range borders are included in the comparison.</remarks>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="parameterName"></param>
        [ContractArgumentValidator]
        public static void InRange(long value, long minValue, long maxValue, string parameterName)
        {
            if (value < minValue || value > maxValue) {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotInRangeFormat, minValue, maxValue));
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Check that an argument is in a given range.
        /// </summary>
        /// <remarks>Range borders are included in the comparison.</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="range"></param>
        /// <param name="parameterName"></param>
        [ContractArgumentValidator]
        public static void InRange<T>(T value, Range<T> range, string parameterName)
            where T : struct, IComparable<T>, IEquatable<T>
        {
            if (!range.Includes(value)) {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotInRangeFormat, range.LowerEnd, range.UpperEnd));
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void GreaterThanOrEqualTo(int value, int minValue, string parameterName)
        {
            if (value < minValue) {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotGreaterThanOrEqualToFormat, minValue));
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void GreaterThanOrEqualTo(long value, long minValue, string parameterName)
        {
            if (value < minValue) {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotGreaterThanOrEqualToFormat, minValue));
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void GreaterThanOrEqualTo<T>(T value, T minValue, string parameterName)
            where T : IComparable<T>
        {
            if (value == null) {
                throw ExceptionFactory.ArgumentNull("value");
            }

            if (value.CompareTo(minValue) < 0) {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotGreaterThanOrEqualToFormat, minValue));
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void LessThanOrEqualTo(int value, int maxValue, string parameterName)
        {
            if (value > maxValue) {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotLessThanOrEqualToFormat, maxValue));
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void LessThanOrEqualTo(long value, long maxValue, string parameterName)
        {
            if (value > maxValue) {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotLessThanOrEqualToFormat, maxValue));
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void LessThanOrEqualTo<T>(T value, T maxValue, string parameterName)
            where T : IComparable<T>
        {
            if (value == null) {
                throw ExceptionFactory.ArgumentNull("value");
            }

            if (value.CompareTo(maxValue) > 0) {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    value,
                    Format.CurrentCulture(Strings_Core.Require_NotLessThanOrEqualToFormat, maxValue));
            }

            Contract.EndContractBlock();
        }
    }
}