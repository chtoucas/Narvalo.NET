// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Analyzers
{
    using global::StyleCop;
    using global::StyleCop.CSharp;

    public abstract class CSharpAnalyzer
    {
        protected CSharpAnalyzer(SourceAnalyzer sourceAnalyzer)
        {
            Param.RequireNotNull(sourceAnalyzer, "sourceAnalyzer");

            SourceAnalyzer = sourceAnalyzer;
        }

        protected SourceAnalyzer SourceAnalyzer { get; private set; }

        public void AnalyzeDocument(CsDocument document, bool userCode)
        {
            Param.RequireNotNull(document, "document");

            if (document.RootElement != null && !document.RootElement.Generated)
            {
                AnalyzeDocumentCore(document, userCode);
            }
        }

        // csdocument.WalkDocument(new CodeWalkerElementVisitor<object>(ProcessElement_), null, null);
        protected abstract void AnalyzeDocumentCore(CsDocument document, bool userCode);
    }
}
