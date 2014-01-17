namespace Narvalo.Web.Optimization
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Représente un nettoyeur simple d'espaces blancs.
    /// </summary>
    public class GentleWhiteSpaceBuster : IWhiteSpaceBuster
    {
        static readonly Regex MultipleTabsOrSpacesRegex_
            = new Regex(@"[\x09\x20]{2,}", RegexOptions.Compiled);
        
        // TODO: Désactiver la capture dans la regex.
        static readonly Regex MultipleEmptyLinesRegex_
            = new Regex(@"([\x0A\x0D]+\x20*){2,}", RegexOptions.Compiled);

        /// <summary>
        /// Nettoie une chaîne de caractères contenant du code HTML.
        /// En résumé :
        /// - on remplace tous les espaces et tabulations multiples par un espace blanc simple;
        /// - on remplace les lignes vides multiples par un retour à la ligne simple ;
        /// - on supprime les espaces en fin et début de chaîne.
        /// </summary>
        /// <param name="value">La chaîne de caractères à nettoyer.</param>
        /// <returns>La chaîne de caractères nettoyée.</returns>
        public string Bust(string value)
        {
            Requires.NotNull(value, "value");

            if (value.Length == 0) {
                return String.Empty;
            }

            //// WARNING: L'ordre est important car on prend en compte le fait 
            //// que les espaces et tabulations sont d'abord remplacés par un espace simple.

            string result = MultipleTabsOrSpacesRegex_.Replace(value, "\x20");
            result = MultipleEmptyLinesRegex_.Replace(result, "\n");

            return result.Trim(new char[] { ' ' });
        }
    }
}
