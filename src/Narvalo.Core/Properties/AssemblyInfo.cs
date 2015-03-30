// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security;

[assembly: AssemblyTitle("Narvalo.Core")]
[assembly: AssemblyDescription("Narvalo Core Library containing implementations of functional patterns.")]

[assembly: SecurityTransparent]

[assembly: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "Narvalo.Properties.Strings_Core",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Narvalo.Properties.Strings_Core.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Narvalo.Properties.Strings_Core.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]

#if !NO_INTERNALS_VISIBLE_TO // Make internals visible to the test projects.
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
