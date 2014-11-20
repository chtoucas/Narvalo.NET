TODO
====

En vrac :
- OK Core & Playground
- Narvalo.DocuMaker & Benchmarks should use the Narvalo rulesets.
- Remove the ruleset for Samples.
- Fix regions, ////, /*!, Comments in english
- GetHashCode
- Review csproj and make Narvalo.Facts a true test proj. Check ExcludeFrom
- Create Narvalo (NuGet).sln
- Rework versioning & complete AssemblyDescription.
- Global suppression files.
- NuGet pub script (in Fake or PowerShell)
- Fix all TODO, FIXME, FIXME_PCL, XXX.
- Use Error for ruleset?
- Migrate from psake to Fake.
- Review all SuppressMessage directives and GlobalSuppression files.
- Narvalo.Facts fails from Narvalo.proj when run twice.
- Replace all empty method with "// Intentionally left blank.".
- Review StyleCop settings.
- Re-enable Gendarme.
- Re-enable a few code contracts.
- Remove Markdown Deep.
- Add a XML schema for Narvalo config.
- Review the use of Debug, DebugCheck and Require.
- Use resources for messages?
- AssemblyInfo:
  [assembly: SatelliteContractVersion("X.X.X.X")] ?
  [assembly: AssemblyMetadata("Serviceable", "True")] ?


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
