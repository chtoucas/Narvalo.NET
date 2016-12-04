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

Status (2016/12/04)
-------------------

Library                   | Static Analysis | Code Contracts | LC    | BC
--------------------------|-----------------|----------------|-------|-------
Narvalo.Build             | Complete        |                |       |
Narvalo.Cerbere           | Complete        | Complete       | 100%  | 100%
Narvalo.Common            | Complete        | Complete       | 1.1%  | 0%
Narvalo.Core              | Complete        | Complete       | 29%   | 46%
Narvalo.Finance           | Complete        | Complete       | 83.2% | 22.7%
Narvalo.Fx                | Complete        | Complete       | 15.1% | 11.7%
Narvalo.Mvp               | Complete        | Complete       | 14.6% | 6.5%
Narvalo.Mvp.Web           | Complete        | Complete       | 23.1% | 15.7%
Narvalo.Web               | Pretty good     | Minimal        | 17%   | 17.9%

Notes:
- Static Analysis is done with the analyzers shipped with VS.
- LC = Line Coverage, BC = Branch Coverage.