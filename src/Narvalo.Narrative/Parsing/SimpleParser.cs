// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using Narvalo.Narrative.Internal;

    // Cf.
    // https://github.com/icsharpcode/NRefactory/
    // http://msdn.microsoft.com/en-us/roslyn
    // For compiled assemblies
    // http://ccimetadata.codeplex.com/
    // http://www.mono-project.com/Cecil

    public class SimpleParser : IParser
    {
        static readonly Regex IgnoreRegex_ = new Regex(@"^\s*(?:/\*|\*/|////)", RegexOptions.Compiled);
        static readonly Regex MarkdownRegex_ = new Regex(@"^\s*\*\s", RegexOptions.Compiled);

        public IEnumerable<Block> Parse(TextReader reader)
        {
            Require.NotNull(reader, "reader");

            var lineReader = new LineReader(() => reader);

            return Parse_(lineReader);
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

        //IEnumerable<string> Read_()
        //{
        //    string line;

        //    while ((line = _reader.ReadLine()) != null) {
        //        yield return line;
        //    }
        //}
    }
}
