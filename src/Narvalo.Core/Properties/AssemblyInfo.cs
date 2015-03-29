// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Narvalo.Core")]
[assembly: AssemblyDescription("Narvalo Core Library containing implementations of functional patterns.")]

#if NO_INTERNALS_VISIBLE_TO
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "Narvalo.Resources.Strings_Core",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Narvalo.Resources.Strings_Core.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Narvalo.Resources.Strings_Core.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
#else
[assembly: InternalsVisibleTo("Narvalo.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
