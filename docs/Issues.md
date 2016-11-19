Current works
=============

- Put the private key in the repository
  See: https://msdn.microsoft.com/en-us/library/wd40t7ad(v=vs.110).aspx
- Documentation:
  * Version & dependencies.
  * Changelog
  * Icons in Wiki
  * GitHub projects.
- Build scripts:
  * OpenCover & Gendarme: move the core logic from PSake to MSBuild.
  * In addition to `$(SkipCodeContractsReferenceAssembly)`, should we check
    `$(SkipCodeContractsReferenceAssembly)` too?
  * `ReadDependenciesFromPackagesConfig` should exclude dev dependencies. They
     sould be marked with `developmentDependency="true"`.
     We need also to filter on target.
  * Nuspec: handle automatically PCL, `FrameworkProfiles.props` import
    and files to be added (we certainly can do this for all packages, non-PCL).
  * Nuspec (oboslete): In `Make.CustomAfter.props`, we use $(TargetFrameworkProfile) to
    patch the description $(NuDescription) for PCL libraries, is it the right way
    to do this (In `Make.CustomAfter.targets`, we use
    `'$(TargetFrameworkProfile.StartsWith(Profile))' == 'true'`).
  * Should I call Rebuild before Package?
  * Should I call Rebuild before the analysis tasks?
- DocFX:
  * External links via `<see cref="!:" />` are not understood by docfx.
  * NamespaceDocs are not understood by docfx, rewrite needed.
  * `<content markup="commonmark">` is not understood, rewrite needed.
  * Reference the MSDN package without version attached.
  * Build tasks (clean, build)
