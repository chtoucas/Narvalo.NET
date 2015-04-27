// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using global::StyleCop;
    using global::StyleCop.CSharp;

    using Narvalo.Analyzers;

    [SourceAnalyzer(typeof(CsParser))]
    public sealed class CSharpRules : SourceAnalyzer
    {
        public override void AnalyzeDocument(CodeDocument document)
        {
            Param.RequireNotNull(document, "document");

            var csdocument = (CsDocument)document;

            if (csdocument.RootElement != null && !csdocument.RootElement.Generated)
            {
                bool userCode = !IsGeneratedOrDesignerFile_(csdocument);

                new DocumentAnalyzer(this).AnalyzeDocument(csdocument, userCode);
                new ElementAnalyzer(this).AnalyzeDocument(csdocument, userCode);
                new TokenAnalyzer(this).AnalyzeDocument(csdocument, userCode);
            }
        }

        private bool IsGeneratedOrDesignerFile_(CsDocument document)
        {
            Param.AssertNotNull(document, "document");

            string fileName = document.SourceCode.Name;

            return fileName.EndsWith(".g.cs", StringComparison.OrdinalIgnoreCase)
               || fileName.EndsWith(".Designer.cs", StringComparison.OrdinalIgnoreCase);
        }
    }
}
