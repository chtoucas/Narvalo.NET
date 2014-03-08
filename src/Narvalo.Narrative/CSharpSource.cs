// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    // Cf.
    // https://github.com/icsharpcode/NRefactory/
    // http://msdn.microsoft.com/en-us/roslyn
    // For compiled assemblies
    // http://ccimetadata.codeplex.com/
    // http://www.mono-project.com/Cecil

    public class CSharpSource
    {
        static Regex CommentFilter_ { get { return new Regex(@"(^#![/]|^\s*#\{)"); } }

        static Regex CommentRegex_ { get { return new Regex(@"^\s*(\*|//)\s"); } }
        static Regex IgnoreRegex_ = new Regex(@"^\s*(/\*|\*/|////)");
        static Regex RegionRegex_ = new Regex(@"^\s*#region");

        readonly List<Section> _sections = new List<Section>();
        readonly string _source;

        public CSharpSource(string source)
        {
            _source = source;
        }

        public IEnumerable<Section> Sections { get { return _sections; } }

        public string Source { get { return _source; } }

        public void Parse()
        {
            // FIXME: Completely inefficient.

            var lines = ReadDiscardingMultiBlankLines(_source);

            var hasCode = false;
            var docsText = new StringBuilder();
            var codeText = new StringBuilder();

            foreach (var line in lines) {
                if (IgnoreRegex_.IsMatch(line)) {
                    continue;
                }

                if (RegionRegex_.IsMatch(line)) {
                    if (hasCode) {
                        AddSection_(docsText, codeText);
                        hasCode = false;
                        docsText = new StringBuilder();
                        codeText = new StringBuilder();
                    }

                    docsText.AppendLine(RegionRegex_.Replace(line, "####"));
                }

                if (CommentRegex_.IsMatch(line) && !CommentFilter_.IsMatch(line)) {
                    if (hasCode) {
                        AddSection_(docsText, codeText);
                        hasCode = false;
                        docsText = new StringBuilder();
                        codeText = new StringBuilder();
                    }

                    docsText.AppendLine(CommentRegex_.Replace(line, ""));
                }
                else {
                    hasCode = true;
                    codeText.AppendLine(line);
                }
            }

            AddSection_(docsText, codeText);
        }


        static IEnumerable<string> ReadDiscardingMultiBlankLines(string fileName)
        {
            bool previousLineWasBlank = false;
            string line;

            using (var reader = new StreamReader(fileName)) {
                while ((line = reader.ReadLine()) != null) {
                    if (String.IsNullOrWhiteSpace(line)) {
                        previousLineWasBlank = true;
                    }
                    else {
                        if (previousLineWasBlank) {
                            previousLineWasBlank = false;
                            yield return Environment.NewLine + line;
                        }
                        else {
                            yield return line;
                        }
                    }
                }
            }
        }

        void AddSection_(StringBuilder htmlDoc, StringBuilder htmlCode)
        {
            _sections.Add(new Section { HtmlDoc = htmlDoc.ToString(), HtmlCode = htmlCode.ToString() });
        }
    }
}
