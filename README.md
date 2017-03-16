Narvalo.NET
===========

- [Documentation](https://github.com/chtoucas/Narvalo.NET/tree/master/docs)
- [License](https://github.com/chtoucas/Narvalo.NET/tree/master/LICENSE.txt)
- [License for WebFormsMvp](https://github.com/chtoucas/Narvalo.NET/tree/master/LICENSE-WebFormsMvp.txt)
  on which depend Narvalo.Mvp and Narvalo.Mvp.Web.

Project | Summary | Dependencies* | .NET Platform | Package
--------|---------|---------------|---------------|--------
[Narvalo.Build](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Build/) | Custom **MSBuild** tasks | - | Framework 4.5 | [![NuGet](https://img.shields.io/nuget/v/Narvalo.Build.svg)](https://www.nuget.org/packages/Narvalo.Build/)
[Narvalo.Cerbere](https://github.com/chtoucas/Cerbere) | **Code Contracts** and debugging put together | - | Standard 1.0 |  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Cerbere.svg)](https://www.nuget.org/packages/Narvalo.Cerbere/)
[Narvalo.Common](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Common/) | Utilities and extension methods | Core, Fx | Framework 4.5 |  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Common.svg)](https://www.nuget.org/packages/Narvalo.Common/)
[Narvalo.Core](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Core/) | Dependency for the other packages | - | Standard 1.0 |  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Core.svg)](https://www.nuget.org/packages/Narvalo.Core/)
[Narvalo.Finance](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Finance/) | **BIC** and **IBAN** types | Core, Fx | Standard 1.0 |  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Finance.svg)](https://www.nuget.org/packages/Narvalo.Finance/)
[Narvalo.Fx](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Fx/) | **Monads** and functional C# | Core | Standard 1.0 |  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Fx.svg)](https://www.nuget.org/packages/Narvalo.Fx/)
[Narvalo.Money](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Money/) | **Money** and **Currency** types | Core | Standard 1.0 |  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Money.svg)](https://www.nuget.org/packages/Narvalo.Money/)
[Narvalo.Mvp](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Mvp/) | **Model-View-Presenter** (MVP) framework | Core | Framework 4.5 |  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Mvp.svg)](https://www.nuget.org/packages/Narvalo.Mvp/)
[Narvalo.Mvp.Web](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Mvp.Web/) | MVP for WebForms | Core, MVP | Framework 4.5 |  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Mvp.Web.svg)](https://www.nuget.org/packages/Narvalo.Mvp.Web/)
[Narvalo.Web](https://github.com/chtoucas/Narvalo.NET/tree/master/src/Narvalo.Web/) | ASP.NET optimization | Core, Common, Fx | Framework 4.5 |  [![NuGet](https://img.shields.io/nuget/v/Narvalo.Web.svg)](https://www.nuget.org/packages/Narvalo.Web/)

(*) We only list the internal dependencies.

Samples:
  - [Command-Line MVP sample](https://github.com/chtoucas/Narvalo.NET/tree/master/samples/MvpCommandLine)
  - [WebForms MVP sample](https://github.com/chtoucas/Narvalo.NET/tree/master/samples/MvpWebForms)
