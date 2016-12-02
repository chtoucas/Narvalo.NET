// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    using System.Collections;
    using System.Web.UI;

    using Narvalo.Web.Configuration;
    using Narvalo.Web.Internal;
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
    public sealed class WhiteSpaceBusterPageParserFilter : UnrestrictedPageParserFilter
    {
        private const string DIRECTIVE_NAME = "WhiteSpaceBusting";

        /// <summary>
        /// Initializes a new instance of the <see cref="WhiteSpaceBusterPageParserFilter"/> class.
        /// </summary>
        public WhiteSpaceBusterPageParserFilter() { }

        /// <summary>
        /// Retourne <code>true</code> si le filtre est actif pour le contrôle, <code>false</code> sinon.
        /// </summary>
        /// <remarks>Par défaut, le filtre n'est pas actif.</remarks>
        public bool Enabled { get; private set; }

        public override void PreprocessDirective(string directiveName, IDictionary attributes)
        {
            Require.NotNull(attributes, "attributes");

            // NB: Si rien n'est précisé, on considère que le filtre est actif localement.
            bool enabled = true;

            if (attributes.Contains(DIRECTIVE_NAME))
            {
                enabled = ParseTo.Boolean((string)attributes[DIRECTIVE_NAME], BooleanStyles.Literal) ?? enabled;

                // On supprime la directive afin de ne pas perturber le fonctionnement de ASP.NET.
                attributes.Remove(DIRECTIVE_NAME);
            }

            var section = NarvaloWebConfigurationManager.OptimizationSection;

            // Si le filtre est activé globalement (valeur par défaut), on vérifie la directive locale, sinon on
            // considère que le filtre ne doit pas être utilisé quelque soit la directive locale.
            Enabled = section.EnableWhiteSpaceBusting && enabled;

            base.PreprocessDirective(directiveName, attributes);
        }

        public override void ParseComplete(ControlBuilder rootBuilder)
        {
            Require.NotNull(rootBuilder, "rootBuilder");

            if (Enabled)
            {
                // FIXME
                var buster = WhiteSpaceBusterProvider.Current.Buster;

                new WhiteSpaceControlTransformer(buster).TransformRecursively(rootBuilder);
            }

            base.ParseComplete(rootBuilder);
        }
    }
}
