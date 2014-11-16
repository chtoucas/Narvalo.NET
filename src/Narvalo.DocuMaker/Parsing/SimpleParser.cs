// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.DocuMaker.Parsing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using Narvalo;

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

            var lines = new TextLineCollection(() => reader);

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

        // Borrowed from Jon Skeet.
        // Cf. http://csharpindepth.com/articles/chapter6/iteratorblockimplementation.aspx
        sealed class TextLineCollection : IEnumerable<string>
        {
            readonly Func<TextReader> _readerThunk;

            public TextLineCollection(Func<TextReader> readerThunk)
            {
                _readerThunk = readerThunk;
            }

            public IEnumerator<string> GetEnumerator()
            {
                using (var reader = _readerThunk.Invoke()) {
                    string line;

                    while ((line = reader.ReadLine()) != null) {
                        yield return line;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
