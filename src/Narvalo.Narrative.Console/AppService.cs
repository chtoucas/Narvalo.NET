// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
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

            var sources = from path in paths
                          from source in FindSources_(path)
                          select source;

            foreach (var source in sources) {
                Log.Debug("Processing {Source}", source);
                break;
            }

            //return;

            //var templateCreator = new TemplateCreator();
            //var razorTemplate = templateCreator.Create("XXX");

            //var essay = new CSharpEssay(razorTemplate, new MarkdownDeepEngine());

            //foreach (var file in sources) {
            //    var text = essay.Build(file);
            //    SaveResult_(file, text);
            //}
        }

        void PrepareEnvironment_()
        {
            var outputPath = _settings.OutputDirectory;

            if (!Directory.Exists(outputPath)) {
                Log.Debug("Creating output path: {Path}", outputPath);

                Directory.CreateDirectory(outputPath);
            }
        }

        IEnumerable<string> FindSources_(string path)
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
