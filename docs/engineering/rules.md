Coding Rules
============

Coding Style
------------

In general, we follow the guidelines from the .NET team with few differences:
- [CoreFX](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/)
- [CoreClr](https://github.com/dotnet/coreclr/blob/master/Documentation/coding-guidelines/)

All rules (included a few custom ones) are checked by StyleCop.

Rules not yet enforced via StyleCop are:
- Namespace imports should be specified at the top of the file, _outside_ the namespace declarations.
  In case of namespace conflict, use the 'global::' prefix.
- Consider separating System imports from the others.
- Do not use language keywords for methods calls (i.e. prefer `Int32.Parse` over `int.Parse`),
  for object creations and when used with `typeof`.
- Do not use PascalCasing to name private constants, prefer `MY_PRIVATE_CONSTANT` over `MyPrivateConstant`.
- Directories must mirror namespaces.
- Do not put more than one public class per file. The only exception is for Code Contracts classes.
- Consider using regions or (preferably) partial classes to organize code.
- Consider using named parameters for disambiguation of constant or null parameters.
- For concrete helper classes try to find a more useful suffix than "Helper" or "Utility"
  or use a verb. Examples: "Require", "ParseTo", StringManip"...
  If not, use "Helper" for concrete classes and "Utility" for static classes.

### StyleCop

For a detailed description of each rule, check out the official
[documentation](http://www.stylecop.com/docs/).

To suppress a StyleCop warning for a Narvalo.StyleCop rule, use:
```csharp
[SuppressMessage("Narvalo.CSharpRules", "NA1006:InternalMethodsMustNotEndWithInternal",
    Justification = "...")]
```

### Tasks

Consider using tasks:
- FIXME
- TODO
- REVIEW

For temporary string content, use `"XXX"`.

Design
------

### Rules

#### Mandatory Rules

- Do not write extension methods on core types.
- Do not use reserved words unless they are used in their intended sense:
  * `Current`
  * `Select` (LINQ operator)
  * `Add`, collection initializer
- All static members should be thread-safe.

#### Optional Rules

- Projects should use a minimal set of references.
- Consider making read-only properties thread-safe.
- Consider putting internal classes in a subnamespace named "Internal".
- Consider putting classes intended for advanced usages or uncommon scenarios in a subnamespace
  named "Advanced". Same applies to optional extensions.

### Localization & Resources

- Consider putting localized resources in the `Properties` folder.
- Consider putting other resources in a `Resources` folder.

### Portable Class Libraries

The behaviour should be 100% identical across all supported platforms without
resorting to the [bait and switch PCL trick](http://log.paulbetts.org/the-bait-and-switch-pcl-trick/).

Code Analysis
-------------

### Suppressions

All suppressions must be justified and tagged:
- `[Ignore]` Only use this to tag a false positive and for unrecognized
  Code Contracts postconditions; if they are no longer necessary CCCheck will tell us.
- `[GeneratedCode]` Used to mark a suppression related to generated code.
- `[Intentionally]` Used in all other cases.

Consider putting the justification on its own line. This helps to quickly see them in search results.

For Gendarme, we use a global suppression file `etc\gendarme.ignore` shared across
all projects. This file is used exclusively for defects that can not be masked
with a `SuppressMessage` attribute and for suppressions at assembly-level or at namespace-level.

### FxCop

Except for test projects we use a strict ruleset;
only one rule is disabled: _[CA1006] Do not nest generic types in member signatures_.
Nevertheless, we will test the equivalent rule with Gendarme which has the ability
to disable a rule at assembly level.

Test projects use a relaxed ruleset. Roughly, you don't have to create C# documentation.

#### Dictionary
Every project already load the dictionary `etc\CodeAnalysisDictionary.xml`.
If needed, consider adding a local dictionary `CustomDictionary.xml` in the
directory `Properties` rather than modifying the global one.

### Gendarme

Code Contracts
--------------

The target is full CC coverage: we enable all options of the static contract checker
except "Check redundant assume".
```
-outputwarnmasks -show unreached
```

Remember that you can mark a type/member with the attribute `[ContractVerification(false)]`.
If this is expected to be permanent, justify it.

### Object Invariants

Wrap any object invariants method and contract class with a compiler conditional clause:
```csharp
    public class MyType
    {
#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [System.Diagnostics.Contracts.ContractInvariantMethod]
        void ObjectInvariant()
        {
            // Contract invariants directives.
        }

#endif
    }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

    [ContractClass(typeof(IMyTypeContract))]
    public partial interface IMyType
    {
        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            // Contract invariants directives.
        }
    }

    [ContractClassFor(typeof(IMyType))]
    internal abstract class IMyTypeContract : IMyType
    {
    }

#endif
```

Compilation Symbols
-------------------

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
- `<content></content>` used for documenting partial classes.
- `<internalonly/>` for internal members or types.

### Literate Programming (Obsolete)

```csharp
/**
 * <content markup="commonmark">
 * <![CDATA[
 *
 * ]]>
 * </content>
 */
```

Tests
-----

Test projects are first-class citizens; translate: they must pass all static analysis.

### Rules

#### Mandatory Rules
- Use the same directory hierarchy that the one used by the libraries.
- Name [TypeUnderTest]Facts a test class.
- Name [UnitOfWork]_[ExpectedOutcomeOrBehaviour]_[Context] a unit test.
- When testing for exceptions use: `_Throws[ExpectedException]` or `_DoesNotThrow`.
- After a bugfix, create a unit test, decorate it with the `Issue` attribute
  and add a detailed summary of the bug.
- Always justify a skipped test.
- Do not run a different set of tests depending on the build configuration.

Example:
```csharp
public static class MyTypeFacts
{
    public static void MyMethod_ReturnsTrue_ForEmptyInput() { }

    public static void MyType_IsImmutable() { }
}
```

#### Optional Rules
- Consider adding a suffix _For{WhichArgument} to describe the arguments used.
- Consider adding a suffix _{Context} to describe the context.
- Consider using the same ordering for tests than the one used inside classes.
- Consider wrapping each set of tests with `#region ... #endregion`.
- Consider using traits:
  * "Slow" for slow tests.

### White-Box Tests

Wrap white-box tests as follow:
```csharp
    public static partial class Facts
    {
         // Black-box tests.
    }

#if !NO_INTERNALS_VISIBLE_TO // White-box tests.

    public static partial class Facts
    {
         // White-box tests
    }

#endif
```

Add `Narvalo.TestCommon.EnvironmentFacts.cs` to test projects.

Useful attributes
-----------------

### Usability attributes
- `DebuggerStepThrough`
- `DebuggerHidden`
- `[DebuggerBrowsable(DebuggerBrowsableState.Never)]`
- `[DebuggerDisplay("{{ToString()}}")]`
- `[EditorBrowsableAttribute(EditorBrowsableState.Never)]`

### Performance attributes
- `[MethodImpl(MethodImplOptions.AggressiveInlining)]`

Multi-Threading
---------------

Always remember that you are not an expert in multi-threading:
**Always prefer high-level constructs to low-level primitives**.

Security
--------

**WARNING:** Currently, security attributes are not present in the assemblies distributed
via NuGet, therefore they use the default policy (security critical).

Consider applying the `SecurityTransparent` attribute or the `AllowPartiallyTrustedCallers`
attribute to the assembly. If you do so, verify the assembly with the `SecAnnotate` tool (done
automatically if we use the SecurityAnalysis task from the PSake file).

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
- http://stackoverflow.com/questions/5055632/net-4-allowpartiallytrustedcallers-attribute-and-security-markings-like-secur
- https://msdn.microsoft.com/en-us/magazine/ee336023.aspx
