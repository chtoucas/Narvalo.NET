Guidelines
==========

Coding Style
------------

### General

In general, we follow the [guidelines](https://github.com/dotnet/corefx/wiki/Coding-style)
from the .NET team with few differences:
- Namespace imports should be specified at the top of the file, _inside_ the namespace declarations.
- Do not use language keywords for methods calls (i.e. `Int32.Parse` instead of `int.Parse`).
- Do not use PascalCasing to name private constants (i.e `MY_PRIVATE_CONSTANT` instead of `MyPrivateConstant`).

We also enforce the following rules:
- Suffix all private methods and classes with `_`.
- Directories must mirror namespaces.
- Do not put more than one public class per file. The only exception is for Code Contracts classes.
- All files must contain a copyright header:
```csharp
// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.
```

Optional rules:
- Consider using regions or partial classes to organize code.
- Consider separating System imports from other imports.
- Source lines should not exceed 120 characters.

### Tasks

List of recognized tasks:
- FIXME
- TODO
- REVIEW

For temporary string content, use `"XXX"`.

### Naming tests

- {TypeUnderTest}Facts
- {MemberUnderTest}_{ExpectedOutcome}
- {TypeName}_{PropertyDescription}
- {ActionDescription}_{ExpectedOutcome}

Suffixes:
- _For{WhichArgument}
- _{Context}

Example:
```csharp
public static class MyTypeFacts
{
    public static void MyMethod_ReturnsTrue_ForEmptyInput() { }

    public static void MyType_IsImmutable() { }
}
```

If a test suite contains white-box tests, add a fake test as follows:
```csharp
#if NO_INTERNALS_VISIBLE_TO // White-box tests.

    public static partial class MyTypeFacts
    {
        [Fact(Skip = "White-box tests disabled in this configuration.")]
        public static void Maybe_BlackBox() { }
    }

#else

    // Here goes the white-box tests.

#endif
``` 

Design Recommendations
----------------------

Rules:
- Internal classes must be in a subdirectory named "Internal".
- Do not use reserved words:
  * `Current`
  * `Select` (LINQ operator)

Optional rules:
- Consider putting optional extensions in a subdirectory named "Extensions".
- Projects should use a minimal set of references.

Code Analysis
-------------

### Suppressions

All suppressions must be justified and tagged:
- `[Ignore]` Only use this to tag a false positive and for unrecognized
  Code Contracts postconditions; if they are no longer necessary CCCheck will tell us.
- `[GeneratedCode]` Used to mark a suppression related to generated code.
- `[Intentionally]` Used in all other cases
- `[Educational]` Only used inside the project Narvalo.Brouillons.

In addition, defects that need to be fixed are tagged with `[FIXME]`
in the global suppression file.

For Gendarme, we use a global suppression file `etc\gendarme.ignore` shared across 
all projects. This file is used exclusively for defects that can not be masked
with a `SuppressMessage` attribute and for defects that need a fix.

### StyleCop

For a detailed description of each rule, check out the
[documentation](http://www.stylecop.com/docs/).

### FxCop
         
Except for test projects we use a strict ruleset; 
only one rule is disabled: _[CA1006] Do not nest generic types in member signatures_.
Nevertheless, we will test the equivalent rule with Gendarme which has the ability
to disable a rule at assembly level.

Test projects use a relaxed ruleset.

### Gendarme

### Code Contracts

Compilation Symbols
-------------------

Always prefer conditional attributes to `#ifdef`. The only two exceptions are described below.
If you use `#ifdef` directives you must justify it with a comment just after the `#if`.

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

### Object Invariants

Wrap any object invariants method and contract class with a compiler conditional clause:
```csharp
#if CONTRACTS_FULL // Using directive.
    using System.Diagnostics.Contracts;
#endif

    public class MyType
    {
#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
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

### White-box Tests

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

Performance
-----------
