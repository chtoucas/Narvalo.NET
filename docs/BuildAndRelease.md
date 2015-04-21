Build Infrastructure
====================

The main build script is `make.ps1`. It comes with complete documentation:
- Use `make.ps1 -h` to display the help.
- Use `make.ps1 -docs` to display the list of available tasks.

Releasing a NuGet package
-------------------------

To release a new version of all core packages:

1. Review shared version: `Narvalo.CurrentVersion.props` or local version props file.
2. Review individual versions in `src\NuGet`, if they differ from the shared one.
3. Review individual package descriptions in `src\NuGet`.
4. Build and publish the packages: `make.ps1 -r pushcore`.
5. Tag the repository. For instance, for version 1.1.0
```
git tag -a core-1.1.0 -m 'Core Version 1.1.0' 33e07eceba5a56cde7b0dc753aed0fa5d0e101dc
git push origin core-1.1.0
```

