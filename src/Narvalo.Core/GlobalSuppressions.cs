// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#if !NO_GLOBAL_SUPPRESSIONS

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Narvalo.Xml",
    Justification = "Hopefully more will come.")]

[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "Narvalo.Collections.Internal.EnumerableOutputExtensions+<>c__DisplayClass3`1+<>c__DisplayClass5.#CS$<>8__locals4",
    Justification = "[GeneratedCode] This method has been overridden for performance reasons.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "Narvalo.Collections.Internal.EnumerableMaybeExtensions+<>c__DisplayClass3`1+<>c__DisplayClass5.#CS$<>8__locals4",
    Justification = "[GeneratedCode] This method has been overridden for performance reasons.")]

#endif