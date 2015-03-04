// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public abstract class HtmlControlPageParserFilterBase<TControl> : UnrestrictedPageParserFilter
        where TControl : HtmlControl
    {
        public HtmlControlPageParserFilterBase() : base() { }

        protected abstract bool Enabled { get; }

        protected abstract string TransformLiteral(string literal);

        public override void ParseComplete(ControlBuilder rootBuilder)
        {
            Require.NotNull(rootBuilder, "rootBuilder");

            if (Enabled) {
                TransformRecursively_(rootBuilder);
            }

            base.ParseComplete(rootBuilder);
        }

        void TransformRecursively_(ControlBuilder cb)
        {
            ArrayList subBuilders = cb.SubBuilders;

            for (int i = 0; i < subBuilders.Count; i++) {
                var literal = subBuilders[i] as string;
                if (String.IsNullOrEmpty(literal)) {
                    continue;
                }

                subBuilders[i] = TransformLiteral(literal);
            }

            if (cb.ControlType == typeof(TControl)) {
                foreach (SimplePropertyEntry entry in cb.GetSimplePropertyEntries()) {
                    entry.Value = TransformLiteral(entry.PersistedValue);
                }
            }
            else {
                foreach (object subBuilder in subBuilders) {
                    var controlBuilder = subBuilder as ControlBuilder;
                    if (controlBuilder != null) {
                        TransformRecursively_(controlBuilder);
                    }
                }

                foreach (TemplatePropertyEntry entry in cb.TemplatePropertyEntries) {
                    TransformRecursively_(entry.Builder);
                }

                foreach (ComplexPropertyEntry entry in cb.ComplexPropertyEntries) {
                    TransformRecursively_(entry.Builder);
                }
            }

            ControlBuilder defaultPropertyBuilder = cb.GetDefaultPropertyBuilder();
            if (defaultPropertyBuilder != null) {
                TransformRecursively_(defaultPropertyBuilder);
            }
        }
    }
}
