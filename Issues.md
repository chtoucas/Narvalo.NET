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
- Improvement: Wrap all SuppressMessage that are not really justified either
  by a `!NO_GLOBAL_SUPPRESSIONS` or by a `!NO_HACK`. Tag with [GeneratedCode] those
  related to generated code...
- Improvement: Remove the local CA & SA overrides. Fix any remaining CA and 
  SA warnings and errors. 
- Bug: Some assemblies raise a CA warning on string resources supposably not used.
- Bug: We use [module: SuppressMessage(...)] to suppress some CA or SA warnings
  but this does not work as expected when used in an assembly info file:
  it suppresses the warning for the whole assembly. We should further 
  review all cases where we do such a thing (T4 files).                
- Improvement: Make unnecessary to add StyleCop settings to each project.
  Review StyleCop settings, StyleCop cache & ability to change settings used. 
  Review all project files for ExcludeFromSyleCop directives.   

MSBuild files:             
- Enhancement: Publish non retail packages to a remove MyGet server and
  Publish retail packages to NuGet server.
- Bug: Whatever we use for CodeAnalysisSucceededFile, it does not seem to be 
  understood by MSBuild. Setting CodeAnalysisLogFile or CodeAnalysisSucceededFile
  disables incremental building. Code Analysis Hooks. 
  For _WarnOnTemporaryOverridenSettings, RunCodeAnalysis is not available in VS.   
- Bug: FullClean target from PSakefile.ps1 fails sometimes for obscure reasons.
                       
Miscs:
- Bugfix: CC & Format.CurrentCulture. Narvalo.Int64Encoder.FromFlickrBase58String.
- Improvement: Complete Guidelines. 
  * Explain NuGet package versioning (retail or not). More details on the effect
    of using Retail=true.
  * Fully document any requirement. See:
    [Visual Studio Downloads](http://www.visualstudio.com/downloads/download-visual-studio-vs)    
  * Document compiler conditional symbols in use.  
- Improvement: Review and fill assembly and NuGet spec.  


Not yet planned
---------------

### More automation and further improvements to the overall code quality.

Build Automation:     
- Enhancement: PSake CI target should build and test all possible configurations, 
  currently Debug|Release, AnyCPU, with or without visible internals.
- Enhancement: Script to push packages to NuGet with extensive checks. For instance,
  we should not try to publish a version already available on NuGet, check 
  the dependencies tree.            
- Enhancement: Enable T4-regeneration in VS since, for us, it does not work
  when building from the command-line.
- Enhancement: Add Git commit hash to assembly configuration or info?
- Enhancement: Start to use Git tags.
- Enhancement: Create symbol packages (or use GitLink?).
- Enhancement: %comspec% /k (@pause).
- Improvement: Complete Narvalo.Build with Gendarme and Versioning tasks.   
- Review: What's going on when Package target is also defined.

Code Quality:  
- Enhancement: Static analysis with Gendarme. 
- Improvement: Fix all FIXME, FIXME_PCL, TODO, REVIEW, XXX.
- Improvement: Remove linked files (IEquatable & co).   

### Code Cleanup.

Bug corrections:
- Bug: `Maybe<t>`, ensures that if the underlying value is null-ed, things 
  continue to work as expected (I really don't think so).           
- Bug: Check Monads, VoidOr... for nullity. I am pretty sure
  that VoirOr... and Output are broken.         
- Bug: `Enforce`, why can't I use `ContractAbbreviator`? The method get erased.
  The current workaround makes the API too different.
- Bug: `Require.Condition`, `Require.RangeCondition`. CCCheck fails on them. 

Code improvements:
- Improvement: Remove all hidden files.
- Improvement: Review the core Code Contracts classes.
- Improvement: Review Narvalo.Minimal.cs.
- Improvement: Remove `Tracer`?  
- Improvement: Use true argument check for extension methods.      
- Improvement: Add a description to all `Contract.Assume`.
- Improvement: Add `Contract.Ensures` directives, first for Monads. 
- Improvement: Check any use of `AssumeNotNull`.    
- Improvement: Use `Format` instead of `String.Format`.
- Improvement: `Serializable` and PCL.
- Improvement: `String.IsNullOrWhiteSpace` vs `String.IsNullOrEmpty`.    
- Improvement: Validate the usefulness of `ExceptionFactory`. Replace it with 
  something like a `ThrowHelper`?   
- Improvement: Cleanup then get rid of Narvalo.Junk.    
- Improvement: Review all GetHashCode() methods.
- Improvement: Review all ToString() methods.                   
              
Style improvements:                
- Improvement: Always use resources for messages.     
- Improvement: Fix regions, ////, /*!.
- Improvement: Rewrite all french comments in english.
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
                    
- Bug: Prose is simply broken.
- Enhancement: Build C# documentation.
- Enhancement: Use CommonMark instead of Markdown Deep. 
  NB: PostSharp, Roslyn, MEF, Serilog, Autofac.
- Enhancement: ILMerge Prose.           
- Enhancement: Create a website for the project.
- Improvement: Move namespace docs to NamespaceDocs files.
                      
At this point we should have a first useful release for the core assemblies.

### Strengthen core assemblies.
                                               
- Enhancement: Check reports from performed analysis.   
- Enhancement: Add Code Coverage + Report Generator.     
- Enhancement: Explore Pex.
- Enhancement: Explore SpecFlow.  

### Secure core assemblies.     

- Bug: The SecAnnotate output file does not seem to be created.  
- Improvement: Finish SecAnnotate. Requires to fully understand 
  the security model of .NET (CAS, APTCA).
- Improvement: Make sure a build fails when SecAnnotate does too. 
- Enhancement: Implements security attributes, for instance:
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
- Improvement: Move Narvalo.Benchmarking to the solution Miscs. 

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
        
- Enhancement: More supported platforms (64bit) and target frameworks.    
- Enhancement: Create Narvalo.FxCop.
- Improvement: Complete Narvalo.StyleCop.CSharp.                  
- Enhancement: Zip artefacts when done building.
- Improvement: T4 re-generation has been disabled since it requires
  VS hosting. Is there a way we can enable T4 re-generation during a build?  
