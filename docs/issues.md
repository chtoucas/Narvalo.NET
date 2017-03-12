Issues (en vrac)
================

Infrastructure
--------------

- Incremental build is broken when building w/ make.ps1
  <- certainly due to the generation of an AssemblyInfo file on the fly.
- Upgrade the MyGet server (requires .NET 4.6 to be installed).

Next:
- We can certainly simplify the MSBuild scripts a bit.
- Discuss `StructLayout.Auto`.

Narvalo.Fx
----------

- Find better names for `Stateful` methods and decide what to do with it:
  should it be part of Narvalo.Fx or Edufun.

Next:
- Add `SelectMany` to `Outcome` and `Fallible`.
- Explain what do we mean by shadowing.
- More Haskell API, eg When, Forever & co?
- Add async and lazy alternatives?
- Add Continuation and IO monads, at least in Edufun?
- Complete T4 generation of tests for all monads.
- [Idioms](http://tomasp.net/blog/idioms-in-linq.aspx/)

Narvalo.Finance
---------------

- Protect `Multiply`, `Divide` and `Remainder` against absurd results when rounding.
- `DecimalRounding.Scale()`, check for minimal value?

Next:
- Inspirations:
  * JodaMoney & NodaMoney
  * [Money Type for the CLR](https://bitbucket.org/rplaire/money-type-for-the-clr)
  * [CSharpMoney](https://csharpmoney.codeplex.com/)
  * [NMoneys](https://github.com/dgg/nmoneys)
  * [France](http://marlot.org/util/calcul-de-la-cle-nir.php)
- BBAN and IIBAN implementations:
  * [PHP-IBAN](https://github.com/globalcitizen/php-iban)
  * [BBAN](https://github.com/globalcitizen/php-iban/issues/39)
  * [python-stdnum](https://github.com/arthurdejong/python-stdnum)

Narvalo.Mvp
-----------

- Review `ThrowIfNoPresenterBound`, `Load` event, `PresenterBinder.Release`.

Next:
- Add localized messages?
- Application Controller and Navigator.
- Review the use of custom presenter types per platform prevents the reuse of
  presenters across different platforms. Maybe is it a necessary evil?
- Add support for EventAggregator (not the same as cross-presenter communication).
- Incorporate ideas from MVCSharp (Task) and maybe GWT, Caliburn.Micro, ReactiveUI
  or MVVM Light?
  See [here](http://aspiringcraftsman.com/tag/model-view-presenter/)
  and [here](http://aspiringcraftsman.com/2007/08/25/interactive-application-architecture/)

Future plans (?)
----------------

- [CLDR](http://cldr.unicode.org/index/downloads)
  * [NCLDR](https://github.com/GuySmithFerrier/NCLDR)
  * [Onism](https://github.com/pgolebiowski/onism-cldr)
  * [cldrjs](https://github.com/rxaviers/cldrjs)
- INSEE COG
