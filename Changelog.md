ChangeLog
=========

### Narvalo.Core & Narvalo.Common v0.19.1 (Released 2014/12/17)
- Fixed: When adding a NuGet package, the Code Contracts library was incorrectly
  added to the project references.

### Narvalo.Core, Narvalo.Common, Narvalo.Web v0.19.0 (Released 2014/12/13)
- New: Preliminary support for Code Contracts.
- New: Portable class library Narvalo.Core which becomes the new base instead
  of Narvalo.Common.
- Fixed: Lot of small bugfixes needed after running Code Contracts analysis.

### Narvalo.Mvp, Narvalo.Mvp.Web v1.0.0-alpha (Released 2014/12/13)
- New: Just bumping the version to an alpha release of the version 1.0.

### Build Infrastructure (Released 2014/12/13)
- Improved: Brand new MSBuild infrastructure.
- Improved: Assembly versioning scheme.
- Improved: Reorganization of the solutions.
- Improved: Narvalo.Facts becomes a true VS test project.
- Fixed: Narvalo.Facts failed whend run twice in a row when using Make.proj.
