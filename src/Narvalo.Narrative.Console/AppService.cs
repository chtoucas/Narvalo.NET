// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Serilog;

    public sealed class AppService
    {
        static readonly List<string> FilesToIgnore_ = new List<string> {
			"Designer.cs"
		};

        readonly AppSettings _settings;

        public AppService(AppSettings settings)
        {
            _settings = settings;
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

            var sources = from path in paths
                          from source in FindCSharpSources_(path)
                          select source;

            foreach (var fileName in sources) {
                ProcessSource_(template, fileName);
                break;
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

        void ProcessSource_(RazorTemplate template, string fileName)
        {
            Log.Debug("Processing {Source}", fileName);

            var source = new CSharpSource(fileName);
            source.Parse();

            var output = template.Execute(fileName, source.Sections);

            System.Console.Write(output);
        }

        void PrepareEnvironment_()
        {
            var outputPath = _settings.OutputDirectory;

            if (!Directory.Exists(outputPath)) {
                Log.Debug("Creating output path: {Path}.", outputPath);

                Directory.CreateDirectory(outputPath);
            }
        }

        IEnumerable<string> FindCSharpSources_(string path)
        {
            return Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories)
                .Where(filename => !FilesToIgnore_.Any(_ => filename.EndsWith(_)));
        }

        //void SaveResult_(string source, string text)
        //{
        //    int depth;
        //    var destination = GetDestination_(source, out depth);

        //    string pathToRoot = string.Concat(Enumerable.Repeat(".." + Path.DirectorySeparatorChar, depth));

        //    File.WriteAllText(destination, text);
        //}

        //string GetDestination_(string filepath, out int depth)
        //{
        //    var dirs = Path.GetDirectoryName(filepath).Substring(1).Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
        //    depth = dirs.Length;

        //    var dest = Path.Combine("docs", string.Join(Path.DirectorySeparatorChar.ToString(), dirs)).ToLower();
        //    Directory.CreateDirectory(dest);

        //    return Path.Combine("docs", Path.ChangeExtension(filepath, "html").ToLower());
        //}
    }
}
