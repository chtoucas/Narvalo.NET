Issues
======

### Quick tasks
- Review csproj's.
- Add Narvalo.LocalData to all necessary targets (security, build, ...) + packaging.
- `IEnumerable` extensions for `null`-checking and deferred execution.
- Put all `ObjectInvariant()` at the end.
- Strange thing, CC reports that there are two remaining code fixes but does
  not give any details about them.

### Locales - Unassigned
- [CLDR](http://cldr.unicode.org/index/downloads)
  * [NCLDR](https://github.com/GuySmithFerrier/NCLDR)
  * [Onism](https://github.com/pgolebiowski/onism-cldr)
  * [cldrjs](https://github.com/rxaviers/cldrjs)
  * main/root.xml
  * main/en.xml `<territories>`;
    UN M.49. codes `<territory type="AD">Andorra</territory>`,
    `<currencyFormats>`, `<currencies>`
  * bcp47/currency.xml, Code -> Description
- United Nations
  * [United Nations Region Codes](http://unstats.un.org/unsd/methods/m49/m49alpha.htm)
  * [UN M.49](https://en.wikipedia.org/wiki/UN_M.49)
- [CIA World Factbook](https://www.cia.gov/library/publications/download/)
  and [Country Data Codes](https://www.cia.gov/library/publications/the-world-factbook/appendix/appendix-d.html)
- http://www.unc.edu/~rowlett/units/codes/country.htm
- https://en.wikipedia.org/wiki/Country_code
- [Geonames](http://www.geonames.org/export/)
- IANA code
- ITU calling
- [DotNetTimeZoneDb](https://github.com/chrisdostert/DotNetTimeZoneDb)
- INSEE

Narvalo.Fx
------------

- **Bug:** Monad.tt
  * `SumCore()` and `CollectCore()` assert that they never return `null`, is this correct?
  * `Then()` in `JoinCore()` and `GroupJoinCore()` can return `null`.
- `Maybe<T>`, add a reference constraint?
- Enhance EmitFacts.tt.

Narvalo.Core
------------

- `Range<T>`. Why force struct constraint?

Narvalo.Common
--------------

- Complete unchecked alternatives for `SqlDataReader`.

Narvalo.Finance
---------------

- For debugging the T4 templates, see
  [here](http://stackoverflow.com/questions/5588792/determine-solution-configuration-debug-release-when-running-a-t4-template).
- LocalCurrency
- `IConvertible` aka conversion between currencies.
- `Decimal`, `Double`, `BigDecimal`, `BigInteger`... Overflows...
  [Microsoft SQL Server implementation](https://msdn.microsoft.com/en-au/library/ms179882.aspx):
  `Int32` or `Int64`, and designate the lower four digits (or possibly even 2) as
  "right of the decimal point". So "on the edges" you'll need some "* 10000"
  on the way in and some "/ 10000" on the way out. The nicity of this is that
  all your summation can be done using (fast) integer arithmetic.
- `BigMoney` and `BigMoney<TCurrency>`.
- References:
  * [Money Pattern](http://martinfowler.com/eaaCatalog/money.html)
  * [IBAN Calculator](http://www.ibancalculator.com/)
  * [The Swift Codes](https://www.theswiftcodes.com/)
  * [SNV](http://www.currency-iso.org/)
  * [ISO 4217](http://www.iso.org/iso/home/standards/currency_codes.htm)
  * [ISO 4217 - xe](http://www.xe.com/iso4217.php/)
  * [ISO 4217 - Wikipedia](http://en.wikipedia.org/wiki/ISO_4217)
  * [Currency Symbol - Wikipedia](http://en.wikipedia.org/wiki/Currency_symbol)
- Implementations:
  * JodaMoney
  * [NodaMoney](https://github.com/remyvd/NodaMoney/)
  * [Money Type for the CLR](https://bitbucket.org/rplaire/money-type-for-the-clr)
  * [CSharpMoney](https://csharpmoney.codeplex.com/)
  * [NMoneys](https://github.com/dgg/nmoneys)

Narvalo.Web
-----------

- Strengthen handling of paths.
- Make `AssetSection` and `Optimization` sections optional.
- Use static readonly fields instead of const for some fields in Narvalo.Web.Semantic?
- Add an XML schema for the Narvalo.Web config.

Narvalo.Mvp
-----------

- Review `ThrowIfNoPresenterBound`, `Load` event, `PresenterBinder.Release`.
- Review the use of custom presenter types per platform prevents the reuse of
  presenters across different platforms. Maybe is it a necessary evil?
- Add support for EventAggregator (not the same as cross-presenter communication).
- Incorporate ideas from MVCSharp (Task) and maybe GWT, Caliburn.Micro, ReactiveUI or MVVM Light?
  See [here](http://aspiringcraftsman.com/tag/model-view-presenter/)
  and [here](http://aspiringcraftsman.com/2007/08/25/interactive-application-architecture/)

Code Infrastructure
-------------------

### Code Style
- CodeFormatter (crashes with the last exe).
- Change `retval` for a more meaningful name.

### Documentation
- Create a better CSS for docfx.
- Provide better assembly descriptions.
- Document behaviour regarding infinite sequences.
- Reference the MSDN package without version attached.
- Coding Guidelines:
 * rules to explain when to use the new `=>` syntax for methods.
 * rules for using `var`.
- Design notes:
  * Structs vs classes. See
  [CLR Inside Out - Building Tuple](https://msdn.microsoft.com/en-us/magazine/dd942829.aspx)
  and [Proposal: Language support for Tuples](https://github.com/dotnet/roslyn/issues/347)

### Security
- Apply security attributes to NuGet packages?
- Review the (usefulness)[https://github.com/dotnet/corefx/issues/12592] of the security attributes:
  * `AllowPartiallyTrustedCallers`,  `SecurityTransparent`
  * `SecurityCritical`, `SecuritySafeCritical`
- We force security transparency for our PCL libraries (MSBuild).
  `$(_ForceTransparency)` looks a bit fragile to have to explicitly list the PCL
  librairies, better, we could use a rsp file.
- What about permcalc?

Infrastructure
--------------

- Solution-level packages are no longer supported. Currently, we use
  `make.ps1 restore`, it works but updates require manual editing.
  * [GitHub Issue](https://github.com/NuGet/Home/issues/522)
  * [Bring back solution level packages](https://github.com/NuGet/Home/issues/1521)
- Move to .NET Standard
  * [CoreFX](https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/porting.md#unsupported-technologies)
  * [Porting to .NET Core](https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/)
- Enable Continuous Integration (AppVeyor?).
- Create symbol packages (GitLink?).
- Create a Retail solution to test the installation of our NuGet packages. Maybe,
  we could also run the tests against the production packages?

### Scripts
MSBuild and `PSakefile`:
- **Bug:** If a test project fails, the build does not. Also, PSake does not
  always see a failure when an error occurs.
- **Bug:** We only get one xunit report (xunit.html and xunit.xml), the result
  should on the test library.
- **Bug:** Whatever we use for `CodeAnalysisSucceededFile`,
  it does not seem to be understood by MSBuild. Setting `CodeAnalysisLogFile` or
  `CodeAnalysisSucceededFile` disables incremental building. Currently, Code
  Analysis hooks disabled in `Narvalo.Common.targets`.
- **Bug:** in the MvpWebForms sample project, `OutputPath` is wrong (temporary
  fix: override `OutputPath`  in project file). Other strange thing,
  `MvpWebForms.dll.config` is created. The database requires
  [SQL Server 2012 Express LocalDB](https://www.microsoft.com/en-us/download/details.aspx?id=29062).
- Do we need to force `VisualStudioVersion` in `_MyGet-Publish`?
- OpenCover & ReportGenerator: move the core logic from PSake to MSBuild.
  See [OpenCover](https://github.com/OpenCover/opencover/wiki/MSBuild-Support)
- Enable T4-regeneration outside VS?
- What's going on when the `Package` target is also defined?
- Code Contracts:
  * Verify that `SkipDocumentation` is true when building Code Contracts documentation.
  * In addition to `$(SkipCodeContractsReferenceAssembly)`, should we check
    `$(SkipCodeContractsReferenceAssembly)` too?
- `ReadDependenciesFromPackagesConfig` should exclude dev dependencies; they
  sould be marked with `developmentDependency="true"`. We should filter on target.
- PCL libraries:
  * Handle PCL automatically: `FrameworkProfiles.props` import
    and files to be added (we certainly can do this for all packages, non-PCL).
  * In `Make.CustomAfter.props`, we use $(TargetFrameworkProfile)
    to the description $(NuDescription) for PCL libraries, is it the right way
    to do this (In `Make.CustomAfter.targets`, we use
    `'$(TargetFrameworkProfile.StartsWith(Profile))' == 'true'`).

`checkup.ps1`:
- Check completeness of resources.
- Find projects not using `Narvalo.Common.props`.
- Find `DependentUpon` without `SubType` files.
- Find hidden Visual Studio files.
- Find files ignored by git: `git status -u --ignored`.
- Remove `Invoke-RepairTask`. It is only used to check for the copyright header.
  * CodeFormatter can do the same.
  * There are (external) files that should not have a Narvalo copyright.

### nuget-agent.exe
- Watch NuGet update to v3.

### MyGet

- **WARNING:** We MUST upgrade the server to .NET 4.6.
- **WARNING:** Review MyGet.NarvaloOrg.transform.
