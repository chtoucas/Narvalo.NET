// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;

[assembly: AssemblyTitle("Narvalo.Money")]
[assembly: AssemblyDescription("Narvalo Money Library.")]

#if SECURITY_ANNOTATIONS
[assembly: System.Security.SecurityTransparent]
#endif

#if !NO_INTERNALS_VISIBLE_TO
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Money.Facts" + Narvalo.AssemblyInfo.PublicKeySuffix)]
#endif
