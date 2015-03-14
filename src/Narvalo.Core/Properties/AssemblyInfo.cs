// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;
using System.Runtime.CompilerServices;
//using System.Security;

[assembly: AssemblyTitle("Narvalo.Core")]
[assembly: AssemblyDescription("")]

//[assembly: AllowPartiallyTrustedCallers]

#if !NO_INTERNALS_VISIBLE_TO
[assembly: InternalsVisibleTo("Narvalo.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
