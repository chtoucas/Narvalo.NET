// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using StyleCop;
    using StyleCop.CSharp;

    public sealed class TokenAnalyzer : CSharpAnalyzer
    {
        private bool _lastTokenWasWhitespace;

        public TokenAnalyzer(SourceAnalyzer sourceAnalyzer) : base(sourceAnalyzer) { }

        public override void Analyze(CsDocument document, bool userCode)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            // Skip non-user code.
            if (!userCode) { return; }

            foreach (CsToken token in document.Tokens)
            {
                CheckTrailingWhiteSpaces(document, token);
            }
        }

        private void CheckTrailingWhiteSpaces(CsDocument document, CsToken token)
        {
            Param.AssertNotNull(document, "document");
            Param.AssertNotNull(token, "token");

            if (token.CsTokenType == CsTokenType.EndOfLine && _lastTokenWasWhitespace)
            {
                SourceAnalyzer.AddViolation(
                    document.DocumentContents, token.LineNumber, RuleName.AvoidLinesWithTrailingWhiteSpaces);
            }

            _lastTokenWasWhitespace = token.CsTokenType == CsTokenType.WhiteSpace;
        }
    }
}
