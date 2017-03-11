// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;

[assembly: AssemblyTitle("Narvalo.Core")]
[assembly: AssemblyDescription("Provides helpers on which depend the other Narvalo libraries.")]

#if !NO_INTERNALS_VISIBLE_TO
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Core.Facts" + Narvalo.AssemblyInfo.PublicKeySuffix)]
#endif
