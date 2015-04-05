ChangeLog
=========

XXXX-XX-XX - Version 0.24 of Narvalo.Core, Narvalo.Common & Narvalo.Web
-----------------------------------------------------------------------

_Better test coverage for Narvalo.Core & Narvalo.Web_
_Very much improved the asset providers_

#### Bugfixes
- At start of `Require.InRange()`, we did not validate the range before using it.
- Fixed uncorrect and inefficient initialization of `AssetManager`.

#### Breaking changes
- Merged Narvalo.Web.UI.Assets with Narvalo.Web.UI.
- Renamed `UriExtensions.ToProtocolLessString()` to `UriExtensions.ToProtocolRelativeString()`.
  Protocol-relative URL appears to be the accepted name, not protocol-less URL.
- `HttpHandlerBase.AcceptedVerbs` is now public.
  `HttpHandlerBase.IsReusable` and `HttpHandlerBase.TrySkipIisCustomErrors` are
   no longer virtual.
- Refactored the asset providers in an attempt to make them more robust and configurable.
- Refactored the HTML helpers into purely static methods.

### Enhancements
- Added localized resources for the french language.
- Applied the `SecurityTransparent` attribute to Narvalo.Core.
- New helpers for parameter validation or Code Contracts:
  `Acknowledge`, `Check` and `ValidatedNotNull`.

#### Improvements
- Use the new `Format.Resource()` instead of `Format.CurrentCulture()` for resource formatting.
- Improved the error messages. Whenever it is possible, we give an hint on how to fix the problem.
- Rollback on `Require` methods not throwing an `ArgumentNullException`. Throwing a more specific
  exception is certainly better.
- For `Require` and `Enforce`, throw an `ArgumentException` if the specified range is invalid.
- In `StringManip`, use direct concatenation instead of `String.Format()`.
- Use "en" instead of "en-US" as the default resource language.
- Added more tests.
- Clearly shows skipped tests. This includes debug or release only tests and white-box tests.
- Use traits to mark slow tests.

2015-03-26 - Version 0.23 of Narvalo.Core, Narvalo.Common & Narvalo.Web
-----------------------------------------------------------------------

_Aiming for API stability of Narvalo.Core_

#### Breaking changes
- Renamed `VoidOrBreak.Abort()` to `VoidOrBreak.Break()`, `VoidOrBreak.Aborted` to `VoidOrBreak.IsBreak`
  and `VoidOrBreak.Success` to `VoidOrBreak.Void`.
- Renamed `VoidOrError.Failure()` to `VoidOrError.Error()` and `VoidOrError.Success` to `VoidOrError.Void`.
- Renamed `Sequence.Create()` to `Sequence.Generate()`.

#### Enhancements
- New `Promise` class to make promises and check them.
- New `Assume` class to help the Code Contracts tools recognize that certain conditions are met.
- Provides unsafe alternates to some extension methods for `SqlParameterCollection` and `SqlCommand`.
  These unsafe methods delegate parameter validation to the caller.
- New validation method: `Require.PropertyNotWhiteSpace()`.

#### Improvements
- `Require.PropertyNotEmpty()` no longer throws an `ArgumentNullException`.
- Whenever it was possible, moved `SqlDataReader` extensions to `IDataRecord` extensions.
- We no longer patch the documentation with Code Contracts annotations; this created too
  much noise and annoying duplicates of descriptions for exceptions. The patched documentation
  is still available in the NuGet package alongside the contract assemblies.
- Added more Code Contracts.
- Changed a few parameter names to ensure a CA1303 error is triggered whenever
  a string should be localized.

2015-03-24 - Version 0.22.1 of Narvalo.Core, Narvalo.Common & Narvalo.Web
-------------------------------------------------------------------------

_Correct a problem with NuGet and Code Contracts_

#### Bugfixes
- For Narvalo.Web, the Code Contracts library was wrongly added to the project references.

#### Improvements
- More meaningful description of the NuGet packages.
- Replaced all calls to Enum.HasFlag by a specialized extension method that should be more efficient.

2015-03-23 - Version 0.22 of Narvalo.Core, Narvalo.Common & Narvalo.Web
-----------------------------------------------------------------------

_Focus on API and code quality of Narvalo.Core_

#### Breaking changes
- To improve usability, merged most of the `Narvalo.Collections` namespace with
  `Narvalo.Fx` and moved almost all others classes to Narvalo.Common.
  Narvalo.Core is now focused on implementing functional patterns and a minimal set
  of helpers, namely what is necessary to perform argument validation.
- Moved from `Require` to `Enforce` all methods that do not play well with Code Contracts.
- Renamed `Maybe.Create()` to `Maybe.Of()` and `Range.Create()` to `Range.Of()`.
- Apply to all monads: renamed `Apply()` to `Invoke()`, `Match()` to `Map()` and
  changed the signature of `When()` and `Then()`.
- Moved the `UriExtensions` class to Narvalo.Web as it is only useful there.

#### Enhancements
- New types inspired by functional programming: `Output<T>`, `Either<T1, T2>`, `Switch<T1, T2>`...
- New types to help writing naïve benchmarks.

#### Improvements
- Narvalo.Core fully passes FxCop, Gendarme and Code Contracts static analysis.
- Added more C# documentation.
- Made the `Maybe<T>.IsSome` property public; there was no compelling reason to continue hiding it.
- Follow more closely the coding style recommandations by the .NET team.

2015-02-20 - Version 0.21 of Narvalo.Core & Narvalo.Common
----------------------------------------------------------

_Bugfix for currency types_

#### Bugfixes
- Use 0 for the numeric code for a currency when none is defined. Before that, we  registered the
  alphabetic code but not the currency info since we didn't have a numeric code.

#### Improvements
- Removed from the currency classes anything related to culture, namely the currency symbol methods.
  We need a better way of handling localization problems. Unicode CLDR seems the way to go.

2015-02-19 - Version 0.20 of Narvalo.Core & Narvalo.Common
----------------------------------------------------------

_New currency types_

#### Bugfixes
- In non-retail mode, when resolving project references, we should not include
  the prerelease label; otherwise, later on, nuget won't be able to resolve the
  dependency.
- NuGet.Server require `runAllManagedModulesForAllRequests` to be true, otherwise
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
- Upgraded NSubstitute to v1.8.1.
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

2014-12-17 - Version 0.19.1 of Narvalo.Core, Narvalo.Common & Narvalo.Web
----------------------------------------------------------------------------

_No API changes. Correct a problem with NuGet and Code Contracts_

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

2014-12-13 - Version 1.0.0-alpha of Narvalo.Mvp & Narvalo.Mvp.Web
-----------------------------------------------------------------

2014-12-13 - Version 0.19.0 of Narvalo.Core, Narvalo.Common & Narvalo.Web
--------------------------------------------------------------------

_New Code Contracts assemblies. Plenty of small bugfixes_

#### Bugfixes
- Lot of small bugfixes needed after enabling Code Contracts analysis.
- When building a PCL project _from the command line_, MSBuild generates output inside a subdirectory
  of `$(OutDir)`. To correct this, we instruct MSBuild to not change the default behaviour:
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
- New portable class library Narvalo.Core which becomes the new base instead of Narvalo.Common.

#### Improvements
- Brand new MSBuild infrastructure. Extracted all targets and properties not required by VS.
- Implemented Semantic Versioning rules for assembly versions.
- Narvalo.Facts becomes a true Visual Studio test project.
