ChangeLog
=========

- (2014/12/18) Improvement: Using partial classes we artificially separate
  whitebox tests from blackbox tests.
- (2014/12/18) Enhancement: New compilation symbol `NO_GLOBAL_SUPPRESSIONS`
  to enable or disable global suppressions. Global suppressions are a
  necessary evil that shouldn't be used blindly and that require periodic review.
- (2014/12/18) Improvement: Moved IssueAttribute.cs and IssueSeverity.cs to a
  shared directory then added them as linked files to test projects.
- (2014/12/18) Improvement: Much needed rewriting of ChangeLog.md and Issues.md.
- **(2014/12/17) Released Narvalo.Core & Narvalo.Common v0.19.1:**
  No code changes. Correct the problem with NuGet and Code Contracts assemblies.
- (2014/12/17) Fixed: When adding a NuGet package, the Code Contracts library
  was incorrectly added to the project references. We just need to use the
  section references in the nuspec files to help NuGet identify _true_ references.
- Improvement: Packages not for retail get a completely different ID.
  For instance, Narvalo.Core becomes Narvalo.Core.EDGE.
- Enhancement: Created a solution solely for project management, MSBuild and documentation.
- Enhancement: Created a local NuGet server to test the creation of NuGet packages.
- Improvement: It is now possible to unconditionally hide internal classes and
  methods of an assembly. On the way, we fixed CA issues that appear when this
  is turned on.
- Improvement: Thanks to the previous enhancement, now we can instruct the projects
  that do not yet pass CA or SA to use a relaxed set of rules. A warning is
  raised when it is the case, so that we don't forget to fix this later on.
- Enhancement: Allow for local customization of the build process. If we find a file
  named {AssemblyName}.props in the project root, it will be loaded at the very
  end of Narvalo.Common.props. The same holds true for {AssemblyName}.targets
  and Narvalo.Common.targets.
- **(2014/12/13) Released Narvalo.Core, Narvalo.Common & Narvalo.Web v0.19.0:**
  New Code Contracts assemblies. Plenty of small bug fixes.
- **(2014/12/13) Released Narvalo.Mvp & Narvalo.Mvp.Web v1.0.0-alpha:**
  No API changes. Alpha release of the first stable version.
- Fixed: Lot of small bugfixes needed after enabling Code Contracts analysis.
- Enhancement: PSake script to provide aliases to the most common build configurations.
- Improvement: Brand new MSBuild infrastructure. Extracted all targets and
  properties not required by Visual Studio. Continuous Integration builds are
  now drived by a newly created MSBuild project (Make.proj).
- Enhancement: Preliminary support for Code Contracts.
- Enhancement: Portable class library Narvalo.Core which becomes the new base
  instead of Narvalo.Common.
- Improvement: Implemented Semantic Versioning rules for assembly versions.
- Improvement: Reorganization of the solutions.
- Improvement: Narvalo.Facts becomes a true Visual Studio test project.
- Fixed: When building a PCL project _from the command line_, MSBuild generates
  output inside a subdirectory of `$(OutDir)`. To correct this, we instruct
  MSBuild to not change the default behaviour:
  `$(GenerateProjectSpecificOutputFolder) = false`.
- Fixed: Narvalo.Facts fails when called from Make.proj and run twice in a row.
  Narvalo.Core and Narvalo.Common use the default namespace (namely `Narvalo`)
  and both define a resource named `SR.resx` (with default access modifier kept,
  that is internal) which confused .NET when trying to resolve one of the string
  resources. I have not found the reason but, when running the tests
  from the command line, the resource embedded in Narvalo.Core got replaced
  by the one in Narvalo.Common. The solution was to use different names for
  both resources.
