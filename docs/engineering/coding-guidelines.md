Coding Guidelines
=================

Design and Style
----------------

We mostly follow the guidelines produced by the .NET team:
- [CoreFX](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/)
- [CoreClr](https://github.com/dotnet/coreclr/blob/master/Documentation/coding-guidelines/)

### Mandatory Rules
- Directories must mirror namespaces.
- Namespace imports should be specified at the top of the file _inside_ the namespace declarations.
  In case of namespace conflict, use the 'global::' prefix.
- Do not use language keywords for methods calls (i.e. prefer `Int32.Parse` over `int.Parse`),
  for object creations and when used with `typeof`.
- Do not use PascalCasing to name private constants, prefer `MY_PRIVATE_CONSTANT` over
  `MyPrivateConstant`.
- Do not put more than one public class per file. The only exceptions are for Code Contracts
  classes and enum extensions.
- Do not write extension methods on core types.
- Do not use reserved words unless they are used in their intended sense:
  * `Current`
  * `Select` (LINQ operator)
  * `Add`, collection initializer
- All static members must be thread-safe.
- Portable Class Libraries: the behaviour must be 100% identical across all supported platforms
  without resorting to the [bait and switch PCL trick](http://log.paulbetts.org/the-bait-and-switch-pcl-trick/).

### Optional Rules
- Projects should use a minimal set of references.
- Consider separating System imports from the others. Separate static imports from the others.
- Consider using regions or (preferably) partial classes to organize code.
- Consider using named parameters for disambiguation of constant or null parameters.
- For concrete helper classes try to find a more useful suffix than "Helper" or "Utility"
  or use a verb. Examples: "Require", "ParseTo", StringManip"...
  If not, use "Helper" for concrete classes and "Helpers" for static classes.
- Consider making read-only properties thread-safe.
- Consider putting internal classes in a subnamespace named "Internal".
- Consider putting classes intended for advanced usages or uncommon scenarios in a subnamespace
  named "Advanced". Same applies to optional extensions.
- Consider putting localized resources in the `Properties` folder.
- Consider putting other resources in a `Resources` folder.
- Consider using tasks:
  * FIXME
  * HACK
  * TODO
  * REVIEW
  For temporary strings, use `"XXX"`.

Code Analysis
-------------

### Suppressions
All suppressions must be justified and tagged:
- `[Ignore]` Only use this one to tag a false positive and for unrecognized
  Code Contracts postconditions; by the way, if they are no longer necessary
  CCCheck will tell us.
- `[GeneratedCode]` Used to mark a suppression related to generated code.
- `[Intentionally]` Used in all other cases.

Consider putting the justification on its own line. This helps to quickly see
them in search results.

```csharp
using global::System.Diagnostics.CodeAnalysis;

[module: SuppressMessage("Narvalo.CSharpRules", "NA1201:FilesMustStartWithCopyrightText", Justification = "[Ignore] Microsoft source file.")]
```

### FxCop
Except for test projects we use a strict ruleset;
only two rules are disabled: CA1006 and IDE0001.

### Dictionary
Every project already load the dictionary `etc\CodeAnalysisDictionary.xml`.
If needed, consider adding a local dictionary `CustomDictionary.xml` in the
directory `Properties` rather than modifying the global one.

Code Contracts
--------------

The target is full CC coverage: we enable all options of the static contract checker
except "Check redundant assume".

Remember that you can mark a type/member with the attribute `[ContractVerification(false)]`.
If this is expected to be permanent, justify it.

### Object Invariants

Wrap any object invariants method and contract class with a compiler conditional clause.

Compilation Symbols and Conditional Attributes
----------------------------------------------

Compilation Symbols are a pain in the ass: it prevents clean refatoring, things might
or might not work depending on the build configuration.

Standard compilation symbols:
- `DEBUG`
- `TRACE`
- `CODE_ANALYSIS`
- `CONTRACTS_FULL`

Symbols used to define the assembly properties:
- `BUILD_GENERATED_VERSION`
- `DUMMY_GENERATED_VERSION`
- `NOT_CLS_COMPLIANT`
- `NO_INTERNALS_VISIBLE_TO`
- `SECURITY_ANNOTATIONS`
- `SIGNED_ASSEMBLY`

**Always** prefer conditional attributes to `#if` directives. We only accept three exceptions:
object invariants and CC hacks (`CONTRACTS_FULL`), exposing internals to test projects
and white-box tests (`NO_INTERNALS_VISIBLE_TO`), and security annotations (`SECURITY_ANNOTATIONS`).
If you use an `#if` directive you should justify it with a comment placed on
the same line as the `#if` (this helps to quickly spot the justification in search results):
```csharp
#if !NO_INTERNALS_VISIBLE_TO // Make internals visible to the test projects.
#endif
#if !NO_INTERNALS_VISIBLE_TO // White-box tests.
#endif
#if CONTRACTS_FULL // Contract Class and Object Invariants.
#endif
#if CONTRACTS_FULL // Custom ctor visibility for the contract class only.
#endif
#if CONTRACTS_FULL // Helps CCCheck with the object invariance.
#endif
```
NB: This is not necessary for security annotations.

The list of namespace imports **must** not depend on the value of compilation symbols.
For instance, do not write:
```csharp
using System.Security;

#if SECURITY_ANNOTATIONS
[assembly: SecurityTransparent]
#endif
```
but prefer:
```csharp
#if SECURITY_ANNOTATIONS
[assembly: System.Security.SecurityTransparent]
#endif
```

References:
- [Eric Lippert](http://ericlippert.com/2009/09/10/whats-the-difference-between-conditional-compilation-and-the-conditional-attribute/)

Documentation
-------------

Instead of literal true, false, null, use `<see langword="true"/>`...

Non-standard tags:
- `<inheritdoc cref=""/>`
- `<internalonly/>` for internal members or types.

```csharp
namespace Namespace
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The <c>Namespace</c> namespace contains classes and interfaces inspired by functional programming.
    /// </summary>
    [CompilerGenerated]
    internal static class NamespaceDoc { }
}
```

Tests
-----

Test projects are first-class citizens; translate: they must pass static analysis.

### Mandatory Rules
- Use the same directory hierarchy that the one used by the libraries.
- Name `[Type]Facts` a test class.
- Name `[UnitOfWork]_[OutcomeOrBehaviour]_[Context]` a unit test.
- After a bugfix, create a unit test, decorate it with the `Issue` attribute
  and add a detailed summary of the bug.
- Always justify a skipped test.

### Optional Rules
- Consider adding a suffix `For{Argument}` to describe the arguments used.
- When testing for exceptions use: `Throws[Exception]` or `DoesNotThrow`.
- Consider using the same ordering for tests than the one used inside classes.
- Consider wrapping each set of tests with `#region ... #endregion`.
- Consider using traits:
  * "Slow" for slow tests.
  * "Unsafe" for unsafe tests (`AppDomain` for instance).

Useful attributes
-----------------

### Usability attributes
- `EditorBrowsableAttribute`
- `EditorBrowsableState`

- `DebuggerHidden` means that the method won't appear in the call stack.
  This attribute CAN NOT be set on a class.
- `DebuggerStepThrough` means that the code will be marked as _external_
  in the call stack. This attribute can be set on a class.
- `DebuggerNonUserCode`
- `DebuggerBrowsable`
- `DebuggerDisplay`
- `DebuggerTypeProxy`

See http://geekswithblogs.net/terje/archive/2008/11/10/hiding-generated-code-from-code-analysis-metrics-and-test-coverage.aspx

### Performance attributes
- `MethodImpl`
- `StructLayout`

Multi-Threading
---------------

Always remember that you are most certainly not an expert in multi-threading:
**Always prefer high-level constructs to low-level primitives**.

Security
--------

**WARNING:** Currently, security attributes are not present in the assemblies distributed
via NuGet, therefore they use the default policy (security critical).

Consider applying the `SecurityTransparent` attribute or the `AllowPartiallyTrustedCallers`
attribute to the assembly. If you do so, verify the assembly with the `SecAnnotate` tool (done
automatically if you use the SecurityAnalysis task from the PSake file).

Remark:
All methods in ASP.NET MVC v5 default to security critical, our only choice would
be to mark Narvalo.Web with the APTCA attribute and to apply the correct security
attribute where it is needed, but APTCA and ASP.NET MVC
[do not work together](https://github.com/DotNetOpenAuth/DotNetOpenAuth/issues/307).

References:
- [CAS](http://msdn.microsoft.com/en-us/library/c5tk9z76%28v=vs.110%29.aspx)
- [Security-Transparent Code, Level 2](https://msdn.microsoft.com/en-us/library/dd233102(v=vs.110).aspx)
- [APTCA](https://msdn.microsoft.com/en-us/magazine/ee336023.aspx)
- [SecAnnotate](http://blogs.msdn.com/b/shawnfa/archive/2009/11/18/using-secannotate-to-analyze-your-assemblies-for-transparency-violations-an-example.aspx)
- [SecAnnotate and PCL](http://stackoverflow.com/questions/12360534/how-can-i-successfully-run-secannotate-exe-on-a-library-that-depends-on-a-portab)
- [Simple Talk Tutorial](https://www.simple-talk.com/dotnet/net-framework/whats-new-in-code-access-security-in-net-framework-4-0-part-i/)
- [Stackoverflow](http://stackoverflow.com/questions/5055632/net-4-allowpartiallytrustedcallers-attribute-and-security-markings-like-secur)
- [Migrating an APTCA Assembly to the .NET Framework 4](https://msdn.microsoft.com/en-us/magazine/ee336023.aspx)
