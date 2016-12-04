Overview
========

- [User Documentation](userdocs/index.md)
- [Project Changelog](changelogs/index.md)
- [Near Future Plans](changelogs/vNext.md)
- [Engineering Guides](engineering/index.md)
- [Issues, TODOs & Ideas](engineering/issues.md)

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

Library                   | Code Analysis | Code Contracts | Code Coverage
--------------------------|---------------|----------------|---------------
Narvalo.Build             | Excellent     |                |
Narvalo.Cerbere           | Excellent     | Excellent      | Excellent
Narvalo.Common            | Excellent     | Excellent      |
Narvalo.Core              | Excellent     | Excellent      |
Narvalo.Finance           | Excellent     | Excellent      |
Narvalo.Fx                | Excellent     | Excellent      |
Narvalo.Mvp               | Excellent     | Excellent      |
Narvalo.Mvp.Web           | Excellent     | Excellent      |
Narvalo.Web               | Pretty good   | Minimal        |

Notes:
- Code Analysis is done with the default VS analyzers
- Code Analysis: excellent means no error or warning found,
  all `SuppressMessage` fully justified.
- Code Contracts: excellent means 100% verified, no error or warning found,
  all `SuppressMessage` fully justified.
- Code Coverage: excellent means 100% line and branch coverages.
