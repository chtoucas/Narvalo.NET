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

    delegate void MessageLogger(string errorCode, string file, int lineNumber, string message);

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
    public class FxCop : Task
    {
        string currentDirectory 
            = Directory.GetCurrentDirectory().ToUpperInvariant().TrimEnd('\\') + "\\";
        //private Decoder decoder = new Decoder();

        public FxCop()
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


/*
    Microsoft (R) FxCop Command-Line Tool, Version 10.0 (10.0.30319.1) X86
    Copyright (C) Microsoft Corporation, All Rights Reserved.


    More help on command-line options:

    /file:<file/directory>  [Short form: /f:<file/directory>]
    Assembly file(s) to analyze. 

    /rule:<[+|-]file / directory >  [Short form: /r:<[+|-]file / directory >]
    Directory containing rule assemblies or path to rule assembly. '+' enables all 
    rules, '-' disables all rules. 

    /ruleid:<[+|-]Namespace.CheckId|Category#CheckId>
     [Short form: /rid:<[+|-]Namespace.CheckId|Category#CheckId>]
    Namespace and CheckId strings that identify a Rule. Category and CheckId 
    strings can be used instead to identify a Rule, but this format has been 
    deprecated. '+' enables the rule; '-' disables the rule. 

    /ruleset:<<+|-|=>file>  [Short form: /rs:<<+|-|=>file>]
    Rule set to be used for the analysis. It can be a file path to the rule set 
    file or the file name of a built-in rule set. '+' enables all rules in the 
    rule set; '-' disables all rules in the rule set; '=' sets rules to match the 
    rule set and disables all rules that are not enabled in the rule set. 

    /rulesetdirectory:<directory>  [Short form: /rsd:<directory>]
    Directory to search for rule set files that are specified by the /ruleset 
    switch or are included by one of the specified rule sets. 

    /out:<file>  [Short form: /o:<file>]
    FxCop project or XML report output file. 

    /outxsl:<file>  [Short form: /oxsl:<file>]
    Reference the specified XSL in the XML report file; use /outxsl:none to 
    generate an XML report with no XSL style sheet. 

    /applyoutxsl  [Short form: /axsl]
    Apply the XSL style sheet to the output. 

    /project:<file>  [Short form: /p:<file>]
    Project file to load. 

    /platform:<directory>  [Short form: /plat:<directory>]
    Location of platform assemblies. 

    /directory:<directory>  [Short form: /d:<directory>]
    Location to search for assembly dependencies. 

    /reference:<file>  [Short form: /ref:<file>]
    Reference assemblies required for analysis. 

    /types:<type list>  [Short form: /t:<type list>]
    Analyze only these types and members. 

    /import:<file/directory>  [Short form: /i:<file/directory>]
    import XML report(s) or FxCop project file(s) 

    /summary  [Short form: /s]
    Display summary after analysis. 

    /verbose  [Short form: /v]
    Give verbose output during analysis. 

    /update  [Short form: /u]
    Update the project file if there are any changes. 

    /console  [Short form: /c]
    Output messages to console, including file and line number information. 

    /consolexsl:<file>  [Short form: /cxsl:<file>]
    Apply specified XSL to console output. 

    /forceoutput  [Short form: /fo]
    Write output XML and project files even in the case where no violations 
    occurred. 

    /dictionary:<file>  [Short form: /dic:<file>]
    Custom dictionary used by spelling rules. 

    /quiet  [Short form: /q]
    Suppress all console output other than the reporting implied by /console or 
    /consolexsl. 

    /ignoreinvalidtargets  [Short form: /iit]
    Silently ignore invalid target files. 

    /aspnet  [Short form: /asp]
    Analyze only ASP.NET-generated binaries and honor global suppressions in 
    App_Code.dll for all assemblies under analysis. 

    /searchgac  [Short form: /gac]
    Search Global Assembly Cache for missing references. 

    /successfile  [Short form: /sf]
    Create .lastcodeanalysissucceeded file in output report directory if no 
    build-breaking messages occur during analysis. 

    /timeout:<seconds>  [Short form: /to:<seconds>]
    Override timeout for analysis deadlock detection. Analysis will be aborted 
    when analysis of a single item by a single rule exceeds the specified amount 
    of time. Specify a value of 0 to disable deadlock detection. 

    /savemessagestoreport:<Active|Excluded|Absent (default: Active)>
     [Short form: /smr:<Active|Excluded|Absent (default: Active)>]
    Save messages of specified kind to output report. 

    /ignoregeneratedcode  [Short form: /igc]
    Suppress analysis results against generated code. 

    /overriderulevisibilities  [Short form: /orv]
    Run all overridable rules against all targets. 

    /failonmissingrules  [Short form: /fmr]
    Treat missing rules or rule sets as an error and halt execution. 

    /culture  [Short form: /cul]
    Culture for spelling rules. 

    /help  [Short form: /?]
    Display this help message. 

*/