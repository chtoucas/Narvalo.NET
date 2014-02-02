namespace Narvalo.Web.Optimization
{
    using System;
    using System.Web.Razor.Parser;
    using System.Web.Razor.Parser.SyntaxTree;
    using System.Web.Razor.Text;

    public sealed class WhiteSpaceBusterHtmlMarkupParser : ParserBase
    {
        readonly RazorOptimizer _optimizer;
        readonly ParserBase _inner;

        public WhiteSpaceBusterHtmlMarkupParser(ParserBase inner, RazorOptimizer optimizer)
        {
            _inner = inner;
            _optimizer = optimizer;
        }

        protected override ParserBase OtherParser
        {
            get { return Context.CodeParser; }
        }

        public override void BuildSpan(SpanBuilder span, SourceLocation start, string content)
        {
            _inner.BuildSpan(span, start, content);
        }

        public override void ParseBlock()
        {
            _inner.ParseBlock();
        }

        public override void ParseDocument()
        {
            // On initialise le contexte avant de lancer la phase d'analyse.
            _inner.Context = Context;
            _inner.ParseDocument();

            _optimizer.OptimizeBlock(Context.CurrentBlock);
        }

        public override void ParseSection(Tuple<string, string> nestingSequences, bool caseSensitive)
        {
            _inner.ParseSection(nestingSequences, caseSensitive);
        }
    }
}
