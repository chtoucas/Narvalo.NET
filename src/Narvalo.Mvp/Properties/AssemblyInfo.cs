// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Narvalo.Mvp")]
[assembly: AssemblyDescription("MVP Framework.")]

[assembly: Guid("65ea83eb-47d3-44c1-8d85-2654954ad0ed")]

[assembly: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
    Scope = "type", Target = "Narvalo.Mvp.Properties.Strings",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
    Scope = "member", Target = "Narvalo.Mvp.Properties.Strings.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]

#if !NO_INTERNALS_VISIBLE_TO
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Mvp.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
