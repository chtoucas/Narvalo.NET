// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    /// <summary>
    /// Decorating a parameter with this attribute informs the Code Analysis tool
    /// that the method is validating the parameter against <see langword="null"/> value.
    /// </summary>
    /// <remarks>
    /// Using the parameter suppresses the CA1062 warning.
    /// <see href="http://geekswithblogs.net/terje/archive/2010/10/14/making-static-code-analysis-and-code-contracts-work-together-or.aspx" />
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
#if CONTRACTS_FULL // Public visibility is mandatory for preconditions in Require.
    // If Code Contracts are enabled, this attribute is used by the Require class for writing preconditions. 
    // Therefore, to be able to perform Static Contracts checkings, this attribute MUST remain public. 
    // Not doing so would lead to a CC1038 error. 
    public sealed class ValidatedNotNullAttribute : Attribute { }
#else
    // This class is of no use outside this assembly. In fact, we do expect to have the CONTRACTS_FULL symbol 
    // defined ONLY when we explicitly run the Code Contracts analysis tools.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Gendarme.Rules.Performance", "AvoidUninstantiatedInternalClassesRule",
        Justification = "[Ignore] Looking at the decompiled source, I can confirm that this attribute is in use (Require.cs). Honestly only the Code Analysis tool will really use of it.")]
    internal sealed class ValidatedNotNullAttribute : Attribute { }
#endif
}
