// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Collections;
    using System.Web.UI;

    // Ce code est inspiré de la classe Omari.Web.UI.WhiteSpaceCleaner.
    internal abstract class LiteralControlTransformer
    {
        protected LiteralControlTransformer() { }

        protected abstract bool AllowControl(Type controlType);

        protected abstract string TransformLiteral(string literal);

        public void TransformRecursively(ControlBuilder controlBuilder)
        {
            Require.NotNull(controlBuilder, "controlBuilder");

            ArrayList subBuilders = controlBuilder.SubBuilders;

            for (int i = 0; i < subBuilders.Count; i++)
            {
                var subBuilder = subBuilders[i];
                var literal = subBuilder as string;

                if (literal == null)
                {
                    // Transform the sub builders.
                    var controlSubBuilder = subBuilder as ControlBuilder;
                    if (controlSubBuilder != null)
                    {
                        TransformRecursively(controlSubBuilder);
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

            foreach (TemplatePropertyEntry entry in controlBuilder.TemplatePropertyEntries)
            {
                TransformRecursively(entry.Builder);
            }

            foreach (ComplexPropertyEntry entry in controlBuilder.ComplexPropertyEntries)
            {
                TransformRecursively(entry.Builder);
            }

            ControlBuilder defaultPropertyBuilder = controlBuilder.GetDefaultPropertyBuilder();
            if (defaultPropertyBuilder != null)
            {
                TransformRecursively(defaultPropertyBuilder);
            }
        }
    }
}
