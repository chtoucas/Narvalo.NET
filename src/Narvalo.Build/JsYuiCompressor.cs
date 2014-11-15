namespace Narvalo.Build
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <example>
    /// <UsingTask TaskName="Narvalo.Build.JsYuiCompressor" 
    ///   AssemblyFile="$(ProjectDir)..\Narvalo.Build\bin\$(ConfigurationName)\Narvalo.Build.dll"/>
    ///
    /// <ItemGroup>
    ///   <JavaScriptFiles Include="$(SourceWebPhysicalPath)\Scripts\*.js" />
    /// </ItemGroup>
    ///
    /// <Target Name="AfterBuild">
    ///   <JsYuiCompressor Files="@(JavaScriptFiles)" />
    /// </Target>
    /// </example>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
        Justification = "Js is not an acronym.")]
    public sealed class JsYuiCompressor : YuiCompressorBase
    {
         bool _disableOptimizations = false;
         bool _noMunge = false;
         bool _preserveSemi = false;

        public bool DisableOptimizations
        {
            get { return _disableOptimizations; }
            set { _disableOptimizations = value; }
        }

        public bool NoMunge
        {
            get { return _noMunge; }
            set { _noMunge = value; }
        }

        public bool PreserveSemi
        {
            get { return _preserveSemi; }
            set { _preserveSemi = value; }
        }

        protected override string FileExtension
        {
            get { return "js"; }
        }

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
