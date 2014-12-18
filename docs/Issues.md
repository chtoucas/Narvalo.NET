Issues
======
       
When fixed, issues not directly related to testable codes are described
in the ChangeLog file. 

Other closed issues are explicitely described and tested in one of the test 
libraries. NB: To spot them, you should look for the `Issue` attribute.

Work In Progress
----------------

- Improvement (Medium): Move DummyGeneratedVersion to Make.CustomAfter.props.   
- Improvement (Medium): Test and sample projects should not raise a warning 
  when no version is specified.

Generic Issues
--------------
           
### High Priority   
- Enhancement: Create a Retail.proj project and remove the SkipPrivateProjects option.
- Enhancement: Better organization for black box tests.
- Improvement: Review all GlobalSuppression's and tag them [REVIEW], [GeneratedCode]...
  in particular those added to fix warnings when internals are hidden

### Medium Priority
- Bug: Some assemblies raise a CA warning on resources not used.
- Bug: We used [module: SuppressMessage(...)] to suppress some CA or SA warnings
  but this does not work as expected when used in an assembly info file:
  it suppresses the warning for the whole assembly. We should further 
  review all cases where we do such a thing (T4 files).
- Enhancement: PSake CI target should build and test all possible configurations, 
  currently Debug|Release, AnyCPU, with or without visible internals.
- Enhancement: Script to push packages to NuGet.
- Enhancement: Non retail packages should be copied to MyGet.
- Enhancement: Static analysis with Gendarme.
- Enhancement: Create symbol packages (or use GitLink?).
- Improvement: Fix all CA and SA warnings and errors.

### Low Priority
- Enhancement: Zip artefacts when done building.
- Enhancement: Build documentation.
- Enhancement: Create a website for the project.
- Enhancement: Add Code Coverage + Report Generator.
- Improvement: Make unnecessary to add StyleCop settings to each project.
- Improvement: T4 re-generation has been disabled since it requires VS hosting.
  Is there a way we can enable T4 re-generation during a build?
- Improvement: Write assembly and NuGet descriptions.
- Improvement: Some projects don't have an {AssemblyName}.Version.props.

