// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using Narvalo.Properties;

    /// <summary>
    /// Provides helper methods to specify preconditions on a method.
    /// </summary>
    /// <remarks>
    /// <para>If a condition does not hold, an exception is thrown.</para>
    /// <para>The methods will be recognized as Code Contracts preconditions.</para>
    /// <para>The methods will be recognized by FxCop as guards against null value.</para>
    /// <para>Only useful if you are using the "Custom Parameter Validation" assembly mode.</para>
    /// </remarks>
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

    // Methods to perform argument validation.
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
        /// Validates that the specified argument is not null.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// null.</exception>
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
        /// Validates that the specified argument is not null.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// null.</exception>
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
        /// Validates that the specified argument is not null or empty.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is
        /// null or empty.</exception>
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
    }
}