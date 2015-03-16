// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    // Maybe we should use a separate AppDomain: AppDomainIsolatedTask
    public abstract class YuiCompressorBase : JavaTaskBase
    {
        private int _lineBreak = 0;
        private int _processTimeout = 5000;
        private bool _verbose = false;

        protected YuiCompressorBase() : base() { }

        [Required]
        public ITaskItem[] Files { get; set; }

        [Output]
        public ITaskItem[] CompressedFiles { get; protected set; }

        public int LineBreak
        {
            get { return _lineBreak; }
            set { _lineBreak = value; }
        }

        public string OutputDirectory { get; set; }

        public int ProcessTimeout
        {
            get { return _processTimeout; }
            set { _processTimeout = value; }
        }

        public bool Verbose
        {
            get { return _verbose; }
            set { _verbose = value; }
        }

        protected abstract string FileExtension { get; }

        public override bool Execute()
        {
            var compressedFiles = new List<ITaskItem>(Files.Length);

            string javaExe = GenerateFullPathToTool();

            foreach (ITaskItem file in Files) {
                string inFile = file.ItemSpec;

                if (!File.Exists(inFile)) {
                    Log.LogError(String.Format(CultureInfo.CurrentCulture, Strings_Build.FileNotFoundFomat, inFile));
                    break;
                }

                string outFile = GenerateCompressedFilePath(inFile);

                Log.LogMessage(
                    MessageImportance.Normal,
                    String.Format(CultureInfo.CurrentCulture, Strings_Build.YuiCompressor_ProcessingFormat, new FileInfo(inFile).Name));

                if (File.Exists(outFile)) {
                    File.Delete(outFile);
                }

                using (var process = new Process()) {
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

                    if (process.ExitCode != 0) {
                        LogJavaFailure(process);
                        return false;
                    }

                    compressedFiles.Add(new TaskItem(outFile));
                }
            }

            CompressedFiles = compressedFiles.ToArray();

            return !Log.HasLoggedErrors;
        }

        protected abstract string GenerateCommandLineArguments(string inFile, string outFile);

        protected string GenerateCompressedFilePath(string fileName)
        {
            Require.NotNullOrEmpty(fileName, "fileName");

            string name = fileName.Replace("." + FileExtension, ".min." + FileExtension);
            return String.IsNullOrEmpty(OutputDirectory) ? name : Path.Combine(OutputDirectory, new FileInfo(name).Name);
        }
    }
}
