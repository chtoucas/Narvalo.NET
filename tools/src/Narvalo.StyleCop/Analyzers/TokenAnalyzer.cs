// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Analyzers
{
    using global::StyleCop;
    using global::StyleCop.CSharp;

    public sealed class TokenAnalyzer : CSharpAnalyzer
    {
        private bool _lastTokenWasWhitespace;

        public TokenAnalyzer(SourceAnalyzer sourceAnalyzer) : base(sourceAnalyzer) { }

        protected override void AnalyzeDocumentCore(CsDocument document, bool userCode)
        {
            Param.AssertNotNull(document, "document");

            // Skip non-user code
            if (!userCode)
            {
                return;
            }

            foreach (var token in document.Tokens)
            {
                VisitToken_(document, token);
            }
        }

        private void VisitToken_(CsDocument document, CsToken token)
        {
            Param.AssertNotNull(document, "document");

            if (token.CsTokenType == CsTokenType.EndOfLine && _lastTokenWasWhitespace)
            {
                SourceAnalyzer.AddViolation(
                    document.DocumentContents, token.LineNumber, RuleName.AvoidLinesWithTrailingWhiteSpaces);
            }

            _lastTokenWasWhitespace = token.CsTokenType == CsTokenType.WhiteSpace;
        }

    }
}
