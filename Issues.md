Issues
======
       
When fixed, issues not directly related to testable codes are described
in the ChangeLog file. 

Other closed issues are explicitely described and tested in one of the test 
libraries. NB: To spot them, you should look for the `Issue` attribute.

All issues are either Bug, Enhancement, Improvement or Review
and get a priority, High, Medium or Low.


Work In Progress
----------------

- Improvement (Medium): Move DummyGeneratedVersion to Make.CustomAfter.props. 
- Improvement (Medium): Move AssemblyInfo.Common.cs to the Common directory? 
- Improvement (Medium): Move IssueAttribute.cs and IssueSeverity.cs to the
  Common directory and link them automatically to test projects.
- Improvement (Medium): Test and sample projects should not raise a warning 
  when no version is specified.
- Improvement (Low): Write better documentation.


Build & Documentation
---------------------
           
### High Priority   
- Enhancement: Split Make.proj in sub-projects. Create a Retail.proj project
  and remove the SkipPrivateProjects option.
- Review: All project files. In particular, remove any ExcludeFromSyleCop directive
  and move custom properties to {AssemblyName}.props.

### Medium Priority
- Enhancement: PSake CI target should build and test all possible configurations, 
  currently Debug|Release, AnyCPU, with or without visible internals.
- Enhancement: Script to push packages to NuGet.
- Enhancement: Non retail packages should be copied to MyGet.
- Enhancement: Static analysis with Gendarme.
- Enhancement: Create symbol packages (or use GitLink?).
- Enhancement: Automatic analysis of reports from performed analysis.
- Improvement: Finish SecAnnotate. Requires to fully understand the security 
  model of .NET (CAS, APTCA). See:
  * [CAS](http://msdn.microsoft.com/en-us/library/c5tk9z76%28v=vs.110%29.aspx)
  * [APTCA](http://msdn.microsoft.com/fr-fr/magazine/ee336023.aspx)
  * [SecAnnotate](http://blogs.msdn.com/b/shawnfa/archive/2009/11/18/using-secannotate-to-analyze-your-assemblies-for-transparency-violations-an-example.aspx)
- Improvement: Fix all CA and SA warnings and errors.
- Improvement: Retire the CA ruleset for Samples.
- Review: StyleCop settings. StyleCop cache & ability to change settings used
  by the VS extension.
- Review: Make sure a build fails when SecAnnotate fails.
- Review: Available platforms, 64bit?

### Low Priority
- Enhancement: Zip artefacts when done building.
- Enhancement: Build documentation.
- Enhancement: Create a website for the project.
- Enhancement: Add Code Coverage + Report Generator.
- Enhancement: Add Git commit hash to assembly configuration or info?
- Improvement: Document compiler conditional symbols in used: NET_35.
- Improvement: Make unnecessary to add StyleCop settings to each project.
- Improvement: T4 re-generation has been disabled since it requires VS hosting.
  Is there a way we can enable T4 re-generation during a build?
- Improvement: Write assembly and NuGet descriptions.
- Improvement: Some projects don't have an {AssemblyName}.Version.props.
- Review: %comspec% /k (@pause).


Code Quality
------------

### High Priority                                  
- Enhancement: Better organization for black box tests.         
- Improvement: Remove linked files (IEquatable & co)      
- Improvement: Use true argument check for extension methods.
- Review: Check all SuppressMessage (GlobalSuppression too) and tag them
  with [REVIEW], [GeneratedCode]... in particular those added to fix warnings
  when internals are hidden.

### Medium Priority
- Bug: Some assemblies raise a CA warning on resources supposably not used.
- Bug: We used [module: SuppressMessage(...)] to suppress some CA or SA warnings
  but this does not work as expected when used in an assembly info file:
  it suppresses the warning for the whole assembly. We should further 
  review all cases where we do such a thing (T4 files).
- Enhancement: Explore Pex.
- Enhancement: Explore SpecFlow.
- Improvement: Always use resources for messages.
- Improvement: Document all classes & methods.
- Improvement: Fix all FIXME, FIXME_PCL, TODO, REVIEW, XXX.
- Improvement: Fix regions, ////, /*!.
- Imporvement: Replace all french comments in english.
- Imporvement: Replace all empty method with "// Intentionally left blank.".
- Review: All GetHashCode() methods.
- Review: All ToString() methods.

### Low Priority


Narvalo (Core)
--------------

### High Priority         
- Improvement: Move Narvalo.Benchmarking to the solution Miscs. 
- Improvement: Cleanup then get rid off Narvalo.Junk.
- Review: `Maybe<t>`, ensures that if the underlying value is null-ed, things 
  continue to work as expected (I really don't think so).                         

### Medium Priority
- Enhancement: Write a T4 template for Monad tests.   
- Improvement: Add a description to all `Contract.Assume`.
- Improvement: Add `Contract.Ensures` directives, first for Monads.  
- Review: Check Monads, VoidOr... for nullity. I am pretty sure
  that VoirOr... and Output are broken.         
- Review: Check all use of `AssumeNotNull`.
- Review: `Enforce`, why can't I use `ContractAbbreviator`? The method get erased.
  The current workaround makes the API too different.
- Review: `Require.Condition`, `Require.RangeCondition`. CCCheck fails on them.
           
### Low Priority
- Enhancement: Add a XML schema for Narvalo config.    
- Improvement: Use `Format` instead of `String.Format`.
- Improvement: Remove `Tracer`?  
- Review: `Serializable` and PCL.
- Review: `String.IsNullOrWhiteSpace` vs `String.IsNullOrEmpty`.    
- Review: Validate the usefulness of `ExceptionFactory`. Replace it with 
  something like a `ThrowHelper`?

          
Narvalo (Mvp)
-------------

### Medium Priority
- Enhancement: Tests. We can start by porting those from WebFormsMvp.
- Enhancement: Documentation and userguide.
- Review: Using custom presenter types per platform prevents the reuse
  of presenters across different platforms. Maybe is it a necessary evil...?

### Low Priority
- Enhancement: Add support for WPF.
- Enhancement: Add support for Application Controller, Navigator, EventAggregator
  (not the same as cross-presenter communication).
- Improvement: [Narvalo.Mvp.Windows.Forms] Cross-presenter communication 
  is not functional. Thinks to work on before it might proved to be useful:
  * Right now, only controls contained in a MvpForm share the same presenter binder.
    We need something similar to what is done with ASP.NET (PageHost) but the situation
    is a bit more complicated due to the different execution model. Controls
    are fully loaded before we reach the CreateControl or Load event in the form
    container where we normally perform the binding.          
  * The message coordinator must support unsubscription (automatic or manual).
- Review: Incorporate ideas from MVCSharp (Task) and maybe GWT, Caliburn.Micro,
  ReactiveUI or MVVM Light?


Narvalo (Mics)
--------------

### Medium Priority     
- Bug: DocuMaker is broken.
- Bug: Narvalo.Benchmarking is broken. 
- Enhancement: DocuMaker should use CommonMark instead of Markdown Deep.
- Enhancement: ILMerging DocuMaker.

### Low Priority
- Enhancement: Create Narvalo.FxCop.
- Improvement: Complete Narvalo.StyleCop.CSharp.

