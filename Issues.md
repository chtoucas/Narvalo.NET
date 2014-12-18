Issues
======
       
When fixed, issues not directly related to testable codes are described
in the ChangeLog file. 

Other closed issues are explicitely described and tested in one of the test 
libraries. NB: To spot them, you should look for the `Issue` attribute.

All issues are either Bug, Enhancement or Improvement.

When an issue is closed, move it to the ChangeLog. Further more, if it is a bug,
add the necessary tests to be sure it does not pass through again.


Work in progress
----------------

**Objectives: Small improvements to SA, CA and the MSBuild infrastructure**

Code Analysis and Source Analysis:               
- Improvement: Check all SuppressMessage (GlobalSuppression too) and tag them
  with [REVIEW], [GeneratedCode]... in particular those added to fix warnings
  when internals are hidden. Maybe create new SuppressMessage attributes.
- Improvement: Fix any remaining CA and SA warnings and errors. 
- Bug: Some assemblies raise a CA warning on resources supposably not used.
- Bug: We use [module: SuppressMessage(...)] to suppress some CA or SA warnings
  but this does not work as expected when used in an assembly info file:
  it suppresses the warning for the whole assembly. We should further 
  review all cases where we do such a thing (T4 files).                
- Improvement: Make unnecessary to add StyleCop settings to each project.
  Review StyleCop settings, StyleCop cache & ability to change settings used. 
  Review all project files for ExcludeFromSyleCop directives.   
- Improvement: Retire the CA ruleset for Samples.

MSBuild files: 
- Bug: Test and sample projects should not raise a warning when no version is specified.                           
- Improvement: Move DummyGeneratedVersion to Make.CustomAfter.props.    
- Improvement: Move custom properties to {AssemblyName}.props.  
- Improvement: Some projects don't have an {AssemblyName}.Version.props.      
- Enhancement: Split Make.proj in sub-projects. Create a Retail.proj project
  and remove the SkipPrivateProjects option.         
- Enhancement: Copy non retail packages to the local NuGet server. 
                       
Miscs:
- Improvement: Better organization for black box tests. 
- Improvement: Move IssueAttribute.cs and IssueSeverity.cs to the
  Common directory and link them automatically to test projects.  
  Also move AssemblyInfo.Common.cs to the Common directory? 
