ChangeLog
=========

- **Narvalo.Core & Narvalo.Common v0.19.1 (Released 2014/12/17)**
- Fixed (2014/12/17): When adding a NuGet package, the Code Contracts library 
  was incorrectly added to the project references. We just need to use the 
  section references in the nuspec files to help NuGet identify _true_ references.
- Improved: Packages not for retail get a completely different ID.
  For instance, Narvalo.Core becomes Narvalo.Core.EDGE.
- New: Created a local NuGet server to test the creation of NuGet packages.
- Improved: It is now possible to unconditionally hide internal classes and
  methods of an assembly. On the way, we fixed CA issues that appear when this
  is turned on.
- Improved: Thanks to the previous capability, now we can instruct the projects 
  that do not yet pass CA or SA to use a relaxed set of rules. A warning is
  raised when it is the case, so that we don't forget to later fix this.
- New: Allow for local customization of the build process. If we find a file
  named {AssemblyName}.props in the project root, it will be loaded at the very
  end of Narvalo.Common.props. The same holds true for {AssemblyName}.targets 
  and Narvalo.Common.targets.
- **Narvalo.Core, Narvalo.Common & Narvalo.Web v0.19.0 (Released 2014/12/13)**
- **Narvalo.Mvp & Narvalo.Mvp.Web v1.0.0-alpha (Released 2014/12/13)**   
- Fixed: Lot of small bugfixes needed after enabling Code Contracts analysis.
- Improved: Brand new MSBuild infrastructure.        
- New: Preliminary support for Code Contracts.       
- New: Portable class library Narvalo.Core which becomes the new base instead
  of Narvalo.Common.
- Improved: Assembly versioning scheme. Follow Semantic Versioning rules.
- Improved: Reorganization of the solutions.
- Improved: Narvalo.Facts becomes a true VS test project.
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
