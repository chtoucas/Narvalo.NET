namespace Narvalo.Web.Minification
{
    using System;
    using System.Text.RegularExpressions;

    public static class Minifier
    {
        static readonly Regex MultipleTabsOrSpacesRegex 
            = new Regex(@"[\x09\x20]{2,}", RegexOptions.Compiled);
        static readonly Regex MultipleNewlinesRegex 
            = new Regex(@"([\x0A\x0D]+\x20*){2,}", RegexOptions.Compiled);
        // Pour rappel, \s correspond au schéma [\f\n\r\t\v]
        static readonly Regex MutipleWhitespacesRegex 
            = new Regex(@"\s{2,}", RegexOptions.Compiled);
        static readonly Regex LeadingLeftAngleBracketRegex 
            = new Regex(@"^\x20+<", RegexOptions.Compiled);
        static readonly Regex TrailingRightAngleBracketRegex 
            = new Regex(@">\x20+$", RegexOptions.Compiled);
        //static readonly Regex RegexLeadingLeftAngleBracket 
        //  = new Regex(@"([\n\s])+?(?<= {2,})<", RegexOptions.Compiled);
        //static readonly Regex RegexTrailingRightAngleBracket 
        //  = new Regex(@">(?! )\s+", RegexOptions.Compiled);

        public static string RemoveWhiteSpaces(string literal, MinifyLevel level)
        {
            Requires.NotNull(literal, "literal");

            if (literal.Length == 0) {
                return String.Empty;
            }

            switch (level) {
                case MinifyLevel.Safe:
                    return SafeMinify_(literal);
                case MinifyLevel.Advanced:
                    return AdvancedMinify_(literal);
                case MinifyLevel.None:
                default:
                    return literal;
            }
        }

        /// <summary>
        /// Réduit la taille d'une chaîne HTML de manière conservatrice.
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
        /// <remarks>
        /// Comme précisé en préambule, on doit faire très attention quand aux règles que
        /// nous appliquons. Par ex, on pourrait avoir dans une page, le code suivant :
        /// <code>
        /// bla bla
        /// <%= Url.Encode("~/") %>
        /// </code>,
        /// mais rien ne nous dit que nous allons recevoir ce fragment d'un seul bloc, ainsi si
        /// on n'y fait pas attention on peut remplacer les sauts de ligne par des espaces blancs
        /// et aussi décider de supprimer tous les espaces en début et fin de ligne. Cela ne marchera pas
        /// on pourrait avoir au final : <code>bla bla<%= Url.Encode("~/") %>/code>.
        /// </remarks>
        static string SafeMinify_(string literal)
        {
            string result = literal;

            // WARNING: l'ordre est important car les regex suivantes tiennent compte
            // du fait que les espaces et tabs sont remplacés par un espace simple.
            // On remplace toutes les occurences de caractères TAB ou SPACE en un espace simple.
            result = MultipleTabsOrSpacesRegex.Replace(result, "\x20");
            // On remplace toutes les occurences de caractères de fin de ligne en une fin de ligne simple.
            result = MultipleNewlinesRegex.Replace(result, "\n");

            // On remplace les espaces simples en fin et en début de chaîne.
            return result.Trim(new char[] { ' ' });
        }

        /// <summary>
        /// Réduit la taille d'une chaîne HTML de manière très agressive. En particulier on supprime 
        /// tous les retours à la ligne ce qui peut se révéler problématique si la chaîne contient des
        /// commentaires JavaScript.
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
        static string AdvancedMinify_(string literal)
        {
            string result = literal;

            // WARNING: l'ordre est important car les regex suivantes tiennent compte
            // du fait que les espaces et tabs sont remplacés par un espace simple.
            // On remplace tous les caractères blancs en un espace simple.
            result = MutipleWhitespacesRegex.Replace(result, "\x20");
            // On remplace tous les crochets ouvrants en début de ligne par un crochet ouvrant simple.
            result = LeadingLeftAngleBracketRegex.Replace(result, "<");
            // On remplace tous les crochets fermants en fin de ligne par un crochet fermant simple.
            result = TrailingRightAngleBracketRegex.Replace(result, ">");

            return result;
        }
    }
}
