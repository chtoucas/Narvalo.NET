// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using Narvalo.Properties;

    using static Narvalo.Internal.Predicates;

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
    /// <seealso cref="Demand"/>
    [DebuggerStepThrough]
    public static class Require
    {
        [ContractArgumentValidator]
        public static void True(bool testCondition, string parameterName)
        {
            if (!testCondition)
            {
                throw new ArgumentException(Strings_Cerbere.Argument_FailedCondition, parameterName);
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
        public static void False(bool testCondition, string parameterName)
        {
            if (testCondition)
            {
                throw new ArgumentException(Strings_Cerbere.Argument_FailedCondition, parameterName);
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void False(bool testCondition, string parameterName, string message)
        {
            if (testCondition)
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
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    Strings_Cerbere.ArgumentOutOfRange_FailedCondition);
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
        [ContractArgumentValidator]
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, Strings_Cerbere.ArgumentNull_Generic);
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
        /// and does not consist only of white-space characters.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>
        /// or empty, or does not consist only of white-space characters.</exception>
        [ContractArgumentValidator]
        public static void NotNullOrWhiteSpace([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (IsEmptyOrWhiteSpace(value))
            {
                throw new ArgumentException(Strings_Cerbere.Argument_EmptyOrWhiteSpaceString, parameterName);
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
        /// <see langword="null"/>.</exception>
        [ContractArgumentValidator]
        public static void Object<T>([ValidatedNotNull]T @this) where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this", Strings_Cerbere.ArgumentNull_Object);
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
        [ContractArgumentValidator]
        public static void Property<T>([ValidatedNotNull]T value) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", Strings_Cerbere.ArgumentNull_Property);
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
        public static void PropertyNotEmpty([ValidatedNotNull]string value)
        {
            Property(value);

            if (value.Length == 0)
            {
                throw new ArgumentException(Strings_Cerbere.Argument_EmptyString, "value");
            }

            Contract.EndContractBlock();
        }

        /// <summary>
        /// Validates that the specified property value is not <see langword="null"/> or empty,
        /// or does not consist only of white-space characters.
        /// Meant to be used inside a property setter to validate the new value.
        /// </summary>
        /// <param name="value">The property value to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>
        /// or empty, or does not consist only of white-space characters.</exception>
        [ContractArgumentValidator]
        public static void PropertyNotWhiteSpace([ValidatedNotNull]string value)
        {
            Property(value);

            if (IsEmptyOrWhiteSpace(value))
            {
                throw new ArgumentException(Strings_Cerbere.Argument_EmptyOrWhiteSpaceString, "value");
            }

            Contract.EndContractBlock();
        }

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
        /// <seealso cref="Demand"/>
        public static class ThrowOnly
        {
            public static void True(bool testCondition, string parameterName)
                => True(testCondition, parameterName, Strings_Cerbere.Argument_FailedCondition);

            public static void True(bool testCondition, string parameterName, string message)
            {
                if (!testCondition)
                {
                    throw new ArgumentException(message, parameterName);
                }
            }

            public static void False(bool testCondition, string parameterName)
                => True(!testCondition, parameterName);

            public static void False(bool testCondition, string parameterName, string message)
                => True(!testCondition, parameterName, message);

            public static void Range(bool rangeCondition, string parameterName)
                => Range(rangeCondition, parameterName, Strings_Cerbere.ArgumentOutOfRange_FailedCondition);

            public static void Range(bool rangeCondition, string parameterName, string message)
            {
                if (!rangeCondition)
                {
                    throw new ArgumentOutOfRangeException(parameterName, message);
                }
            }
        }
    }
}