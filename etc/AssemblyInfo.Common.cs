// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[module: SuppressMessage("Microsoft.Usage", "CA2243:AttributeStringLiteralsShouldParseCorrectly", Justification = "Informational version uses semantic versioning.")]

[assembly: AssemblyCompany("Narvalo.Org - http://narvalo.org")]
[assembly: AssemblyCopyright("Copyright © 2010-2014 Narvalo.Org")]
[assembly: AssemblyTrademark("")]

[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en-US")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]

// Versioning objectives:
// - AssemblyVersion, the one used by the CLR should be of the form
//   MAJOR.MINOR.PATCH
// - AssemblyFileVersion, auto-generated, only used to make sure we have
//   a unique version per build.
//   MAJOR.MINOR.BUILD.0
// - AssemblyInformationalVersion, public face of the assembly. We will use
//   it for NuGet package versioning.
//   MAJOR.MINOR.PATCH(-XXX)
//
// We don't mind if the version does not change when building inside Visual Studio.
//
// NARVALO_CORE and NARVALO_MVP are defined in the csproj.
#if NARVALO_CORE && NARVALO_MVP
#error You cannot define both NARVALO_CORE and NARVALO_MVP.
#elif NARVALO_CORE

[assembly: AssemblyProduct("Narvalo.Org Core Libraries.")]

#if CONTINUOUS_BUILD
[assembly: AssemblyVersion("0.26.*")]
[assembly: AssemblyFileVersion("0.26.0.0")]
[assembly: AssemblyInformationalVersion("0.26.0-alpha")]
#else
[assembly: AssemblyVersion("0.26.0.0")]
[assembly: AssemblyFileVersion("0.26.0.0")]
[assembly: AssemblyInformationalVersion("0.26.0-alpha")]
#endif

#if !NO_INTERNALS_VISIBLE_TO
[assembly: InternalsVisibleTo("Narvalo.Facts, PublicKey=0024000004800000940000000602000000240000525341310004000001000100d111d3d81f8e971287763b323df0fb3b122b246eb7ad1ef9f7af2bb83241fc5978c06972b09b4b7e9e68f0be4154ff3cc1e08eb22b98b0914f2b0aac326f2354e73a85e4738db0250b4123d0159b499c834ca7f11f3c717defb790df6d024535aa12d63c31d5f6a68b7f26d12f094547bbb84781e8939f1c64c4c90a3ebe0690")]
#endif

#elif NARVALO_MVP

[assembly: AssemblyProduct("Narvalo.Org MVP Libraries.")]

[assembly: AssemblyVersion("0.99.0.0")]
[assembly: AssemblyFileVersion("0.99.0.0")]
[assembly: AssemblyInformationalVersion("0.99.0-beta")]

#if !NO_INTERNALS_VISIBLE_TO
[assembly: InternalsVisibleTo("Narvalo.Mvp.Facts, PublicKey=0024000004800000940000000602000000240000525341310004000001000100d111d3d81f8e971287763b323df0fb3b122b246eb7ad1ef9f7af2bb83241fc5978c06972b09b4b7e9e68f0be4154ff3cc1e08eb22b98b0914f2b0aac326f2354e73a85e4738db0250b4123d0159b499c834ca7f11f3c717defb790df6d024535aa12d63c31d5f6a68b7f26d12f094547bbb84781e8939f1c64c4c90a3ebe0690")]
#endif

#else

[assembly: AssemblyProduct("Narvalo.Org Libraries.")]

#endif
