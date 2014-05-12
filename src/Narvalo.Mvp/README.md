
A port of "Web Forms Mvp" usable outside Web context. 

See _LICENSE-WebFormsMvp.txt_ for license information.

Changes from the original "Web Forms Mvp"
-----------------------------------------

- Removed all code related to System.Web: HttpContext, TraceContext...
  (support for ASP.NET or Windows Forms is available in separate packages).
- Removed cross-presenter communication from the core.
- An API easier to follow (to me at least).
- No more generic constraint on the ViewModel; namely the class and new() constraints.
- Use ConcurrentDictionary instead of Dictionary for type caching.
- Several possibilities of customization via Configuration/MvpBootstrapper.

### Remarks

- Cross-presenter communication is provided on a per platform basis.

- Support for IOC (Autofac) is provided in src/Narvalo.Mvp.Extras. Since
  it is identical to what is done in WebFormsMvp, I do not intend to make it 
  an official package. Just copy and adapt the code from WebFormsMvp. The same
  applies to StructureMap, Castle or Unity.

TODO (by order of priority)
---------------------------

- Tests. We can start by porting the tests from WebFormsMvp.

- Add support for 
  * ASP.NET WebForms, cf. src/Narvalo.Mvp.Web, almost done.
  * CommandLine, cf. src/Narvalo.Mvp.CommandLine, already functional.
  * Windows Forms, cf. src/Narvalo.Mvp.Windows.Forms, already functional.
  * WPF, not yet started.

- Samples, cf. samples/Playground.WebForms.

- Incorporate ideas from MVCSharp and maybe GWT / Caliburn.Micro / ReactiveUI / MVVM Light?
  AppController, Navigator, Task

- Documentation and userguide.

