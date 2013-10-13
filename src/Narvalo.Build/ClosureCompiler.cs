namespace Narvalo.Build
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class ClosureCompiler : JavaTaskBase
    {
        int _processTimeout = 5000;

        [Required]
        public ITaskItem[] Files { get; set; }

        [Output]
        public ITaskItem[] CompressedFiles { get; protected set; }

        //public ITaskItem[] Externs { get; set; }

        public string CompilationLevel { get; set; }

        public string OutDir { get; set; }

        public int ProcessTimeout
        {
            get { return _processTimeout; }
            set { _processTimeout = value; }
        }

        protected string CompilationLevelString
        {
            get
            {
                switch (CompilationLevel) {
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

        public override bool Execute()
        {
            var compressedFiles = new List<ITaskItem>(Files.Length);

            string fullPathToTool = GenerateFullPathToTool();

            foreach (ITaskItem file in Files) {
                string inFile = file.ItemSpec;

                if (!File.Exists(inFile)) {
                    Log.LogError("The file " + inFile + " does not exist");
                    break;
                }

                string outFile = GetCompressedFilePath(inFile);

                Log.LogMessage(MessageImportance.Normal,
                    "Closure Compiler processing " + new FileInfo(inFile).Name);

                if (File.Exists(outFile)) {
                    File.Delete(outFile);
                }

                using (var process = new Process()) {
                    process.StartInfo = new ProcessStartInfo {
                        FileName = fullPathToTool,
                        Arguments = GetCommandLineArguments(inFile, outFile),
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    process.Start();
                    process.WaitForExit(ProcessTimeout);

                    // FIXME: Terminer le processus sinon on n'a pas accès au code ExitCode.
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

        protected string GetCommandLineArguments(string inFile, string outFile)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"-jar ""{0}"" ", JarPath);

            if (!String.IsNullOrEmpty(CompilationLevelString)) {
                sb.AppendFormat(" --compilation_level {0}", CompilationLevelString);
            }

            sb.AppendFormat(@" --js ""{0}"" --js_output_file ""{1}""", inFile, outFile);
            return sb.ToString();
        }

        protected string GetCompressedFilePath(string fileName)
        {
            string name = fileName.Replace(".js", ".min.js");
            return String.IsNullOrEmpty(OutDir) ? name : Path.Combine(OutDir, new FileInfo(name).Name);
        }
    }
}
