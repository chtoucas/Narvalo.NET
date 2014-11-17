Narvalo.Mvp Namespace
=====================

Narvalo.Mvp
-----------

Narvalo.Mvp is a port of "Web Forms Mvp" usable outside Web context.

See _LICENSE-WebFormsMvp.txt_ for license information.

### Changes from the original "Web Forms Mvp"

- Removed codes related to System.Web: `HttpContext`, `TraceContext`...
- Support for ASP.NET made available in separate packages.
- An API easier to follow (at least to me).
- Use `ConcurrentDictionary` instead of `Dictionary `for type caching.
- Added basic (but usable) support for CommandLine application.


Narvalo.Mvp.Web
---------------

Enhances Narvalo.Mvp to provide support for ASP.NET WebForms.
Similarity with webformsmvp is no accident, lot of ideas are rooted in
webformsmvp codebase.

### Changes from the original "Web Forms Mvp"

Since I don't see any value in `MvpHttpHandler` and `MvpWebService`,
I dropped both.


Narvalo.Mvp.Windows.Forms
-------------------------

Enhances Narvalo.Mvp to provide support for Windows Forms (not ready).


Narvalo.Mvp.Extras
------------------

### Inversion of Control

Narvalo.Mvp.Extras provides sample support for IOC (at least Autofac).
Since it is identical to what is done in WebFormsMvp, I do not intend to make
it an official package. Just copy and adapt the code from WebFormsMvp.
The same applies to StructureMap, Castle or Unity.
