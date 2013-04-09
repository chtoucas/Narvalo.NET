namespace Narvalo.Web.Internal
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Web.UI;
    using Narvalo.Diagnostics;

    // This class relies on the internal implementation details of ASP.NET.
    // Tested for ASP.NET 2.0, 3.5 , 4.0
    public static class ControlBuilderExtensions
    {
        const BindingFlags InstPubNonpub_ 
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static ArrayList GetComplexPropertyEntries(this ControlBuilder controlBuilder)
        {
            Requires.NotNull(controlBuilder);

            ICollection cpes = (ICollection)
                controlBuilder
                    .GetType()
                    .GetProperty("ComplexPropertyEntries", InstPubNonpub_)
                    .GetValue(controlBuilder, null /* index */);

            if (cpes == null || cpes.Count == 0) {
                return new ArrayList(0);
            }
            else {
                return (ArrayList)cpes;
            }
        }

        public static ControlBuilder GetDefaultPropertyBuilder(this ControlBuilder controlBuilder)
        {
            Requires.NotNull(controlBuilder);

            PropertyInfo pi = null;
            Type type = controlBuilder.GetType();

            while (type != null
                   && (pi = type.GetProperty("DefaultPropertyBuilder", InstPubNonpub_)) == null) {
                type = type.BaseType;
            }

            return (ControlBuilder)pi.GetValue(controlBuilder, null /* index */);
        }

        public static ArrayList GetSimplePropertyEntries(this ControlBuilder controlBuilder)
        {
            Requires.NotNull(controlBuilder);

            ICollection cpes = (ICollection)
                controlBuilder
                    .GetType()
                    .GetProperty("SimplePropertyEntries", InstPubNonpub_)
                    .GetValue(controlBuilder, null /* index */);

            if (cpes == null || cpes.Count == 0) {
                return new ArrayList(0);
            }
            else {
                return (ArrayList)cpes;
            }
        }

        public static ArrayList GetChildBuilders(this ControlBuilder controlBuilder)
        {
            Requires.NotNull(controlBuilder);

            return (ArrayList)
                controlBuilder
                   .GetType()
                   .GetProperty("SubBuilders", InstPubNonpub_)
                   .GetValue(controlBuilder, null /* index */);
        }

        public static ArrayList GetTemplatePropertyEntries(this ControlBuilder controlBuilder)
        {
            Requires.NotNull(controlBuilder);

            ICollection tpes = (ICollection)
                controlBuilder
                    .GetType()
                    .GetProperty("TemplatePropertyEntries", InstPubNonpub_)
                    .GetValue(controlBuilder, null /* index */);

            if (tpes == null || tpes.Count == 0) {
                return new ArrayList(0);
            }
            else {
                return (ArrayList)tpes;
            }
        }

        public static ControlBuilder GetParentBuilder(this ControlBuilder controlBuilder)
        {
            Requires.NotNull(controlBuilder);

            return (ControlBuilder)
                controlBuilder
                   .GetType()
                   .GetProperty("ParentBuilder", InstPubNonpub_)
                   .GetValue(controlBuilder, null /* index */);
        }

        public static ControlBuilder GetRootBuilder(this ControlBuilder controlBuilder)
        {
            Requires.NotNull(controlBuilder);

            while (GetParentBuilder(controlBuilder) != null) {
                controlBuilder = GetParentBuilder(controlBuilder);
            }

            return controlBuilder;
        }
    }
}