// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Properties;

    /// <summary>
    /// Provides helper methods to specify preconditions on a method.
    /// </summary>
    /// <remarks>
    /// <para>If a condition does not hold, an exception is thrown.</para>
    /// <para>The methods will be recognized as Code Contracts preconditions.</para>
    /// <para>The methods will be recognized by FxCop as guards against <see langword="null"/> value.</para>
    /// <para>Only useful if you are using the "Custom Parameter Validation" assembly mode.</para>
    /// </remarks>
    /// <seealso cref="Demand"/>
    /// <seealso cref="Enforce"/>
    /// <seealso cref="Expect"/>
    [DebuggerStepThrough]
    public static partial class Require
    {
        [ContractArgumentValidator]
        public static void State(bool testCondition)
        {
            if (!testCondition)
            {
                throw new InvalidOperationException();
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void State(bool testCondition, string message)
        {
            if (!testCondition)
            {
                throw new InvalidOperationException(message);
            }

            Contract.EndContractBlock();
        }
    }

    public static partial class Require
    {
        [ContractArgumentValidator]
        public static void True(bool testCondition, string parameterName)
        {
            if (!testCondition)
            {
                throw new ArgumentException(Strings_Cerbere.Argument_TestFailed, parameterName);
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

        [ContractArgumentValidator]
        public static void Range(bool rangeCondition, string parameterName)
        {
            if (!rangeCondition)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void Range(bool rangeCondition, string parameterName, string message)
        {
            if (!rangeCondition)
            {
                throw new ArgumentOutOfRangeException(parameterName, message);
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        /// <seealso cref="Require.NotNullUnconstrained{T}(T, string)"/>
        [ContractArgumentValidator]
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        /// <seealso cref="Require.NotNull{T}(T, string)"/>
        [ContractArgumentValidator]
        public static void NotNullUnconstrained<T>([ValidatedNotNull]T value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/> or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/> or empty.</exception>
        [ContractArgumentValidator]
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Length == 0)
            {
                throw new ArgumentException(Strings_Cerbere.Argument_EmptyString, parameterName);
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/> or empty,
        /// and does not only consist of white-space characters.
        /// </summary>
        /// <remarks>
        /// This method specifies a weaker contract: the <paramref name="value"/> must not be
        /// <see langword="null"/> or empty.
        /// </remarks>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/> or empty, or does not only consist of white-space characters.</exception>
        [ContractArgumentValidator]
        public static void NotNullOrWhiteSpace([ValidatedNotNull]string value, string parameterName)
        {
            NotNullOrEmpty(value, parameterName);

            Enforce.NotWhiteSpace(value, parameterName);
        }

        /// <summary>
        /// Validates that the specified object is not <see langword="null"/>.
        /// Meant to be used inside an extension method to validate the first parameter which
        /// specifies which type the method operates on.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="this"/>.</typeparam>
        /// <param name="this">The object to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="this"/> is
        /// <see langword="null"/>.</exception>
        /// <seealso cref="Require.ObjectNotNull{T}(T)"/>
        [ContractArgumentValidator]
        public static void Object<T>([ValidatedNotNull]T @this)
            where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this", Strings_Cerbere.Argument_NullObject);
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="this"/> is
        /// <see langword="null"/>.</exception>*
        /// <seealso cref="Require.Object{T}(T)"/>
        [ContractArgumentValidator]
        public static void ObjectNotNull<T>([ValidatedNotNull]T @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this", Strings_Cerbere.Argument_NullObject);
            }

            Contract.EndContractBlock();
        }

        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "[Ignore] This is an alias, as such, the parameter name is correct.")]
        [ContractArgumentValidator]
        public static void Property(bool testCondition)
        {
            if (!testCondition)
            {
                throw new ArgumentException(Strings_Cerbere.ArgumentProperty_TestFailed, "value");
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified property value is not <see langword="null"/>.
        /// Meant to be used inside a property setter to validate the new value.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The property value to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        /// <seealso cref="Require.PropertyNotNull{T}(T)"/>
        [ContractArgumentValidator]
        public static void Property<T>([ValidatedNotNull]T value)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", Strings_Cerbere.ArgumentProperty_Null);
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified property value is not <see langword="null"/>.
        /// Meant to be used inside a property setter to validate the new value.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The property value to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        /// <seealso cref="Require.Property{T}(T)"/>
        [ContractArgumentValidator]
        public static void PropertyNotNull<T>([ValidatedNotNull]T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", Strings_Cerbere.ArgumentProperty_Null);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        [Obsolete("Use Require.PropertyNotNullOrEmpty() instead.")]
        public static void PropertyNotEmpty([ValidatedNotNull]string value)
        {
            Property(value);

            if (value.Length == 0)
            {
                throw new ArgumentException(Strings_Cerbere.ArgumentProperty_EmptyString, "value");
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified property value is not <see langword="null"/> or empty.
        /// Meant to be used inside a property setter to validate the new value.
        /// </summary>
        /// <param name="value">The property value to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/> or empty.</exception>
        [ContractArgumentValidator]
        public static void PropertyNotNullOrEmpty([ValidatedNotNull]string value)
        {
            Property(value);

            if (value.Length == 0)
            {
                throw new ArgumentException(Strings_Cerbere.ArgumentProperty_EmptyString, "value");
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified property value is not <see langword="null"/> or empty,
        /// or does not only consist of white-space characters.
        /// Meant to be used inside a property setter to validate the new value.
        /// </summary>
        /// <remarks>
        /// This method specifies a weaker contract: the <paramref name="value"/> must not be
        /// <see langword="null"/> or empty.
        /// </remarks>
        /// <param name="value">The property value to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/> or empty, or does not only consist of white-space characters.</exception>
        [ContractArgumentValidator]
        public static void PropertyNotNullOrWhiteSpace([ValidatedNotNull]string value)
        {
            PropertyNotNullOrEmpty(value);

            Enforce.NotWhiteSpace(value, "value");
        }
    }
}