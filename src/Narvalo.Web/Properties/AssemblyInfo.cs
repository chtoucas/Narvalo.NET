// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Narvalo.Web")]
[assembly: AssemblyDescription("Narvalo Web Library containing base classes for web development.")]

[assembly: Guid("88d199e9-8029-4c28-93f2-535bb196b06d")]

#if NO_INTERNALS_VISIBLE_TO // Resources & Tests.
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "Narvalo.Web.Properties.Strings_Web",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Narvalo.Web.Properties.Strings_Web.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Narvalo.Web.Properties.Strings_Web.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
#else
[assembly: InternalsVisibleTo("Narvalo.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
