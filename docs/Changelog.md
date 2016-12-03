ChangeLog
=========

### 2016-12-03 ([Details](changelogs/2016-12-03.md))

Package                   | Version
--------------------------|---------
Narvalo.Cerbere           | 2.0.0
Narvalo.Common            | 0.25.0
Narvalo.Core              | 0.25.0
Narvalo.Finance           | 0.25.0
Narvalo.Fx                | 0.25.0
Narvalo.Mvp               | 1.0.0
Narvalo.Mvp.Web           | 1.0.0
Narvalo.Web               | 0.25.0

- New major (and incompatible) release of Narvalo.Cerbere with hopefully a better
  and simpler API.
- First stable versions of Narvalo.Mvp and Narvalo.Mvp.Web fully verified
  with the Code Contracts tools.

### 2016-11-04 ([Details](changelogs/2016-11-04.md))

_Version 0.24 of Narvalo.Core, Narvalo.Common & Narvalo.Web_

- Focus on API and code quality for Narvalo.Web.
- Better test coverage for Narvalo.Core & Narvalo.Web.

### 2015-04-25 ([Details](changelogs/2015-04-25.md))

_Version 1.1.0 of Narvalo.Build_

- New tasks related to semantic versioning.
- New task to download a file from internet.

### 2015-03-26 ([Details](changelogs/2015-03-26.md))

_Version 0.23 of Narvalo.Core, Narvalo.Common & Narvalo.Web_

- Aiming for API stability of Narvalo.Core.

### 2015-03-24 ([Details](changelogs/2015-03-24.md))

_Version 0.22.1 of Narvalo.Core, Narvalo.Common & Narvalo.Web_

- Correct a problem with NuGet and Code Contracts.

### 2015-03-23 ([Details](changelogs/2015-03-23.md))

_Version 0.22 of Narvalo.Core, Narvalo.Common & Narvalo.Web_

- Focus on API and code quality of Narvalo.Core.

### 2015-02-20 ([Details](changelogs/2015-02-20.md))

_Version 0.21 of Narvalo.Core & Narvalo.Common_

- Bugfix for currency types.

### 2015-02-19 ([Details](changelogs/2015-02-19.md))

_Version 0.20 of Narvalo.Core & Narvalo.Common_

- New currency types.

2014-12-17 - Version 0.19.1 of Narvalo.Core, Narvalo.Common & Narvalo.Web
----------------------------------------------------------------------------

_No API changes. Correct a problem with NuGet and Code Contracts_

#### Bugfixes
- When adding a NuGet package, the Code Contracts library was incorrectly added
  to the project references. To help NuGet identify _true_ references, we just
  have to use the section "references" in the nuspec files.

#### Enhancements
- Allowed for local customization of the build process. If we find a file
  named {AssemblyName}.props in the project root, it will be loaded at the very
  end of Narvalo.Common.props. The same holds true for {AssemblyName}.targets
  and Narvalo.Common.targets. Thanks to this, we can instruct the projects
  that do not yet pass CA or SA to use a relaxed set of rules. A warning is
  raised when it is the case, so that we don't forget to fix this later on.

#### Improvements
- It is now possible to unconditionally hide internal classes and methods of an
  assembly. On the way, we fixed CA issues that appear when this is turned on.

2014-12-13 - Version 1.0.0-alpha of Narvalo.Mvp & Narvalo.Mvp.Web
-----------------------------------------------------------------

2014-12-13 - Version 0.19.0 of Narvalo.Core, Narvalo.Common & Narvalo.Web
--------------------------------------------------------------------

_New Code Contracts assemblies. Plenty of small bugfixes_

#### Bugfixes
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

