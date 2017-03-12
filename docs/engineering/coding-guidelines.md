Coding Guidelines
=================

We mostly follow the guidelines produced by the .NET team:
- [CoreFX](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/)
- [CoreClr](https://github.com/dotnet/coreclr/blob/master/Documentation/coding-guidelines/)

Consider using tasks:
  * FIXME
  * HACK
  * TODO
  * REVIEW

For temporary strings, use `"XXX"`.

For tests, consider using traits:
  * "Slow" for slow tests.
  * "Unsafe" for unsafe tests (`AppDomain` for instance).

All Code Analysis suppressions must be justified and tagged:
- `[Ignore]` Only use this one to tag a false positive and for unrecognized
  Code Contracts postconditions; by the way, if they are no longer necessary
  CCCheck will tell us.
- `[GeneratedCode]` Used to mark a suppression related to generated code.
- `[Intentionally]` Used in all other cases.

Every project already load the dictionary `etc\CodeAnalysisDictionary.xml`.
If needed, consider adding a local dictionary `CustomDictionary.xml` in the
directory `Properties` rather than modifying the global one.

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

