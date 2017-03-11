// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;

[assembly: AssemblyTitle("Narvalo.Fx")]
[assembly: AssemblyDescription("Library featuring implementations of functional patterns.")]

#if !NO_INTERNALS_VISIBLE_TO
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Fx.Facts" + Narvalo.AssemblyInfo.PublicKeySuffix)]
#endif
