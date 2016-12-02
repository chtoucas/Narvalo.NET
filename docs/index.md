Overview
========

- [Project Changelog](Changelog.md)
- [Issues and Roadmap](Issues.md)
- [Engineering Guides](engineering/index.md)

Status
------

Library                   | Version     | Target (.NET Standard) | Security (*)
--------------------------|-------------|------------------------|--------------
Narvalo.Build             | 1.1.0       | .NET 4.5               |
Narvalo.Cerbere           | 2.0.0       | Profile259 (v1.0)      | Transparent
Narvalo.Common            | 0.25.0      | .NET 4.5               | Transparent
Narvalo.Core              | 0.25.0      | Profile259 (v1.0)      | Transparent
Narvalo.Finance           | 0.25.0      | Profile111 (v1.1)      | Transparent
Narvalo.Fx                | 0.25.0      | Profile259 (v1.0)      | Transparent
Narvalo.Mvp               | 0.99.1      | .NET 4.5               |
Narvalo.Mvp.Web           | 0.99.1      | .NET 4.5               |
Narvalo.Web               | 0.25.0      | .NET 4.5               |

(*) Security attributes are not applied to the assemblies distributed via NuGet.

Library                   | SA          | CC          | TC
--------------------------|-------------|-------------|-----
Narvalo.Build             | Complete    |             |
Narvalo.Cerbere           | Complete    | Complete    |
Narvalo.Common            | Complete    | Complete    |
Narvalo.Core              | Complete    | Complete    |
Narvalo.Finance           | Complete    | Complete    |
Narvalo.Fx                | Complete    | Complete    |
Narvalo.Mvp               | Complete    | Pretty good |
Narvalo.Mvp.Web           | Complete    |             |
Narvalo.Web               | Pretty good | Minimal     |


Explanations:
- SA: Static Analysis with the analyzers shipped with VS
- CC: Code Contracts
- TC: Code Coverage.