- Improvement: Complete Guidelines. 
  * Fully document any requirement. 
    [Visual Studio Downloads](http://www.visualstudio.com/downloads/download-visual-studio-vs)    
  * Document compiler conditional symbols in used: NET_35.  
- Improvement: Write assembly and NuGet descriptions.  


Not yet planified
-----------------

### More automation and further improvements to the overall code quality.

Build Automation:     
- Enhancement: PSake CI target should build and test all possible configurations, 
  currently Debug|Release, AnyCPU, with or without visible internals.
- Enhancement: Script to push packages to NuGet with extensive checks. For instance,
  we should not try to publish a version already available on NuGet, check 
  the dependencies tree.  
- Enhancement: Add Git commit hash to assembly configuration or info?
- Enhancement: Create symbol packages (or use GitLink?).
- Review: %comspec% /k (@pause).

Code Quality:  
- Enhancement: Static analysis with Gendarme.    
- Improvement: Fix all FIXME, FIXME_PCL, TODO, REVIEW, XXX.

Miscs:
- Improvement: Remove linked files (IEquatable & co).   
- Improvement: Move Narvalo.Benchmarking to the solution Miscs. 
- Enhancement: Use Git tags.

### Code Review.

Bug corrections:
- Bug: `Maybe<t>`, ensures that if the underlying value is null-ed, things 
  continue to work as expected (I really don't think so).           
- Bug: Check Monads, VoidOr... for nullity. I am pretty sure
  that VoirOr... and Output are broken.         
- Bug: `Enforce`, why can't I use `ContractAbbreviator`? The method get erased.
  The current workaround makes the API too different.
- Bug: `Require.Condition`, `Require.RangeCondition`. CCCheck fails on them. 

Code improvements:
- Improvement: Add a description to all `Contract.Assume`.
- Improvement: Add `Contract.Ensures` directives, first for Monads. 
- Improvement: Check any use of `AssumeNotNull`.    
- Improvement: Use `Format` instead of `String.Format`.
- Improvement: Remove `Tracer`?  
- Improvement: `Serializable` and PCL.
- Improvement: `String.IsNullOrWhiteSpace` vs `String.IsNullOrEmpty`.    
- Improvement: Validate the usefulness of `ExceptionFactory`. Replace it with 
  something like a `ThrowHelper`?   
- Improvement: Cleanup then get rid of Narvalo.Junk.    
- Improvement: Review all GetHashCode() methods.
- Improvement: Review all ToString() methods.                   
              
Style improvements:                
- Improvement: Use true argument check for extension methods.      
- Improvement: Always use resources for messages.     
- Improvement: Fix regions, ////, /*!.
- Improvement: Replace all french comments in english.
- Improvement: Replace all empty method with "// Intentionally left blank.".

### Better tested and documented Narvalo.Core assembly.

- Improvement: Add more and more code contracts.
- Improvement: Large code coverage of Narvalo.Core.
- Enhancement: Write a T4 template for Monad tests.   
- Improvement: 100% SA documentation rules.
     
### Better tested and documented Narvalo.Common assembly.

- Improvement: Add more and more code contracts.
- Improvement: Large code coverage of Narvalo.Common.
- Improvement: 100% SA documentation rules.

### Better tested and documented Narvalo.Web assembly.

- Improvement: Add more and more code contracts.
- Improvement: Large code coverage of Narvalo.Web.
- Improvement: 100% SA documentation rules.
- Enhancement: Add an XML schema for the Narvalo config.    
- Enhancement: Finish the Optimization & Semantic classes. 

At this point we should have a first stable release for the core Narvalo assemblies.

### Documentation & Literate Programming.
                    
- Bug: DocuMaker is broken.
- Enhancement: Build C# documentation.
- Enhancement: Use CommonMark instead of Markdown Deep.
- Enhancement: Use Roslyn.
- Enhancement: ILMerge DocuMaker.           
- Enhancement: Create a website for the project.
- Improvement: Move namespace docs to NamespaceDocs files.
                      
At this point we should have a first useful release for the core assemblies.

### Strengthen core assemblies.
                                               
- Enhancement: Check reports from performed analysis.   
- Enhancement: Add Code Coverage + Report Generator.     
- Enhancement: Explore Pex.
- Enhancement: Explore SpecFlow.  

### Secure core assemblies.     

- Improvement: Finish SecAnnotate. Requires to fully understand 
  the security model of .NET (CAS, APTCA).
- Improvement: Make sure a build fails when SecAnnotate does too. 
- Enhancement: Implements security attributes:
  Sample attributes:
```     
[assembly: SecurityCritical]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: SecurityRules(SecurityRuleSet.Level1, SkipVerificationInFullTrust = true)]
[assembly: AllowPartiallyTrustedCallers(PartialTrustVisibilityLevel = PartialTrustVisibilityLevel.NotVisibleByDefault)]
```
  by the VS extension. See:
  * [CAS](http://msdn.microsoft.com/en-us/library/c5tk9z76%28v=vs.110%29.aspx)
  * [APTCA](http://msdn.microsoft.com/fr-fr/magazine/ee336023.aspx)
  * [SecAnnotate](http://blogs.msdn.com/b/shawnfa/archive/2009/11/18/using-secannotate-to-analyze-your-assemblies-for-transparency-violations-an-example.aspx)

### Optimize core assemblies.
                  
- Bug: Narvalo.Benchmarking is broken. 

### Narvalo.Mvp (en vrac)
                            
- Enhancement: Tests. We can start by porting those from WebFormsMvp.
- Enhancement: Documentation and userguide.
- Improvement: Using custom presenter types per platform prevents 
  the reuse of presenters across different platforms. Maybe is it a necessary evil?  
- Enhancement: Add support for WPF.
- Enhancement: Add support for Application Controller, Navigator, 
  EventAggregator (not the same as cross-presenter communication).    
- Enhancement: Incorporate ideas from MVCSharp (Task) and maybe 
  GWT, Caliburn.Micro, ReactiveUI or MVVM Light?
- Improvement: For Narvalo.Mvp.Windows.Forms, cross-presenter 
  communication is not functional. Thinks to work on before it might proved 
  to be useful:
  * Right now, only controls contained in a MvpForm share the same presenter binder.
    We need something similar to what is done with ASP.NET (PageHost) but the situation
    is a bit more complicated due to the different execution model. Controls
    are fully loaded before we reach the CreateControl or Load event in the form
    container where we normally perform the binding.          
  * The message coordinator must support unsubscription (automatic or manual).


Not yet classified
------------------
        
- Enhancement: More supported platforms, 64bit?    
- Enhancement: Create Narvalo.FxCop.
- Improvement: Complete Narvalo.StyleCop.CSharp.                  
- Enhancement: Zip artefacts when done building.
- Improvement: T4 re-generation has been disabled since it requires
  VS hosting. Is there a way we can enable T4 re-generation during a build?  
