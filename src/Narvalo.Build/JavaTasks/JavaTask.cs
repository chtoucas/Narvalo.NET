// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.JavaTasks
{
    using System;
    using System.Diagnostics;
    ////using System.Runtime.CompilerServices;
    ////using System.Runtime.InteropServices;
    using System.IO;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Microsoft.Win32;
    using Narvalo.Build.Internal;
    using Narvalo.Build.Properties;

    /// <summary>
    /// Provides a base class for all java tasks.
    /// </summary>
    public abstract class JavaTask : ToolTask
    {
        /// <summary>
        /// The default registry key for the Java Runtime Environment.
        /// </summary>
        private const string JRE_KEY = @"SOFTWARE\JavaSoft\Java Runtime Environment";

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaTask"/> class.
        /// </summary>
        protected JavaTask() : base() { }

        /// <summary>
        /// Gets or sets the path to the jar file.
        /// </summary>
        /// <value>The path to the jar file.</value>
        [Required]
        public string JarPath { get; set; }

        /// <summary>
        /// Gets the path to the java executable.
        /// </summary>
        /// <value>The path to the java executable.</value>
        protected override string ToolName
        {
            get
            {
                return @"java.exe";
            }
        }

        /// <summary>
        /// Find the path to the installed java executable.
        /// </summary>
        /// <returns>The path to the java executable; <see langword="null"/> if none found.</returns>
        public string FindJavaPath()
        {
            string javaPath = null;

            // On commence par chercher dans la base de registre Windows 32bit.
            javaPath = FindJavaPathInRegistry();

            // On cherche ensuite dans l'environnement local.
            if (javaPath == null)
            {
                javaPath = FindJavaPathInPathLocations();
            }

            // En désespoir de cause, voyons voir dans les endroits communs.
            if (javaPath == null)
            {
                javaPath = FindJavaPathInCommonLocations();
            }

            if (javaPath == null)
            {
                throw new PlatformNotSupportedException(Format.Current(Strings.JavaTaskBase_JavaNotFound));
            }

            Log.LogMessage(
                MessageImportance.Low,
                Format.Current(Strings.JavaTaskBase_JavaPath_Format, javaPath));

            return javaPath;
        }

        /// <summary>
        /// Log a java error.
        /// </summary>
        /// <param name="process">The java process.</param>
        protected void LogJavaFailure(Process process)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process", Format.Current(Strings.ArgumentNull_Format, "process"));
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

            Log.LogError(Format.Current(Strings.JavaTaskBase_Error_Format, process.ExitCode));
        }

        /// <summary>
        /// Returns the path to the java executable.
        /// </summary>
        /// <returns>The path to the java executable.</returns>
        protected override string GenerateFullPathToTool()
        {
            if (String.IsNullOrEmpty(ToolPath))
            {
                ToolPath = FindJavaPath();
            }

            return Path.Combine(ToolPath, ToolName);
        }

        /// <summary>
        /// Find the path to the java executable in the Windows registry.
        /// </summary>
        /// <returns>The path to the java executable; <see langword="null"/> if none found.</returns>
        private string FindJavaPathInRegistry()
        {
            // FIXME: ne marche pas de manière consistante en cas de virtualisation de la base de registre.
            string javaHome = null;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(JRE_KEY))
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
                return File.Exists(Path.Combine(toolPath, ToolName)) ? toolPath : null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find the path to the java executable using the PATH environment variable.
        /// </summary>
        /// <returns>The path to the java executable; <see langword="null"/> if none found.</returns>
        private string FindJavaPathInPathLocations()
        {
            string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? String.Empty;
            string[] paths = pathEnv.Split(new[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries);
            return FindJavaPathInDirectories(paths);
        }

        /// <summary>
        /// Find the path to the java executable in common locations.
        /// </summary>
        /// <returns>The path to the java executable; <see langword="null"/> if none found.</returns>
        private string FindJavaPathInCommonLocations()
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

            return FindJavaPathInDirectories(commonLocations);
        }

        /// <summary>
        /// Find the path to the java executable in the specified directories.
        /// </summary>
        /// <param name="paths">Directories where to look for the java executable.</param>
        /// <returns>The path to the java executable; <see langword="null"/> if none found.</returns>
        private string FindJavaPathInDirectories(string[] paths)
        {
            string javaPath = null;

            foreach (string path in paths)
            {
                string fullPath = Path.Combine(path, ToolName);
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
