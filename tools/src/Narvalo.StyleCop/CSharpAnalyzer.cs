// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using StyleCop;
    using StyleCop.CSharp;

    public abstract class CSharpAnalyzer
    {
        private readonly SourceAnalyzer _sourceAnalyzer;

        protected CSharpAnalyzer(SourceAnalyzer sourceAnalyzer)
        {
            Param.RequireNotNull(sourceAnalyzer, "sourceAnalyzer");

            _sourceAnalyzer = sourceAnalyzer;
        }

        protected SourceAnalyzer SourceAnalyzer { get { return _sourceAnalyzer; } }

        // csdocument.WalkDocument(new CodeWalkerElementVisitor<object>(ProcessElement_), null, null);
        public abstract void Analyze(CsDocument document, bool userCode);
    }
}
