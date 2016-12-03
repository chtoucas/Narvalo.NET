ChangeLog (Infrastructure)
==========================

#### 2016-12-03
- Migration to VS 2015. As part of the process we delete any reference to
  StyleCop and remove any orphan code.
- [Improvement] Add Narvalo.Mvp.Facts to the test coverage.

#### 2016-11-04
- [Improvement] Clearly shows skipped tests. This includes debug or release only tests and white-box tests.
- [Improvement] Use traits to mark slow or unsafe tests.
- [Improvement] Integration of OpenCover.

#### 2015-02-19
- [Bugfix] In non-retail mode, when resolving project references, we should not include
  the prerelease label; otherwise, later on, nuget won't be able to resolve the
  dependency.
- [Bugfix] NuGet.Server require `runAllManagedModulesForAllRequests` to be true, otherwise
  API calls will fail.
- [Bugfix] Only enable white-box tests in `PresenterTypeResolverFacts` when internals
  are visible.
- [Bugfix] Building test and sample projects raised a warning for
  an unknown semantic version. We just needed to check the context. Also
  removed the creation of a fake version when the version was not complete.
- [Bugfix] Using the same property (SolutionFile) for NuGet
  packages restore and for the selection of a custom solution to build
  made it easy to break the build system. In Make.proj, if we imported
  Make.Common.props before defining the ProjectsToBuild property, the path
  of SolutionFile would be absolute but Make.proj expects it to be relative.
  We simply replaced the SolutionFile property used by Make.proj by CustomSolution.
- [Enhancement] Do not change the content of the NuGet package depending on the configuration:
  always add the CC reference assembly and the assembly documentation.
- [Enhancement] Unless explicitely requested, always build the Code
  Contracts reference assembly and the assembly documentation.
- [Improvement] Do not set VisualStudioVersion when building inside Visual Studio...
- [Improvement] Changed the way we override properties for sample and test projects. We no
  longer use extensions to Narvalo.Common.props but rather use the new facility
  offered by $(AssemblyName).props.
- [Improvement] Using a MSBuild response file, allowed to try out the build system without
  actually committing your personal settings.
- [Improvement] Made sure a build fails if a custom import does not exist. Moved the content
  of Narvalo.CustomBefore.props & Narvalo.CustomBefore.targets to CustomAfter
  props & targets and removed them.
- [Improvement] Cleaner separation between optional properties and
  other properties. Move the PEVerify & SecAnnotate targets from Make.Common
  to Narvalo.Custom. This makes possible to generate individual reports per
  assemblies.
- [Improvement] Entirely removed from Make.Common.props &
  Make.Common.targets the `Lean` property which did not bring anything. It was
  supposed to help mimicking a Visual Studio build but MSBuild can just do it.
- [Improvement] Only define the `CODE_ANALYSIS` symbol if Code Analysis is requested.
- [Improvement] Only display MSBuild warnings for local CA or SA
  overrides when CA or SA is actually requested.
- [Improvement] Fully documented the PSake script file. It is now
  possible to specify the MSBuild verbosity level from the command-line.
  In Make.Foundations.proj, added the ability to select a subset of the list
  of projects to build.
- [Improvement] In the CreateAssemblyVersionFile target, instead of
  hardcoding the path to the directory to be created, use the parent directory
  of AssemblyVersionFile.
- [Improvement] Replaced all initial targets used to signal temporary overridden settings
  by a target automatically run before a Build.
- [Improvement] Moved properties relevant to the Code
  Contracts reference assembly to {AssemblyName}.props. Along the way we
  disabled the creation of a CC reference library when working inside Visual
  Studio. This should grealty reduce the build time in Release configuration.
- [Improvement] Added missing {AssemblyName}.Version.props.
- [Improvement] Use the BuildingInsideVisualStudio property
  to only enable some properties when not running inside Visual Studio.
- [Improvement] Created a Make.Public.proj that replaces the SkipPrivateProjects option
  from Make.proj. Moved Make.proj to the tools directory.
- [Improvement] Retired the custom CA ruleset for Samples.
- [Improvement] Moved the DummyGeneratedVersion property to Make.CustomAfter.props. Now,
  inside Visual Studio, versioning for test and sample assemblies works exactly
  the same way as for the other assemblies.
- [Improvement] Using partial classes we artificially separate whitebox tests from blackbox tests.
- [Improvement] Moved IssueAttribute.cs and IssueSeverity.cs to a shared directory then added
  them as linked files to test projects.

#### 2014-12-17
- [Bugfix] When adding a NuGet package, the Code Contracts library was incorrectly added
  to the project references. To help NuGet identify _true_ references, we just
  have to use the section "references" in the nuspec files.
- [Enhancement] Allowed for local customization of the build process. If we find a file
  named {AssemblyName}.props in the project root, it will be loaded at the very
  end of Narvalo.Common.props. The same holds true for {AssemblyName}.targets
  and Narvalo.Common.targets. Thanks to this, we can instruct the projects
  that do not yet pass CA or SA to use a relaxed set of rules. A warning is
  raised when it is the case, so that we don't forget to fix this later on.
- [Improvement] It is now possible to unconditionally hide internal classes and methods of an
  assembly. On the way, we fixed CA issues that appear when this is turned on.

#### 2014-12-13
- [Bugfix] When building a PCL project _from the command line_, MSBuild generates output inside a subdirectory
  of `$(OutDir)`. To correct this, we instruct MSBuild to not change the default behaviour:
  `$(GenerateProjectSpecificOutputFolder) = false`.
- [Bugfix] Narvalo.Facts fails when called from Make.proj and run twice in a row.
  Narvalo.Core and Narvalo.Common use the default namespace (namely `Narvalo`)
  and both define a resource named `SR.resx` (with default access modifier kept,
  that is internal) which confused .NET when trying to resolve one of the string
  resources. I have not found the reason but, when running the tests
  from the command line, the resource embedded in Narvalo.Core got replaced
  by the one in Narvalo.Common. The solution was to use different names for
  both resources.
