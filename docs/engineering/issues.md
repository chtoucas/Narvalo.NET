Issues
======

Tasks
-----

- Automatic code formatting with CodeFormatter.
- Put the private key in the repository.
  See [here](https://msdn.microsoft.com/en-us/library/wd40t7ad(v=vs.110).aspx)
- Remove .nuget directory.

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
----------

- null-check's in generated methods.
- Async versions? Lazy versions?
- Lazy<T, TMetadata> from System.ComponentModel.Composition.
- http://tomasp.net/blog/idioms-in-linq.aspx/

Narvalo.Common
--------------

- Complete unchecked alternatives for `SqlDataReader`.

Narvalo.Finance
---------------

- Sync `Money<TCurrency>`, CurrencyUnit<>`, `MoneyFormatter` for rounding, scale...
- Complete localization of messages.
- Ops with double, float?
- Improve Allocate() and custom allocations.
- Non-decimal moneys: Mauritania & Madagascar;
  see [here](https://en.wikipedia.org/wiki/Denomination_(currency))
- Money.Parse & Money.TryParse.
- `IConvertible` (no) and conversion between currencies.
- Rounding:
  [rounding](https://en.wikipedia.org/wiki/Rounding),
  [cash rounding](https://en.wikipedia.org/wiki/Cash_rounding)
  [sum](https://en.wikipedia.org/wiki/Kahan_summation_algorithm).
- Protect Multiply, Divide and Remainder against absurd results when rounding.
- DecimalRounding.Scale(), check for minimal value?
- Add support for minor units (EUR -> EUr).
  [Wikipedia](https://en.wikipedia.org/wiki/List_of_circulating_currencies),
  [here](http://stackoverflow.com/questions/5023754/do-minor-currency-units-have-a-iso-standard),
  and [here](http://www.fixtradingcommunity.org/pg/discussions/topicpost/167427/gbpgbpgbx).
- Check div and rem.
- Create a currency builder for user-defined currencies.
- Check boxing.
  See [here](http://stackoverflow.com/questions/13558579/are-there-other-ways-of-calling-an-interface-method-of-a-struct-without-boxing-e),
  [here](http://stackoverflow.com/questions/5757324/is-there-boxing-unboxing-when-casting-a-struct-into-a-generic-interface)
  and [here](http://stackoverflow.com/questions/3032750/structs-interfaces-and-boxing).
- Pretty sure that we can get rid off all uint and int overloads.


- BBAN and IIBAN implementations.
- Currency info with implementation from the BCL & CLDR.
- Exchange rate.
- [Liste des devises et de leurs subdivisions](https://fr.wikipedia.org/wiki/Liste_des_monnaies_en_circulation)
- LocalCurrency
- Non-decimal currency: see [here](https://en.wikipedia.org/wiki/Decimalisation)
  and [here](https://en.wikipedia.org/wiki/Non-decimal_currency).
- [Microsoft SQL Server implementation](https://msdn.microsoft.com/en-au/library/ms179882.aspx):
  `Int32` or `Int64`, and designate the lower four digits (or possibly even 2) as
  "right of the decimal point". So "on the edges" you'll need some "* 10000"
  on the way in and some "/ 10000" on the way out. The nicity of this is that
  all your summation can be done using (fast) integer arithmetic.
- References:
  * [Money Pattern](http://martinfowler.com/eaaCatalog/money.html)
  * [IBAN Calculator](http://www.ibancalculator.com/)
  * https://www.iban.com/structure.html
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
  * [PHP-IBAN](https://github.com/globalcitizen/php-iban)
  * [python-stdnum](https://github.com/arthurdejong/python-stdnum)
  * [Ruby](http://www.rubydoc.info/gems/money/Money)
  * [France](http://marlot.org/util/calcul-de-la-cle-nir.php)
  * [BBAN](https://github.com/globalcitizen/php-iban/issues/39)
- Rounding
  * [Lippert](https://blogs.msdn.microsoft.com/ericlippert/2003/09/26/bankers-rounding/)
  * [Microsoft KB](https://support.microsoft.com/en-us/kb/196652)
  * [Xencraft](http://www.xencraft.com/resources/multi-currency.html#rounding)

Narvalo.Web
-----------

- Strengthen handling of paths.
- Make `AssetSection` and `Optimization` sections optional.
- Use static readonly fields instead of const for some fields in Narvalo.Web.Semantic?
- Add an XML schema for the Narvalo.Web config.

Narvalo.Mvp
-----------

- Add localized messages in french for Narvalo.Mvp and Narvalo.Mvp.Web.
- Application Controller and Navigator.
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

### Documentation
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
