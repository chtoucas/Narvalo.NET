
A port of "Web Forms Mvp" usable outside Web context.

See _LICENSE-WebFormsMvp.txt_ for license information.

Changes from the original "Web Forms Mvp"
-----------------------------------------

- Removed all code related to System.Web: HttpContext, TraceContext...
  Support for ASP.NET or Windows Forms is available in separate packages.

- An API easier to follow (to me at least).

- No more generic constraint on the ViewModel; namely the class and new()
  constraints.

- Use ConcurrentDictionary instead of Dictionary for type caching.

- Several points of extensibility.

### Remarks

- Support for IOC (Autofac) is provided in src/Narvalo.Mvp.Extras. Since
  it is identical to what is done in WebFormsMvp, I do not intend to make it
  an official package. Just copy and adapt the code from WebFormsMvp. The same
  applies to StructureMap, Castle or Unity.

TODO (by order of priority)
---------------------------

- MessageCoordinator per binding operation for the Desktop.

- Tests. We can start by porting the tests from WebFormsMvp.

- Add support for
  * ASP.NET WebForms, cf. src/Narvalo.Mvp.Web, almost done.
  * CommandLine, cf. src/Narvalo.Mvp.CommandLine, already functional.
  * Windows Forms, cf. src/Narvalo.Mvp.Windows.Forms, already functional.
  * WPF, not yet started.

- Using custom presenter types per platform prevents the reuse
  of presenters across different platforms. But what about AsyncManager
  (this should be the easy one)	and HttpContext (for the navigation part like
  redirect, a solution might be in the Navigator (see below), but for the
  rest???). Maybe is it a necessary evil...?

- Add support for AppController, Navigator.

- Samples, cf. samples/Playground.WebForms.

- Incorporate ideas from MVCSharp (Task) and maybe GWT / Caliburn.Micro
  / ReactiveUI / MVVM Light?

- Documentation and userguide.

