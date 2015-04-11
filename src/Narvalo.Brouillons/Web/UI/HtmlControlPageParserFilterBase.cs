﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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

            if (Enabled)
            {
                TransformRecursively_(rootBuilder);
            }

            base.ParseComplete(rootBuilder);
        }

        private void TransformRecursively_(ControlBuilder controlBuilder)
        {
            var isControl = controlBuilder.ControlType == typeof(TControl);

            ArrayList subBuilders = controlBuilder.SubBuilders;

            for (int i = 0; i < subBuilders.Count; i++)
            {
                var subBuilder = subBuilders[i];
                var literal = subBuilder as string;

                if (literal == null)
                {
                    // Transform the sub builders.
                    if (isControl)
                    {
                        continue;
                    }

                    var controlSubBuilder = subBuilder as ControlBuilder;
                    if (controlSubBuilder != null)
                    {
                        TransformRecursively_(controlSubBuilder);
                    }
                }
                else
                {
                    // Transform the literal.
                    if (literal.Length != 0)
                    {
                        subBuilders[i] = TransformLiteral(literal);
                    }
                }
            }

            if (isControl)
            {
                foreach (SimplePropertyEntry entry in controlBuilder.GetSimplePropertyEntries())
                {
                    entry.Value = TransformLiteral(entry.PersistedValue);
                }
            }
            else
            {
                foreach (TemplatePropertyEntry entry in controlBuilder.TemplatePropertyEntries)
                {
                    TransformRecursively_(entry.Builder);
                }

                foreach (ComplexPropertyEntry entry in controlBuilder.ComplexPropertyEntries)
                {
                    TransformRecursively_(entry.Builder);
                }
            }

            ControlBuilder defaultPropertyBuilder = controlBuilder.GetDefaultPropertyBuilder();
            if (defaultPropertyBuilder != null)
            {
                TransformRecursively_(defaultPropertyBuilder);
            }
        }
    }
}
