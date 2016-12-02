Overview
========

- [Project Changelog](Changelog.md)
- [Issues and Roadmap](Issues.md)
- [Engineering Guides](engineering/index.md)

Status
------

Library                   | Version     | PCL/Framework    | Security (*) | SA | CC       | TC
--------------------------|-------------|------------------|--------------|----|----------|-----
Narvalo.Build             | 1.1.0       | .NET 4.5         |              | OK |          |
Narvalo.Cerbere           | 2.0.0       | Profile259 (**)  | Transparent  | OK | Complete |
Narvalo.Common            | 0.25.0      | .NET 4.5         | Transparent  | OK | Complete |
Narvalo.Core              | 0.25.0      | Profile259 (**)  | Transparent  | OK | Complete |
Narvalo.Finance           | 0.25.0      | Profile111 (***) | Transparent  | OK | Complete |
Narvalo.Fx                | 0.25.0      | Profile259 (**)  | Transparent  | OK | Complete |
Narvalo.Mvp               | 0.99.1      | .NET 4.5         |              | OK | Partial  |
Narvalo.Mvp.Web           | 0.99.1      | .NET 4.5         |              | OK |          |
Narvalo.Web               | 0.25.0      | .NET 4.5         |              |    | Partial  |

(*) Security attributes are not applied to the assemblies distributed via NuGet.

(**) The NuGet package includes support for .NET Standard 1.0.

(***) The NuGet package includes support for .NET Standard 1.1.

Explanations:
- SA: Static Analysis with the analyzers shipped with VS
- CC: Code Contracts
- TC: Code Coverage.
