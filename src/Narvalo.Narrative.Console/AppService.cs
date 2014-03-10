// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Serilog;

    public sealed class AppService
    {
        static readonly List<string> FilesToIgnore_ = new List<string> {
			"Designer.cs"
		};

        readonly AppSettings _settings;
        readonly IMarkdownEngine _markdown;

        public AppService(AppSettings settings, IMarkdownEngine markdown)
        {
            _settings = settings;
            _markdown = markdown;
        }

        public void Process(string[] paths)
        {
            if (paths.Length == 0) {
                Log.Warning("No path given.");
                return;
            }

            PrepareEnvironment_();

            var template = new RazorTemplate("Resources/linear.cshtml");
            template.OnCompilerError += OnCompilerError_;
            template.Compile();

            var sources = from directoryPath in paths
                          from fileName in FindCSharpFilesInDirectory_(directoryPath)
                          select Tuple.Create(directoryPath, fileName);

            foreach (var source in sources) {
                ProcessSource_(template, source.Item1, source.Item2);
            }
        }

        void OnCompilerError_(object sender, CompilerErrorEventArgs e)
        {
            var errors = e.CompilerErrors.OfType<CompilerError>().Where(error => !error.IsWarning);

            foreach (var error in errors) {
                Log.Error(
                    "Error Compiling Template: ({Line}, {Column}) {ErrorText}",
                    error.Line,
                    error.Column,
                    error.ErrorText);
            }
        }

        void ProcessSource_(RazorTemplate template, string directoryPath, string fileName)
        {
            Log.Debug("Processing {File}...", fileName);

            var filePath = Path.Combine(directoryPath, fileName);
            var source = new CSharpSource(filePath);
            source.ReadAndParse();

            var output = template.Execute(fileName, source.Blocks.Select(_ => CreateHtmlBlock_(_)));

            SaveOutput_(fileName, output);
        }

        HtmlBlock CreateHtmlBlock_(Block block)
        {
            switch (block.BlockType) {
                case BlockType.Code:
                    return new HtmlBlock
                    {
                        BlockType = block.BlockType,
                        Content = new HtmlString(HttpUtility.HtmlEncode(block.Content))
                    };
                case BlockType.Markdown:
                    return new HtmlBlock
                    {
                        BlockType = block.BlockType,
                        Content = new HtmlString(_markdown.Transform(block.Content))
                    };
                default:
                    throw new InvalidOperationException();
            }
        }

        void PrepareEnvironment_()
        {
            var outputPath = _settings.OutputDirectory;

            if (!Directory.Exists(outputPath)) {
                Log.Debug("Creating output path: {Path}.", outputPath);

                Directory.CreateDirectory(outputPath);
            }
        }

        IEnumerable<string> FindCSharpFilesInDirectory_(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*.cs", SearchOption.TopDirectoryOnly)
                .Where(_ => !FilesToIgnore_.Any(v => _.EndsWith(v)))
                .Select(_ => new FileInfo(_).Name);
        }

        void SaveOutput_(string fileName, string text)
        {
            var outputFileName = Path.ChangeExtension(fileName, "html");
            var destination = Path.Combine(_settings.OutputDirectory, outputFileName);

            File.WriteAllText(destination, text);
        }
    }
}
