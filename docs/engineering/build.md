Build Infrastructure
====================

Extra care has been taken to completely isolate the CI environment and to keep
the development inside Visual Studio as smooth and swift as it can be.

Build Scripts
-------------

The main build script is `make.ps1` at the root of the repository,
it comes with complete documentation:
- Use `make.ps1 -h` to display the help.
- Use `make.ps1 -docs` to display the list of available tasks.

MSBuild
-------

```
Project
  Narvalo.Common.props
```

```
Make.proj
  Make.Common.props
    Make.Shared.props
      Shared.props
    -> Inject Make.CustomAfter.props into Narvalo.Common.props
      Make.Shared.props (if not already imported)
        Shared.props
      Make.Custom.tasks
      ? $(AssemblyName).Version.props
      ? $(AssemblyName).nuproj
        ? FrameworkProfiles.props
    -> Inject Make.CustomAfter.targets into Narvalo.Common.targets
  Make.Common.targets
    Make.Common.props (if not already imported)
    Package.Narvalo.targets
      Narvalo.Build.tasks
    Package.Xunit.tasks
```

```
Narvalo.Common.props
  Shared.props
  ? CustomBeforeProps
  ? $(AssemblyName).props
  ? CustomAfterProps
Narvalo.Common.targets
  Custom.After.Microsoft.Common.targets
  Narvalo.Common.props (if not already imported)
  ? CustomBeforeTargets
  ? CustomAfterTargets
  ? $(AssemblyName).targets
```

### Make.proj vs Make.Foundations.proj


