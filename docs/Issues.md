Migration to VS2015
===================

- Disabled StyleCop `StyleCopEnabled` to `false` (see `Package.StyleCop.targets).
- Code contracts (disabled)
  * `PSakefile`: `'/p:SkipCodeContractsReferenceAssembly=true'` instead of
    `'/p:SkipCodeContractsReferenceAssembly=false'`
    in `_CI-InitializeVariables` and `_Package-InitializeVariables`.
  * Raise a warning instead of an error in `Make.CustomAfter.targets` if skipping CC.
  * Disabled `CodeContractsEmitXMLDocs` in `Make.CustomAfter.props`.

- Force `MininalVisualStudioVersion` in the solution files? 
- Do we really need to use `ToolsVersion="14.0"`.
- Analyzers:
  * Migrate from StyleCop to StyleCop.Analyzers.
  * Improve `DisableAnalyzersForVisualStudioBuild` target in `Narvalo.Common.targets`.
- In `PSakefile.ps1`, do we need to change `VisualStudioVersion` (`_MyGet-Publish`)? See also
  the Framework property at the beginning of the file.
- For test projects, we should automatically add the TestCommon shared project.
    `<Import Project="..\Narvalo.TestCommon\Narvalo.TestCommon.projitems" Label="Shared" />`
- MvpWebForms sample project: `OutputPath` is wrong (temporary fix: override `OutputPath`
  property in project file). Other strange thing, `MvpWebForms.dll.config` is created.
  LocalDB requires SQL Server Express.
- C#
  * use `nameof` everywhere.


Issues & Roadmap
================

- CodeAnalysisDictionary does not seem to be understood.
- When, Guard & co.
- IsNullable && HasZero
- Remove Maybe.None.
- Maybe and IEquatable<T>.

Design
------

- Statics and thread-safety.
- Review all `Format` and boxing
- Where it makes sense, add `EditorBrowsableState`, `DebuggerDisplay` and `DebuggerTypeProxy` attributes.
- Review `IList<T>`, `IEnumerable<T>` and so on in APIs. Document behaviour regarding infinite sequences.
- Review all `IEnumerable` extensions for null-checking and deferred execution.

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
- Use of .Then(): in JoinCore and GroupJoinCore() can return null.

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

- Very much a work in progress.
- See
  * kite (java)
    https://github.com/williewheeler/kite
  * jrugged (java)
  * Hystrix (java)

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
  http://aspiringcraftsman.com/tag/model-view-presenter/
  http://aspiringcraftsman.com/2007/08/25/interactive-application-architecture/

Infrastructure
--------------

- Publishing Narvalo.Mvp should fail if we do not publish Narvalo.Core?

### Continuous Integration

- Enable Continuous Integration with AppVeyor.
- Analyze logs and reports across builds.
- Build C# documentation.
- Build and test all possible configurations.

### Packages

- Create symbol packages (or use GitLink?).
- Maybe we can simplify semantic versioning for edge packages:
  * http://semver.npmjs.com/
  * http://www.infoq.com/news/2015/04/semver-calculator-npm

### Security

- https://msdn.microsoft.com/en-us/library/dd233102.aspx
- https://msdn.microsoft.com/en-us/magazine/ee336023.aspx
- http://stackoverflow.com/questions/5055632/net-4-allowpartiallytrustedcallers-attribute-and-security-markings-like-secur
- Bug: In MSBuild, we force security transparency for our PCL libraries in MSBuild.
- Do not run SecAnnotate on test libraries?
- See permcalc & PEVerify /transparent  https://msdn.microsoft.com/en-us/library/62bwd2yd.aspx
- Narvalo.Web
    * Review added SecuritySafeCritical & SecurityCritical attributes.
    * Security attributes and ASP.NET MVC do not work together:
      See https://github.com/DotNetOpenAuth/DotNetOpenAuth/issues/307
- AllowPartiallyTrusted & SecurityRules options:
```
[assembly: AllowPartiallyTrustedCallers(PartialTrustVisibilityLevel = PartialTrustVisibilityLevel.NotVisibleByDefault)]
[assembly: SecurityRules(SecurityRuleSet.Level2, SkipVerificationInFullTrust = true)]
```

References:
- [CAS](http://msdn.microsoft.com/en-us/library/c5tk9z76%28v=vs.110%29.aspx)
- [APTCA](https://msdn.microsoft.com/en-us/magazine/ee336023.aspx)
- [SecAnnotate](http://blogs.msdn.com/b/shawnfa/archive/2009/11/18/using-secannotate-to-analyze-your-assemblies-for-transparency-violations-an-example.aspx)
- [SecAnnotate and PCL](http://stackoverflow.com/questions/12360534/how-can-i-successfully-run-secannotate-exe-on-a-library-that-depends-on-a-portab)
- [Tutorial](http://www.codeproject.com/Articles/329666/Things-I-learned-while-implementing-my-first-Level)

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
- Make sure a build fails when SecAnnotate does too.
- What's going on when the `Package` target is also defined?
- Verify that `SkipDocumentation` is true when building Code Contracts documentation.

`publish-*.fsx`
- **Bug:** Fails to push to the official NuGet server but works otherwise.
- **Bug:** Finding previous version of packages seems incorrect and deleting fails sometimes?

`checkup.ps1`:
- Find projects not using `Narvalo.Common.props`.
- Find uncommon project settings.
- Find `DependentUpon` without `SubType` files.
- Find hidden Visual Studio files.
- Find files ignored by git: `git status -u --ignored`.
- Repair StyleCop settings.

New script or T4 template to create tests from code contracts.

New script to check for completeness of resources.
