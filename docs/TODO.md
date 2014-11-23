TODO
====

In General
----------

### High Priority

- Use true argument check for extension methods.
- Review csproj, in particular, remove any ExcludeFromSyleCop.
- Rework MSBuild import in csproj: use pre and post imports.
  We then get access to $(ProjectName). We can also apply a common policy
  more easily.

### Medium Priority

- NuGet publication script (use Fake or PSake). Maybe create a new
  solution: Narvalo (NuGet).sln.
- Rework assembly versioning.
- Write assembly descriptions.
- Fix all FIXME, FIXME_PCL, TODO, REVIEW, XXX.
- Review all GetHashCode() methods.
- Check that we always use Error for CA/SA/CC.
- Re-enable Gendarme.
- ReferenceEquals or null-check

### Low Priority

- Fix regions, ////, /*!, Comments in english
- Remove linked files (IEquatable...)
- Always use resources for messages.
- Remove the ruleset for Samples.
- Review all ToString() methods.
- Review StyleCop settings.
- Review all SuppressMessage directives and tag them correctly.
- Review all GlobalSuppression files and tag them correctly.
- Replace all empty method with "// Intentionally left blank.".
- I see that Microsoft projects include the following lines in their
  assembly infos, what's the purpose?
  [assembly: SatelliteContractVersion("X.X.X.X")]
  [assembly: AssemblyMetadata("Serviceable", "True")]


Narvalo (Core)
--------------

### High Priority

- Code Analysis is not working with Code Contracts.

### Medium Priority

- Add Contract.Ensures directives, first for Monad.tt
- Check all use of AssumeNotNull.
- Check that Check do really make sense or better user Require.
- VoirOr... and Output seem broken.
- Add a description to all `Contract.Assume`.
- Write a T4 Template for Monad tests.
- Narvalo.Benchmarking is obviously broken.

### Low Priority

- Add a XML schema for Narvalo config.
- Make Narvalo.Facts a true VS test proj.

Narvalo (Mvp)
-------------

### High Priority

### Medium Priority

- Tests. We can start by porting the tests from WebFormsMvp.
- Documentation and userguide.
- Using custom presenter types per platform prevents the reuse
  of presenters across different platforms. Maybe is it a necessary evil...?

### Low Priority

- Add support for WPF, not yet started.
- Add support for Application Controller, Navigator, EventAggregator
  (not the same as cross-presenter communication).
- Incorporate ideas from MVCSharp (Task) and maybe GWT / Caliburn.Micro
  / ReactiveUI / MVVM Light?


Narvalo (Miscs)
---------------

### High Priority

### Medium Priority

### Low Priority


Narvalo (Playground)
--------------------

### High Priority

### Medium Priority

- Narvalo.DocuMaker & Playground could use the Narvalo rulesets.
- Use CommonMark instead of Markdown Deep.

### Low Priority
