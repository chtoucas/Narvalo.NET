Coding Principles
=================


Creating a new project
----------------------

### Add relevant files as linked files to the "Properties" folder.

- Shared infos: "etc\AssemblyInfo.Common.cs"
- Library specific infos:
    * for a core library: "etc\AssemblyInfo.Library.cs"
    * for a "miscs" library: "etc\AssemblyInfo.Library.Miscs.cs"
    * for a MVP library: "etc\AssemblyInfo.Library.Mvp.cs"
    * for a test project: "etc\AssemblyInfo.Facts.cs"
- Version infos:
    * for a MVP library: "etc\AssemblyInfo.Version.Mvp.cs"
    * for other libraries: "etc\AssemblyInfo.Version.cs"
- Code Analysis dictionary: "etc\CodeAnalysisDictionary.xml"
  with build action "CodeAnalysisDictionary"
- Strong Name Key: "etc\Narvalo.snk"

### Edit the project Properties.

In Debug mode:
- "Build", treat all warnings as errors
- "Build", check for arithmetic overflow/underflow
- "Code Analysis", use "Narvalo Debug Rules"

In Release mode:
- "Build", suppress compiler warnings 1591
- "Build", treat all warnings as errors
- "Build", output XML documentation file
- "Code Analysis", use "Narvalo Release Rules"

In all modes:
- "Signing", sign the assembly

### Configure StyleCop.

Edit the local StyleCop settings to link to "etc\Settings.SourceAnalysis".
Edit the project file and add the following lines:
    <PropertyGroup>
      ...
      <SourceAnalysisOverrideSettingsFile>..\..\etc\Settings.SourceAnalysis</SourceAnalysisOverrideSettingsFile>
      <SourceAnalysisTreatErrorsAsWarnings>false</SourceAnalysisTreatErrorsAsWarnings>
      ...
    </PropertyGroup>
    <PropertyGroup Condition="'$(ReportsDir)' != ''">
      <SourceAnalysisOutputFile>$(ReportsDir)$(AssemblyName).StyleCopViolations.xml</SourceAnalysisOutputFile>
      <CodeAnalysisLogFile>$(ReportsDir)$(AssemblyName).CodeAnalysisLog.xml</CodeAnalysisLogFile>
    </PropertyGroup>
    <Import Project="..\..\scripts\Narvalo.stylecop.targets" />


Strong Name Key
---------------

Create the key pair: `sn -k Application.snk`.

Extract the public key: `sn -p Application.snk Application.pk`.

Extract the public key token: `sn -tp Application.pk > Application.txt`.


References
----------

+ [Strong Name Tool](http://msdn.microsoft.com/en-us/library/k5b5tt23.aspx)
