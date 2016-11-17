Overview
========

- [Project Changelog](Changelog.md)
- [Issues and Roadmap](Issues.md)
- [Engineering Guides](engineering/index.md)

Status
------

Library                   | Version     | PCL/Framework    | Security (*) | SA | CC | TC
--------------------------|-------------|------------------|--------------|----|----|-----
Narvalo.Build             | 1.1.0       | .NET 4.5         |              |    |    |
Narvalo.Cerbere           | 1.0.0       | Profile259       | Transparent  |    | OK | 100%
Narvalo.Common            | 0.24.0      | .NET 4.5         | Transparent  |    | OK |
Narvalo.Core              | 0.24.0      | Profile259       | Transparent  |    | OK |
Narvalo.Finance           | 0.24.0      | Profile111       | Transparent  |    | OK |
Narvalo.Fx                | 0.24.0      | Profile259       | Transparent  |    | OK |
Narvalo.Mvp               | 0.99.0      | .NET 4.5         |              |    |    |
Narvalo.Mvp.Web           | 0.99.0      | .NET 4.5         |              |    |    |
Narvalo.Web               | 0.24.0      | .NET 4.5         |              |    | OK |

(*) Security attributes are not applied to the assemblies distributed via NuGet.

Explanations:
- SA: Static Analysis with:
  * Analyzers shipped with VS
  * SonarCube analyzers
  * StyleCop analyzers
- CC: Code Contracts
- TC: Code Coverage.
