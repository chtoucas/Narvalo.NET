// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[assembly: AssemblyTitle("Narvalo.Cerbere")]
[assembly: AssemblyDescription("Library providing argument validation methods and Code Contracts helpers.")]

#if SECURITY_ANNOTATIONS
[assembly: System.Security.SecurityTransparent]
#endif

[assembly: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
    Scope = "type", Target = "Narvalo.Properties.Strings_Cerbere",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
    Scope = "member", Target = "Narvalo.Properties.Strings_Cerbere.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]

#if !NO_INTERNALS_VISIBLE_TO // Make internals visible to the test projects.
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
