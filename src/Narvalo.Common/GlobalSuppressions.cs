// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#if !NO_GLOBAL_SUPPRESSIONS

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Narvalo.Fx.Extensions",
    Justification = "Complements the namespace from Narvalo.Core.")]

#endif