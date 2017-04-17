---
uid: Narvalo.Mvp
---

The **Narvalo.Mvp** namespace contains classes and interfaces that support
the Model View Presenter (MVP).

Narvalo.Mvp is a port of [WebFormsMvp](https://github.com/webformsmvp/webformsmvp)
usable outside Web context.

See _LICENSE-WebFormsMvp.txt_ for license information.

Changes from WebFormsMvp:
- Removed codes related to System.Web: `HttpContext`, `TraceContext`...
- Support for ASP.NET made available in separate packages.
- An API easier to follow (at least to me).
- Use `ConcurrentDictionary` instead of `Dictionary `for type caching.
- Added basic (but usable) support for command-line application.
