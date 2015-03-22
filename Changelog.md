ChangeLog
=========

XXXX-XX-XX - Stabilizing the Narvalo.Core API
---------------------------------------------

#### Breaking changes
- To improve usability, merged the `Narvalo.Collections` namespace with `Narvalo.Fx`.
- Moved from `Require` to `Enforce` all methods that do not play well
  with Code Contracts.

#### Enhancements
- New types inspired by functional programming:
  `Output<T>`, `Either<T1, T2>`, `Switch<T1, T2>`...

#### Improvements
- Narvalo.Core now fully passes FxCop, Gendarme and Code Contracts analysis.
- Added more C# documentation.

2015-02-20 - Bugfix for currency types
--------------------------------------
- [v0.21] Narvalo.Core & Narvalo.Common                         

#### Bugfixes
- Use 0 for the numeric code for a currency when none is defined. Before that, we
  registered the alphabetic code but not the currency info since we didn't have
  a numeric code.

#### Improvements
- Removed from the currency classes anything related to culture, namely the
  currency symbol methods. We need a better way of handling localization problems.
  Unicode CLDR seems the way to go.

2015-02-19 - New currency types
-------------------------------
- [v0.20] Narvalo.Core & Narvalo.Common

#### Bugfixes
- In non-retail mode, when resolving project references, we should not include
  the prerelease label; otherwise, later on, nuget won't be able to resolve the
  dependency.
- NuGet.Server require runAllManagedModulesForAllRequests to be true, otherwise
  API calls will fail.
- Only enable white-box tests in `PresenterTypeResolverFacts` when internals
  are visible.
- In Prose, replaced references to NuGet packages of Narvalo.Core
  & Narvalo.Common by simple project references of the same
  assemblies. Before that, tests run from MSBuild command-line might fail
  for obscure reasons. Worst, they might as well succeed even if nothing had
  changed. In fact, it truly depended on the order of builds decided by MSBuild
  at that time; parallel build in action. The problem was that Prose carried
  its own versions of Narvalo.Core and Narvalo.Common, albeit the NuGet ones,
  and, when built, it overrided the freshly compiled versions of the same
  assemblies. Followed version and API missmatchs. The bottom line is to never
  again add references to one of our NuGet packages or be sure not to include
  the project in any MSBuild project.
- Building test and sample projects raised a warning for
  an unknown semantic version. We just needed to check the context. Also
  removed the creation of a fake version when the version was not complete.
- Using the same property (SolutionFile) for NuGet
  packages restore and for the selection of a custom solution to build
  made it easy to break the build system. In Make.proj, if we imported
  Make.Common.props before defining the ProjectsToBuild property, the path
  of SolutionFile would be absolute but Make.proj expects it to be relative.
  We simply replaced the SolutionFile property used by Make.proj by CustomSolution.

#### Enhancements
- Added classes to represent a currency defined by ISO 4217.
- Created a PowerShell scripts for all maintenance tasks.
- Added git commit hash to assembly informations.
- Do not change the content of the NuGet package depending on the configuration:
  always add the CC reference assembly and the assembly documentation.
- Unless explicitely requested, always build the Code
  Contracts reference assembly and the assembly documentation.
- If necessary, automatically donwload and install
  nuget.exe and PSake when running make.ps1 or publish.ps1.

#### Improvements
- Prevent creation of retail packages if there are uncommitted changes.
- Created a shared properties file for F# projects.
- New project to check the installation of retail packages after they are
  published to the official NuGet server.
- Added missing copyright headers.
- Moved private settings to a single folder.
- Replace the build script for MyGet by a PSake task
- Do not set VisualStudioVersion when building inside Visual Studio...
- Made NuGet projects self-contained: the copyright tag is automatically
  initialized.
- Changed the way we override properties for sample and test projects. We no
  longer use extensions to Narvalo.Common.props but rather use the new facility
  offered by $(AssemblyName).props.
- Better validation of version properties and NuGet project properties.
  Simplified Make.CustomAfter.*.
- Using a MSBuild response file, allowed to try out the build system without
  actually committing your personal settings.
- Made sure a build fails if a custom import does not exist. Moved the content
  of Narvalo.CustomBefore.props & Narvalo.CustomBefore.targets to CustomAfter
  props & targets and removed them.
- Cleaner separation between optional properties and
  other properties. Move the PEVerify & SecAnnotate targets from Make.Common
  to Narvalo.Custom. This makes possible to generate individual reports per
  assemblies.
- Entirely removed from Make.Common.props &
  Make.Common.targets the `Lean` property which did not bring anything. It was
  supposed to help mimicking a Visual Studio build but MSBuild can just do it.
