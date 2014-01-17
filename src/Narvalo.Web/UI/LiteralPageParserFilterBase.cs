// Ce code est inspiré de la classe Omari.Web.UI.WhiteSpaceCleaner.
    
namespace Narvalo.Web.UI
{
    using System.Collections;
    using System.Web.UI;

    public abstract class LiteralPageParserFilterBase : UnrestrictedPageParserFilter
    {
        protected LiteralPageParserFilterBase() : base() { }

        protected abstract bool Enabled { get; }

        public override void ParseComplete(ControlBuilder rootBuilder)
        {
            Requires.NotNull(rootBuilder, "rootBuilder");

            if (Enabled) {
                TransformRecursively_(rootBuilder);
            }

            base.ParseComplete(rootBuilder);
        }

        protected abstract string TransformLiteral(string literal);

        void TransformRecursively_(ControlBuilder controlBuilder)
        {
            ArrayList subBuilders = controlBuilder.SubBuilders;

            for (int i = 0; i < subBuilders.Count; i++) {
                var subBuilder = subBuilders[i];

                var literal = subBuilder as string;
                if (literal != null) {
                    if (literal.Length != 0) {
                        subBuilders[i] = TransformLiteral(literal);
                    }

                    continue;
                }

                var controlSubBuilder = subBuilder as ControlBuilder;
                if (controlSubBuilder != null) {
                    TransformRecursively_(controlSubBuilder);
                }
            }

            foreach (TemplatePropertyEntry entry in controlBuilder.TemplatePropertyEntries) {
                TransformRecursively_(entry.Builder);
            }

            foreach (ComplexPropertyEntry entry in controlBuilder.ComplexPropertyEntries) {
                TransformRecursively_(entry.Builder);
            }

            ControlBuilder defaultPropertyBuilder = controlBuilder.GetDefaultPropertyBuilder();
            if (defaultPropertyBuilder != null) {
                TransformRecursively_(defaultPropertyBuilder);
            }
        }
    }
}
