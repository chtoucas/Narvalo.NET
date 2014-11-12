// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    /// <summary>
    /// Fournit une classe abstraite permettant de supprimer des espaces blancs non significatifs.
    /// </summary>
    public interface IWhiteSpaceBuster
    {
        /// <summary>
        /// Supprime un certain nombre d'espaces blancs non significatifs d'une chaîne de caractères
        /// contenant du code HTML.
        /// </summary>
        /// <remarks>
        /// On doit faire très attention quant au filtre que nous allons appliquer. 
        /// Par exemple, on pourrait avoir le code suivant :
        /// <code>
        /// bla bla
        /// &lt;%= Url.Encode("~/") %&gt;
        /// </code>,
        /// mais rien ne nous dit que nous allons recevoir ce fragment d'un seul bloc, ainsi si
        /// on n'y fait pas attention on peut remplacer les sauts de ligne par des espaces blancs
        /// et aussi décider de supprimer tous les espaces en début et fin de ligne. Cela ne marchera pas
        /// car on pourrait obtenir au final : <code>bla bla&lt;%= Url.Encode("~/") %&gt;</code>.
        /// </remarks>
        /// <param name="value">La chaîne de caractères à nettoyer.</param>
        /// <returns>La chaîne de caractères nettoyée.</returns>
        string Bust(string value);
    }
}
