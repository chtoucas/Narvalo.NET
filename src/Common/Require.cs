// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides helper methods to perform argument validation.
    /// If Code Contracts is enabled, these methods are recognized as preconditions.
    /// </summary>
    [DebuggerStepThrough]
    internal static class Require
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

        /// <summary>
        /// Validates that the specified property value is not <see langword="null"/>. 
        /// Meant to be used inside a property setter to validate the new value.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The property value to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
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

        /// <summary>
        /// Validates that the specified property value is not <see langword="null"/> or empty.
        /// Meant to be used inside a property setter to validate the new value.
        /// </summary>
        /// <param name="value">The property value to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is an empty string.</exception>
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

        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, "The parameter '" + parameterName + "' is null.");
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is <see langword="null"/> or empty.</exception>
        [ContractArgumentValidator]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Helper method shared among projects.")]
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string parameterName)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException(
                    "The parameter '" + parameterName + "' is null or empty.",
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
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is <see langword="null"/> 
        /// or empty, or does not consist only of white-space characters.</exception>
        [ContractArgumentValidator]
        public static void NotNullOrWhiteSpace([ValidatedNotNull]string value, string parameterName)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "The parameter '" + parameterName + "' is null or empty, or contains only white-space characters.",
                    parameterName);
            }

            Contract.EndContractBlock();
        }

        [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
        public sealed class ValidatedNotNullAttribute : Attribute { }
    }
}
