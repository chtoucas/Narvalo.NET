// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Web.UI;

    /// <summary>
    /// Provides extension methods for <see cref="ControlBuilder"/>.
    /// </summary>
    public static class ControlBuilderExtensions
    {
        private const BindingFlags BINDING_ATTR
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static ControlBuilder GetDefaultPropertyBuilder(this ControlBuilder @this)
        {
            Require.NotNull(@this, nameof(@this));

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
            Require.NotNull(@this, nameof(@this));

            return (ControlBuilder)@this
                .GetType()
                .GetProperty("ParentBuilder", BINDING_ATTR)
                .GetValue(@this, null /* index */);
        }

        public static ControlBuilder GetRootBuilder(this ControlBuilder @this)
        {
            Require.NotNull(@this, nameof(@this));

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
            Require.NotNull(@this, nameof(@this));

            return (ICollection)@this
                .GetType()
                .GetProperty("SimplePropertyEntries", BINDING_ATTR)
                .GetValue(@this, null /* index */);
        }
    }
}