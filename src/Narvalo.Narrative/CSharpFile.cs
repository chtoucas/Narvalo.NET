// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
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

    public sealed class CSharpFile
    {
        static readonly Regex IgnoreRegex_ = new Regex(@"^\s*(?:/\*|\*/|////)", RegexOptions.Compiled);
        static readonly Regex MarkdownRegex_ = new Regex(@"^\s*\*\s", RegexOptions.Compiled);

        readonly string _path;

        public CSharpFile(string path)
        {
            Require.NotNullOrEmpty(path, "path");

            _path = path;
        }

        public string Path { get { return _path; } }

        public IEnumerable<BlockBase> Parse()
        {
            return Parse_(Read_());
        }

        IEnumerable<BlockBase> Parse_(IEnumerable<string> lines)
        {
            var hasCode = false;
            var markdownText = new StringBuilder();
            var codeText = new StringBuilder();

            foreach (var line in lines) {
                if (IgnoreRegex_.IsMatch(line)) {
                    continue;
                }

                if (MarkdownRegex_.IsMatch(line)) {
                    if (hasCode) {
                        yield return new CodeBlock
                        {
                            Content = codeText.ToString()
                        };

                        codeText.Clear();
                    }

                    hasCode = false;
                    markdownText.AppendLine(MarkdownRegex_.Replace(line, ""));
                }
                else {
                    if (!hasCode) {
                        yield return new MarkdownBlock
                        {
                            Content = markdownText.ToString()
                        };

                        markdownText.Clear();
                    }

                    hasCode = true;
                    codeText.AppendLine(line);
                }
            }

            if (hasCode) {
                yield return new CodeBlock
                {
                    Content = codeText.ToString()
                };
            }
            else {
                yield return new MarkdownBlock
                {
                    Content = markdownText.ToString()
                };
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
