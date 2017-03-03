// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Web.Razor.Parser.SyntaxTree;
    using System.Web.Razor.Text;
    using System.Web.Razor.Tokenizer.Symbols;

    public sealed partial class RazorOptimizer
    {
        private readonly IWhiteSpaceBuster _buster;

        public RazorOptimizer(IWhiteSpaceBuster buster)
        {
            Require.NotNull(buster, nameof(buster));

            _buster = buster;
        }

        public void OptimizeBlock(BlockBuilder block)
        {
            Require.NotNull(block, nameof(block));

            for (int i = 0; i < block.Children.Count; i++)
            {
                var span = block.Children[i] as Span;

                if (!IsMarkup(span))
                {
                    // On ne touche pas les éléments qui ne sont pas de type Markup.
                    continue;
                }

                // Si on est déjà arrivé à la dernière position, on récupère le contenu directement.
                string content
                    = i == block.Children.Count - 1
                    ? span.Content
                    : RemoveMarkupAndMergeContentAfter(block, span.Content, ref i);

                if (String.IsNullOrWhiteSpace(content))
                {
                    // Le contenu n'est constitué que d'espaces blancs.
                    block.Children.RemoveAt(i);
                    continue;
                }

                // On optimise le contenu et on recrée l'élément.
                var builder = new SpanBuilder(span);
                builder.ClearSymbols();

                // FIXME: On perd toute information contextuelle.
                builder.Accept(new Symbol_ { Content = BustWhiteSpaces(content) });
                span.ReplaceWith(builder);
            }
        }

        public void OptimizeSpan(Span span)
        {
            Require.NotNull(span, nameof(span));

            var builder = new SpanBuilder(span);
            builder.ClearSymbols();

            var prevType = HtmlSymbolType.Unknown;

            foreach (ISymbol item in span.Symbols)
            {
                var sym = item as HtmlSymbol;

                if (sym == null)
                {
                    // On ne touche pas les éléments qui ne sont pas de type HtmlSymbol.
                    builder.Accept(item);
                    continue;
                }

                // FIXME: On perd toute information contextuelle. Peut-être pour remédier à ce problème
                // on pourrait ré-utiliser un HtmlSymbol.
                builder.Accept(new Symbol_ { Content = OptimizeContent(sym, prevType) });
                prevType = sym.Type;
            }

            span.ReplaceWith(builder);
        }

        private static bool IsMarkup(Span span)
        {
            return span != null && span.Kind == SpanKind.Markup;
        }

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "[Intentionally] The use of a parameter passed by reference simplifies the algorithm. Only used privately.")]
        private static string RemoveMarkupAndMergeContentAfter(BlockBuilder block, string content, ref int currentIndex)
        {
            Demand.NotNull(block);

            var sb = new StringBuilder(content);

            // WARNING: On est succeptible de modifier la collection currentBlock.Children,
            // on ne peut donc pas mettre en cache la valeur de currentBlock.Children.Count.
            var j = currentIndex + 1;

            while (j < block.Children.Count)
            {
                var nextSpan = block.Children[j] as Span;

                // On s'arrête dès qu'on rencontre un Span qui n'est pas de type Markup.
                if (!IsMarkup(nextSpan))
                {
                    break;
                }

                // On ajoute le contenu de l'élément si il n'est pas vide.
                var nextContent = nextSpan.Content;
                if (!String.IsNullOrWhiteSpace(nextContent))
                {
                    sb.Append(nextContent);
                }

                // On supprime l'élément.
                block.Children.RemoveAt(j);

                // On incrémente la position.
                currentIndex = j;
                j++;
            }

            return sb.ToString();
        }

        private string BustWhiteSpaces(string content)
        {
            return _buster.Bust(content);
        }

        private string OptimizeContent(SymbolBase<HtmlSymbolType> sym, HtmlSymbolType previousType)
        {
            Demand.NotNull(sym);

            string content;

            if (sym.Type == HtmlSymbolType.WhiteSpace && previousType == HtmlSymbolType.NewLine)
            {
                // Si le symbole n'est constitué que d'espace blancs et le symbole
                // précédent est un retour à la ligne, on peut vider son contenu.
                content = String.Empty;
            }
            else
            {
                content = BustWhiteSpaces(sym.Content);
            }

            return content;
        }

        private class Symbol_ : ISymbol
        {
            private string _content;
            private SourceLocation _start = SourceLocation.Zero;

            public string Content
            {
                get { return _content; }
                set { _content = value; }
            }

            public SourceLocation Start { get { return _start; } }

            public void ChangeStart(SourceLocation newStart)
            {
                _start = newStart;
            }

            public void OffsetStart(SourceLocation documentStart)
            {
                _start = documentStart;
            }
        }
    }
}
