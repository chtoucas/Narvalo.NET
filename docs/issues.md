Issues & TODOs (en vrac)
========================

Divers et variés
----------------

- [High] `make.ps1` fails sometimes for weird reasons (most of the time
  it works perfectly). There seems to be a problem with the new MSBuild when
  we have VS opened and it is running at the same time:
```
C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\Microsoft.Common.CurrentVersion.targets(
3245,5): error MSB3491: Impossible d'écrire des lignes dans le fichier "I:\github\Narvalo.NET\work\obj\Debug\CoreCompil
eInputs.cache". Le processus ne peut pas accéder au fichier 'I:\github\Narvalo.NET\work\obj\Debug\CoreCompileInputs.cac
he', car il est en cours d'utilisation par un autre processus. [I:\github\Narvalo.NET\tests\Narvalo.Fx.Facts\Narvalo.Fx
.Facts.csproj]
```
  See [dotnet issue](https://github.com/dotnet/cli/issues/4786)?
- Resources for Narvalo.Money get mixed up when using `make.ps1 pack`:
  it embeds `Strings.resx` from Narvalo.Fx????!!!! Really disturbing.
  Current fix: rename `Strings.resx` to `Strings_Money.resx`.
- Review localization for Narvalo.Build, Narvalo.Mvp, Narvalo.Mvp.Web
  and Narvalo.Web.
- `make.ps1`: Add filter to test only one Trait.
- [Low] Automatically create the assembly infos from the NuGet spec.
- [Low] Update CodeFormatter when it supports C# 7.0. After that, change
  from `format-code.cmd` to `format-code.ps1`.
- [Low] Add a "dry run" option to NuGetAgent.
- [Low] Enable continuous integration (Travis, AppVeyor, Coverall, Readthedoc, GitLink)?

### Migration to .NET Standard projects.
- Pending: support for Code Analysis (I really want this).
- We might have a conflict between the new MSBuild and our target `Package`.
- Use `PackageReference` instead of parsing `packages.config`.
- Update to use the (new) native package properties: `Version`, `AssemblyVersion`,
  `FileVersion`... instead of `_AssemblyVersion`, `_AssemblyInformationalVersion`
  and `_AssemblyFileVersion`.

Narvalo.Common
--------------

Next:
- Make it a .NET Standard 2.0 library.

Narvalo.Finance
---------------

- Implement BBAN validation.

Next:
- [Internet International Bank Account Number](https://tools.ietf.org/html/draft-iiban-00)
- BBAN and IBAN implementations:
  * [PHP-IBAN](https://github.com/globalcitizen/php-iban)
  * [BBAN](https://github.com/globalcitizen/php-iban/issues/39)
  * [python-stdnum](https://github.com/arthurdejong/python-stdnum)
  * [France](http://marlot.org/util/calcul-de-la-cle-nir.php)

Narvalo.Fx
----------

- LINQ changes:
  * Replace `CollectIterator` by `CollectAny` and make it public. (**DONE**)
  * Hide or remove `Collect`, `WhereBy`, `ZipWith` and `SelectWith`. (**DONE**)
  * Move `Repeat` to `Sequence`? Move `CollectAny` and `Sum` to
    Narvalo.Linq? They act on `IEnumerable<Monad<T>>` but, being in
    Narvalo.Applicative, they are hard to find.
  * Add monadic `Fold` w/ `resultSelector`.
  * Merge `Sequence.Gather` and `Sequence.Unfold`?
- More QEP for Monads? Simplify LINQ for `Maybe<T?>`?
- Auto-generate tests for null-guards (missing only for `Either`).
  Add more tests beyond the auto-generated ones. Add tests for purity?
- `Either`, should we throw if we have a lefty method for a righty object
  (see `WhenLeft` for instance).
- Add variants operators on the right for Either?
- Add `MapMany` to `Outcome` and `Fallible`....

Next:
- Add async and lazy alternatives?
- Implement trampoline.
- Custom `is` operators (it is in the proposal but it is not yet possible).
- Deconstruction for `Maybe<T?>`,
```csharp
public static void Deconstruct<T>(
    this Maybe<T?> @this,
    out bool isSome,
    out T value)
    where T : struct
{
    isSome = @this.IsSome;
    value = @this.IsSome ? @this.Value.Value : default(T);
}
```
  this is perfectly legal but will always be ignored in favor of the
  deconstructor method on `Maybe<T?>`.
- Add other monads - prototypes [here](https://github.com/chtoucas/Brouillons/tree/master/src/play/Functional/Monadic)
- `OnError()`, `WhenError()` & co could return a boolean to signal when they
  actually did something.
- More Haskell API, eg When, Forever & co?
- [Idioms](http://tomasp.net/blog/idioms-in-linq.aspx/)

Narvalo.Money
-------------

- Protect `Multiply`, `Divide` and `Remainder` against absurd results when rounding.
- `DecimalRounding.Scale()`, check for minimal value?

Next:
- Review the formatting stuff + enhance.
- Parsing.
- Add `DitheredRoundingAdjuster`.
- Bias allocation: First, Last, Lowest, Highest, Pseudorandom.
- Distribution: PseudoUniform, Single, Evenly, Spread.
- Inspirations:
  * JodaMoney
  * NodaMoney
  * [Money Type for the CLR](https://bitbucket.org/rplaire/money-type-for-the-clr)
  * [CSharpMoney](https://csharpmoney.codeplex.com/)
  * [NMoneys](https://github.com/dgg/nmoneys)

Narvalo.Mvp
-----------

- Review `ThrowIfNoPresenterBound`, `Load` event, `PresenterBinder.Release`.

Next:
- Could it be a .NET Standard library?
- Application Controller and Navigator.
- Review the use of custom presenter types per platform prevents the reuse of
  presenters across different platforms. Maybe is it a necessary evil?
- Add support for EventAggregator (not the same as cross-presenter communication).
- Incorporate ideas from MVCSharp (Task) and maybe GWT, Caliburn.Micro, ReactiveUI
  or MVVM Light?
  See [here](http://aspiringcraftsman.com/tag/model-view-presenter/)
  and [here](http://aspiringcraftsman.com/2007/08/25/interactive-application-architecture/)

Futures
=======

Notes for the other [repository](https://github.com/chtoucas/Brouillons).

- Use a common MSBuild configuration for .NET Standard and .NET Core.
- Use the `dotnet` command-line.
- Sync Autofac helpers & Release class w/ Quaderno.
- Do we really need GC.SuppressFinalize(this).
- Reboot Narvalo.Tap and Narvalo.Ghostscript?
- [CLDR](http://cldr.unicode.org/index/downloads)
  * [NCLDR](https://github.com/GuySmithFerrier/NCLDR)
  * [Onism](https://github.com/pgolebiowski/onism-cldr)
  * [cldrjs](https://github.com/rxaviers/cldrjs)
- INSEE COG

Play
----

- Finish monad rules.
- Add demo codes for all monads.
- Add Writer and Continuation monads.
- Give examples: parser combinator (Sprache), state-machine (Stateless).

Narvalo.Reliability
-------------------

References:
- [Polly](https://github.com/App-vNext/Polly)
- [kite](https://github.com/williewheeler/kite)
- jrugged
- [Hystrix](https://github.com/Netflix/Hystrix)

Narvalo.Mvp.Futures
-------------------

- Cross-presenter communication in WinForms is not functional.
  Things to work on before it might prove to be useful:
  * Right now, only controls contained in a MvpForm share the same presenter binder.
    We need something similar to what is done with ASP.NET (`PageHost`) but the situation
    is a bit more complicated due to the different execution model. Controls
    are fully loaded before we reach the `CreateControl` or `Load` events in the form
    container where we normally perform the binding.
  * The message coordinator must support unsubscription (automatic or manual).
