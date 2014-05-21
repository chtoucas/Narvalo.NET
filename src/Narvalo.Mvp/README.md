
A port of "Web Forms Mvp" usable outside Web context.

See _LICENSE-WebFormsMvp.txt_ for license information.

Changes from the original "Web Forms Mvp"
-----------------------------------------

- Removed all code related to System.Web: HttpContext, TraceContext...
  Support for ASP.NET or Windows Forms is available in separate packages.

- An API easier to follow (to me at least).

- Use ConcurrentDictionary instead of Dictionary for type caching.

### Remarks

- Sample support for IOC (at least Autofac) is provided in src/Narvalo.Mvp.Extras. 
  Since it is identical to what is done in WebFormsMvp, I do not intend to make
  it an official package. Just copy and adapt the code from WebFormsMvp.
  The same applies to StructureMap, Castle or Unity.

TODO (by order of priority)
---------------------------

- Tests. We can start by porting the tests from WebFormsMvp.

- Add support for
  * CommandLine, cf. src/Narvalo.Mvp.CommandLine, basic but functional.
  * Windows Forms, cf. src/Narvalo.Mvp.Windows.Forms, not ready.
  * WPF, not yet started.

- REVIEW: Using custom presenter types per platform prevents the reuse
  of presenters across different platforms. Maybe it is a necessary evil...?

- Add support for Appication Controller, Navigator, EventAggregator (not the same
  as cross-presenter communication).

- Incorporate ideas from MVCSharp (Task) and maybe GWT / Caliburn.Micro
  / ReactiveUI / MVVM Light?

- Documentation and userguide.

