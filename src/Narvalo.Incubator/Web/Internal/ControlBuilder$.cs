namespace Narvalo.Web.Internal
{
    using System.Collections;
    using System.Reflection;
    using System.Web.UI;

    /// <summary />
    /// <see cref="Narvalo.Web.UI.Extensions.ControlBuilderExtensions"/>.
    static class ControlBuilderFurtherExtensions
    {
        const BindingFlags BindingAttr_
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static ControlBuilder GetParentBuilder(this ControlBuilder @this)
        {
            Requires.Object(@this);

            return (ControlBuilder)@this
                .GetType()
                .GetProperty("ParentBuilder", BindingAttr_)
                .GetValue(@this, null /* index */);
        }

        public static ICollection GetSimplePropertyEntries(this ControlBuilder @this)
        {
            Requires.Object(@this);

            return (ICollection)@this
                .GetType()
                .GetProperty("SimplePropertyEntries", BindingAttr_)
                .GetValue(@this, null /* index */);
        }

        public static ControlBuilder GetRootBuilder(this ControlBuilder @this)
        {
            Requires.Object(@this);

            ControlBuilder result = null;

            while (true) {
                var parentBuilder = @this.GetParentBuilder();
                if (parentBuilder == null) {
                    break;
                }
                result = parentBuilder;
            }

            return result;
        }
    }
}