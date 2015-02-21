// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    using System.Collections;
    using Narvalo.Web.Configuration;
    using Narvalo.Web.UI;

    /// <summary>
    /// Permet de supprimer un certain nombre d'espaces blancs pendant la phase de compilation.
    /// WARNING: Ne pas activer ce filtre en production sans vérifier au préalable le résultat.
    /// </summary>
    /// <remarks>
    /// De la manière dont on procède, on n'a accès qu'à certains fragments du 
    /// contenu sans la moindre information contextuelle. On ne peut donc pas prendre
    /// de mesures trop extrèmes.
    /// </remarks>
    public sealed class WhiteSpaceBusterPageParserFilter : LiteralPageParserFilterBase
    {
        private const string DIRECTIVE_NAME = "WhiteSpaceBusting";

        // Par défaut, le filtre n'est pas actif.
        private bool _enabled = false;

        private IWhiteSpaceBuster _buster;

        /// <summary>
        /// Initialise un nouvel objet de type <see cref="Narvalo.Web.Optimization.WhiteSpaceBusterPageParserFilter"/>.
        /// </summary>
        public WhiteSpaceBusterPageParserFilter() { }

        /// <summary>
        /// Retourne <code>true</code> si le filtre est actif pour le contrôle, <code>false</code> sinon.
        /// </summary>
        protected override bool Enabled { get { return _enabled; } }

        private static bool EnableWhiteSpaceBusting_
        {
            get { return NarvaloWebConfigurationManager.OptimizationSection.EnableWhiteSpaceBusting; }
        }

        public override void PreprocessDirective(string directiveName, IDictionary attributes)
        {
            Require.NotNull(attributes, "attributes");

            // NB: Si rien n'est précisé, on considère que le filtre est actif localement.
            bool enabled = true;

            if (attributes.Contains(DIRECTIVE_NAME)) {
                enabled = ParseTo.Boolean((string)attributes[DIRECTIVE_NAME], BooleanStyles.Literal) ?? enabled;

                // On supprime la directive afin de ne pas perturber le fonctionnement de ASP.NET.
                attributes.Remove(DIRECTIVE_NAME);
            }

            // Si le filtre est activé globalement (valeur par défaut), on vérifie la directive locale, sinon on 
            // considère que le filtre ne doit pas être utilisé quelque soit la directive locale.
            _enabled = EnableWhiteSpaceBusting_ && enabled;

            if (_enabled) {
                _buster = WhiteSpaceBusterProvider.Current.PageBuster;
            }

            base.PreprocessDirective(directiveName, attributes);
        }

        protected override string TransformLiteral(string literal)
        {
            return _buster.Bust(literal);
        }
    }
}
