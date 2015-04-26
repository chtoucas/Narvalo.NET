// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.JavaTasks
{
    using System.Text;

    /// <summary>
    /// MSBuild task to execute the YUI compressor on JavaScript files.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <ItemGroup>
    ///     <JavaScriptFiles Include="$(SourceWebPhysicalPath)\Scripts\*.js" />
    /// </ItemGroup>
    /// <Target Name="AfterBuild">
    ///     <JavaScriptYuiCompressor Files="@(JavaScriptFiles)" />
    /// </Target>
    /// ]]>
    /// </code>
    /// </example>
    public sealed class JavaScriptYuiCompressor : YuiCompressor
    {
        /// <summary>
        /// Gets or sets a value indicating whether optimizations are enabled. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if optimizations are enabled; otherwise, <see langword="false"/></value>
        public bool DisableOptimizations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether obfuscation is enabled. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if obfuscation is enabled; otherwise, <see langword="false"/></value>
        public bool NoMunge { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether semicolons are preserved. Default to false.
        /// </summary>
        /// <value><see langword="true"/> if semicolons are preserved; otherwise, <see langword="false"/></value>
        public bool PreserveSemi { get; set; }

        /// <inheritdoc />
        protected override string FileExtension
        {
            get { return "js"; }
        }

        /// <inheritdoc />
        protected override string GenerateCommandLineArguments(string inFile, string outFile)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"-jar ""{0}"" --type js --charset utf8", JarPath);

            if (DisableOptimizations) {
                sb.Append(" --disable-optimizations");
            }

            if (LineBreak > 0) {
                sb.AppendFormat(" --line-break {0}", LineBreak);
            }

            if (NoMunge) {
                sb.Append(" --nomunge");
            }

            if (PreserveSemi) {
                sb.Append(" --preserve-semi");
            }

            if (Verbose) {
                sb.Append(" --verbose");
            }

            sb.AppendFormat(@" -o ""{0}"" ""{1}""", outFile, inFile);

            return sb.ToString();
        }
    }
}
