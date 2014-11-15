// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    using System;
    using System.Text.RegularExpressions;
    using Narvalo;

    /// <summary>
    /// Représente un nettoyeur simple d'espaces blancs.
    /// </summary>
    public sealed class DefaultWhiteSpaceBuster : IWhiteSpaceBuster
    {
        static readonly Regex LeadingSpacesRegex_
            = new Regex(@"^\x20+", RegexOptions.Compiled | RegexOptions.Multiline);

        static readonly Regex MultipleNewLinesRegex_
            = new Regex(@"(\r\n){2,}", RegexOptions.Compiled);

        static readonly Regex TabsOrMultipleSpacesRegex_
            = new Regex(@"(?:\t+|\x20{2,})", RegexOptions.Compiled);

        static readonly Regex TrailingSpacesRegex_
            = new Regex(@"\x20+$", RegexOptions.Compiled | RegexOptions.Multiline);

        /// <summary>
        /// Réduit le nombre d'espace blancs présents dans une chaîne de caractères contenant
        /// des extraits de code HTML.
        /// </summary>
        /// <remarks>
        /// Cette classe n'est pas compatible avec la présence d'élément "pre",
        /// de règle CSS telle que "white-space: pre;" ou de code JavaScript.
        /// </remarks>
        /// <param name="value">La chaîne de caractères à nettoyer.</param>
        /// <returns>La chaîne de caractères nettoyée.</returns>
        public string Bust(string value)
        {
            Require.NotNull(value, "value");

            if (value.Length == 0) {
                return String.Empty;
            }

            // On remplace les chaînes de caractères constituées uniquement
            // d'espace ou tabulation par un seul espace.
            if (IsTabsOrSpaces_(value)) {
                return "\x20";
            }

            // On remplace les chaînes de caractères constituées d'espace, tabulation 
            // et contenant un retour à la ligne par un retour à ligne.
            if (String.IsNullOrWhiteSpace(value)) {
                return "\r\n";
            }

            // 1. On remplace toute combinaison d'espace et tabulation par un seul espace.
            string result = TabsOrMultipleSpacesRegex_.Replace(value, "\x20");
            // 2. On supprime tous les espaces et tabulations en début de ligne.
            result = LeadingSpacesRegex_.Replace(result, String.Empty);
            // 3. On supprime tous les espaces et tabulations en fin de ligne.
            result = TrailingSpacesRegex_.Replace(result, String.Empty);
            // 4. On remplace les retours à la ligne consécutifs par un seul retour à la ligne.
            result = MultipleNewLinesRegex_.Replace(result, "\r\n");

            return result;
        }

        static bool IsTabsOrSpaces_(string value)
        {
            for (int i = 0; i < value.Length; i++) {
                if (!IsTabOrSpace_(value[i])) {
                    return false;
                }
            }

            return true;
        }

        static bool IsTabOrSpace_(char ch)
        {
            return ch == '\x0009' || ch == '\x0020';
        }
    }
}
