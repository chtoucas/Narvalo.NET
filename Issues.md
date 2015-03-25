Issues
======

When fixed, issues not directly related to testable codes are described
in the ChangeLog file.

Other closed issues are explicitely described and tested in one of the test
libraries. NB: To spot them, you should look for the `Issue` attribute.

All issues are either Bug, Enhancement or Improvement.

When an issue is closed, move it to the ChangeLog. Further more, if it is a bug,
add the necessary tests to be sure it does not pass through again.


Work in progress
----------------

- Review the definition of DEBUG in CodeContracts builds.
- Review ValidatedNotNull in the shared Require file.
- Inlining.
- Unsafe alternates: validate their usefulness and complete SqlDataReader.
- Confirm that Maybe methods never returns null.
- Refactorings:
 * Range
 * Currencies
 * Benchmarks
- Enforce & Range T struct constraint? Require for T? where T is a struct?
- Review Monad.tt.
- Improvement: True argument check for extension methods.
- Improvement: Review all IEnumerable extensions for null-checking and deferred execution.
- Improvement: Check any use of `AssumeNotNull`.
- Improvement: `String.IsNullOrWhiteSpace` vs `String.IsNullOrEmpty`.
- Improvement: Review all GetHashCode() methods.
- Improvement: Review all ToString() methods.
- make.ps1 -r fails to push to the official NuGet server but works otherwise.


Not yet planned
---------------

### More automation and further improvements to the overall code quality.

Build Automation:
- Enhancement: PSake CI target should build and test all possible configurations,
  currently Debug|Release, AnyCPU, with or without visible internals.
- Enhancement: Enable T4-regeneration outside VS since, for us, it won't work
  when building from the command-line.
- Setting SourceAnalysisOverrideSettingsFile has no effect
- Enhancement: Start to use Git tags.
- Enhancement: Create symbol packages (or use GitLink?).
- Enhancement: %comspec% /k (@pause).
- Improvement: Complete PSake / Gendarme and Versioning tasks.
- Improvement: Add Gendarme and Versioning tasks to Narvalo.Build.
- Review: What's going on when Package target is also defined.
- Enhancement: Add more tasks to checkup.ps1
  * Find projects not using Narvalo.Common.props
  * Find uncommon project settings
  * Find DependentUpon without SubType files
  * Find hidden VS files
  * Find files ignored by git: git status -u --ignored
  * Repair StyleCop settings
- Bug: Some assemblies raise a CA warning on string resources supposably not used.
- Bug: Code Analysis problems. Whatever we use for CodeAnalysisSucceededFile,
  it does not seem to be understood by MSBuild. Setting CodeAnalysisLogFile or
  CodeAnalysisSucceededFile disables incremental building. Code Analysis hooks
  in Narvalo.Common.targets are disabled.
- Enhancement: Analyze logs.
- Bug: F#. Finding previous version of packages seems incorrect and deleting fails sometimes?
- Improvement: Complete Guidelines.
  * Explain NuGet package versioning (retail or not). More details on the effect
    of using Retail=true.
  * Fully document any requirement. See:
    [Visual Studio Downloads](http://www.visualstudio.com/downloads/download-visual-studio-vs)
  * Document compiler conditional symbols in use.
- Improvement: Review and fill assembly and NuGet spec.
- Sign F# assemblies.
- Enhancement: More supported platforms (64bit) and target frameworks.
- Enhancement: Create Narvalo.FxCop.
- Improvement: Complete Narvalo.StyleCop.CSharp.
- Enhancement: Zip artefacts when done building.

Code Quality:
- Improvement: Remove the local CA & SA overrides.
- Improvement: Make unnecessary to add StyleCop settings to each project.
  Review StyleCop settings, StyleCop cache & ability to change settings used.
- Create custom StyleCop/FxCop rules:
  * private readonly static must start with s_
  * private const must be uppercase
  * private fields must start with underscore
  * do not create a Maybe<T> where T is struct
- Enhancement: Add Code Coverage + Report Generator.
- Enhancement: Explore SpecFlow.

### Better tested and documented Narvalo.Core assembly.

- Enhancement: Write a T4 template for Monad tests.
- Improvement: Add more code contracts and large code coverage.

### Better tested and documented Narvalo.Common assembly.

- Improvement: Add more code contracts and large code coverage.

### Better tested and documented Narvalo.Web assembly.

- Enhancement: Add an XML schema for the Narvalo config.
- Improvement: Add more code contracts and large code coverage.

### Documentation & Literate Programming.

- Enhancement: Build C# documentation.
- Enhancement: Use CommonMark instead of Markdown Deep.
  NB: PostSharp, Roslyn, MEF, Serilog, Autofac. Or no, simply XSLTize the xml.

### Strengthen core assemblies.

- Enhancement: Benchmark across builds.
- Enhancement: Check reports from performed analysis.

### Secure core assemblies.

- Check libraries with SecAnnotate (AllowPartiallyTrustedCallers).
  Do not run SecAnnotate on test libraries? See permcalc.
- Improvement: Make sure a build fails when SecAnnotate does too.
- Enhancement: Implements security attributes, for instance:
```
[assembly: SecurityRules(SecurityRuleSet.Level2)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityTransparent]
//[assembly: SecurityCritical]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: SecurityRules(SecurityRuleSet.Level1, SkipVerificationInFullTrust = true)]
[assembly: AllowPartiallyTrustedCallers(PartialTrustVisibilityLevel = PartialTrustVisibilityLevel.NotVisibleByDefault)]
```
  by the VS extension. See:
  * [CAS](http://msdn.microsoft.com/en-us/library/c5tk9z76%28v=vs.110%29.aspx)
  * [APTCA](http://msdn.microsoft.com/fr-fr/magazine/ee336023.aspx)
  * [SecAnnotate](http://blogs.msdn.com/b/shawnfa/archive/2009/11/18/using-secannotate-to-analyze-your-assemblies-for-transparency-violations-an-example.aspx)

### Narvalo.Mvp (en vrac)

- Review: ThrowIfNoPresenterBound, Load event, PresenterBinder.Release.
- Enhancement: Tests. We can start by porting those from WebFormsMvp.
- Enhancement: Documentation and userguide.
- Improvement: Using custom presenter types per platform prevents
  the reuse of presenters across different platforms. Maybe is it a necessary evil?
- Enhancement: Add support for WPF.
- Enhancement: Add support for Application Controller, Navigator,
  EventAggregator (not the same as cross-presenter communication).
- Enhancement: Incorporate ideas from MVCSharp (Task) and maybe
  GWT, Caliburn.Micro, ReactiveUI or MVVM Light?
- Improvement: For Narvalo.Mvp.Windows.Forms, cross-presenter
  communication is not functional. Things to work on before it might prove
  to be useful:
  * Right now, only controls contained in a MvpForm share the same presenter binder.
    We need something similar to what is done with ASP.NET (PageHost) but the situation
    is a bit more complicated due to the different execution model. Controls
    are fully loaded before we reach the CreateControl or Load events in the form
    container where we normally perform the binding.
  * The message coordinator must support unsubscription (automatic or manual).

