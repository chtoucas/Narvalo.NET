// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
//using System.Security;

[assembly: AssemblyTitle("Narvalo.Common")]
[assembly: AssemblyDescription("")]

[assembly: Guid("8cd3c522-030f-49b4-bd87-285e2b35425b")]

//[assembly: SecurityRules(SecurityRuleSet.Level2)]
//[assembly: AllowPartiallyTrustedCallers]

#if !NO_INTERNALS_VISIBLE_TO
[assembly: InternalsVisibleTo("Narvalo.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif