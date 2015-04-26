// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.JavaTasks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Narvalo.Build.Internal;
    using Narvalo.Build.Properties;

    /// <summary>
    /// MSBuild task to execute the Closure compiler.
    /// </summary>
    public sealed class ClosureCompiler : JavaTask
    {
        /// <summary>
        /// The amount of time, in milliseconds, to wait for the associated process to exit.
        /// </summary>
        private int _processTimeout = 5000;

        /// <summary>
        /// Gets or sets the list of files to be compressed.
        /// </summary>
        /// <value>The list of files to be compressed.</value>
        [Required]
        public ITaskItem[] Files { get; set; }

        /// <summary>
        /// Gets the list of compressed files.
        /// </summary>
        /// <value>The list of compressed files.</value>
        [Output]
        public ITaskItem[] CompressedFiles { get; private set; }

        /// <summary>
        /// Gets or sets the compilation level.
        /// </summary>
        /// <value>The compilation level.</value>
        public string CompilationLevel { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, to wait for the associated process to exit.
        /// The maximum is the largest possible value of a 32-bit integer, which represents infinity to the operating system.
        /// </summary>
        /// <value>The process timeout.</value>
        public int ProcessTimeout
        {
            get { return _processTimeout; }
            set { _processTimeout = value; }
        }

        /// <summary>
        /// Gets the compilation level string.
        /// </summary>
        /// <value>The compilation level string.</value>
        private string CompilationLevelString_
        {
            get
            {
                switch (CompilationLevel)
                {
                    case "WhitespaceOnly":
                        return "WHITESPACE_ONLY";
                    case "Simple":
                        return "SIMPLE_OPTIMIZATIONS";
                    case "Advanced":
                        return "ADVANCED_OPTIMIZATIONS";
                    default:
                        return String.Empty;
                }
            }
        }

        /// <inheritdoc />
        public override bool Execute()
        {
            var compressedFiles = new List<ITaskItem>(Files.Length);

            string fullPathToTool = GenerateFullPathToTool();

            foreach (ITaskItem file in Files)
            {
                string inFile = file.ItemSpec;

                if (!File.Exists(inFile))
                {
                    Log.LogError(Format.Resource(Strings.FileNotFound_Format, inFile));
                    break;
                }

                string outFile = GetCompressedFilePath_(inFile);

                Log.LogMessage(
                    MessageImportance.Normal,
                    Format.Resource(Strings.ClosureCompiler_Processing_Format, new FileInfo(inFile).Name));

                if (File.Exists(outFile))
                {
                    File.Delete(outFile);
                }

                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo {
                        FileName = fullPathToTool,
                        Arguments = GetCommandLineArguments_(inFile, outFile),
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    process.Start();
                    process.WaitForExit(ProcessTimeout);

                    // FIXME: Terminer le processus sinon on n'a pas accès au code ExitCode.
                    if (process.ExitCode != 0)
                    {
                        LogJavaFailure(process);
                        break;
                    }

                    compressedFiles.Add(new TaskItem(outFile));
                }
            }

            CompressedFiles = compressedFiles.ToArray();

            return !Log.HasLoggedErrors;
        }

        /// <summary>
        /// Generates the command-line arguments for the Closure executable
        /// for the specified input and output files.
        /// </summary>
        /// <param name="inFile">The input file.</param>
        /// <param name="outFile">The output file.</param>
        /// <returns>The set of command-line arguments to use when starting the Closure executable.</returns>
        private string GetCommandLineArguments_(string inFile, string outFile)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"-jar ""{0}"" ", JarPath);

            if (!String.IsNullOrEmpty(CompilationLevelString_))
            {
                sb.AppendFormat(" --compilation_level {0}", CompilationLevelString_);
            }

            sb.AppendFormat(@" --js ""{0}"" --js_output_file ""{1}""", inFile, outFile);
            return sb.ToString();
        }

        /// <summary>
        /// Returns the filename for the compressed file.
        /// </summary>
        /// <param name="fileName">The name of the input file.</param>
        /// <returns>The filename for the compressed file.</returns>
        private string GetCompressedFilePath_(string fileName)
        {
            string name = fileName.Replace(".js", ".min.js");
            return String.IsNullOrEmpty(OutputDirectory) ? name : Path.Combine(OutputDirectory, new FileInfo(name).Name);
        }
    }
}
