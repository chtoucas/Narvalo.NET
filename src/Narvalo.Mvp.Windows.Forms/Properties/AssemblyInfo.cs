// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Narvalo.Mvp.Windows.Forms")]
[assembly: AssemblyDescription("")]

[assembly: Guid("33d18069-44db-41d9-b8e0-bfc60bf7713c")]

#if !NO_INTERNALS_VISIBLE_TO // Make internals visible to the test projects.
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Mvp.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
