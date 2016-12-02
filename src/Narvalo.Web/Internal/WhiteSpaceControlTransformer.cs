// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

// Ce code est inspiré de la classe Omari.Web.UI.WhiteSpaceCleaner.

namespace Narvalo.Web.Internal
{
    using System.Collections;
    using System.Diagnostics.Contracts;
    using System.Web.UI;

    using Narvalo.Web.Optimization;
    using Narvalo.Web.UI;

    internal sealed class WhiteSpaceControlTransformer
    {
        private readonly IWhiteSpaceBuster _buster;

        public WhiteSpaceControlTransformer(IWhiteSpaceBuster buster)
        {
            Demand.NotNull(buster);

            _buster = buster;
        }

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

        private string TransformLiteral(string value)
        {
            return _buster.Bust(value);
        }
    }
}
