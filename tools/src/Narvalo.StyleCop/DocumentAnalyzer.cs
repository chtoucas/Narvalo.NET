// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using StyleCop;
    using StyleCop.CSharp;

    public sealed class DocumentAnalyzer : CSharpAnalyzer
    {
        private const int MAX_LINE_LENGTH = 120;
        private const string COPYRIGHT_TEXT =
            "// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.";

        public DocumentAnalyzer(SourceAnalyzer sourceAnalyzer) : base(sourceAnalyzer) { }

        public override void Analyze(CsDocument document, bool userCode)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            // Skip non-user code.
            if (!userCode) { return; }

            using (var reader = document.SourceCode.Read())
            {
                string line = reader.ReadLine();
                int lineNumber = 1;

                CheckCopyrightText(document, line);

                while (line != null)
                {
                    CheckLineLength(document, lineNumber, line);

                    line = reader.ReadLine();
                    lineNumber++;
                }
            }
        }

        // Skip:
        // - Analysis suppressions.
        // - Literal strings.
        // - Comments.
        private static bool IgnoreLengthyLine(string line)
        {
            Param.AssertNotNull(line, "line");

            string trimmedLine = line.TrimStart();
            char firstChar = trimmedLine[0];
            string firstTwoChars = trimmedLine.Substring(0, 2);

            return firstChar == '"'
                || firstChar == '*'
                || firstTwoChars == @"@"""
                || firstTwoChars == "//"
                || firstTwoChars == "/*"
                || trimmedLine.StartsWith("Justification =", StringComparison.OrdinalIgnoreCase)
                || trimmedLine.StartsWith("[assembly: ", StringComparison.OrdinalIgnoreCase);
        }

        private void CheckCopyrightText(CsDocument document, string firstLine)
        {
            Param.AssertNotNull(document, "document");

            if (firstLine != null && firstLine != COPYRIGHT_TEXT)
            {
                SourceAnalyzer.AddViolation(
                    document.RootElement,
                    1,
                    RuleName.FilesMustStartWithCopyrightText);
            }
        }

        private void CheckLineLength(CsDocument document, int lineNumber, string line)
        {
            Param.AssertNotNull(document, "document");
            Param.AssertNotNull(line, "line");

            if (line.Length <= MAX_LINE_LENGTH) { return; }

            if (IgnoreLengthyLine(line)) { return; }

            SourceAnalyzer.AddViolation(
                document.RootElement,
                lineNumber,
                RuleName.AvoidLinesExceeding120Characters);
        }
    }
}
