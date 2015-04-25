// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.JavaTasks
{
    using System;
    using System.Diagnostics;
    ////using System.Runtime.CompilerServices;
    ////using System.Runtime.InteropServices;
    using System.Globalization;
    using System.IO;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Microsoft.Win32;
    using Narvalo.Build.Properties;

    public abstract class JavaTaskBase : ToolTask
    {
        private const string JRE_KEY = @"SOFTWARE\JavaSoft\Java Runtime Environment";

        protected JavaTaskBase() : base() { }

        [Required]
        public string JarPath { get; set; }

        protected override string ToolName
        {
            get
            {
                return @"java.exe";
            }
        }

        public string FindJavaPath()
        {
            string javaPath = null;

            // On commence par chercher dans la base de registre Windows 32bit.
            javaPath = FindJavaPathInRegistry_(JRE_KEY, ToolName);

            // On cherche ensuite dans l'environnement local.
            if (javaPath == null)
            {
                javaPath = FindJavaPathInPathLocations_(ToolName);
            }

            // En désespoir de cause, voyons voir dans les endroits communs.
            if (javaPath == null)
            {
                javaPath = FindJavaPathInCommonLocations_(ToolName);
            }

            if (javaPath == null)
            {
                throw new PlatformNotSupportedException(
                    "Could not find java.exe. Looked in Registry, PATH locations and various common folders inside Program Files.");
            }

            Log.LogMessage(
                MessageImportance.Low,
                String.Format(CultureInfo.CurrentCulture, Strings.JavaTask_JavaPath_Format, javaPath));

            return javaPath;
        }

        protected void LogJavaFailure(Process process)
        {
            if (process == null)
            {
                throw new ArgumentNullException();
            }

            string[] errors = process.StandardError.ReadToEnd()
                .Replace("\r", String.Empty)
                .Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string error in errors)
            {
                Log.LogMessage(
                    MessageImportance.High,
                    error.Trim().Replace("[WARNING] ", String.Empty));
            }

            Log.LogError(Strings.JavaTask_Error_Format, process.ExitCode);
        }

        protected override string GenerateFullPathToTool()
        {
            if (String.IsNullOrEmpty(ToolPath))
            {
                ToolPath = FindJavaPath();
            }

            return Path.Combine(ToolPath, ToolName);
        }

        private static string FindJavaPathInRegistry_(string keyName, string toolName)
        {
            // FIXME: ne marche pas de manière consistante en cas de virtualisation de la base de registre.
            string javaHome = null;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName))
            {
                if (key != null)
                {
                    object currentVersion
                        = key.GetValue("CurrentVersion", null, RegistryValueOptions.None);
                    using (RegistryKey subKey = key.OpenSubKey(currentVersion.ToString()))
                    {
                        if (subKey != null)
                        {
                            javaHome = subKey.GetValue("JavaHome", null, RegistryValueOptions.None) as string;
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(javaHome))
            {
                string toolPath = Path.Combine(javaHome, "bin");
                return File.Exists(Path.Combine(toolPath, toolName)) ? toolPath : null;
            }
            else
            {
                return null;
            }
        }

        private static string FindJavaPathInPathLocations_(string toolName)
        {
            string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? String.Empty;
            string[] paths = pathEnv.Split(new[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries);
            return FindJavaPathInDirectories(paths, toolName);
        }

        private static string FindJavaPathInCommonLocations_(string toolName)
        {
            // FIXME: programFilesPath dépend du type de compilation (AnyCPU, x64, x32),
            // de la plate-forme, du processus courant :
            // - pour un système x32 on obtient "C:\Program Files"
            // - pour un système x64 avec compilation AnyCPU ou x64, on obtient "C:\Program Files"
            // - pour un système x64 avec compilation x32, on obtient "C:\Program Files(x86)"
            // Peut-être Environment.ExpandEnvironmentVariables("%ProgramFiles%")
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string[] commonLocations = {
                // JRE/JDK v6
                Path.Combine(programFilesPath, @"\Java\jre6\bin"),
                @"C:\Program Files\Java\jre6\bin",

                // JRE/JDK v7
                Path.Combine(programFilesPath, @"\Java\jre7\bin"),
                @"C:\Program Files\Java\jre7\bin",
            };

            return FindJavaPathInDirectories(commonLocations, toolName);
        }

        private static string FindJavaPathInDirectories(string[] paths, string toolName)
        {
            string javaPath = null;

            foreach (string path in paths)
            {
                string fullPath = Path.Combine(path, toolName);
                if (File.Exists(fullPath))
                {
                    javaPath = path;
                    break;
                }
            }

            return javaPath;
        }

        //// FIXME: does not work on x64 systems

        //// String x86folder = Environment.GetEnvironmentVariable("ProgramFiles(x86)");

        ////static string ProgramFilesx86() {
        ////    if (8 == IntPtr.Size
        ////        || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")))) {
        ////        return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
        ////    }

        ////    return Environment.GetEnvironmentVariable("ProgramFiles");
        ////}

        //// IntPtr.Size won't return the correct value if running in 32-bit .NET Framework 2.0
        //// on 64-bit Windows (it would return 32-bit).
        //// As Microsoft's Raymond Chen describes, you have to first check if running in a
        //// 64-bit process (I think in .NET you can do so by checking IntPtr.Size), and if
        //// you are running in a 32-bit process, you still have to call the Win API function
        //// IsWow64Process. If this returns true, you are running in a 32-bit process on 64-bit Windows.
        //// See : How to detect programmatically whether you are running on 64-bit Windows
        //// http://blogs.msdn.com/b/oldnewthing/archive/2005/02/01/364563.aspx

        ////private static readonly string Jre64Key = @"SOFTWARE\Wow6432Node\JavaSoft\Java Runtime Environment";
        ////private static bool Is64BitProcess = (IntPtr.Size == 8);
        ////private static bool Is64BitOperatingSystem = Is64BitProcess || InternalCheckIsWow64();

        ////[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        ////[return: MarshalAs(UnmanagedType.Bool)]
        ////public static extern bool IsWow64Process(
        ////    [In] IntPtr hProcess,
        ////    [Out] out bool wow64Process
        ////);

        ////[MethodImpl(MethodImplOptions.NoInlining)]
        ////private static bool InternalCheckIsWow64() {
        ////    if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1)
        ////        || Environment.OSVersion.Version.Major >= 6) {
        ////        using (Process p = Process.GetCurrentProcess()) {
        ////            bool retVal;
        ////            if (!IsWow64Process(p.Handle, out retVal)) {
        ////                return false;
        ////            }
        ////            return retVal;
        ////        }
        ////    }
        ////    else {
        ////        return false;
        ////    }
        ////}
    }
}
