// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System.Reflection;

[assembly: AssemblyTitle("Narvalo.Fx")]
[assembly: AssemblyDescription("Narvalo FX Library featuring implementations of functional patterns.")]

#if SECURITY_ANNOTATIONS
[assembly: System.Security.SecurityTransparent]
#endif

#if !NO_INTERNALS_VISIBLE_TO // Make internals visible to the test projects.
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Narvalo.Facts" + Narvalo.Properties.AssemblyInfo.PublicKeySuffix)]
#endif
