// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Narvalo.Web")]
[assembly: AssemblyDescription("Narvalo Web Library containing base classes for web development.")]

[assembly: Guid("88d199e9-8029-4c28-93f2-535bb196b06d")]

[assembly: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "Narvalo.Web.Properties.Strings",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Narvalo.Web.Properties.Strings.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Narvalo.Web.Properties.Strings.#Culture",
    Justification = "[GeneratedCode] Default visibility for resources is internal.")]

#if !NO_INTERNALS_VISIBLE_TO // Make internals visible to the test projects.
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