- Only define the `CODE_ANALYSIS` symbol if Code Analysis is requested.
- Only display MSBuild warnings for local CA or SA
  overrides when CA or SA is actually requested.
- Upgraded NSubsitute to v1.8.1.
- Fully documented the PSake script file. It is now
  possible to specify the MSBuild verbosity level from the command-line.
  In Make.Foundations.proj, added the ability to select a subset of the list
  of projects to build.
- In the CreateAssemblyVersionFile target, instead of
  hardcoding the path to the directory to be created, use the parent directory
  of AssemblyVersionFile.
- Cleaned up Make.CustomAfter.props and Make.CustomAfter.targets
- Renamed DocuMaker to Prose.
- Replaced all initial targets used to signal temporary overridden settings
  by a target automatically run before a Build.
- Moved properties relevant to the Code
  Contracts reference assembly to {AssemblyName}.props. Along the way we
  disabled the creation of a CC reference library when working inside Visual
  Studio. This should grealty reduce the build time in Release configuration.
- Added missing {AssemblyName}.Version.props.
- Use the BuildingInsideVisualStudio property
  to only enable some properties when not running inside Visual Studio.
- Created a Make.Public.proj that replaces the SkipPrivateProjects option
  from Make.proj. Moved Make.proj to the tools directory.
- Retired the custom CA ruleset for Samples.
- Moved the DummyGeneratedVersion property to Make.CustomAfter.props. Now,
  inside Visual Studio, versioning for test and sample assemblies works exactly
  the same way as for the other assemblies.
- Using partial classes we artificially separate whitebox tests from blackbox tests.
- Moved IssueAttribute.cs and IssueSeverity.cs to a shared directory then added
  them as linked files to test projects.
- Much needed rewriting of ChangeLog.md and Issues.md.

2014-12-17 - No API changes. Correct a problem with NuGet and Code Contracts
----------------------------------------------------------------------------
- [v0.19.1] Narvalo.Core, Narvalo.Common & Narvalo.Web

#### Bugfixes
- When adding a NuGet package, the Code Contracts library was incorrectly added
  to the project references. To help NuGet identify _true_ references, we just
  have to use the section "references" in the nuspec files.

#### Enhancements
- Created a solution solely for project management, MSBuild and documentation.
- Created a local NuGet server to test the creation of NuGet packages.
- Allowed for local customization of the build process. If we find a file
  named {AssemblyName}.props in the project root, it will be loaded at the very
  end of Narvalo.Common.props. The same holds true for {AssemblyName}.targets
  and Narvalo.Common.targets. Thanks to this, we can instruct the projects
  that do not yet pass CA or SA to use a relaxed set of rules. A warning is
  raised when it is the case, so that we don't forget to fix this later on.

#### Improvements
- Packages not for retail get a completely different ID.
  For instance, Narvalo.Core becomes Narvalo.Core.EDGE.
- It is now possible to unconditionally hide internal classes and methods of an
  assembly. On the way, we fixed CA issues that appear when this is turned on.

2014-12-13 - New Code Contracts assemblies. Plenty of small bugfixes
--------------------------------------------------------------------
- [v0.19.0] Narvalo.Core, Narvalo.Common & Narvalo.Web
- [v1.0.0-alpha] for Narvalo.Mvp & Narvalo.Mvp.Web

#### Bugfixes
- Lot of small bugfixes needed after enabling Code Contracts analysis.
- When building a PCL project _from the command line_, MSBuild generates
  output inside a subdirectory of `$(OutDir)`. To correct this, we instruct
  MSBuild to not change the default behaviour:
  `$(GenerateProjectSpecificOutputFolder) = false`.
- Narvalo.Facts fails when called from Make.proj and run twice in a row.
  Narvalo.Core and Narvalo.Common use the default namespace (namely `Narvalo`)
  and both define a resource named `SR.resx` (with default access modifier kept,
  that is internal) which confused .NET when trying to resolve one of the string
  resources. I have not found the reason but, when running the tests
  from the command line, the resource embedded in Narvalo.Core got replaced
  by the one in Narvalo.Common. The solution was to use different names for
  both resources.

#### Enhancements
- PSake script for the most common build configurations.
- Preliminary support for Code Contracts.
- New portable class library Narvalo.Core which becomes the new base
  instead of Narvalo.Common.

#### Improvements
- Brand new MSBuild infrastructure. Extracted all targets and
  properties not required by Visual Studio.
- Implemented Semantic Versioning rules for assembly versions.
- Narvalo.Facts becomes a true Visual Studio test project.
