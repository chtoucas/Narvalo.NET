// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Decorating a parameter with this attribute informs Code Analysis
    /// that the method is validating the parameter for <see langword="null"/>.
    /// This will silence the CA1062 warning.
    /// <seealso href="http://geekswithblogs.net/terje/archive/2010/10/14/making-static-code-analysis-and-code-contracts-work-together-or.aspx" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    [SuppressMessage("Gendarme.Rules.Performance", "AvoidUninstantiatedInternalClassesRule",
        Justification = "This method is used by the Code Analysis tools.")]
#if CONTRACTS_FULL
    // This attribute is only used by the Require class. When performing Static Contracts checking, 
    // this attribute MUST remain public. This should silence the CC1038 error.
    public
#else
    internal
#endif
    sealed class ValidatedNotNullAttribute : Attribute { }
}
