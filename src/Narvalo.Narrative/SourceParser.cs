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

    public sealed class SourceParser
    {
        static readonly Regex IgnoreRegex_ = new Regex(@"^\s*(?:/\*|\*/|////)", RegexOptions.Compiled);
        static readonly Regex MarkdownRegex_ = new Regex(@"^\s*\*\s", RegexOptions.Compiled);

        readonly string _path;

        public SourceParser(string path)
        {
            Require.NotNullOrEmpty(path, "path");

            _path = path;
        }

        public string Path { get { return _path; } }

        public IEnumerable<Block> Parse()
        {
            var lines = Read_();
            
            return Parse_(lines);
        }

        IEnumerable<Block> Parse_(IEnumerable<string> lines)
        {
            var inCode = false;
            var markdownText = new StringBuilder();
            var codeText = new StringBuilder();

            foreach (var line in lines) {
                if (IgnoreRegex_.IsMatch(line)) {
                    continue;
                }

                // FIXME: Inefficient.
                if (MarkdownRegex_.IsMatch(line)) {
                    if (inCode) {
                        yield return Block.Code(codeText);

                        codeText.Clear();
                    }

                    inCode = false;
                    markdownText.AppendLine(MarkdownRegex_.Replace(line, String.Empty));
                }
                else {
                    if (!inCode) {
                        yield return Block.Markdown(markdownText);

                        markdownText.Clear();
                    }

                    inCode = true;
                    codeText.AppendLine(line);
                }
            }

            if (inCode) {
                yield return Block.Code(codeText);
            }
            else {
                yield return Block.Markdown(markdownText);
            }
        }

        IEnumerable<string> Read_()
        {
            string line;

            using (var reader = new StreamReader(Path)) {
                while ((line = reader.ReadLine()) != null) {
                    yield return line;
                }
            }
        }
    }
}
