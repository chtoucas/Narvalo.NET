
A port of "Web Forms Mvp" usable outside Web context. 

See _LICENSE-WebFormsMvp.txt_ for license information.

Changes from the original "Web Forms Mvp"
-----------------------------------------

- Removed all code related to System.Web: HttpContext, TraceContext...
  (support for ASP.NET or Windows Forms is available in separate packages)
- Removed cross-presenter communication from the core.
- An API easier to follow (to me at least)
- No more generic constraint on the ViewModel; namely the class and new() constraints
- Use ConcurrentDictionary instead of Dictionary for type caching
- Several possibilities of customization via MvpConfiguration

### TODO

- Documentation and userguide.

- Incorporate ideas from MVCSharp (Task, Navigator) and maybe Caliburn.Micro / ReactiveUI / MVVM Light?

