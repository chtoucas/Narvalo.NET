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

**WIP**
- Setting SourceAnalysisOverrideSettingsFile has no effect
- Monad.tt, review.
- Review all IEnumerable extensions for null-checking and deferred execution.
- Check all uses of AssumeNotNull
- Remove IEnumerable for Maybe?
- Narvalo.Web.UI.Assets.AssetManager -> Add volatile to the Initialized... fields.
- CC, review and understand all the NO_CCCHECK_SUPPRESSIONS
  Format.CurrentCulture. Narvalo.Int64Encoder.FromFlickrBase58String.
- Remove the use of HasFlag
- Check libraries with SecAnnotate (AllowPartiallyTrustedCallers).
  Do not run SecAnnotate on test libraries? See permcalc.
- Reduce the number of #ifdef -> Should we always define CONTRACTS_FULL
  Output<T> and Maybe<T> remove DEBUG
  to ease refactoring? See Narvalo.Core.props.
- Fix the ContractInvariantMethod warning with CA. See BenchmarkTimer.
- Review the AggressiveInlining
- Create custom FxCop rules:
  * private readonly static must start with s_
  * private const must be uppercase
  * private fields must start with underscore
  * do not create a Maybe<T> where T is struct

**Objectives: Small improvements to SA, CA and the MSBuild infrastructure**

Code Analysis and Source Analysis:
- Improvement: Remove the local CA & SA overrides. Fix any remaining CA and
  SA warnings and errors.
- Bug: Some assemblies raise a CA warning on string resources supposably not used.
- Improvement: Make unnecessary to add StyleCop settings to each project.
  Review StyleCop settings, StyleCop cache & ability to change settings used.
  Review all project files for ExcludeFromSyleCop directives.

MSBuild files:
- Bug: Code Analysis problems. Whatever we use for CodeAnalysisSucceededFile,
  it does not seem to be understood by MSBuild. Setting CodeAnalysisLogFile or
  CodeAnalysisSucceededFile disables incremental building. Code Analysis hooks
  in Narvalo.Common.targets are disabled.

PowerShell
- Enhancement: Analyze logs.

F#
- Finding previous version of packages seems incorrect and deleting fails sometimes?

Miscs:
- Improvement: Complete Guidelines.
  * Explain NuGet package versioning (retail or not). More details on the effect
    of using Retail=true.
  * Fully document any requirement. See:
    [Visual Studio Downloads](http://www.visualstudio.com/downloads/download-visual-studio-vs)
  * Document compiler conditional symbols in use.
- Improvement: Review and fill assembly and NuGet spec.
- Sign F# assemblies.


Not yet planned
---------------

### More automation and further improvements to the overall code quality.

Build Automation:
- Enhancement: PSake CI target should build and test all possible configurations,
  currently Debug|Release, AnyCPU, with or without visible internals.
- Enhancement: Enable T4-regeneration outside VS since, for us, it won't work
  when building from the command-line.
- Enhancement: Start to use Git tags.
- Enhancement: Create symbol packages (or use GitLink?).
- Enhancement: %comspec% /k (@pause).
- Improvement: Complete Narvalo.Build with Gendarme and Versioning tasks.
- Review: What's going on when Package target is also defined.
- Enhancement: Add more tasks to checkup.ps1
  * Find projects not using Narvalo.Common.props
  * Find uncommon project settings
  * Find DependentUpon without SubType files
  * Find hidden VS files
  * Find files ignored by git: git status -u --ignored
  * Repair StyleCop settings

Code Quality:
- Enhancement: Static analysis with Gendarme.
- Improvement: Fix all FIXME, FIXME_PCL, TODO, REVIEW, XXX.
- Improvement: Remove linked files (IEquatable & co).

### Code Cleanup.

Bug corrections:
- Bug: `Maybe<t>`, ensures that if the underlying value is null-ed, things
  continue to work as expected (I really don't think so).
- Bug: Check Monads, VoidOr... for nullity. I am pretty sure
  that VoirOr... and Output are broken.
- Bug: `Enforce`, why can't I use `ContractAbbreviator`? The method get erased.
  The current workaround makes the API too different.
- Bug: `Require.Condition`, `Require.RangeCondition`. CCCheck fails on them.

Code improvements:
- Improvement: Remove all hidden files.
- Improvement: Review the core Code Contracts classes.
- Improvement: Review Narvalo.Minimal.cs.
- Improvement: Remove `Tracer`?
- Improvement: Use true argument check for extension methods.
- Improvement: Add a description to all `Contract.Assume`.
- Improvement: Add `Contract.Ensures` directives, first for Monads.
- Improvement: Check any use of `AssumeNotNull`.
- Improvement: Use `Format` instead of `String.Format`.
- Improvement: `Serializable` and PCL.
- Improvement: `String.IsNullOrWhiteSpace` vs `String.IsNullOrEmpty`.
- Improvement: Validate the usefulness of `ExceptionFactory`. Replace it with
  something like a `ThrowHelper`?
- Improvement: Cleanup Narvalo.Brouillons.
- Improvement: Review all GetHashCode() methods.
- Improvement: Review all ToString() methods.

Style improvements:
- Improvement: Always use resources for messages.
- Improvement: Fix regions, ////, /*!.
- Improvement: Rewrite all french comments in english.
- Improvement: Replace all empty method with "// Intentionally left blank.".

### Better tested and documented Narvalo.Core assembly.

- Improvement: Add more and more code contracts.
- Improvement: Large code coverage of Narvalo.Core.
- Enhancement: Write a T4 template for Monad tests.
- Improvement: 100% SA documentation rules.

### Better tested and documented Narvalo.Common assembly.

- Improvement: Add more and more code contracts.
- Improvement: Large code coverage of Narvalo.Common.
- Improvement: 100% SA documentation rules.

### Better tested and documented Narvalo.Web assembly.

- Improvement: Add more and more code contracts.
- Improvement: Large code coverage of Narvalo.Web.
- Improvement: 100% SA documentation rules.
- Enhancement: Add an XML schema for the Narvalo config.
- Enhancement: Finish the Optimization & Semantic classes.

At this point we should have a first stable release for the core Narvalo assemblies.

### Documentation & Literate Programming.

- Bug: Prose is simply broken.
- Enhancement: Build C# documentation.
- Enhancement: Use CommonMark instead of Markdown Deep.
  NB: PostSharp, Roslyn, MEF, Serilog, Autofac.
- Enhancement: ILMerge Prose.
- Enhancement: Create a website for the project.
- Improvement: Move namespace docs to NamespaceDocs files.

At this point we should have a first useful release for the core assemblies.

### Strengthen core assemblies.

- Enhancement: Check reports from performed analysis.
- Enhancement: Add Code Coverage + Report Generator.
- Enhancement: Explore Pex.
- Enhancement: Explore SpecFlow.

### Secure core assemblies.

- Improvement: Finish SecAnnotate.
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

### Optimize core assemblies.


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


Not yet classified
------------------

- Enhancement: More supported platforms (64bit) and target frameworks.
- Enhancement: Create Narvalo.FxCop.
- Improvement: Complete Narvalo.StyleCop.CSharp.
- Enhancement: Zip artefacts when done building.
- Improvement: T4 re-generation has been disabled since it requires
  VS hosting. Is there a way we can enable T4 re-generation during a build?
