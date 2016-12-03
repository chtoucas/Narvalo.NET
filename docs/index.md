Overview
========

- [User Documentation](userdocs/index.md)
- [Project Changelog](changelogs/index.md)
- [Near Future Plans](changelogs/vNext.md)
- [Engineering Guides](engineering/index.md)
- [Issues, TODOs & Ideas](Issues.md)

Library                   | Version | vNext  | Target               | Security (*)
--------------------------|---------|--------|----------------------|--------------
Narvalo.Build             | 1.1.0   |        | .NET 4.5             |
Narvalo.Cerbere           | 2.0.0   |        | Profile259 + Std 1.0 | Transparent
Narvalo.Common            | 0.25.0  |        | .NET 4.5             | Transparent
Narvalo.Core              | 0.25.0  |        | Profile259 + Std 1.0 | Transparent
Narvalo.Finance           | 0.25.0  |        | Profile111 + Std 1.1 | Transparent
Narvalo.Fx                | 0.25.0  |        | Profile259 + Std 1.0 | Transparent
Narvalo.Mvp               | 1.0.0   |        | .NET 4.5             |
Narvalo.Mvp.Web           | 1.0.0   |        | .NET 4.5             |
Narvalo.Web               | 0.25.0  |        | .NET 4.5             |

(*) Security attributes are not applied to the assemblies distributed via NuGet.

In general, we provide localized messages in English and French.

Status
------

Library                   | Static Analysis | Code Contracts | Code Coverage
--------------------------|-----------------|----------------|---------------
Narvalo.Build             | Complete        |                |
Narvalo.Cerbere           | Complete        | Complete       | 55.2%
Narvalo.Common            | Complete        | Complete       |
Narvalo.Core              | Complete        | Complete       | 46%
Narvalo.Finance           | Complete        | Complete       | 22.7%
Narvalo.Fx                | Complete        | Complete       | 11.7%
Narvalo.Mvp               | Complete        | Complete       |  6.5%
Narvalo.Mvp.Web           | Complete        | Complete       | 15.7%
Narvalo.Web               | Pretty good     | Minimal        | 17.9%

NB: Static Analysis is done with the analyzers shipped with VS.
