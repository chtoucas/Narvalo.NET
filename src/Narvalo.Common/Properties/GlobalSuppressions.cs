    using System.Diagnostics.CodeAnalysis;

#if NET_40

[assembly: SuppressMessage("Microsoft.Design",
    "CA1020:AvoidNamespacesWithFewTypes",
    Scope = "namespace",
    Target = "System.Diagnostics.Contracts")]

#endif

[assembly: SuppressMessage("Microsoft.Design",
    "CA1006:DoNotNestGenericTypesInMemberSignatures",
    Scope = "namespace",
    Target = "Narvalo.Fx",
    Justification="XXX")]

