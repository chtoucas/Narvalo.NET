// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Templating
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Linq;
    using System.Web.Razor;
    using Microsoft.CSharp;
    using Narvalo;

    public sealed class RazorTemplateCompiler
    {
        const string Namespace_ = "RazorOutput";

        string _input;
        string _className;

        public RazorTemplateCompiler(string input)
        {
            Require.NotNullOrEmpty(input, "input");

            _input = input;
        }

        string ClassName_
        {
            get
            {
                if (_className == null) {
                    _className = GenerateUniqueClassName_();
                }

                return _className;
            }
        }

        string TypeFullName_
        {
            get { return Namespace_ + "." + ClassName_; }
        }

        public Type Compile()
        {
            var codeCompileUnit = GenerateCode_();
            var compilerResults = CompileCode_(codeCompileUnit);

            return compilerResults.CompiledAssembly.GetType(TypeFullName_);
        }

        static string GenerateUniqueClassName_()
        {
            return "Template" + Guid.NewGuid().ToString("N");
        }

        static CompilerResults CompileCode_(CodeCompileUnit codeCompileUnit)
        {
            var compilerParams = new CompilerParameters
            {
                CompilerOptions = "/target:library /optimize",
                GenerateInMemory = true,
                GenerateExecutable = false,
                IncludeDebugInformation = false,
            };

            var templateAssembly = typeof(RazorTemplateBase).Assembly
                .CodeBase.Replace("file:///", String.Empty);

            compilerParams.ReferencedAssemblies.Add(templateAssembly);
            compilerParams.ReferencedAssemblies.Add("System.Web.dll");

            var codeProvider = new CSharpCodeProvider();
            var compilerResults = codeProvider
                .CompileAssemblyFromDom(compilerParams, codeCompileUnit);

            if (compilerResults.Errors.HasErrors) {
                var exceptions = compilerResults.Errors
                    .OfType<CompilerError>()
                    .Where(error => !error.IsWarning)
                    .Select(error => new TemplateException(error.ErrorText)
                    {
                        Column = error.Column,
                        Line = error.Line,
                    });

                throw new TemplateException(
                    "Failed to compile the template.",
                    new AggregateException(exceptions));
            }

            return compilerResults;
        }

        CodeCompileUnit GenerateCode_()
        {
            var host = new RazorEngineHost(new CSharpRazorCodeLanguage());
            host.DefaultNamespace = Namespace_;
            host.DefaultBaseClass = typeof(RazorTemplateBase).FullName;
            host.DefaultClassName = ClassName_;
            host.NamespaceImports.Add("System");
            host.NamespaceImports.Add("Narrative");

            var generator = new RazorTemplateEngine(host);

            GeneratorResults generatorResults = null;
            using (var reader = new StringReader(_input)) {
                generatorResults = generator.GenerateCode(reader);
            }

            return generatorResults.GeneratedCode;
        }
    }
}
