// Ce code est inspiré de la classe Omari.Web.UI.ControlBuilderExtensions.
// Par rapport au code d'origine, on a effectué les changements suivants :
// - passage en méthodes d'extension ;
// - certaines propriétés étant maintenant publiques, on a supprimé les méthodes
//   permettant d'accéder aux propriétés suivantes : 
//   - ComplexPropertyEntries ;
//   - SubBuilders ;
//   - TemplatePropertyEntries.
// - on préfère conserver la signature d'origine, ie ICollection plutôt que ArrayList ;
// - désactivation des méthodes dont l'utilité ne saute pas aux yeux (en ce qui nous concerne) :
//   - GetParentBuilder() qui donne accès à la propriété interne ParentBuilder ;
//   - GetSimplePropertyEntries() qui donne accès à la propriété interne SimplePropertyEntries ;
//   - GetRootBuilder().
// WARNING: Cette classe dépend de l'API non publique de ControlBuilder.

namespace Narvalo.Web.UI
{
    using System;
    using System.Reflection;
    using System.Web.UI;

    public static class ControlBuilderExtensions
    {
        const BindingFlags BindingAttribute_
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static ControlBuilder GetDefaultPropertyBuilder(this ControlBuilder @this)
        {
            Require.Object(@this);

            PropertyInfo pi = null;
            Type type = @this.GetType();

            while (type != null
                   && (pi = type.GetProperty("DefaultPropertyBuilder", BindingAttribute_)) == null) {
                type = type.BaseType;
            }

            return (ControlBuilder)pi.GetValue(@this, null /* index */);
        }
    }
}