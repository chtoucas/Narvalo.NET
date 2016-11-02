// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Narvalo.Mvp")]
[assembly: AssemblyDescription("MVP Framework.")]

[assembly: Guid("65ea83eb-47d3-44c1-8d85-2654954ad0ed")]

#if !NO_INTERNALS_VISIBLE_TO // Make internals visible to the test projects.
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Mvp.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
