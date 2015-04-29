// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using StyleCop;
    using StyleCop.CSharp;

    [SourceAnalyzer(typeof(CsParser))]
    public sealed class CSharpRules : SourceAnalyzer
    {
        /// <inheritdoc />
        public override void AnalyzeDocument(CodeDocument document)
        {
            Param.RequireNotNull(document, "document");

            var csdocument = (CsDocument)document;

            if (csdocument.RootElement != null && !csdocument.RootElement.Generated)
            {
                bool userCode = !AnalyzerUtility.IsGeneratedOrDesignerFile(csdocument);

                new DocumentAnalyzer(this).Analyze(csdocument, userCode);
                new ElementAnalyzer(this).Analyze(csdocument, userCode);
                new TokenAnalyzer(this).Analyze(csdocument, userCode);
            }
        }
    }
}
