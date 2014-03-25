// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    // Borrowed from Jon Skeet.
    public sealed class LineReader : IEnumerable<string>
    {
        /// <summary>
        /// Means of creating a TextReader to read from.
        /// </summary>
        readonly Func<TextReader> dataSource;

        /// <summary>
        /// Creates a LineReader from a stream source. The delegate is only
        /// called when the enumerator is fetched. UTF-8 is used to decode
        /// the stream into text.
        /// </summary>
        /// <param name="streamSource">Data source</param>
        public LineReader(Func<Stream> streamSource)
            : this(streamSource, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Creates a LineReader from a stream source. The delegate is only
        /// called when the enumerator is fetched.
        /// </summary>
        /// <param name="streamSource">Data source</param>
        /// <param name="encoding">Encoding to use to decode the stream into text</param>
        public LineReader(Func<Stream> streamSource, Encoding encoding)
            : this(() => new StreamReader(streamSource(), encoding))
        {
        }

        /// <summary>
        /// Creates a LineReader from a filename. The file is only opened
        /// (or even checked for existence) when the enumerator is fetched.
        /// UTF8 is used to decode the file into text.
        /// </summary>
        /// <param name="filename">File to read from</param>
        public LineReader(string filename)
            : this(filename, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Creates a LineReader from a filename. The file is only opened
        /// (or even checked for existence) when the enumerator is fetched.
        /// </summary>
        /// <param name="filename">File to read from</param>
        /// <param name="encoding">Encoding to use to decode the file into text</param>
        public LineReader(string filename, Encoding encoding)
            : this(() => new StreamReader(filename, encoding))
        {
        }

        /// <summary>
        /// Creates a LineReader from a TextReader source. The delegate
        /// is only called when the enumerator is fetched
        /// </summary>
        /// <param name="dataSource">Data source</param>
        public LineReader(Func<TextReader> dataSource)
        {
            this.dataSource = dataSource;
        }

        /// <summary>
        /// Enumerates the data source line by line.
        /// </summary>
        public IEnumerator<string> GetEnumerator()
        {
            using (TextReader reader = dataSource()) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    yield return line;
                }
            }
        }

        /// <summary>
        /// Enumerates the data source line by line.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // Cf.
    // https://github.com/icsharpcode/NRefactory/
    // http://msdn.microsoft.com/en-us/roslyn
    // For compiled assemblies
    // http://ccimetadata.codeplex.com/
    // http://www.mono-project.com/Cecil

    public class CSharpParser
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
