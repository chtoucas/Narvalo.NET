// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Internal
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Razor;
    using Microsoft.CSharp;

    public static class TemplateCompiler
    {
        const string Namespace_ = "RazorOutput";

        public static Type Compile(string path)
        {
            var className = CreateUniqueClassName_(path);
            var codeCompileUnit = GenerateCode_(path, className);
            var compilerResults = Compile_(codeCompileUnit);

            var typeName = Namespace_ + "." + className;

            return compilerResults.CompiledAssembly.GetType(typeName);
        }

        static string CreateUniqueClassName_(string path)
        {
            return Path.GetFileNameWithoutExtension(path) + "_" + Guid.NewGuid().ToString("N");
        }

        static CompilerResults Compile_(CodeCompileUnit codeCompileUnit)
        {
            var compilerParams = new CompilerParameters
            {
                CompilerOptions = "/target:library /optimize",
                GenerateInMemory = true,
                GenerateExecutable = false,
                IncludeDebugInformation = false,
            };

            var templateAssembly = typeof(TemplateBase).Assembly
                .CodeBase.Replace("file:///", "");

            compilerParams.ReferencedAssemblies.Add(templateAssembly);
            compilerParams.ReferencedAssemblies.Add("System.Web.dll");

            var codeProvider = new CSharpCodeProvider();
            var compilerResults = codeProvider
                .CompileAssemblyFromDom(compilerParams, codeCompileUnit);

            if (compilerResults.Errors.HasErrors) {
                var exceptions = new List<CompilerException>();

                var errors = compilerResults.Errors.OfType<CompilerError>().Where(error => !error.IsWarning);

                foreach (var error in errors) {
                    exceptions.Add(new CompilerException(error.ErrorText)
                    {
                        Column = error.Column,
                        Line = error.Line,
                    });
                }

                throw new NarrativeException(
                    "Failed to compile the template.",
                    new AggregateException(exceptions));
            }

            return compilerResults;
        }

        static CodeCompileUnit GenerateCode_(string path, string className)
        {
            var host = new RazorEngineHost(new CSharpRazorCodeLanguage());
            host.DefaultNamespace = Namespace_;
            host.DefaultBaseClass = typeof(TemplateBase).FullName;
            host.DefaultClassName = className;
            host.NamespaceImports.Add("System");
            host.NamespaceImports.Add("Narvalo.Narrative");

            var generator = new RazorTemplateEngine(host);

            GeneratorResults generatorResults = null;
            using (var reader = new StreamReader(path)) {
                generatorResults = generator.GenerateCode(reader);
            }

            return generatorResults.GeneratedCode;
        }
    }
}
