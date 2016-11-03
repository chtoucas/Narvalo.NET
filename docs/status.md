Status
======

Library             | Status | PCL        | CA | GA | CC | SA  | TC
--------------------|--------|------------|----|----|----|-----|-----
Narvalo.Cerbere (*) | Beta   | Profile259 | OK | OK | OK | OK+ | 100%
Narvalo.Fx      (*) | Beta   | Profile259 | OK | OK | OK | OK  |
Narvalo.Finance (*) |        | Profile111 | OK | !  | OK | OK  |
Narvalo.Core        | Alpha  | Profile259 | !  | OK | OK | OK  |
Narvalo.Common      | Alpha  |            | !  | !  | OK | OK  |
Narvalo.Web         |        |            |    |    |    | OK  |
Narvalo.Mvp         | Beta   |            | !  |    |    | OK  |
Narvalo.Mvp.Web     | Beta   |            | !  |    |    | OK  |
Narvalo.Build       | Stable |            | !  |    |    | OK+ |

(*) Not yet published.

Explanations:
- CA: Static Analysis with FxCop
- GA: Static Analysis with Gendarme
- CC: Static Analysis with Code Contracts
- SA: Source Analysis with StyleCop. OK+ means that the assembly is fully documented.
- TC: Code Coverage. OK means > 90%.

Security
--------

Library             | Attribute
--------------------|------------
Narvalo.Cerbere     | Transparent
Narvalo.Common      | Transparent
Narvalo.Core        | Transparent
Narvalo.Finance     | Transparent
Narvalo.Fx          | Transparent
Narvalo.Web         | (None)      <- see below.

Remark:
All methods in ASP.NET MVC v5 default to SecurityCritical, our only choice would be to mark Narvalo.Web with
the APTCA attribute, but APTCA and ASP.NET MVC [do not work together](https://github.com/DotNetOpenAuth/DotNetOpenAuth/issues/307).
