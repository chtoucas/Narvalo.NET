// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Narvalo.Futures")]
[assembly: AssemblyDescription("")]

[assembly: Guid("e394c38b-f999-4322-b821-7630368da16f")]

#if !NO_INTERNALS_VISIBLE_TO
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Futures.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif