Issues (en vrac)
================

Miscs
-----

- Integrate publish-*.fsx into make.ps1.
- Reorganize NuGet projects (remove the useless subdirectories).
- For EDGE packages, increment the patch by 1 on the fly.
  There is a problem w/ dependcy resolution for EDGE packages on myget.org.
- Finish localization in french and english.

Narvalo.Fx
----------

- Simplify LINQ for Maybe<T?>.

Next:
- Add `SelectMany` to `Outcome` and `Fallible`.
- Deprecate some of the methods `ValueOr...`
- `OnError()`, `WhenError()` & co could return a boolean to signal if it did anything.
- Explain what do we mean by shadowing.
- More Haskell API, eg When, Forever & co?
- Add async and lazy alternatives?
- Add Continuation and IO monads, doubtful?
- Complete T4 generation of tests for all monads.
- [Idioms](http://tomasp.net/blog/idioms-in-linq.aspx/)

Narvalo.Money
-------------

- Protect `Multiply`, `Divide` and `Remainder` against absurd results when rounding.
- `DecimalRounding.Scale()`, check for minimal value?

Next:
- Implements `DitheredRoundingAdjuster`.
- Bias allocation: First, Last, Lowest, Highest, Pseudorandom.
- Distribution: PseudoUniform, Single, Evenly, Spread.
- Inspirations:
  * JodaMoney
  * NodaMoney
  * [Money Type for the CLR](https://bitbucket.org/rplaire/money-type-for-the-clr)
  * [CSharpMoney](https://csharpmoney.codeplex.com/)
  * [NMoneys](https://github.com/dgg/nmoneys)

Narvalo.Finance
---------------

- BBAN validation.

Next:
- BBAN and IIBAN implementations:
  * [PHP-IBAN](https://github.com/globalcitizen/php-iban)
  * [BBAN](https://github.com/globalcitizen/php-iban/issues/39)
  * [python-stdnum](https://github.com/arthurdejong/python-stdnum)
  * [France](http://marlot.org/util/calcul-de-la-cle-nir.php)

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
