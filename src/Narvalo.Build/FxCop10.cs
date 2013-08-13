namespace Narvalo.Build
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Xml.Linq;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    //delegate void MessageLogger(string errorCode, string file, int lineNumber, string message);

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="http://bradwilson.typepad.com/blog/2010/04/writing-an-fxcop-task-for-msbuild.html"/>
    /// <seealso cref="http://www.ademiller.com/blogs/tech/2009/06/making-sure-fxcop-warnings-and-errors-break-the-build/"/>
    /// <example>
    /// <Project DefaultTargets="FxCop"
    ///   xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
    ///   <UsingTask AssemblyFile="bin\Debug\FxCopTask.dll" TaskName="FxCop" />
    ///   <Target Name="FxCop">
    ///     <MSBuild Projects="FxCopTask.csproj"
    ///       Targets="Build"
    ///       Properties="Configuration=FxCop" />
    ///     <FxCop Assemblies="bin\FxCop\FxCopTask.dll" />
    ///   </Target>
    /// </Project>
    /// </example>
    public class FxCop10 : Task
    {
        string currentDirectory 
            = Directory.GetCurrentDirectory().ToUpperInvariant().TrimEnd('\\') + "\\";
        // Decoder decoder = new Decoder();

        public FxCop10()
        {
            FailOnError = true;
            FailOnWarning = false;
            RuleSet = "AllRules.ruleset";

            // Pre-fill information from Visual Studio 2010, if available
            string vs10ToolsPath = Environment.GetEnvironmentVariable("VS100COMNTOOLS");
            if (vs10ToolsPath != null) {
                string executable = Path.Combine(vs10ToolsPath, @"..\..\Team Tools\Static Analysis Tools\FxCop\FxCopCmd.exe");
                if (File.Exists(executable)) {
                    Executable = new TaskItem(executable);
                }

                string ruleSetDirectory = Path.Combine(vs10ToolsPath, @"..\..\Team Tools\Static Analysis Tools\Rule Sets");
                if (Directory.Exists(ruleSetDirectory)) {
                    RuleSetDirectory = new TaskItem(ruleSetDirectory);
                }
            }
        }

        [Required]
        public ITaskItem[] Assemblies { get; set; }

        public ITaskItem Dictionary { get; set; }

        public ITaskItem Executable { get; set; }

        public bool FailOnError { get; set; }

        public bool FailOnWarning { get; set; }

        public ITaskItem Output { get; set; }

        public string RuleSet { get; set; }

        public ITaskItem RuleSetDirectory { get; set; }

        public override bool Execute()
        {
            if (Executable == null) {
                Log.LogError("Executable was not set, and FxCop from Visual Studio 2010 could not be located");
                return false;
            }
            if (RuleSetDirectory == null) {
                Log.LogError("RuleSetDirectory was not set, and FxCop from Visual Studio 2010 could not be located");
                return false;
            }

            bool deleteOutput = false;

            if (Output == null) {
                deleteOutput = true;
                Output = new TaskItem(Path.GetTempFileName());
            }

            try {
                foreach (ITaskItem assembly in Assemblies) {
                    Log.LogMessage(MessageImportance.High, "Running FxCop on \"{0}\"", GetRelativePath(assembly.GetMetadata("FullPath")));
                }

                using (Process proc = CreateProcess()) {
                    proc.Start();
                    proc.WaitForExit();

                    if (proc.ExitCode != 0) {
                        LogExecutionFailure(proc);
                        return false;
                    }
                }

                return ParseXml();
            }
            finally {
                if (deleteOutput) {
                    File.Delete(Output.GetMetadata("FullPath"));
                }
            }
        }

        Process CreateProcess()
        {
            Process process = new Process {
                StartInfo = new ProcessStartInfo {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = Executable.GetMetadata("FullPath"),
                    Arguments = String.Format(
                        CultureInfo.InvariantCulture,
                        @"/q /iit /fo /igc /gac /rs:""={0}"" /rsd:""{1}"" /o:""{2}""",
                        RuleSet,
                        RuleSetDirectory.GetMetadata("FullPath"),
                        Output.GetMetadata("FullPath")
                    )
                }
            };

            if (Dictionary != null) {
                process.StartInfo.Arguments += String.Format(
                    CultureInfo.InvariantCulture, @" /dic:""{0}""", Dictionary.GetMetadata("FullPath"));
            }

            foreach (ITaskItem assembly in Assemblies) {
                process.StartInfo.Arguments += String.Format(
                    CultureInfo.InvariantCulture, @" /f:""{0}""", assembly.GetMetadata("FullPath"));
            }

            return process;
        }

        static string GetErrorCode(XElement xml)
        {
            string id = (string)xml.Attribute("CheckId");
            string category = (string)xml.Attribute("Category");
            return String.Format(CultureInfo.InvariantCulture, "{0} ({1})", id, category);
        }

        static string GetLevel(XElement xml)
        {
            return (string)xml.Attribute("Level");
        }

        static int GetLineNumber(XElement xml)
        {
            string line = (string)xml.Attribute("Line");
            if (line == null) {
                return 0;
            }

            return Int32.Parse(line, CultureInfo.InvariantCulture);
        }

        static string GetMessage(XElement xml)
        {
            return HtmlDecode((string)xml.FirstNode.ToString().Replace("\r\n", "\n"));
            //return decoder.Decode((string)xml.FirstNode.ToString().Replace("\r\n", "\n"));
        }

        static string HtmlDecode(string value)
        {
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture)) {
                HttpUtility.HtmlDecode(value, writer);
                return writer.ToString();
            }
        }

        string GetFilename(string assembly, XElement xml)
        {
            string result = assembly;
            string path = (string)xml.Attribute("Path");
            string file = (string)xml.Attribute("File");

            if (path != null && file != null) {
                result = Path.Combine(path, file);
            }

            return GetRelativePath(result);
        }

        string GetRelativePath(string path)
        {
            if (path.ToUpperInvariant().StartsWith(currentDirectory, StringComparison.OrdinalIgnoreCase)) {
                path = path.Substring(currentDirectory.Length);
            }

            return path;
        }

        void LogExecutionFailure(Process proc)
        {
            string line;
            while ((line = proc.StandardOutput.ReadLine()) != null) {
                Log.LogMessage(MessageImportance.High, line);
            }
            while ((line = proc.StandardError.ReadLine()) != null) {
                Log.LogMessage(MessageImportance.High, line);
            }

            Log.LogError("FxCop exited with error code {0}", proc.ExitCode);
        }

        bool ParseXml()
        {
            bool hasErrors = false;
            bool hasWarnings = false;
            MessageLogger errorLogger = (errorCode, file, lineNumber, message) => Log.LogError("FxCop", errorCode, null, file, lineNumber, 0, 0, 0, message);
            MessageLogger warningLogger = (errorCode, file, lineNumber, message) => Log.LogWarning("FxCop", errorCode, null, file, lineNumber, 0, 0, 0, message);
            XElement root = XElement.Load(Output.GetMetadata("FullPath"));

            foreach (XElement targetXml in root.Descendants("Target")) {
                string assembly = (string)targetXml.Attribute("Name");

                foreach (XElement messageXml in targetXml.Descendants("Message")) {
                    string errorCode = GetErrorCode(messageXml);

                    foreach (XElement issueXml in messageXml.Descendants("Issue")) {
                        string level = GetLevel(issueXml);
                        MessageLogger logger;

                        if (level.ToUpperInvariant().EndsWith("ERROR", StringComparison.OrdinalIgnoreCase)) {
                            logger = FailOnError ? errorLogger : warningLogger;
                            hasErrors = true;
                        }
                        else if (level.ToUpperInvariant().EndsWith("WARNING", StringComparison.OrdinalIgnoreCase)) {
                            logger = FailOnWarning ? errorLogger : warningLogger;
                            hasWarnings = true;
                        }
                        else {
                            logger = warningLogger;
                        }

                        string file = GetFilename(assembly, issueXml);
                        string message = GetMessage(issueXml);
                        int lineNumber = GetLineNumber(issueXml);

                        logger(errorCode, file, lineNumber, message);
                    }
                }
            }

            return (!((hasErrors && FailOnError) || (hasWarnings && FailOnWarning)));
        }
    }

    //class Decoder : HttpEncoder {
    //    public string Decode(string value) {
    //        using (StringWriter writer = new StringWriter()) {
    //            this.HtmlDecode(value, writer);
    //            return writer.ToString();
    //        }
    //    }
    //}
}
