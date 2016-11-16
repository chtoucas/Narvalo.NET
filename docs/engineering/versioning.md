Assembly Versioning
===================

Versions
--------

- AssemblyVersion, version used by the runtime.
  MAJOR.MINOR.0.0
- AssemblyFileVersion, version as seen in the file explorer,
  also used to uniquely identify a build.
  MAJOR.MINOR.BUILD.REVISION
- AssemblyInformationalVersion, the product version. In most cases
  this is the version I shall use for NuGet package versioning.
  This attribute follows semantic versioning rules.
  MAJOR.MINOR.PATCH(-PreRelaseLabel)

MAJOR, MINOR, PATCH and PreRelaseLabel (alpha, beta) are manually set.

BUILD and REVISION are generated automatically:
- Inside Visual Studio, I don't mind if the versions do not change between builds.
- Continuous build or publicly released build should increment it.

I do not change the AssemblyVersion when PATCH is incremented.

Version updates
---------------

## Behaviour

## Non-retail packages

## Package dependencies:

Library                   | Internal Dependencies
--------------------------|-----------------------------------------------------
Narvalo.Build             | -
Narvalo.Cerbere           | -
Narvalo.Common            | Cerbere, Fx
Narvalo.Core              | Cerbere, Fx
Narvalo.Finance           | Cerbere
Narvalo.Fx                | Cerbere
Narvalo.Mvp               | Cerbere
Narvalo.Mvp.Web           | Cerbere, Mvp
Narvalo.Web               | Cerbere, Core, Common, Fx

TODO: Explain package versioning and the consequences on major, minor and patch upgrades.

All core Narvalo projects use the same version, let's see if things work with NuGet:
- Patch update: X.Y.0.0 -> X.Y.1.0
  * If I publish Narvalo.Core but not Narvalo.Common, binding redirect works
    for Narvalo.Core and Narvalo.Common can reference the newly published assembly.
  * If I publish Narvalo.Common but not Narvalo.Core, even if Narvalo.Common
    references Narvalo.Core X.Y.1.0, obviously unknown outside, it doesn't
    matter for the CLR: the assembly version _did not actually change_,
    it's still X.Y.0.0.
- Major or Minor upgrade: 1.1.0.0 -> 1.2.0.0 (or 1.1.0.0 -> 2.1.0.0)
  * If I publish Narvalo.Core but not Narvalo.Common, binding redirect works.
    Let's cross fingers that I did not make a mistake by not releasing
    Narvalo.Common too.
  * If I publish Narvalo.Common but not Narvalo.Core, we get a runtime error
    since Narvalo.Common references an assembly version unknown outside my
    development environment. The solution is obvious. Narvalo.Core has not
    changed so Narvalo.Common should replace the direct reference and use
    the NuGet package for Narvalo.Core. If necessary we can roll back
    at any time and next time we must publish both packages.
