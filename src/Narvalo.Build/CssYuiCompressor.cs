namespace Narvalo.Build
{
    using System.Globalization;
    using System.Text;

    /// <example>
    /// <UsingTask TaskName="Narvalo.Build.CssYuiCompressor" 
    ///   AssemblyFile="$(ProjectDir)..\Narvalo.Build\bin\$(ConfigurationName)\Narvalo.Build.dll"/>
    /// <ItemGroup>
    ///   <CssFiles Include="$(SourceWebPhysicalPath)\Styles\*.css" />
    /// </ItemGroup>
    /// <Target Name="AfterBuild">
    ///   <CssYuiCompressor Files="@(CssFiles)" />
    /// </Target>
    /// </example>
    public sealed class CssYuiCompressor : YuiCompressorBase
    {
        protected override string FileExtension
        {
            get { return "css"; }
        }

        protected override string GenerateCommandLineArguments(string inFile, string outFile)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"-jar ""{0}"" --type css --charset utf8", JarPath);

            if (LineBreak > 0) {
                sb.AppendFormat(" --line-break {0}", LineBreak.ToString(CultureInfo.InvariantCulture));
            }

            if (Verbose) {
                sb.Append(" --verbose");
            }

            sb.AppendFormat(@" -o ""{0}"" ""{1}""", outFile, inFile);

            return sb.ToString();
        }
    }
}
