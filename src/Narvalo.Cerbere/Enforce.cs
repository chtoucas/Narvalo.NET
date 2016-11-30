// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Properties;

    /// <summary>
    /// Provides helper methods for argument validation.
    /// </summary>
    /// <remarks>
    /// <para>If a condition does not hold, an <see cref="ArgumentException"/> is thrown.</para>
    /// <para>The methods will be recognized by FxCop as guards against <see langword="null"/> value.</para>
    /// <para>The methods MUST appear after all Code Contracts.</para>
    /// </remarks>
    /// <seealso cref="Demand"/>
    /// <seealso cref="Expect"/>
    /// <seealso cref="Require"/>
    public static partial class Enforce
    {
        /// <summary>
        /// Validates that the specified argument is not <see langword="null"/> or empty,
        /// and does not consist only of white-space characters.
        /// </summary>
        /// <param name="value">The argument to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is
        /// <see langword="null"/> or empty, or does not consist only of white-space characters.</exception>
        public static void NotNullOrWhiteSpace([ValidatedNotNull]string value, string parameterName)
        {
            Require.NotNull(value, parameterName);

            if (IsEmptyOrWhiteSpace(value))
            {
                throw new ArgumentException(Strings_Cerbere.Argument_EmptyOrWhiteSpaceString, parameterName);
            }
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
        /// <see langword="null"/> or empty, or does not consist only of white-space characters.</exception>
        [ContractArgumentValidator]
        public static void PropertyNotWhiteSpace([ValidatedNotNull]string value)
        {
            Require.Property(value);

            if (IsEmptyOrWhiteSpace(value))
            {
                throw new ArgumentException(Strings_Cerbere.ArgumentProperty_EmptyOrWhiteSpaceString, "value");
            }

            Contract.EndContractBlock();
        }

        /// <remarks>
        /// Cette méthode ne doit être utilisée qu'après une vérification Require.NotNullOrEmpty().
        /// </remarks>
        public static void NotWhiteSpace(string value, string parameterName)
        {
            Expect.NotNull(value);
            Expect.True(value.Length != 0);

            if (IsWhiteSpace(value))
            {
                throw new ArgumentException(Strings_Cerbere.Argument_WhiteSpaceString, parameterName);
            }
        }

        /// <summary>
        /// Retourne <see langword="true"/> si une chaîne de caractères est vide ou
        /// n'est constituée que d'espaces blancs, sinon <see langword="false"/>.
        /// </summary>
        /// <remarks>
        /// Cette méthode n'est doit être utilisée qu'après une vérification String.IsNullOrEmpty().
        /// </remarks>
        [Pure]
        public static bool IsWhiteSpace(string value)
        {
            Require.NotNull(value, nameof(value));
            Expect.True(value.Length != 0);

            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a value indicating whether the specified value is empty or consists only
        /// of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns><see langword="true"/> if the specified value is empty or consists only
        /// of white-space characters; otherwise <see langword="false"/>.</returns>
        [Pure]
        private static bool IsEmptyOrWhiteSpace(string value)
        {
            Demand.NotNull(value);

            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
