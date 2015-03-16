// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System.Collections;
    using System.Reflection;
    using System.Web.UI;

    public static class ControlBuilderExtensions
    {
        const BindingFlags BindingAttr_
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static ControlBuilder GetParentBuilder(this ControlBuilder @this)
        {
            Require.Object(@this);

            return (ControlBuilder)@this
                .GetType()
                .GetProperty("ParentBuilder", BindingAttr_)
                .GetValue(@this, null /* index */);
        }

        public static ICollection GetSimplePropertyEntries(this ControlBuilder @this)
        {
            Require.Object(@this);

            return (ICollection)@this
                .GetType()
                .GetProperty("SimplePropertyEntries", BindingAttr_)
                .GetValue(@this, null /* index */);
        }

        public static ControlBuilder GetRootBuilder(this ControlBuilder @this)
        {
            Require.Object(@this);

            ControlBuilder result = null;

            while (true)
            {
                var parentBuilder = @this.GetParentBuilder();
                if (parentBuilder == null)
                {
                    break;
                }
                result = parentBuilder;
            }

            return result;
        }
    }
}
