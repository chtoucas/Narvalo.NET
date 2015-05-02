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
     * Narvalo.T4
     * ==========
     *
     * After a succesful build, the dll is copied to `tools\lib\`.
     *
     * The interop library `EnvDTE` must not be embedded into `Narvalo.T4`;
     * otherwise the `DTE` property of `VSTemplate` won't work.
     * By the way, to access this property, you must add the following directive
     * to your template:
     * ```
     * <#@ assembly name="EnvDTE" #>
     * ```
     *
     * References:
     * - [Managing Complexity in T4 Code-Generation Solutions](https://msdn.microsoft.com/en-us/magazine/hh975350.aspx)
     * - [Automation and Extensibility for Visual Studio](https://msdn.microsoft.com/en-us/library/vstudio/xc52cke4(v=vs.100).aspx)
     * - [Oleg Sych Blog](http://www.olegsych.com/)
     * - [T4Toolbox](https://t4toolbox.codeplex.com/)
     * ]]>
     * </content>
     */
    [CompilerGenerated]
    internal static class NamespaceDoc { }
}
