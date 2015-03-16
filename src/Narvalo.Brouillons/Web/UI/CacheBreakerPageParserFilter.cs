// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    // Ce classe est adaptée de :
    //  http://www.sergeyakopov.com/2011/05/hooking-into-page-parsing-with-pageparserfilter

    using System;
    using System.Collections;
    using System.Web.UI.HtmlControls;

    public class CacheBreakerPageParserFilter : HtmlControlPageParserFilterBase<HtmlLink>
    {
        // Par défaut, le filtre n'est pas actif.
        bool _enabled = false;

        public CacheBreakerPageParserFilter() : base() { }

        /// <summary>
        /// Retourne <code>true</code> si le filtre est actif, <code>false</code> sinon.
        /// </summary>
        protected override bool Enabled { get { return _enabled; } }

        public override void PreprocessDirective(string directiveName, IDictionary attributes)
        {
            throw new NotImplementedException("Initialiser CssVersion & JSVersion");
        }

        protected override string TransformLiteral(string literal)
        {
            if (literal.Contains(".css"))
            {
                literal = literal.Replace(".css", ".css?v=1234567890");
            }
            if (literal.Contains(".js"))
            {
                literal = literal.Replace(".js", ".js?v=1234567890");
            }
            return literal;
        }

    }
}
