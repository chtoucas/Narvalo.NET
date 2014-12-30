ChangeLog
=========
             
- (2014/12/30) _Improvement:_ Fully documented the PSake script file. It is now 
  possible to specify the MSBuild verbosity level from the command line.
  In Make.Foundations.proj, added the ability to select a subset of the list
  of projects to build. Using a MSBuild response file, allowed to try out the 
  build system without actually committing your personal MSBuild parameters.
- (2014/12/19) _Improvement:_ In the CreateAssemblyVersionFile target, instead of 
  hardcoding the path to the directory to be created, use the parent directory 
  of AssemblyVersionFile.
- (2014/12/19) _Bugfix:_ In Prose, replaced references to NuGet packages of
  Narvalo.Core & Narvalo.Common by simple project references of the same 
  assemblies. Before that, tests run from MSBuild command-line might fail
  for obscure reasons. Worst, they might as well succeed even if nothing had 
  changed. In fact, it truly depended on the order of builds decided by MSBuild
  at that time; parallel build in action. The problem was that Prose carried 
  its own versions of Narvalo.Core and Narvalo.Common, albeit the NuGet ones, 
  and, when built, it overrided the freshly compiled versions of the same 
  assemblies. Followed version and API missmatchs. The bottom line is to never 
  again add references to one of our NuGet packages or be sure not to include 
  the project in any MSBuild project. 
- (2014/12/19) _Improvement:_ Cleaned up Make.CustomAfter.props and 
   Make.CustomAfter.targets
- (2014/12/19) _Improvement:_ Renamed DocuMaker to Prose.
- (2014/12/19) _Improvement:_ Replaced all initial targets used to signal 
  temporary overriden settings by a target automatically run before a Build.
- (2014/12/19) _Bugfix:_ Building test and sample projects raised a warning for
  an unknown semantic version. We just needed to check the context. Also
  removed the creation of a fake version when the version was not complete.
- (2014/12/19) _Improvement:_ Moved properties relevant to the Code 
  Contracts reference assembly to {AssemblyName}.props. Along the way we 
  disabled the creation of a CC reference library when working inside Visual
  Studio. This should grealty reduce the build time in Release configuration.
- (2014/12/19) _Improvement:_ Added missing {AssemblyName}.Version.props.
- (2014/12/19) _Improvement:_ Use the BuildingInsideVisualStudio property 
  to only enable some properties when not running inside Visual Studio.
- (2014/12/19) _Bugfix:_ Using the same property (SolutionFile) for NuGet
  packages restore and for the selection of a custom solution to build 
  made it easy to break the build system. In Make.proj, if we imported
  Make.Common.props before defining the ProjectsToBuild property, the path
  of SolutionFile would be absolute but Make.proj expects it to be relative.
  We simply replaced the SolutionFile property used by Make.proj by CustomSolution.
- (2014/12/19) _Improvement:_ Created a Make.Public.proj that replaces the 
  SkipPrivateProjects option from Make.proj. Moved Make.proj to the tools
  directory.
- (2014/12/19) _Improvement:_ Retired the custom CA ruleset for Samples.
- (2014/12/19) _Improvement:_ Moved the DummyGeneratedVersion property 
  to Make.CustomAfter.props. Now, inside Visual Studio, versioning for test 
  and sample assemblies works exactly the same way as for the other assemblies.
- (2014/12/18) _Improvement:_ Using partial classes we artificially separate
  whitebox tests from blackbox tests.
- (2014/12/18) _Enhancement:_ New compilation symbol `NO_GLOBAL_SUPPRESSIONS`
  to enable or disable global suppressions. Global suppressions are a
  necessary evil that shouldn't be used blindly and that require periodic review.
- (2014/12/18) _Improvement:_ Moved IssueAttribute.cs and IssueSeverity.cs to a
  shared directory then added them as linked files to test projects.
- (2014/12/18) _Improvement:_ Much needed rewriting of ChangeLog.md and Issues.md.
- **(2014/12/17) Released Narvalo.Core & Narvalo.Common v0.19.1:**
  No code changes. Correct the problem with NuGet and Code Contracts assemblies.
- (2014/12/17) _Bugfix:_ When adding a NuGet package, the Code Contracts library
  was incorrectly added to the project references. We just need to use the
  section references in the nuspec files to help NuGet identify _true_ references.
- _Improvement:_ Packages not for retail get a completely different ID.
  For instance, Narvalo.Core becomes Narvalo.Core.EDGE.
-  _Enhancement:_ Created a solution solely for project management, MSBuild and documentation.
-  _Enhancement:_ Created a local NuGet server to test the creation of NuGet packages.
- _Improvement:_ It is now possible to unconditionally hide internal classes and
  methods of an assembly. On the way, we fixed CA issues that appear when this
  is turned on.
- _Improvement:_ Thanks to the previous enhancement, now we can instruct the projects
  that do not yet pass CA or SA to use a relaxed set of rules. A warning is
  raised when it is the case, so that we don't forget to fix this later on.
-  _Enhancement:_ Allow for local customization of the build process. If we find a file
  named {AssemblyName}.props in the project root, it will be loaded at the very
  end of Narvalo.Common.props. The same holds true for {AssemblyName}.targets
  and Narvalo.Common.targets.
- **(2014/12/13) Released Narvalo.Core, Narvalo.Common & Narvalo.Web v0.19.0:**
  New Code Contracts assemblies. Plenty of small bug fixes.
- **(2014/12/13) Released Narvalo.Mvp & Narvalo.Mvp.Web v1.0.0-alpha:**
  No API changes. Alpha release of the first stable version.
- _Bugfix:_ Lot of small bugfixes needed after enabling Code Contracts analysis.
-  _Enhancement:_ PSake script to provide aliases to the most common build configurations.
- _Improvement:_ Brand new MSBuild infrastructure. Extracted all targets and
  properties not required by Visual Studio. Continuous Integration builds are
  now drived by a newly created MSBuild project (Make.proj).
-  _Enhancement:_ Preliminary support for Code Contracts.
-  _Enhancement:_ Portable class library Narvalo.Core which becomes the new base
  instead of Narvalo.Common.
- _Improvement:_ Implemented Semantic Versioning rules for assembly versions.
- _Improvement:_ Reorganization of the solutions.
- _Improvement:_ Narvalo.Facts becomes a true Visual Studio test project.
- _Bugfix:_ When building a PCL project _from the command line_, MSBuild generates
  output inside a subdirectory of `$(OutDir)`. To correct this, we instruct
  MSBuild to not change the default behaviour:
  `$(GenerateProjectSpecificOutputFolder) = false`.
- _Bugfix:_ Narvalo.Facts fails when called from Make.proj and run twice in a row.
  Narvalo.Core and Narvalo.Common use the default namespace (namely `Narvalo`)
  and both define a resource named `SR.resx` (with default access modifier kept,
  that is internal) which confused .NET when trying to resolve one of the string
  resources. I have not found the reason but, when running the tests
  from the command line, the resource embedded in Narvalo.Core got replaced
  by the one in Narvalo.Common. The solution was to use different names for
  both resources.
