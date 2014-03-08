// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Linq;
    using System.Web.Razor;
    using Microsoft.CSharp;

    public class TemplateCreator
    {
        public TemplateBase Create(string source)
        {
            var templateType = Compile_(source);
            var template = Activator.CreateInstance(templateType) as TemplateBase;
            if (template == null) {
                throw new ApplicationException("Could not construct RazorOutput.Template or it does not inherit from TemplateBase");
            }

            return template;
        }

        Type Compile_(string source)
        {
            var type = typeof(TemplateBase);

            var host = new RazorEngineHost(new CSharpRazorCodeLanguage());
            host.DefaultBaseClass = typeof(TemplateBase).FullName;
            host.DefaultNamespace = "RazorOutput";
            host.DefaultClassName = "Template";

            host.NamespaceImports.Add("System");

            GeneratorResults generatorResults = null;
            using (var reader = new StreamReader(source)) {
                generatorResults = new RazorTemplateEngine(host).GenerateCode(reader);
            }

            var compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = false,
                IncludeDebugInformation = false,
                CompilerOptions = "/target:library /optimize"
            };

            compilerParams.ReferencedAssemblies.Add(type.Assembly.CodeBase.Replace("file:///", "").Replace("/", "\\"));

            var compilerResults = new CSharpCodeProvider()
                .CompileAssemblyFromDom(compilerParams, generatorResults.GeneratedCode);

            if (compilerResults.Errors.HasErrors) {
                foreach (var err in compilerResults.Errors.OfType<CompilerError>().Where(ce => !ce.IsWarning)) {
                    // FIXME
                    Console.WriteLine("Error Compiling Template: ({0}, {1}) {2}", err.Line, err.Column, err.ErrorText);
                }
            }

            return compilerResults.CompiledAssembly.GetType("RazorOutput.Template");
        }
    }
}
