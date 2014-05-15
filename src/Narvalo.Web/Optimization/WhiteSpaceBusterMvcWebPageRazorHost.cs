// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    using System.Web.Mvc.Razor;
    using System.Web.Razor.Generator;
    using System.Web.Razor.Parser;

    public sealed class WhiteSpaceBusterMvcWebPageRazorHost : MvcWebPageRazorHost
    {
        readonly RazorOptimizer _optimizer;

        public WhiteSpaceBusterMvcWebPageRazorHost(
            string virtualPath,
            string physicalPath,
            RazorOptimizer optimizer)
            : base(virtualPath, physicalPath)
        {
            _optimizer = optimizer;
        }

        public override RazorCodeGenerator DecorateCodeGenerator(RazorCodeGenerator incomingCodeGenerator)
        {
            if (incomingCodeGenerator is CSharpRazorCodeGenerator) {
                return new WhiteSpaceBusterMvcCSharpRazorCodeGenerator(
                    incomingCodeGenerator.ClassName,
                    incomingCodeGenerator.RootNamespaceName,
                    incomingCodeGenerator.SourceFileName,
                    incomingCodeGenerator.Host,
                    _optimizer);
            }
            else {
                // NB: On ne prend pas en charge les projets VB.NET.
                return base.DecorateCodeGenerator(incomingCodeGenerator);
            }
        }

        public override ParserBase DecorateMarkupParser(ParserBase incomingMarkupParser)
        {
            var parser = base.DecorateMarkupParser(incomingMarkupParser);

            // REVIEW: Peut-on plutôt utiliser parser.IsMarkupParser ?
            if (!(parser is HtmlMarkupParser)) {
                return parser;
            }

            return new WhiteSpaceBusterHtmlMarkupParser(parser, _optimizer);
        }
    }
}
