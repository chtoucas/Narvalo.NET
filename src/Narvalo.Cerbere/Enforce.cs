// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;

    public static class Enforce
    {
        /// <remarks>
        /// Cette méthode ne doit être utilisée qu'après une vérification Require.NotNullOrEmpty().
        /// </remarks>
        public static void NotWhiteSpace(string value, string parameterName)
        {
            Expect.NotNull(value);
            Expect.True(value.Length != 0);

            if (IsWhiteSpace(value))
            {
                throw Failure.Argument(
                    "La chaîne de caractères ne contient que des espaces blancs.",
                    parameterName);
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
    }
}
