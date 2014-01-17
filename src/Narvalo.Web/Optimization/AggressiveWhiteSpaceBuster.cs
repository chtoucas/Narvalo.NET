namespace Narvalo.Web.Optimization
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Représente un nettoyeur agressif d'espaces blancs.
    /// </summary>
    public class AggressiveWhiteSpaceBuster : IWhiteSpaceBuster
    {
        // Pour rappel, \s est un alias pour [\f\n\r\t\v].
        static readonly Regex MultipleWhiteSpacesRegex_
            = new Regex(@"\s{2,}", RegexOptions.Compiled);
        
        static readonly Regex LeadingWhiteSpacesThenLeftAngleBracketRegex_
            = new Regex(@"^\s+<", RegexOptions.Compiled);
        
        static readonly Regex TrailingRightAngleBracketThenWhiteSpacesRegex_
            = new Regex(@">\s+$", RegexOptions.Compiled);

        /// <summary>
        /// Nettoie une chaîne de caractères contenant du code HTML.
        /// En particulier on supprime tous les retours à la ligne ce qui peut se révéler problématique
        /// si la chaîne contient des commentaires JavaScript.
        /// En résumé :
        /// - on supprime tous les espaces blancs multiples ;
        /// - en début de ligne on supprime les espaces blancs avant un crochet ouvrant ;
        /// - en fin de ligne on supprime les espaces blancs après un crochet fermant.
        /// </summary>
        /// <param name="value">La chaîne de caractères à nettoyer.</param>
        /// <returns>La chaîne de caractères nettoyée.</returns>
        public string Bust(string value)
        {
            Requires.NotNull(value, "value");

            if (value.Length == 0) {
                return String.Empty;
            }

            string result = MultipleWhiteSpacesRegex_.Replace(value, String.Empty);
            result = LeadingWhiteSpacesThenLeftAngleBracketRegex_.Replace(result, "<");
            result = TrailingRightAngleBracketThenWhiteSpacesRegex_.Replace(result, ">");

            return result;
        }
    }
}
