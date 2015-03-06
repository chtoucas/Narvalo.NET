// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Provides helper methods to perform argument validation.
    /// If Code Contracts is enabled, these methods will be understood as Preconditions.
    /// </summary>
    [DebuggerStepThrough]
    internal static class Require
    {
        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void Object<T>([ValidatedNotNull]T @this) where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this", "The object 'this' is null.");
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void Object<T>([ValidatedNotNull]T? @this) where T : struct
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this", "The object 'this' is null.");
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void Property<T>([ValidatedNotNull]T value) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "The property value is null.");
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void Property<T>([ValidatedNotNull]T? value) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "The property value is null.");
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void PropertyNotEmpty([ValidatedNotNull]string value)
        {
            Property(value);

            if (value.Length == 0)
            {
                throw new ArgumentException("The property value is empty.", "value");
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw ExceptionFactory.ArgumentNull(parameterName);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void NotNull<T>([ValidatedNotNull]T? value, string parameterName) where T : struct
        {
            if (value == null)
            {
                throw ExceptionFactory.ArgumentNull(parameterName);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Length == 0)
            {
                throw new ArgumentException(
                    "The parameter '" + parameterName + "' is empty.",
                    parameterName);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void Condition(bool predicate, string parameterName)
        {
            if (!predicate)
            {
                throw new ArgumentException(
                    "The parameter '" + parameterName + "' does not satisfy the precondition.",
                    parameterName);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void InRange(int value, int minValue, int maxValue, string parameterName)
        {
            if (value < minValue || value > maxValue)
            {
                var message =
                    "The value is not in range ["
                    + minValue.ToString(CultureInfo.CurrentCulture)
                    + ", "
                    + maxValue.ToString(CultureInfo.CurrentCulture)
                    + "].";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void InRange(long value, long minValue, long maxValue, string parameterName)
        {
            if (value < minValue || value > maxValue)
            {
                var message =
                    "The value is not in range ["
                    + minValue.ToString(CultureInfo.CurrentCulture)
                    + ", "
                    + maxValue.ToString(CultureInfo.CurrentCulture)
                    + "].";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void GreaterThanOrEqualTo(int value, int minValue, string parameterName)
        {
            if (value < minValue)
            {
                var message =
                    "The value is not greater than or equal to "
                    + minValue.ToString(CultureInfo.CurrentCulture) + ".";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void GreaterThanOrEqualTo(long value, long minValue, string parameterName)
        {
            if (value < minValue)
            {
                var message =
                    "The value is not greater than or equal to "
                    + minValue.ToString(CultureInfo.CurrentCulture) + ".";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void GreaterThanOrEqualTo<T>(T value, T minValue, string parameterName)
            where T : IComparable<T>
        {
            if (value == null)
            {
                throw ExceptionFactory.ArgumentNull("value");
            }

            if (value.CompareTo(minValue) < 0)
            {
                var message = "The value is not greater than or equal to " + minValue.ToString() + ".";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void LessThanOrEqualTo(int value, int maxValue, string parameterName)
        {
            if (value > maxValue)
            {
                var message =
                    "The value is not less than or equal to "
                    + maxValue.ToString(CultureInfo.CurrentCulture) + ".";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void LessThanOrEqualTo(long value, long maxValue, string parameterName)
        {
            if (value > maxValue)
            {
                var message =
                    "The value is not less than or equal to "
                    + maxValue.ToString(CultureInfo.CurrentCulture) + ".";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void LessThanOrEqualTo<T>(T value, T maxValue, string parameterName)
            where T : IComparable<T>
        {
            if (value == null)
            {
                throw ExceptionFactory.ArgumentNull("value");
            }

            if (value.CompareTo(maxValue) > 0)
            {
                var message = "The value is not less than or equal to " + maxValue.ToString() + ".";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
#if CONTRACTS_FULL
        public
#else
        internal
#endif
 sealed class ValidatedNotNullAttribute : Attribute { }

        private static class ExceptionFactory
        {
            public static ArgumentNullException ArgumentNull(string parameterName)
            {
                var message = "The parameter '" + parameterName + "' is null.";
                return new ArgumentNullException(parameterName, message);
            }
        }
    }
}
