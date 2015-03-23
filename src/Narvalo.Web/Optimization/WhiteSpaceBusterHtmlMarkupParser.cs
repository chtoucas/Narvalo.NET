// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
#if CONTRACTS_FULL // [Intentionally] Using directive.
    using System.Diagnostics.Contracts;
#endif
    using System.Web.Razor.Parser;
    using System.Web.Razor.Parser.SyntaxTree;
    using System.Web.Razor.Text;

    public sealed class WhiteSpaceBusterHtmlMarkupParser : ParserBase
    {
        private readonly RazorOptimizer _optimizer;
        private readonly ParserBase _inner;

        public WhiteSpaceBusterHtmlMarkupParser(ParserBase inner, RazorOptimizer optimizer)
        {
            Require.NotNull(inner, "inner");
            Require.NotNull(optimizer, "optimizer");

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

#if CONTRACTS_FULL // [Ignore] Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_inner != null);
            Contract.Invariant(_optimizer != null);
        }

#endif
    }
}
