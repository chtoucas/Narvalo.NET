namespace Narvalo.Web.UI
{
    // Ce code est inspiré de la classe Omari.Web.UI.WhiteSpaceCleaner.

    using System.Collections;
    using System.Web.UI;
    using Narvalo.Web.Internal;

    public abstract class LiteralPageParserFilterBase : UnrestrictedPageParserFilter
    {
        protected LiteralPageParserFilterBase() : base() { }

        protected abstract bool Enabled { get; }

        protected abstract string TransformLiteral(string literal);

        public override void ParseComplete(ControlBuilder rootBuilder)
        {
            Requires.NotNull(rootBuilder, "rootBuilder");

            if (Enabled) {
                TransformRecursively_(rootBuilder);
            }

            base.ParseComplete(rootBuilder);
        }

        void TransformRecursively_(ControlBuilder cb)
        {
            ArrayList subBuilders = cb.SubBuilders;

            for (int i = 0; i < subBuilders.Count; i++) {
                var subBuilder = subBuilders[i];

                var literal = subBuilder as string;
                if (literal != null) {
                    if (literal.Length != 0) {
                        subBuilders[i] = TransformLiteral(literal);
                    }
                    continue;
                }

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

            ControlBuilder defaultPropertyBuilder = cb.GetDefaultPropertyBuilder();
            if (defaultPropertyBuilder != null) {
                TransformRecursively_(defaultPropertyBuilder);
            }
        }
    }
}
