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
    public sealed class StyleSheetYuiCompressor : YuiCompressor
    {
        /// <inheritdoc />
        protected override string FileExtension
        {
            get { return "css"; }
        }

        /// <inheritdoc />
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
