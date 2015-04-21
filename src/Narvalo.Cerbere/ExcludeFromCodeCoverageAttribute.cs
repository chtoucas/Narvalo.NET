// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    /// <summary>
    /// Specifies that the attributed code should be excluded from code coverage information.
    /// </summary>
    /// <remarks>PCL shim for the System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute class.</remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
    public sealed class ExcludeFromCodeCoverageAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the justification for the exclusion from code coverage.
        /// </summary>
        /// <value>The justification for the exclusion from code coverage.</value>
        public string Justification { get; set; }
    }
}
