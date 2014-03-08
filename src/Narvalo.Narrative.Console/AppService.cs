namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class AppService
    {
        static readonly string ExecutingDirectory_
            = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static readonly string[] FoldersToExclude_ = new string[] { @"\docs", @"\bin", @"\obj" };
        static readonly List<string> FilesToIgnore_ = new List<string> {
			"Designer.cs",
            "g.cs",
		};

        public void Process(string[] sources)
        {
            if (sources.Length > 0) {
                return;
            }

            Directory.CreateDirectory("docs");

            var files = new List<string>();
            foreach (var source in sources) {
                files.AddRange(Directory.GetFiles(".", source, SearchOption.AllDirectories).Where(filename =>
                {
                    return FilesToIgnore_.Any(_ => filename.EndsWith(_))
                        || FoldersToExclude_.Any(_ => Path.GetDirectoryName(filename).Contains(_));
                }));
            }

            var templateCreator = new TemplateCreator();
            var razorTemplate = templateCreator.Create("XXX");

            var essay = new CodeEssay(razorTemplate, new MarkdownDeepEngine());

            foreach (var file in files) {
                var text = essay.Process(file);
                SaveResult_(file, text);
            }
        }

        void SaveResult_(string source, string text)
        {
            int depth;
            var destination = GetDestination_(source, out depth);

            string pathToRoot = string.Concat(Enumerable.Repeat(".." + Path.DirectorySeparatorChar, depth));

            File.WriteAllText(destination, text);
        }

        string GetDestination_(string filepath, out int depth)
        {
            var dirs = Path.GetDirectoryName(filepath).Substring(1).Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            depth = dirs.Length;

            var dest = Path.Combine("docs", string.Join(Path.DirectorySeparatorChar.ToString(), dirs)).ToLower();
            Directory.CreateDirectory(dest);

            return Path.Combine("docs", Path.ChangeExtension(filepath, "html").ToLower());
        }
    }
}
