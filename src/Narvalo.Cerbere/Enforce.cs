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
    /// <para>The methods MUST appear AFTER all Code Contracts.</para>
    /// </remarks>
    public static partial class Enforce { }

    // Obsolete methods.
    public static partial class Enforce
    {
        [ExcludeFromCodeCoverage(Justification = "Obsolete method.")]
        [Obsolete("No direct replacement, nevertheless see Require.NotNullOrWhiteSpace().", true)]
        public static void NotWhiteSpace(string value, string parameterName)
        {
            if (Check.IsWhiteSpace(value))
            {
                throw new ArgumentException(Strings_Cerbere.Argument_WhiteSpaceString, parameterName);
            }
        }

        [Pure]
        [ExcludeFromCodeCoverage(Justification = "Obsolete method.")]
        [Obsolete("Use Check.IsWhiteSpace() instead.", true)]
        public static bool IsWhiteSpace(string value)
            => Check.IsWhiteSpace(value);

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
    }
}
