// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.T4
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The <c>Narvalo.T4</c> namespace contains classes dealing with Text Templating.
    /// </summary>
    /**
     * <content markup="commonmark">
     * <![CDATA[
     * Narvalo.StyleCop
     * ================
     *
     * - After a succesful build, the dll is copied to `tools\lib\StyleCop`.
     * This operation might fail if Source Analysis has been performed before (it locks the dll).
     * - The assembly is called Narvalo.StyleCop but the default namespace is Narvalo.
     * When displaying warnings, the default behaviour of StyleCop is to use the part
     * after StyleCop in the namespace if any and to remove the "Rules" from the name of the
     * analyzer type. For instance:
     * * `Narvalo.StyleCop.CSharp.NamingRules` -> `CSharp.NamingRules`
     * * `Narvalo.StyleCop.NamingRules` -> `Naming`
     * * `Narvalo.NamingRules` -> `Narvalo.`
     * * `Narvalo.Rules` -> `Narvalo.Rules`
     *
     * We use: `Narvalo.CSharpRules` -> `Narvalo.CSharp`
     * ]]>
     * </content>
     */
    [CompilerGenerated]
    internal static class NamespaceDoc { }
}
