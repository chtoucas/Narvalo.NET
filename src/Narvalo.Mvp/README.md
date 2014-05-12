
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

### TODO by order of priority

- Tests. We can start by porting the tests from WebFormsMvp.

- Add support for 
  * ASP.NET WebForms, cf. src/Narvalo.Mvp.Web, almost done.
  * CommandLine, cf. src/Narvalo.Mvp.CommandLine, already functional.
  * Windows Forms, cf. src/Narvalo.Mvp.Windows.Forms, already functional.
  * WPF.

- Add support for Autofac (Cf. src/Narvalo.Mvp.Extras) / Structuremap / Castle / Unity.

- Samples, cf. samples/Playground.WebForms.

- Incorporate ideas from MVCSharp and maybe GWT / Caliburn.Micro / ReactiveUI / MVVM Light?
  AppController, Navigator, Task

- Documentation and userguide.

