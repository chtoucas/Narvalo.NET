namespace Narvalo.Oueb
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Razor;

    public class Nocco
    {
        static string _executingDirectory;
        static List<string> _files;
        static Type _templateType;

        // Find all the files that match the pattern(s) passed in as arguments and
        // generate documentation for each one.
        public static void Generate(string[] targets)
        {
            if (targets.Length > 0) {
                Directory.CreateDirectory("docs");

                _executingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                //File.Copy(Path.Combine(_executingDirectory, "Resources", "Nocco.css"), Path.Combine("docs", "nocco.css"), true);
                //File.Copy(Path.Combine(_executingDirectory, "Resources", "prettify.js"), Path.Combine("docs", "prettify.js"), true);

                _templateType = SetupRazorTemplate();

                _files = new List<string>();
                foreach (var target in targets) {
                    _files.AddRange(Directory.GetFiles(".", target, SearchOption.AllDirectories).Where(filename =>
                    {
                        var language = GetLanguage(Path.GetFileName(filename));

                        if (language == null) {
                            return false;
                        }

                        // Check if the file extension should be ignored
                        if (language.Ignores != null && language.Ignores.Any(ignore => filename.EndsWith(ignore))) {
                            return false;
                        }

                        // Don't include certain directories
                        var foldersToExclude = new string[] { @"\docs", @"\bin", @"\obj" };
                        if (foldersToExclude.Any(folder => Path.GetDirectoryName(filename).Contains(folder))) {
                            return false;
                        }

                        return true;
                    }));
                }

                foreach (var file in _files) {
                    GenerateDocumentation(file);
                }
            }
        }

        // Generate the documentation for a source file by reading it in, splitting it
        // up into comment/code sections, highlighting them for the appropriate language,
        // and merging them into an HTML template.
        static void GenerateDocumentation(string source)
        {
            //var lines = File.ReadAllLines(source);
            var lines = GetLinesFromFile(source);
            var sections = Parse(source, lines);
            Hightlight(sections);
            GenerateHtml(source, sections);
        }

        static IEnumerable<string> GetLinesFromFile(string fileName)
        {
            bool previousLineWasBlank = false;
            string line;

            using (var sr = new StreamReader(fileName)) {
                while ((line = sr.ReadLine()) != null) {
                    if (String.IsNullOrWhiteSpace(line)) {
                        previousLineWasBlank = true;
                    }
                    else {
                        if (previousLineWasBlank) {
                            previousLineWasBlank = false;
                            yield return Environment.NewLine + line;
                        }
                        else {
                            yield return line;
                        }
                    }
                }
            }
        }

        // Given a string of source code, parse out each comment and the code that
        // follows it, and create an individual `Section` for it.
        static List<Section> Parse(string source, IEnumerable<string> lines)
        {
            var sections = new List<Section>();
            var language = GetLanguage(source);
            var hasCode = false;
            var docsText = new StringBuilder();
            var codeText = new StringBuilder();

            Action<string, string> save = (docs, code) => sections.Add(new Section { DocsHtml = docs, CodeHtml = code });
            Func<string, string> mapToMarkdown = docs =>
            {
                if (language.MarkdownMaps != null)
                    docs = language.MarkdownMaps.Aggregate(
                        docs,
                        (currentDocs, map) => Regex.Replace(currentDocs, map.Key, map.Value, RegexOptions.Multiline));

                return docs;
            };

            Regex ignoreLineRegex = new Regex(@"^\s*(/\*|\*/|////)");
            Regex regionLineRegex = new Regex(@"^\s*#region");

            foreach (var line in lines) {
                if (ignoreLineRegex.IsMatch(line)) {
                    continue;
                }

                if (regionLineRegex.IsMatch(line)) {
                    if (hasCode) {
                        save(mapToMarkdown(docsText.ToString()), codeText.ToString());
                        hasCode = false;
                        docsText = new StringBuilder();
                        codeText = new StringBuilder();
                    }

                    docsText.AppendLine(regionLineRegex.Replace(line, "####"));
                }

                if (language.CommentMatcher.IsMatch(line) && !language.CommentFilter.IsMatch(line)) {
                    if (hasCode) {
                        save(mapToMarkdown(docsText.ToString()), codeText.ToString());
                        hasCode = false;
                        docsText = new StringBuilder();
                        codeText = new StringBuilder();
                    }

                    docsText.AppendLine(language.CommentMatcher.Replace(line, ""));
                }
                else {
                    hasCode = true;
                    codeText.AppendLine(line);
                }
            }

            save(mapToMarkdown(docsText.ToString()), codeText.ToString());

            return sections;
        }

        // Prepares a single chunk of code for HTML output and runs the text of its
        // corresponding comment through **Markdown**, using a C# implementation
        // called [MarkdownSharp](http://code.google.com/p/markdownsharp/).
        static void Hightlight(List<Section> sections)
        {
            //var md = new MarkdownDeep.Markdown {
            //    ExtraMode = true,
            //    SafeMode = false
            //};

            //foreach (var section in sections) {
            //    section.DocsHtml = md.Transform(section.DocsHtml);
            //    section.CodeHtml = HttpUtility.HtmlEncode(section.CodeHtml);
            //}

            var markdown = new MarkdownSharp.Markdown();

            foreach (var section in sections) {
                section.DocsHtml = markdown.Transform(section.DocsHtml);
                section.CodeHtml = HttpUtility.HtmlEncode(section.CodeHtml);
            }
        }

        // Once all of the code is finished highlighting, we can generate the HTML file
        // and write out the documentation. Pass the completed sections into the template
        // found in `Resources/Nocco.cshtml`
        static void GenerateHtml(string source, List<Section> sections)
        {
            int depth;
            var destination = GetDestination(source, out depth);

            string pathToRoot = string.Concat(Enumerable.Repeat(".." + Path.DirectorySeparatorChar, depth));

            var htmlTemplate = Activator.CreateInstance(_templateType) as TemplateBase;

            htmlTemplate.Title = Path.GetFileName(source);
            htmlTemplate.PathToCss = Path.Combine(pathToRoot, "Nocco.css").Replace('\\', '/');
            htmlTemplate.PathToJs = Path.Combine(pathToRoot, "prettify.js").Replace('\\', '/');
            htmlTemplate.GetSourcePath = s => Path.Combine(pathToRoot, Path.ChangeExtension(s.ToLower(), ".html").Substring(2)).Replace('\\', '/');
            htmlTemplate.Sections = sections;
            htmlTemplate.Sources = _files;

            htmlTemplate.Execute();

            File.WriteAllText(destination, htmlTemplate.Buffer.ToString());
        }

        // Setup the Razor templating engine so that we can quickly pass the data in
        // and generate HTML.
        //
        // The file `Resources\Nocco.cshtml` is read and compiled into a new dll
        // with a type that extends the `TemplateBase` class. This new assembly is
        // loaded so that we can create an instance and pass data into it
        // and generate the HTML.
        static Type SetupRazorTemplate()
        {
            var host = new RazorEngineHost(new CSharpRazorCodeLanguage());
            host.DefaultBaseClass = typeof(TemplateBase).FullName;
            host.DefaultNamespace = "RazorOutput";
            host.DefaultClassName = "Template";

            host.NamespaceImports.Add("System");

            GeneratorResults razorResult = null;
            using (var reader = new StreamReader(Path.Combine(_executingDirectory, "Resources", @"linear.cshtml"))) {
                razorResult = new RazorTemplateEngine(host).GenerateCode(reader);
            }

            var compilerParams = new CompilerParameters {
                GenerateInMemory = true,
                GenerateExecutable = false,
                IncludeDebugInformation = false,
                CompilerOptions = "/target:library /optimize"
            };
            compilerParams.ReferencedAssemblies.Add(typeof(Nocco).Assembly.CodeBase.Replace("file:///", "").Replace("/", "\\"));

            var codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
            var results = codeProvider.CompileAssemblyFromDom(compilerParams, razorResult.GeneratedCode);

            // Check for errors that may have occurred during template generation
            if (results.Errors.HasErrors) {
                foreach (var err in results.Errors.OfType<CompilerError>().Where(ce => !ce.IsWarning))
                    Console.WriteLine("Error Compiling Template: ({0}, {1}) {2}", err.Line, err.Column, err.ErrorText);
            }

            Type type = results.CompiledAssembly.GetType("RazorOutput.Template");
            TemplateBase newTemplate = Activator.CreateInstance(type) as TemplateBase;
            if (newTemplate == null) { 
                throw new ApplicationException("Could not construct RazorOutput.Template or it does not inherit from TemplateBase");
            }

            return type;
        }

        // A list of the languages that Nocco supports, mapping the file extension to
        // the symbol that indicates a comment. To add another language to Nocco's
        // repertoire, add it here.
        //
        // You can also specify a list of regular expression patterns and replacements. This
        // translates things like
        // [XML documentation comments](http://msdn.microsoft.com/en-us/library/b2s063f7.aspx) into Markdown.
        static Dictionary<string, Language> Languages = new Dictionary<string, Language> {
			{ ".cs", new Language {
				Name = "csharp",
				Symbol = @"(\*|//)\s",
				Ignores = new List<string> {
					"Designer.cs"
				},
				MarkdownMaps = new Dictionary<string, string> {
					{ @"<c>([^<]*)</c>", "`$1`" },
					{ @"<param[^\>]*>([^<]*)</param>", "" },
					{ @"<returns>([^<]*)</returns>", "" },
					{ @"<see\s*cref=""([^""]*)""\s*/>", "see `$1`"},
					{ @"(</?example>|</?summary>|</?remarks>)", "" },
				}
			}}
		};

        // Get the current language we're documenting, based on the extension.
        static Language GetLanguage(string source)
        {
            var extension = Path.GetExtension(source);
            return Languages.ContainsKey(extension) ? Languages[extension] : null;
        }

        // Compute the destination HTML path for an input source file path. If the source
        // is `Example.cs`, the HTML will be at `docs/example.html`
        static string GetDestination(string filepath, out int depth)
        {
            var dirs = Path.GetDirectoryName(filepath).Substring(1).Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            depth = dirs.Length;

            var dest = Path.Combine("docs", string.Join(Path.DirectorySeparatorChar.ToString(), dirs)).ToLower();
            Directory.CreateDirectory(dest);

            return Path.Combine("docs", Path.ChangeExtension(filepath, "html").ToLower());
        }
    }
}
