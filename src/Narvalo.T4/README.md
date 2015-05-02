Narvalo.T4
==========

The interop library `EnvDTE` must not be embedded into `Narvalo.T4`;
otherwise the `DTE` property of `VSTemplate` won't work.
By the way, to access this property, you must add the following directive
to your template:
```
<#@ assembly name="EnvDTE" #>
```

References:
- [Managing Complexity in T4 Code-Generation Solutions](https://msdn.microsoft.com/en-us/magazine/hh975350.aspx)
- [Automation and Extensibility for Visual Studio](https://msdn.microsoft.com/en-us/library/vstudio/xc52cke4(v=vs.100).aspx)
- [Oleg Sych Blog](http://www.olegsych.com/)
- [T4Toolbox](https://t4toolbox.codeplex.com/)
