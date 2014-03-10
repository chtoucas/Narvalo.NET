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

    public class CSharpSource
    {
        static Regex CommentFilter_ { get { return new Regex(@"(^#![/]|^\s*#\{)"); } }

        static Regex MarkdownRegex_ { get { return new Regex(@"^\s*\*\s"); } }
        static Regex IgnoreRegex_ = new Regex(@"^\s*(/\*|\*/|////)");

        readonly List<Block> _blocks = new List<Block>();
        readonly string _path;

        public CSharpSource(string path)
        {
            _path = path;
        }

        public IEnumerable<Block> Blocks { get { return _blocks; } }

        public string Path { get { return _path; } }

        public void ReadAndParse()
        {
            Parse(Read());
        }

        public IEnumerable<string> Read()
        {
            string line;

            using (var reader = new StreamReader(_path)) {
                while ((line = reader.ReadLine()) != null) {
                    yield return line;
                }
            }
        }

        public void Parse(IEnumerable<string> lines)
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
                        AddCodeBlock_(codeText);
                        codeText = new StringBuilder();
                    }

                    hasCode = false;
                    markdownText.AppendLine(MarkdownRegex_.Replace(line, ""));
                }
                else {
                    if (!hasCode) {
                        AddMarkdownBlock_(markdownText);
                        markdownText = new StringBuilder();
                    }

                    hasCode = true;
                    codeText.AppendLine(line);
                }
            }

            if (hasCode) {
                AddCodeBlock_(codeText);
            }
            else {
                AddMarkdownBlock_(markdownText);
            }
        }

        void AddBlock_(BlockType blockType, StringBuilder content)
        {
            _blocks.Add(new Block { BlockType = blockType, Content = content.ToString() });
        }

        void AddCodeBlock_(StringBuilder content)
        {
            AddBlock_(BlockType.Code, content);
        }

        void AddMarkdownBlock_(StringBuilder content)
        {
            AddBlock_(BlockType.Markdown, content);
        }
    }
}
