Issues & Roadmap
================

Design
------

- Comment `Contract.Assume` and `AssumeNotNull`.
- Statics and thread-safety.
- Review all `Format` and boxing
- Where it makes sense, add `EditorBrowsableState`, `DebuggerDisplay` and `DebuggerTypeProxy` attributes.
- Review `IList<T>`, `IEnumerable<T>` and so on in APIs. Document behaviour regarding infinite sequences.
- Review all `IEnumerable` extensions for null-checking and deferred execution.

Narvalo.Core
------------

- Confirm that Maybe methods never returns null.
- Monad.tt, review true argument check for extension methods.

Narvalo.Common
--------------

- Complete unchecked alternates for SqlDataReader.
- Refactore:
  * `Range<T>`. Why force struct constraint? Require for "T?" where T is a struct?
  * Currencies (Serialization & providers)
  * Benchmarks
- Fix Gendarme errors (after refactoring).
- Fix all ContractVerification(false).

Narvalo.Reliability
-------------------

- Very much a work in progress.

Narvalo.Finance
---------------

- Handle overflows.
- BigMoney and BigMoney<TCurrency>.

Narvalo.Web
-----------

- Strengthen handling of paths
  * http://weblog.west-wind.com/posts/2007/Sep/18/ResolveUrl-without-Page
  * http://weblog.west-wind.com/posts/2009/Dec/21/Making-Sense-of-ASPNET-Paths
  * http://stackoverflow.com/questions/1268738/asp-net-mvc-find-absolute-path-to-the-app-data-folder-from-controller
- Make `AssetSection` and `Optimization` sections optional.
- Use static readonly fields instead of const for some fields in Narvalo.Web.Semantic?
- Add an XML schema for the Narvalo.Web config.
- Fix all ContractVerification(false).

Narvalo.Facts
-------------

- Explore SpecFlow.
- Write a T4 template for Monad tests.

Narvalo.Build
-------------

- Add Gendarme and Versioning tasks to Narvalo.Build.

Narvalo.StyleCop.CSharp
-----------------------

- `private readonly static` must start with `s_`.
- `private const` must be uppercase.
- `private` fields must start with an underscore.
- `private` methods must end with an underscore.

Narvalo.FxCop
-------------

- Do not create a `Maybe<T>` where `T` is struct.

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

Infrastructure
--------------

Update necessary following the new organization:
- MSBuild
- PSake
- NuGet

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

### CA, SA...

- Remove the local overrides for CA & SA.
- Make unnecessary to add StyleCop settings to each individual project.
- Fix the double settings: `StyleCop.SourceAnalysis` and local props file.
- Review StyleCop settings, StyleCop cache & ability to change settings used.

### Security

- **Bug:** `SecurityTransparent` attribute and non PCL libraries depending 
  on a PCL library do not play well together.
- Check libraries with SecAnnotate. Do not run SecAnnotate on test libraries? See permcalc.

### Scripts

MSBuild and `PSakefile`:
- **Bug:** If a test project fails, the build does not.
- **Bug:** Whatever we use for `CodeAnalysisSucceededFile`,
  it does not seem to be understood by MSBuild. Setting `CodeAnalysisLogFile` or
  `CodeAnalysisSucceededFile` disables incremental building. Currently, Code Analysis hooks
  are disabled in `Narvalo.Common.targets`.
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
