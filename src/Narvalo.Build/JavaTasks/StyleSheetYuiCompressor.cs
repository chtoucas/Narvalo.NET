// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Build.JavaTasks
{
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// MSBuild task to execute the YUI compressor on CSS files.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <ItemGroup>
    ///     <CssFiles Include="$(SourceWebPhysicalPath)\Styles\*.css" />
    /// </ItemGroup>
    /// <Target Name="AfterBuild">
    ///     <StyleSheetYuiCompressor Files="@(CssFiles)" />
    /// </Target>
    /// ]]>
    /// </code>
    /// </example>
    public sealed class StyleSheetYuiCompressor : YuiCompressorBase
    {
        /// <summary>
        /// Gets the filename extension.
        /// </summary>
        /// <value>The filename extension.</value>
        protected override string FileExtension
        {
            get { return "css"; }
        }

        /// <summary>
        /// Generates the command-line arguments for the YUI executable
        /// for the specified input and output files.
        /// </summary>
        /// <param name="inFile">The input file.</param>
        /// <param name="outFile">The output file.</param>
        /// <returns>The set of command-line arguments to use when starting the YUI executable.</returns>
        protected override string GenerateCommandLineArguments(string inFile, string outFile)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"-jar ""{0}"" --type css --charset utf8", JarPath);

            if (LineBreak > 0)
            {
                sb.AppendFormat(" --line-break {0}", LineBreak.ToString(CultureInfo.InvariantCulture));
            }

            if (Verbose)
            {
                sb.Append(" --verbose");
            }

            sb.AppendFormat(@" -o ""{0}"" ""{1}""", outFile, inFile);

            return sb.ToString();
        }
    }
}
