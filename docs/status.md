Status
======

Library                   | Version     | PCL/Platform     | SA | CC | TC
--------------------------|-------------|------------------|----|----|-----
Narvalo.Build             | 1.1.0       | .NET 4.5         |    |    |
Narvalo.Cerbere           | 1.0.0       | Profile259       |    | OK | 100%
Narvalo.Common            | 0.24.0      | .NET 4.5         |    | OK |
Narvalo.Core              | 0.24.0      | Profile259       |    | OK |
Narvalo.Finance           | 0.24.0      | Profile111       |    | OK |
Narvalo.Fx                | 0.24.0      | Profile259       |    | OK |
Narvalo.Ghostscript       |             | .NET 4.5         |    |    |
Narvalo.Mvp               | 0.99.0      | .NET 4.5         |    |    |
Narvalo.Mvp.Web           | 0.99.0      | .NET 4.5         |    |    |
Narvalo.Mvp.Windows.Forms |             | .NET 4.5         |    |    |
Narvalo.Reliability       |             | .NETStandard 1.2 |    |    |
Narvalo.Web               | 0.24.0      | .NET 4.5         |    | OK |

Explanations:
- SA: Static Analysis with:
  * Analyzers shipped with VS
  * SonarCube analyzers
  * StyleCop analyzers
- CC: Code Contracts
- TC: Code Coverage.

Security
--------

**WARNING** Security attributes are not applied to the assemblies distributed
via NuGet packages.

Library             | Attribute
--------------------|------------
Narvalo.Cerbere     | Transparent
Narvalo.Common      | Transparent
Narvalo.Core        | Transparent
Narvalo.Finance     | Transparent
Narvalo.Fx          | Transparent

Currently, all other assemblies do not specify any security attribute, therefore
use the default policy (security critical).

Remark:
All methods in ASP.NET MVC v5 default to security critical, our only choice would
be to mark Narvalo.Web with the APTCA attribute and to apply the correct security
attribute where it is needed, but APTCA and ASP.NET MVC
[do not work together](https://github.com/DotNetOpenAuth/DotNetOpenAuth/issues/307).
