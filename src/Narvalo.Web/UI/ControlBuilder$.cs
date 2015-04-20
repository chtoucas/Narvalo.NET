// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Web.UI;

    /// <summary>
    /// Provides extension methods for <see cref="ControlBuilder"/>.
    /// </summary>
    /**
     * <content markup="commonmark">
     * <![CDATA[
     * Ce code est inspiré de la classe `Omari.Web.UI.ControlBuilderExtensions`.
     * Par rapport au code d'origine, on a effectué les changements suivants :
     * - passage en méthodes d'extension ;
     * - certaines propriétés étant maintenant publiques, on a supprimé les méthodes
     * permettant d'accéder aux propriétés suivantes : 
     * - `ComplexPropertyEntries` ;
     * - `SubBuilders` ;
     * - `TemplatePropertyEntries`.
     * - on préfère conserver la signature d'origine, ie `ICollection` plutôt que `ArrayList` ;
     * - désactivation des méthodes dont l'utilité ne saute pas aux yeux (en ce qui nous concerne) :
     * - `GetParentBuilder()` qui donne accès à la propriété interne `ParentBuilder` ;
     * - `GetSimplePropertyEntries()` qui donne accès à la propriété interne `SimplePropertyEntries` ;
     * - `GetRootBuilder()`.
     * WARNING: Cette classe dépend de l'API non publique de `ControlBuilder`.
     * ]]>
     * </content>
     */
    public static class ControlBuilderExtensions
    {
        private const BindingFlags BINDING_ATTR
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static ControlBuilder GetDefaultPropertyBuilder(this ControlBuilder @this)
        {
            Require.Object(@this);

            PropertyInfo pi = null;
            Type type = @this.GetType();

            while (type != null
                   && (pi = type.GetProperty("DefaultPropertyBuilder", BINDING_ATTR)) == null)
            {
                type = type.BaseType;
            }

            return (ControlBuilder)pi.GetValue(@this, index: null);
        }
        public static ControlBuilder GetParentBuilder(this ControlBuilder @this)
        {
            Require.Object(@this);

            return (ControlBuilder)@this
                .GetType()
                .GetProperty("ParentBuilder", BINDING_ATTR)
                .GetValue(@this, null /* index */);
        }

        public static ControlBuilder GetRootBuilder(this ControlBuilder @this)
        {
            Require.Object(@this);

            ControlBuilder retval = null;

            while (true)
            {
                var parentBuilder = @this.GetParentBuilder();
                if (parentBuilder == null)
                {
                    break;
                }
                retval = parentBuilder;
            }

            return retval;
        }

        public static ICollection GetSimplePropertyEntries(this ControlBuilder @this)
        {
            Require.Object(@this);

            return (ICollection)@this
                .GetType()
                .GetProperty("SimplePropertyEntries", BINDING_ATTR)
                .GetValue(@this, null /* index */);
        }
    }
}