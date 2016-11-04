Developer Operations
====================

Before going further, you should read [Versioning](versioning.md).

Releasing a NuGet package
-------------------------

Checklist:
- The shared version: `etc\Narvalo.CurrentVersion.props`.
- Individual versions in `src\NuGet`, if they differ from the shared one.
- Individual package descriptions in `src\NuGet`.

To release a new version of a package (for instance the core packages):
1. Build and publish the packages: `make.ps1 -r pushcore`.
2. Tag the repository. For instance, for version 1.1.0
```
git tag -a release-1.1.0 -m 'Core Version 1.1.0' 33e07eceba5a56cde7b0dc753aed0fa5d0e101dc
git push origin core-1.1.0
```
