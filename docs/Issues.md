Current works
=============

- Add Narvalo.Reliability and Narvalo.Reliability.Facts to Make projects (see also OpenCover below).
- Check that moving EnableGendarme form Narvalo.Common.props to Make.CustomAfter.props
  didn't break anything.
- Build C# documentation (docfx):
  * [docfx](http://dotnet.github.io/docfx/tutorial/docfx_getting_started.html)
  * External links via `<see cref="!:" />` are not understood by docfx.
  * NamespaceDocs are not understood by docsfx.
  * `<content markup="commonmark">` is not understood.
  * reference MSDN package without version
  * [Monodoc](http://www.mono-project.com/docs/tools+libraries/tools/monodoc/generating-documentation/)
  * [.NET docs](https://github.com/dotnet/docs/issues/772)
  * DocPlagiarizer
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
  * decide rules to when to use the new `=>` syntax for methods.
  * review the use of `var`.
- For test projects, we should automatically add the TestCommon shared project.
    `<Import Project="..\Narvalo.TestCommon\Narvalo.TestCommon.projitems" Label="Shared" />`
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
- OpenCover
  * Update the PSake task (missing xunit.runner.console package).
  * Handle more than one test project (`Narvalo.Reliability.Facts`).
  * Simply move to a MSBuild target.
- Solution-level packages are no longer supported
  * [GitHub Issue](https://github.com/NuGet/Home/issues/522)
  * [Bring back solution level packages](https://github.com/NuGet/Home/issues/1521)


Issues & Roadmap
================

- Provide better assembly descriptions.
- Thread-safety: statics and read-only properties.
- Prefer `for` to `foreach` with arrays.
- Change `retval` for a more meaningful name.
- Review all `Format` and boxing
- Where it makes sense, add `EditorBrowsableState`, `DebuggerDisplay` and `DebuggerTypeProxy` attributes.
- Review `IList<T>`, `IEnumerable<T>` and so on in APIs. Document behaviour regarding infinite sequences.
- Review all `IEnumerable` extensions for null-checking and deferred execution.
- When, Guard & co.
- IsNullable && HasZero
- Remove Maybe.None.
- Maybe and IEquatable<T>.

Narvalo.Cerbere
---------------

- More overloads for Require.Greater...

Narvalo.Fx
------------

- Update Maybe<T> documentation.
  https://msdn.microsoft.com/en-us/magazine/dd942829.aspx
  http://stackoverflow.com/questions/2410710/why-is-the-new-tuple-type-in-net-4-0-a-reference-type-class-and-not-a-value-t
  https://github.com/dotnet/roslyn/issues/347
- Monad.tt.
  * Split.
  * Review true argument check for extension methods.
  * SumCore() and CollectCore() assert that they never return null but this is not always true.
  * Use of .Then(): in JoinCore and GroupJoinCore() can return null.

Narvalo.Core
------------

- XmlReader
- ParseTo
- StringHelper
- `Range<T>`. Why force struct constraint? Require for "T?" where T is a struct?

Narvalo.Common
--------------

- Complete unchecked alternates for SqlDataReader.

Narvalo.Finance
---------------

- Decimal overloads.
- Handle overflows.
- Comparisons between `Money` and `Money<T>`. Or simply remove `Money`?
- Messages for exceptions!
- IConvertible?
- BigMoney and BigMoney<TCurrency>.

Narvalo.Reliability
-------------------

See
- [Polly](https://github.com/App-vNext/Polly)
- [kite](https://github.com/williewheeler/kite)
- jrugged
- [Hystrix](https://github.com/Netflix/Hystrix)

Narvalo.Web
-----------

- Finish refactoring.
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

Narvalo.T4
----------

- Use SnvCurrencyXmlReader

Narvalo.Mvp
-----------

- Review `ThrowIfNoPresenterBound`, `Load` event, `PresenterBinder.Release`.
- Review the use of custom presenter types per platform prevents the reuse of
  presenters across different platforms. Maybe is it a necessary evil?
- Add support for Application Controller, Navigator, EventAggregator (not the same
  as cross-presenter communication).
- Incorporate ideas from MVCSharp (Task) and maybe GWT, Caliburn.Micro, ReactiveUI or MVVM Light?
- Add support for WPF.
- For Narvalo.Mvp.Windows.Forms, cross-presenter communication is not functional.
  Things to work on before it might prove to be useful:
  * Right now, only controls contained in a MvpForm share the same presenter binder.
    We need something similar to what is done with ASP.NET (`PageHost`) but the situation
    is a bit more complicated due to the different execution model. Controls
    are fully loaded before we reach the `CreateControl` or `Load` events in the form
    container where we normally perform the binding.
  * The message coordinator must support unsubscription (automatic or manual).
- See
  * http://aspiringcraftsman.com/tag/model-view-presenter/
  * http://aspiringcraftsman.com/2007/08/25/interactive-application-architecture/

Infrastructure
--------------

- Publishing Narvalo.Mvp should fail if we do not publish Narvalo.Core?
- CodeAnalysisDictionary does not seem to be understood.

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
