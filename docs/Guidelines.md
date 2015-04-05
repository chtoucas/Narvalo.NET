Guidelines
==========

Coding Style
------------

### Mandatory Rules

In general, we follow the [guidelines](https://github.com/dotnet/corefx/wiki/Coding-style)
from the .NET team with few differences:
- Namespace imports should be specified at the top of the file, _inside_ the namespace declarations.
  In case of namespace conflict, use the 'global::' prefix.
- Do not use language keywords for methods calls (i.e. `Int32.Parse` instead of `int.Parse`) 
  and for object creation.
- Do not use PascalCasing to name private constants, prefer `MY_PRIVATE_CONSTANT` over `MyPrivateConstant`.

We also enforce the following rules:
- Add a suffix to all private methods and classes with `_`.
- Directories must mirror namespaces.
- Do not put more than one public class per file. The only exception is for Code Contracts classes.
- All files must contain a copyright header:
```csharp
// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.
```

### Optional Rules

- Consider using regions or partial classes to organize code.
- Consider separating System imports from the others.
- Source lines should not exceed 120 characters.
- Avoid named parameters.

Naming:

- Consider adding the "Base" suffix to all abstract classes.
- Consider not adding an internal access modifier to members of an internal class.
- Consider adding an "Internal" suffix to the names of internal methods in a non-internal class with "Internal".
- Consider adding an "Internal" prefix to the names of internal static fields and constants in a non-internal class.
- For concrete helper classes try to find a more useful suffix than "Helper" or "Utility"
  or use a verb. Examples: "Require", "ParseTo", StringManip"...
  If not, use "Helper" for concrete classes and "Utility" for static classes.

### Tasks

Consider using tasks:
- FIXME
- TODO
- REVIEW

For temporary string content, use `"XXX"`.

Design Recommendations
----------------------

### Mandatory Rules

- Internal classes must be in a subdirectory named "Internal".
- Do not use reserved words:
  * `Current`
  * `Select` (LINQ operator)
- All static members should be thread-safe.

### Optional Rules

- Consider putting optional extensions in a subdirectory named "Extensions".
- Projects should use a minimal set of references.   

### Localization & Resources

- Consider putting localized resources in the `Properties` folder.
- Consider putting other resources in a `Resources` folder.

Code Analysis
-------------

### Suppressions

All suppressions must be justified and tagged:
- `[Ignore]` Only use this to tag a false positive and for unrecognized
  Code Contracts postconditions; if they are no longer necessary CCCheck will tell us.
- `[GeneratedCode]` Used to mark a suppression related to generated code.
- `[Intentionally]` Used in all other cases
- `[Educational]` Only used inside the project Narvalo.Brouillons.

Consider putting the justification on its own line. This helps to quickly see them in search results.

In addition, defects that need to be fixed are tagged with `[FIXME]`
in the global suppression file. This helps tracking things.

For Gendarme, we use a global suppression file `etc\gendarme.ignore` shared across 
all projects. This file is used exclusively for defects that can not be masked
with a `SuppressMessage` attribute and for defects that need a fix.
    
### StyleCop

For a detailed description of each rule, check out the official
[documentation](http://www.stylecop.com/docs/).

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

### Code Contracts

The target is full CC coverage: we enable all options of the static contract checker 
except "Check redundant assume".
```
-outputwarnmasks -show unreached
```

Remember that you can mark a type/member with the attribute `[ContractVerification(false)]`.
If this is expected to be permanent, justify it.

Compilation Symbols
-------------------

Compilation Symbols are a pain in the ass: it prevents clean refatoring, things might
or might not work depending on the build configuration.

**Always** prefer conditional attributes to `#ifdef`. We only accept three exceptions:
object invariants, exposing internals to test projects and white-box tests (see below).

If you use an `#ifdef` directive you must justify it with a comment placed on 
the same line as the `#if`. This helps to quickly spot the justification in search results.

Standard compilation symbols:
- `DEBUG`
- `TRACE`
- `CODE_ANALYSIS`
- `CONTRACTS_FULL`

Compilation symbols for .NET versions:
- `NET_35`
- `NET_40`

Symbols used to define the assembly properties:
- `BUILD_GENERATED_VERSION`
- `DUMMY_GENERATED_VERSION`
- `NO_INTERNALS_VISIBLE_TO`
- `SIGNED_ASSEMBLY`

[References]
- [Eric Lippert](http://ericlippert.com/2009/09/10/whats-the-difference-between-conditional-compilation-and-the-conditional-attribute/)

### Object Invariants

Wrap any object invariants method and contract class with a compiler conditional clause:
```csharp
    public class MyType
    {
#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [System.Diagnostics.Contracts.ContractInvariantMethod]
        void ObjectInvariants()
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
        void ObjectInvariants()
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

Documentation
-------------

Non-standard tags:
- `<copydoc cref=""/>`
- `<content></content>` used for documenting partial classes.
- `<internalonly/>` for internal members or types.

### Literate Programming

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

Test projects are first-class citizens; translate: they must pass successfully 
all static analysis.             

### Mandatory Rules
- Use the same directory hierarchy that the one used by the libraries.
- Name {Type}Facts a test class for the type {Type}.
- Name {Member}_{ExpectedOutcome} a unit test for a member {Member}.
- Name {Type}_{ExpectedOutcome} a unit test for the type {Type} not specific 
  to a member of the type.
- When testing for exceptions use: `_Throws{ExpectedException}` or `_DoesNotThrow`.
- After a bugfix, create a unit test, decorate it with the `Issue` attribute 
  and add a detailed summary of the bug.
- Add a justification for all skipped tests.
- Do not run a different set of tests depending on the build configuration.
  If you have to, do not disable the test but skip it.     

Example:
```csharp
public static class MyTypeFacts
{
    public static void MyMethod_ReturnsTrue_ForEmptyInput() { }

    public static void MyType_IsImmutable() { }
}
```

### Optional Rules
- Consider adding a suffix _For{WhichArgument} to describe the arguments used.
- Consider adding a suffix _{Context} to describe the context.
- Consider using the same ordering for tests than the one used inside classes.
- Consider wrapping each set of tests with `#region ... #endregion`. 
- Consider using traits:
  * "Slow" for slow tests.

### White-Box Tests

If a test suite contains white-box tests, add also a fake test as follows:
```csharp
#if NO_INTERNALS_VISIBLE_TO // White-box tests.

    public static partial class MyTypeFacts
    {
        [Fact(Skip = "White-box tests disabled for this configuration.")]
        public static void Maybe_BlackBox() { }
    }

#else

    // Here goes the white-box tests.

#endif
``` 

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

Multi-Threading
---------------

Always remember that you are not an expert in multi-threading:
**Always prefer high-level constructs to low-level primitives**.

Performance
-----------

### Delegates

### Virtual Methods

### Boxing and Unboxing

References:
- [CoreFX](https://github.com/dotnet/corefx/wiki/Performance)
- [CoreClr](https://github.com/dotnet/coreclr/wiki/Performance-Requirements)
- [Delegates](http://blogs.msdn.com/b/pfxteam/archive/2012/02/03/10263921.aspx)

Security
--------

Consider applying the `SecurityTransparent` attribute to the assembly.
If you do so, verify the assembly with the `SecAnnotate` tool.

Right now, this is only done for Narvalo.Core, because of unsolved problems 
with `SecAnnotate` and libraries depending on a PCL project (see `tools\Make.CustomAfter.targets`).

References:
- [CAS](http://msdn.microsoft.com/en-us/library/c5tk9z76%28v=vs.110%29.aspx)
- [APTCA](https://msdn.microsoft.com/en-us/magazine/ee336023.aspx)
- [SecAnnotate](http://blogs.msdn.com/b/shawnfa/archive/2009/11/18/using-secannotate-to-analyze-your-assemblies-for-transparency-violations-an-example.aspx)
- [SecAnnotate and PCL](http://stackoverflow.com/questions/12360534/how-can-i-successfully-run-secannotate-exe-on-a-library-that-depends-on-a-portab)
- [Tutorial](http://www.codeproject.com/Articles/329666/Things-I-learned-while-implementing-my-first-Level)
