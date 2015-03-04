// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Text;
    using Microsoft.JScript;

    // Cf. http://madskristensen.net/post/Verify-JavaScript-syntax-using-C.aspx"
    public class JScriptCompiler : IDisposable
    {
        JScriptCodeProvider _provider = new JScriptCodeProvider();
        CompilerParameters _parameters = new CompilerParameters();
        StringBuilder _errorMessages = new StringBuilder();

        /// <summary>
        /// A list of error codes to ignore.
        /// </summary>
        /// <remarks>
        /// See all error codes at http://msdn.microsoft.com/en-us/library/9c5scey5(VS.71).aspx
        /// </remarks>
        string[] ignoredRules = new string[] {
            "JS1135", "JS1028", "JS1234", "JS5040", "JS1151", "JS0438", "JS1204"
        };

        /// <summary>
        /// Gets the error message from the compiler.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage
        {
            get { return _errorMessages.ToString(); }
        }

        /// <summary>
        /// Gets a value indicating whether the compilation encountered errors.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the Javascript files have errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors
        {
            get { return _errorMessages.Length > 0; }
        }

        /// <summary>
        /// Compiles the specified Javascript files.
        /// </summary>
        /// <param name="files">The absolute file paths to .js files.</param>
        public void Compile(params string[] files)
        {
            foreach (string file in files) {
                using (StreamReader reader = File.OpenText(file)) {
                    string source = reader.ReadToEnd();
                    CompilerResults results = _provider.CompileAssemblyFromSource(_parameters, source);

                    foreach (CompilerError error in results.Errors) {
                        if (Array.IndexOf(ignoredRules, error.ErrorNumber) == -1)
                            WriteError(file, error);
                    }
                }
            }
        }

        private void WriteError(string file, CompilerError error)
        {
            _errorMessages.AppendLine("JavaScript compilation failed");
            _errorMessages.AppendLine("\tError code: " + error.ErrorNumber);
            _errorMessages.AppendLine("\t" + error.ErrorText);
            _errorMessages.AppendLine("\t" + file + "(" + error.Line + ", " + error.Column + ")");
            _errorMessages.AppendLine();
        }

        #region IDisposable

        private void Dispose(bool disposing)
        {
            if (disposing) {
                _provider.Dispose();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        #endregion
    }
}
