TODO
====

- Cleanup VS Projects (remove stylecop).
- Create Narvalo (All).sln (refactoring, NugGet updates) and Narvalo (NuGet).sln
- NuGet pub script (in Fake or PowerShell)
- Fix all TODO, FIXME, FIXME_PCL, XXX.
- Migrate from psake to Fake.
- Review all SuppressMessage directives.
- Enable StyleCop for test libraries.
- Replace all empty method with "// Intentionally left blank.".
- Review StyleCop settings.
- Re-enable Gendarme.
- Remove Markdown Deep


Narvalo.Mvp
-----------

By order of priority:
- Tests. We can start by porting the tests from WebFormsMvp.
- Add support for WPF, not yet started.
- REVIEW: Using custom presenter types per platform prevents the reuse
  of presenters across different platforms. Maybe is it a necessary evil...?
- Add support for Application Controller, Navigator, EventAggregator
  (not the same as cross-presenter communication).
- Incorporate ideas from MVCSharp (Task) and maybe GWT / Caliburn.Micro
  / ReactiveUI / MVVM Light?
- Documentation and userguide.
