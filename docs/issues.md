Issues (en vrac)
================

Tasks
-----

- Automatic code formatting with CodeFormatter.
- Finish automation of docfx.

Narvalo.Fx
----------

- Async versions? Lazy versions?
- [Idioms](http://tomasp.net/blog/idioms-in-linq.aspx/)

Narvalo.Finance
---------------

- Protect Multiply, Divide and Remainder against absurd results when rounding.
- DecimalRounding.Scale(), check for minimal value?
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

- Add localized messages in french for Narvalo.Mvp and Narvalo.Mvp.Web.
- Application Controller and Navigator.
- Review `ThrowIfNoPresenterBound`, `Load` event, `PresenterBinder.Release`.
- Review the use of custom presenter types per platform prevents the reuse of
  presenters across different platforms. Maybe is it a necessary evil?
- Add support for EventAggregator (not the same as cross-presenter communication).
- Incorporate ideas from MVCSharp (Task) and maybe GWT, Caliburn.Micro, ReactiveUI
  or MVVM Light?
  See [here](http://aspiringcraftsman.com/tag/model-view-presenter/)
  and [here](http://aspiringcraftsman.com/2007/08/25/interactive-application-architecture/)

Narvalo.Globalization
---------------------

- [CLDR](http://cldr.unicode.org/index/downloads)
  * [NCLDR](https://github.com/GuySmithFerrier/NCLDR)
  * [Onism](https://github.com/pgolebiowski/onism-cldr)
  * [cldrjs](https://github.com/rxaviers/cldrjs)
- INSEE
