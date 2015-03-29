// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Narvalo.Common")]
[assembly: AssemblyDescription("Narvalo Common Library containing helpers and extension methods for classes from the BCL.")]

[assembly: Guid("8cd3c522-030f-49b4-bd87-285e2b35425b")]

#if NO_INTERNALS_VISIBLE_TO
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "Narvalo.Resources.Strings_Common",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Narvalo.Resources.Strings_Common.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Narvalo.Resources.Strings_Common.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
#else
[assembly: InternalsVisibleTo("Narvalo.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif