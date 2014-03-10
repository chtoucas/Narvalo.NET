// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Razor;
    using Microsoft.CSharp;

    public sealed class RazorTemplate
    {
        const string Namespace_ = "RazorOutput";

        readonly string _path;

        Type _templateType;

        public RazorTemplate(string path)
        {
            _path = path;
        }

        public event EventHandler<CompilerErrorEventArgs> OnCompilerError;

        public string Execute(string title, IEnumerable<HtmlBlock> blocks)
        {
            var template = Activator.CreateInstance(_templateType) as TemplateBase;
            if (template == null) {
                throw new NarrativeException(
                    "The template does not inherit from TemplateBase.");
            }

            template.Title = title;
            template.Blocks = blocks;

            template.Execute();

            return template.Buffer.ToString();
        }

        public void Compile()
        {
            _templateType = Compile_(_path);
        }

        string CreateClassName_()
        {
            return "FIXME";
        }

        Type Compile_(string source)
        {
            var tmpClassName = CreateClassName_();

            var host = new RazorEngineHost(new CSharpRazorCodeLanguage());
            host.DefaultBaseClass = typeof(TemplateBase).FullName;
            host.DefaultNamespace = Namespace_;
            host.DefaultClassName = tmpClassName;
            host.NamespaceImports.Add("System");
            host.NamespaceImports.Add("System.Web");
            host.NamespaceImports.Add("Narvalo.Narrative");

            var generator = new RazorTemplateEngine(host);

            GeneratorResults generatorResults = null;
            using (var reader = new StreamReader(source)) {
                generatorResults = generator.GenerateCode(reader);
            }

            var compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false,
                IncludeDebugInformation = false,
                CompilerOptions = "/target:library /optimize"
            };

            var templateAssembly = typeof(TemplateBase).Assembly.CodeBase
                .Replace("file:///", "")
                .Replace("/", "\\");

            compilerParams.ReferencedAssemblies.Add(templateAssembly);
            compilerParams.ReferencedAssemblies.Add("System.Web.dll");

            var compilerResults = new CSharpCodeProvider()
                .CompileAssemblyFromDom(compilerParams, generatorResults.GeneratedCode);

            if (compilerResults.Errors.HasErrors) {
                OnCompilerError_(new CompilerErrorEventArgs(compilerResults.Errors));

                throw new NarrativeException("Failed to compile the template.");
            }

            var typeName = Namespace_ + "." + tmpClassName;

            return compilerResults.CompiledAssembly.GetType(typeName);
        }

        void OnCompilerError_(CompilerErrorEventArgs e)
        {
            EventHandler<CompilerErrorEventArgs> localHandler = OnCompilerError;

            if (localHandler != null) {
                localHandler(this, e);
            }
        }
    }
}
