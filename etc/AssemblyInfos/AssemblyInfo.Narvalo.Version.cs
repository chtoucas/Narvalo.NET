using System.Reflection;

//// Assembly Version:
//// - Major Version
//// - Minor Version
//// - Build Number only used if we fix a bug and there is no API change
//// - Revision Number
//// File Version:
//// - First three digits strictly identical to AssemblyVersion
//// - Last digit always automaticaly set to SVN revision
//// Informational Version = Product Version:
//// - First two digits strictly identical to AssemblyVersion
//// - Last one always zero

//#if RELEASE
//[assembly: AssemblyVersion("0.1.1")]
//[assembly: AssemblyFileVersion("0.1.1.0")]
//[assembly: AssemblyInformationalVersion("0.1.1")]
//#else
//[assembly: AssemblyVersion("0.1.1")]
//[assembly: AssemblyFileVersion("0.1.1.0")]
//[assembly: AssemblyInformationalVersion("0.1.1")]
//#endif

// Version utilisée par le runtime.
[assembly: AssemblyVersion("0.1.1.1")]
// Version visible dans l'explorateur.
[assembly: AssemblyFileVersion("0.1.1.1")]
// Version utilisée par NuGet.
[assembly: AssemblyInformationalVersion("0.1.1.1")]
