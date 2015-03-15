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
        public static void InRange(int value, int minInclusive, int maxInclusive, string parameterName)
        {
            if (value < minInclusive || value > maxInclusive)
            {
                var message =
                    "The value is not in range ["
                    + minInclusive.ToString(CultureInfo.CurrentCulture)
                    + ", "
                    + maxInclusive.ToString(CultureInfo.CurrentCulture)
                    + "].";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void InRange(long value, long minInclusive, long maxInclusive, string parameterName)
        {
            if (value < minInclusive || value > maxInclusive)
            {
                var message =
                    "The value is not in range ["
                    + minInclusive.ToString(CultureInfo.CurrentCulture)
                    + ", "
                    + maxInclusive.ToString(CultureInfo.CurrentCulture)
                    + "].";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void GreaterThanOrEqualTo(int value, int minInclusive, string parameterName)
        {
            if (value < minInclusive)
            {
                var message =
                    "The value is not greater than or equal to "
                    + minInclusive.ToString(CultureInfo.CurrentCulture) + ".";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void GreaterThanOrEqualTo(long value, long minInclusive, string parameterName)
        {
            if (value < minInclusive)
            {
                var message =
                    "The value is not greater than or equal to "
                    + minInclusive.ToString(CultureInfo.CurrentCulture) + ".";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void GreaterThanOrEqualTo<T>(T value, T minInclusive, string parameterName)
            where T : IComparable<T>
        {
            if (value == null)
            {
                throw ExceptionFactory.ArgumentNull("value");
            }

            if (value.CompareTo(minInclusive) < 0)
            {
                var message = "The value is not greater than or equal to " + minInclusive.ToString() + ".";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void LessThanOrEqualTo(int value, int maxInclusive, string parameterName)
        {
            if (value > maxInclusive)
            {
                var message =
                    "The value is not less than or equal to "
                    + maxInclusive.ToString(CultureInfo.CurrentCulture) + ".";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void LessThanOrEqualTo(long value, long maxInclusive, string parameterName)
        {
            if (value > maxInclusive)
            {
                var message =
                    "The value is not less than or equal to "
                    + maxInclusive.ToString(CultureInfo.CurrentCulture) + ".";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void LessThanOrEqualTo<T>(T value, T maxInclusive, string parameterName)
            where T : IComparable<T>
        {
            if (value == null)
            {
                throw ExceptionFactory.ArgumentNull("value");
            }

            if (value.CompareTo(maxInclusive) > 0)
            {
                var message = "The value is not less than or equal to " + maxInclusive.ToString() + ".";
                throw new ArgumentOutOfRangeException(parameterName, value, message);
            }

            Contract.EndContractBlock();
        }

        [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
        public sealed class ValidatedNotNullAttribute : Attribute { }

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
