// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Représente un nettoyeur agressif d'espaces blancs.
    /// Unsafe because aggressive.
    /// </summary>
    public sealed class UnsafeWhiteSpaceBuster : IWhiteSpaceBuster
    {
        private const string HTML_ELEMENTS = @"
            (
               # Eléments de type block.
               # Cf. https://developer.mozilla.org/en-US/docs/Web/HTML/Block-level_elements
               address
               | article
               | aside
               | audio
               | blockquote
               | canvas
               | dd
               | div
               | dl
               | fieldset
               | figcaption
               | figure
               | footer
               | form
               | h1
               | h2
               | h3
               | h4
               | h5
               | h6
               | header
               | hgroup
               | hr
               | noscript
               | ol
               | output
               | p
               | pre
               | section
               | table
               | table
               | tfoot
               | ul
               | video
               # Eléments dans l'en-tête.
               | head
               | html
               | link
               | meta
               | title
               # Autres éléments.
               | li
               | script
               | style
            )
            ";

        private static readonly Regex s_SpaceAfterRightAngleBracketRegex
            = new Regex(
                HTML_ELEMENTS + @"\>\x20",
                RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        private static readonly Regex s_SpaceBeforeLeftAngleBracketRegex
            = new Regex(
                @"\x20\<(/?" + HTML_ELEMENTS + ")",
                RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        // Pour rappel, "\s" est un alias pour "[\f\n\r\t\v]".
        private static readonly Regex s_WhiteSpacesRegex
            = new Regex(@"\s+", RegexOptions.Compiled);

        /// <summary>
        /// Réduit le nombre d'espace blancs présents dans une chaîne de caractères contenant
        /// des extraits de code HTML.
        /// </summary>
        /// <remarks>
        /// Attention, cette méthode est fortement succeptible de créer un code HTML invalide.
        /// Cette classe n'est pas compatible avec la présence d'élément "pre",
        /// de règle CSS telle que "white-space: pre;" ou de code JavaScript.
        /// </remarks>
        /// <param name="value">La chaîne de caractères à nettoyer.</param>
        /// <returns>La chaîne de caractères nettoyée.</returns>
        public string Bust(string value)
        {
            Require.NotNull(value, nameof(value));

            if (value.Length == 0)
            {
                return String.Empty;
            }

            // On remplace les chaînes de caractères constituées uniquement
            // d'espaces blancs par un seul espace.
            if (String.IsNullOrWhiteSpace(value))
            {
                return "\x20";
            }

            // NB: On peut trouver les fichiers générés par ASP.NET dans le répertoire :
            //  C:\Users\[User]\AppData\Local\Temp\Temporary ASP.NET Files\vs
            // WARNING: Dans la suite, l'ordre est important.
            // 1. On remplace tous les espaces blancs (éventuellement consécutifs) par un seul espace.
            string retval = s_WhiteSpacesRegex.Replace(value, "\x20");

            // 2. On supprime les espaces après certains crochets fermants : "XXX>   " -> "XXX>".
            retval = s_SpaceAfterRightAngleBracketRegex.Replace(retval, "$1>");

            // 3. On supprime les espaces avant certains crochets ouvrants :
            // "   <XXX" -> "<XXX" ou "   </XXX" -> "</XXX".
            retval = s_SpaceBeforeLeftAngleBracketRegex.Replace(retval, "<$1");

            return retval.Trim();
        }
    }
}