TODO
====

In Progress
-----------

- Rework MSBuild to use a common set of settings for all projects.
  Import common target at start of projects.
- Get rid off Narvalo.Junk.


In General
----------

### High Priority

- Use true argument check for extension methods.
- Review csproj, in particular, remove any ExcludeFromSyleCop.

### Medium Priority

- NuGet publication script (use Fake or PSake).
- Rework assembly versioning.
- Write assembly descriptions.
- Fix all FIXME, FIXME_PCL, TODO, REVIEW, XXX.
- Review all GetHashCode() methods.
- Re-enable Gendarme.

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

- Enforce, why can't I use ContractAbbreviator? The method get erased.
  The current workaround makes the API too different.
- Require.Condition, Require.RangeCondition. CCCheck fails on these.
- Check all use of AssumeNotNull.
- Remove Tracer?
- Validate(?) the usefulness of ExceptionFactory. Move to ThrowHelper?
- String.IsNullOrWhiteSpace?

### Medium Priority

- Check module: SuppressMessage
- Serializable and PCL.
- Review Monads, VoidOr... for nullity. I am pretty sure
  that VoirOr... and Output are broken.
- Add Contract.Ensures directives, first for Monads.
- Add a description to all Contract.Assume...
- Write a T4 Template for Monad tests.
- DocuMaker and Narvalo.Benchmarking are obviously broken.

### Low Priority

- Use Format instead of String.Format.
- Document compiler conditional symbols in used: NET_35.
- Add a XML schema for Narvalo config.
- DocuMaker & Playground could use the Narvalo rulesets.
- Use CommonMark instead of Markdown Deep.


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
- [Narvalo.Mvp.Windows.Forms] Cross-presenter communication is not functional.
  Thinks to work on before it might be useful:
  * Right now, only controls contained in a MvpForm share the same presenter binder.
    We need something similar to what is done with ASP.NET (PageHost) but the situation
    is a bit more complicated due to the different execution model. Controls
    are fully loaded before we reach the CreateControl or Load event in the form container
    where we normally perform the binding.

  * The message coordinator must support unsubscription (automatic or manual).

Narvalo (Miscs)
---------------

### High Priority

### Medium Priority

### Low Priority

