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
        [ContractArgumentValidator]
        [ExcludeFromCodeCoverage(Justification = "Obsolete method.")]
        [Obsolete("Use Require.NotNullOrWhiteSpace() instead.", true)]
        public static void NotNullOrWhiteSpace([ValidatedNotNull]string value, string parameterName)
            => Require.NotNullOrWhiteSpace(value, parameterName);

        [ContractArgumentValidator]
        [ExcludeFromCodeCoverage(Justification = "Obsolete method.")]
        [Obsolete("Use Require.NotNullOrWhiteSpace() instead.", true)]
        public static void PropertyNotWhiteSpace([ValidatedNotNull]string value)
            => Require.NotNullOrWhiteSpace(value, "value");

        /// <seealso cref="Require.NotNullOrWhiteSpace(string, string)"/>
        public static void NotWhiteSpace(string value, string parameterName)
        {
            if (IsWhiteSpace(value))
            {
                throw new ArgumentException(Strings_Cerbere.Argument_WhiteSpaceString, parameterName);
            }
        }

        /// <summary>
        /// Returns <see langword="true"/> if the input only consists of white-space characters;
        /// otherwise <see langword="false"/>.
        /// </summary>
        /// <remarks>This method returns <see langword="false"/> if <paramref name="value"/>
        /// is null or empty.</remarks>
        [Pure]
        public static bool IsWhiteSpace(string value)
        {
            if (value == null || value.Length == 0) { return false; }

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
