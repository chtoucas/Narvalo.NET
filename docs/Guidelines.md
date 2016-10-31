Guidelines
==========

Coding Style
------------

### Mandatory Rules

In general, we follow the [guidelines](https://github.com/dotnet/corefx/wiki/Coding-style)
from the .NET team with few differences.

All rules (included a few custom ones) are checked by StyleCop.

Rules not yet enforced via StyleCop are:
- Namespace imports should be specified at the top of the file, _inside_ the namespace declarations.
  In case of namespace conflict, use the 'global::' prefix.
- Consider separating System imports from the others.
- Do not use language keywords for methods calls (i.e. prefer `Int32.Parse` over `int.Parse`),
  for object creations and when used with `typeof`.
- Do not use PascalCasing to name private constants, prefer `MY_PRIVATE_CONSTANT` over `MyPrivateConstant`.
- Directories must mirror namespaces.
- Do not put more than one public class per file. The only exception is for Code Contracts classes.
- Consider using regions or partial classes to organize code.
- Consider using named parameters for disambiguation of constant or null parameters.
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

- Do not write extension methods on core types.
- Do not use reserved words unless they are used in their intended sense:
  * `Current`
  * `Select` (LINQ operator)
  * `Add`, collection initializer
- All static members should be thread-safe.
- Consider putting internal classes in a separate directory named "Internal".

### Optional Rules

- Consider putting optional extensions in a separate directory.
- Projects should use a minimal set of references.

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
- `[Intentionally]` Used in all other cases
- `[Educational]` Only used inside the project Narvalo.Brouillons.

Consider putting the justification on its own line. This helps to quickly see them in search results.

For Gendarme, we use a global suppression file `etc\gendarme.ignore` shared across
all projects. This file is used exclusively for defects that can not be masked
with a `SuppressMessage` attribute and for suppressions at assembly-level or at namespace-level.

### StyleCop

For a detailed description of each rule, check out the official
[documentation](http://www.stylecop.com/docs/).

To suppress a StyleCop warning for a Narvalo.StyleCop rule, use:
```csharp
[SuppressMessage("Narvalo.CSharpRules", "NA1006:InternalMethodsMustNotEndWithInternal", 
    Justification = "...")]
```

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

Compilation symbols for .NET versions (mostly unused):
- `NET_35`
- `NET_40`

Symbols used to define the assembly properties:
- `BUILD_GENERATED_VERSION`
- `DUMMY_GENERATED_VERSION`
- `NO_INTERNALS_VISIBLE_TO`
- `SIGNED_ASSEMBLY`
- `SECURITY_ANNOTATIONS`

[References]
- [Eric Lippert](http://ericlippert.com/2009/09/10/whats-the-difference-between-conditional-compilation-and-the-conditional-attribute/)

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

Documentation
-------------

Instead of literal true, false, null, use `<see langword="true"/>`...

Non-standard tags:
- `<inheritdoc cref=""/>`
- `<content></content>` used for documenting partial classes.
- `<internalonly/>` for internal members or types.

### Literate Programming (Kind Of)

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

Consider applying the `SecurityTransparent` attribute or the `AllowPartiallyTrustedCallers`
attribute (if the assembly contains security critical methods) to the assembly. If you do so, 
verify the  assembly with the `SecAnnotate` tool.
