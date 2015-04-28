// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Analyzers
{
    using System;

    using global::StyleCop;
    using global::StyleCop.CSharp;

    public sealed class DocumentAnalyzer : CSharpAnalyzer
    {
        public DocumentAnalyzer(SourceAnalyzer sourceAnalyzer) : base(sourceAnalyzer) { }

        protected override void AnalyzeDocumentCore(CsDocument document, bool userCode)
        {
            Param.AssertNotNull(document, "document");

            // Skip non-user code
            if (!userCode)
            {
                return;
            }

            using (var reader = document.SourceCode.Read())
            {
                var line = reader.ReadLine();
                var lineNumber = 1;

                if (line != null 
                    && line != @"// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.")
                {
                    SourceAnalyzer.AddViolation(
                        document.RootElement,
                        lineNumber,
                        RuleName.FilesMustStartWithCopyrightText);
                }

                while (line != null)
                {
                    var trimmedLine = line.Trim();

                    if (line.Length > 120)
                    {
                        if (!IgnoreLine_(trimmedLine))
                        {
                            SourceAnalyzer.AddViolation(
                                document.RootElement,
                                lineNumber,
                                RuleName.AvoidLinesExceeding120Characters);
                        }
                    }

                    line = reader.ReadLine();
                    lineNumber++;
                }
            }
        }

        private bool IgnoreLine_(string content)
        {
            return content.StartsWith("Justification =")
                || content.StartsWith("[assembly: AssemblyDescription", StringComparison.OrdinalIgnoreCase)
                || content.StartsWith("[assembly: System.Runtime.CompilerServices.InternalsVisibleTo", StringComparison.OrdinalIgnoreCase)
                || content.StartsWith(@"""", StringComparison.OrdinalIgnoreCase)
                || content.StartsWith("//", StringComparison.OrdinalIgnoreCase)
                || content.StartsWith("/*", StringComparison.OrdinalIgnoreCase)
                || content.StartsWith("*", StringComparison.OrdinalIgnoreCase);
        }
    }
}
