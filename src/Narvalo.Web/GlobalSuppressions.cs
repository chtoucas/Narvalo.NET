// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#if !NO_GLOBAL_SUPPRESSIONS

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Narvalo.Web.UI",
    Justification = "Hopefully more will come.")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Narvalo.Web.Html",
    Justification = "Hopefully more will come.")]

[assembly: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "Narvalo.Web.Strings_Web",
    Justification = "False positive?")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Narvalo.Web.Strings_Web.#Culture",
    Justification = "False positive?")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Narvalo.Web.Strings_Web.#Culture",
    Justification = "False positive?")]

#endif