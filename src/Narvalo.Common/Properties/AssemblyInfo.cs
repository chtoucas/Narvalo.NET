// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("Narvalo.Common")]
[assembly: AssemblyDescription("Narvalo Common Library containing mostly extension methods for classes from the BCL and few goodies.")]

[assembly: Guid("8cd3c522-030f-49b4-bd87-285e2b35425b")]

[assembly: AllowPartiallyTrustedCallers]

#if !NO_INTERNALS_VISIBLE_TO // Make internals visible to the test projects.
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
