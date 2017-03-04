// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Narvalo.Mvp.Web")]
[assembly: AssemblyDescription("MVP Framework for ASP.NET.")]

[assembly: Guid("31740667-bd2d-4974-b0cf-0b7c0a639ff0")]

[assembly: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
    Scope = "type", Target = "Narvalo.Mvp.Web.Properties.Strings",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
    Scope = "member", Target = "Narvalo.Mvp.Web.Properties.Strings.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]

#if !NO_INTERNALS_VISIBLE_TO
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Mvp.Web.Facts" + Narvalo.AssemblyInfo.PublicKeySuffix)]
#endif