- Migrate from StyleCop to StyleCop.Analyzers.
  * Disable StyleCop: set `StyleCopEnabled` to `false` (see `Package.StyleCop.targets`).
  * When migration is done, update the README to detail the new static analysis results
    and the documentation: Overview.md and Guidelines.md.
  * Create `tools\src\Narvalo.ProjectAnalyzers` replacement of `Narvalo.StyleCop`.
  * Create `src\Narvalo.Analyzers`.
  * RuleSet Schema:
    [Format](https://github.com/dotnet/roslyn/blob/master/docs/compilers/Rule%20Set%20Format.md)
    and [Schema](https://github.com/dotnet/roslyn/blob/master/src/Compilers/Core/Portable/RuleSet/RuleSetSchema.xsd)
- C#
  * use `nameof` everywhere.
  * rules to explain when to use the new `=>` syntax for methods.
  * rules for using of `var`.
- **Bug:** in the MvpWebForms sample project, `OutputPath` is wrong (temporary fix: override `OutputPath`
  property in project file). Other strange thing, `MvpWebForms.dll.config` is created.
  The database requires [SQL Server 2012 Express LocalDB](https://www.microsoft.com/en-us/download/details.aspx?id=29062).
- Move to .Net Standard
  * [CoreFX](https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/porting.md#unsupported-technologies)
  * [Porting to .NET Core](https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/)
- VS 2015
  * Do we need to change `VisualStudioVersion` (`_MyGet-Publish`)? See also
    the Framework property at the beginning of the file.
  * Check `TargetFrameworkVersion`.
  * Do we really need to use `ToolsVersion="14.0"`?
  * Force `MininalVisualStudioVersion` in the solution files?
- Solution-level packages are no longer supported. Currently, we can use `make.ps1 restore`,
  we still need an update task.
  * [GitHub Issue](https://github.com/NuGet/Home/issues/522)
  * [Bring back solution level packages](https://github.com/NuGet/Home/issues/1521)


Issues & Roadmap
================

- Provide better assembly descriptions.
- Thread-safety: statics (always) and read-only properties (?).
- Prefer `for` to `foreach` with arrays.
- Change `retval` for a more meaningful name.
- Review all `Format` and boxing
- Where it makes sense, add `EditorBrowsableState`, `DebuggerDisplay` and `DebuggerTypeProxy` attributes.
- Review `IList<T>`, `IEnumerable<T>` and so on in APIs. Document behaviour regarding infinite sequences.
- Review all `IEnumerable` extensions for null-checking and deferred execution.

Narvalo.Fx
------------

- Remove Maybe.None.
- Maybe and IEquatable<T>.
- IsNullable && HasZero
- Update Maybe<T> documentation.
  https://msdn.microsoft.com/en-us/magazine/dd942829.aspx
  http://stackoverflow.com/questions/2410710/why-is-the-new-tuple-type-in-net-4-0-a-reference-type-class-and-not-a-value-t
  https://github.com/dotnet/roslyn/issues/347
- Monad.tt.
  * Split.
  * SumCore() and CollectCore() assert that they never return null but this is not always true.
  * Use of .Then(): in JoinCore and GroupJoinCore() can return null.

Narvalo.Core
------------

- `Range<T>`. Why force struct constraint? Require for "T?" where T is a struct?

Narvalo.Common
--------------

- Complete unchecked alternatives for SqlDataReader.

Narvalo.Finance
---------------

- Use SnvCurrencyXmlReader
- See https://github.com/JodaOrg/joda-money,
  http://www.ibancalculator.com/
  https://www.theswiftcodes.com/
- Since we generate automatically the currencies, is it possible that one currency
  simply disappears after un update? This would be a breaking change.
- Add a numeric representation of a currency.
  See http://www.iso.org/iso/home/standards/currency_codes.htm
- Decimal overloads.
- Handle overflows.
- Comparisons between `Money` and `Money<T>`. Or simply remove `Money`?
- IConvertible?
- BigMoney and BigMoney<TCurrency>.
- Represents a monetary value.
  http://martinfowler.com/eaaCatalog/money.html
- Microsoft SQL Server implementation:
  Int32 or Int64, and designate the lower four digits (or possibly even 2) as
  "right of the decimal point". So "on the edges" you'll need some "* 10000"
  on the way in and some "/ 10000" on the way out. The nicity of this is that
  all your summation can be done using (fast) integer arithmetic.
  https://msdn.microsoft.com/en-au/library/ms179882.aspx
 - Implementation in .NET:
  * http://www.codeproject.com/Articles/28244/A-Money-type-for-the-CLR
  * https://bitbucket.org/rplaire/money-type-for-the-clr
  * https://github.com/danielcrenna/webstack/blob/master/money/Money/
  * https://csharpmoney.codeplex.com/
  * https://code.google.com/p/nmoneys/
  * http://www.xe.com/iso4217.php/
- Standards:
  * http://en.wikipedia.org/wiki/ISO_4217
  * http://www.currency-iso.org/
  * http://en.wikipedia.org/wiki/Currency_symbol
- JodaMoney & NodaMoney
  * https://github.com/JodaOrg/joda-money/blob/master/src/main/java/org/joda/money/BigMoney.java
  * https://github.com/remyvd/NodaMoney/tree/develop/NodaMoney


Narvalo.Web
-----------

- Strengthen handling of paths
  * http://weblog.west-wind.com/posts/2007/Sep/18/ResolveUrl-without-Page
  * http://weblog.west-wind.com/posts/2009/Dec/21/Making-Sense-of-ASPNET-Paths
  * http://stackoverflow.com/questions/1268738/asp-net-mvc-find-absolute-path-to-the-app-data-folder-from-controller
- Make `AssetSection` and `Optimization` sections optional.
- Use static readonly fields instead of const for some fields in Narvalo.Web.Semantic?
- Add an XML schema for the Narvalo.Web config.

Narvalo.Facts
-------------

- Explore SpecFlow.
- Write a T4 template for Monad tests.

Narvalo.Mvp
-----------

- Review `ThrowIfNoPresenterBound`, `Load` event, `PresenterBinder.Release`.
- Review the use of custom presenter types per platform prevents the reuse of
  presenters across different platforms. Maybe is it a necessary evil?
- Add support for Application Controller, Navigator, EventAggregator (not the same
  as cross-presenter communication).
- Incorporate ideas from MVCSharp (Task) and maybe GWT, Caliburn.Micro, ReactiveUI or MVVM Light?
- Add support for WPF.
- See
  * http://aspiringcraftsman.com/tag/model-view-presenter/
  * http://aspiringcraftsman.com/2007/08/25/interactive-application-architecture/

Infrastructure
--------------

- Publishing Narvalo.Mvp should fail if we do not publish Narvalo.Core?

### Continuous Integration

- Enable Continuous Integration (AppVeyor?).
- Analyze logs and reports across builds.

### Packages

- Create symbol packages (or use GitLink?).
- Apply security attributes?

### Security

- Review the (usefulness)[https://github.com/dotnet/corefx/issues/12592] of the security attributes:
  * `AllowPartiallyTrustedCallers`,  `SecurityTransparent`
  * `SecurityCritical`, `SecuritySafeCritical`
- Here are some commonly found assembly level attributes:
```
[assembly: AllowPartiallyTrustedCallers(PartialTrustVisibilityLevel = PartialTrustVisibilityLevel.NotVisibleByDefault)]
[assembly: SecurityRules(SecurityRuleSet.Level2, SkipVerificationInFullTrust = true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
```
- In MSBuild (SecAnnotate), we force security transparency for our PCL libraries.
  `$(_ForceTransparency)` looks a bit fragile to have to explicitly list the PCL
  librairies, better, we could use a rsp file.
- What about permcalc?

### Scripts

MSBuild and `PSakefile`:
- **Bug:** Build target is broken. Add all proj to FullBuild.
- **Bug:** If a test project fails, the build does not.
- **Bug:** Whatever we use for `CodeAnalysisSucceededFile`,
  it does not seem to be understood by MSBuild. Setting `CodeAnalysisLogFile` or
  `CodeAnalysisSucceededFile` disables incremental building. Currently, Code Analysis hooks
  are disabled in `Narvalo.Common.targets`.
- **Bug:** Make sure PSake reports failure whenever an error occurs.
- Complete common settings for F# projects.
- Enable T4-regeneration outside VS since, currently it does not work when building from the command-line.
- What's going on when the `Package` target is also defined?
- Verify that `SkipDocumentation` is true when building Code Contracts documentation.

`publish-*.fsx`
- **Bug:** Fails to push to the official NuGet server but works otherwise.

`checkup.ps1`:
- Find projects not using `Narvalo.Common.props`.
- Find uncommon project settings.
- Find `DependentUpon` without `SubType` files.
- Find hidden Visual Studio files.
- Find files ignored by git: `git status -u --ignored`.
- Repair StyleCop settings.

New script or T4 template to create tests from code contracts.

New script to check for completeness of resources.
