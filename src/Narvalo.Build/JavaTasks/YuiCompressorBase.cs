// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.JavaTasks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Narvalo.Build.Internal;
    using Narvalo.Build.Properties;

    /// <summary>
    /// Provides a base class for YUI tasks.
    /// </summary>
    public abstract class YuiCompressorBase : JavaTaskBase
    {
        /// <summary>
        /// The max-length of lines in the output files.
        /// </summary>
        private int _lineBreak = 0;

        /// <summary>
        /// The amount of time, in milliseconds, to wait for the associated process to exit.
        /// </summary>
        private int _processTimeout = 5000;

        /// <summary>
        /// Initializes a new instance of the <see cref="YuiCompressorBase"/> class.
        /// </summary>
        protected YuiCompressorBase() : base() { }

        /// <summary>
        /// Gets or sets the list of files to be compressed.
        /// </summary>
        /// <value>The list of files to be compressed.</value>
        [Required]
        public ITaskItem[] Files { get; set; }

        /// <summary>
        /// Gets or sets the list of compressed files.
        /// </summary>
        /// <value>The list of compressed files.</value>
        [Output]
        public ITaskItem[] CompressedFiles { get; protected set; }

        /// <summary>
        /// Gets or sets the max-length of lines in the output files.
        /// </summary>
        /// <remarks>Specify 0 to get a line break after each semi-colon in JavaScript, and
        /// after each rule in CSS.</remarks>
        /// <value>The max-length of lines in the output files.</value>
        public int LineBreak
        {
            get { return _lineBreak; }
            set { _lineBreak = value; }
        }

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
        /// Gets or sets a value indicating whether informational messages and warnings are displayed.
        /// </summary>
        /// <value><see langword="true"/> if verbose output is enabled; otherwise, <see langword="false"/></value>
        public bool Verbose { get; set; }

        /// <summary>
        /// Gets the filename extension.
        /// </summary>
        /// <value>The filename extension.</value>
        protected abstract string FileExtension { get; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns><see langword="true"/> if the task successfully executed; otherwise, 
        /// <see langword="false"/>.</returns>
        public override bool Execute()
        {
            var compressedFiles = new List<ITaskItem>(Files.Length);

            string javaExe = GenerateFullPathToTool();

            foreach (ITaskItem file in Files)
            {
                string inFile = file.ItemSpec;

                if (!File.Exists(inFile))
                {
                    Log.LogError(Format.Resource(Strings.FileNotFound_Format, inFile));
                    break;
                }

                string outFile = GenerateCompressedFilePath(inFile);

                Log.LogMessage(
                    MessageImportance.Normal,
                    Format.Resource(Strings.YuiCompressorBase_Processing_Format, new FileInfo(inFile).Name));

                if (File.Exists(outFile))
                {
                    File.Delete(outFile);
                }

                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo {
                        FileName = javaExe,
                        Arguments = GenerateCommandLineArguments(inFile, outFile),
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                    };
                    process.Start();
                    process.WaitForExit(ProcessTimeout);

                    if (process.ExitCode != 0)
                    {
                        LogJavaFailure(process);
                        return false;
                    }

                    compressedFiles.Add(new TaskItem(outFile));
                }
            }

            CompressedFiles = compressedFiles.ToArray();

            return !Log.HasLoggedErrors;
        }

        /// <summary>
        /// Generates the command-line arguments for the YUI executable
        /// for the specified input and output files.
        /// </summary>
        /// <param name="inFile">The input file.</param>
        /// <param name="outFile">The output file.</param>
        /// <returns>The set of command-line arguments to use when starting the YUI executable.</returns>
        protected abstract string GenerateCommandLineArguments(string inFile, string outFile);

        /// <summary>
        /// Returns the filename for the compressed file.
        /// </summary>
        /// <param name="fileName">The name of the input file.</param>
        /// <returns>The filename for the compressed file.</returns>
        protected string GenerateCompressedFilePath(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName", Format.Resource(Strings.ArgumentNull_Format, "fileName"));
            }

            if (String.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(Format.Resource(Strings.ArgumentWhiteSpace_Format, "fileName"), "fileName");
            }

            string name = fileName.Replace("." + FileExtension, ".min." + FileExtension);
            return String.IsNullOrEmpty(OutputDirectory) ? name : Path.Combine(OutputDirectory, new FileInfo(name).Name);
        }
    }
}
