namespace Narvalo.Web.Optimization
{
    using System;
    using System.CodeDom;
    using System.Text.RegularExpressions;
    using System.Web.Mvc.Razor;
    using System.Web.Razor;
    using System.Web.Razor.Generator;
    using System.Web.Razor.Parser.SyntaxTree;
    using System.Web.Razor.Tokenizer.Symbols;

    // NB: Cf. MvcCSharpRazorCodeGenerator
    internal class MinifiedMvcCSharpRazorCodeGenerator : CSharpRazorCodeGenerator
    {
        const string DefaultModelTypeName_ = "dynamic";

        static readonly Regex MutipleWhitespacesRegex_ = new Regex(@"\s{2,}", RegexOptions.Compiled);

        public MinifiedMvcCSharpRazorCodeGenerator(string className, string rootNamespaceName, string sourceFileName, RazorEngineHost host)
            : base(className, rootNamespaceName, sourceFileName, host)
        {
            var mvcHost = host as MvcWebPageRazorHost;
            if (mvcHost != null && !mvcHost.IsSpecialPage) {
                // set the default model type to "dynamic" (Dev10 bug 935656)
                // don't set it for "special" pages (such as "_viewStart.cshtml")
                SetBaseType_(DefaultModelTypeName_);
            }
        }

        public override void VisitSpan(Span span)
        {
            Requires.NotNull(span, "span");

            if (span.Kind == SpanKind.Markup) {
                var builder = new SpanBuilder(span);
                builder.ClearSymbols();
                var prevType = HtmlSymbolType.Unknown;

                foreach (ISymbol item in span.Symbols) {
                    var sym = item as HtmlSymbol;
                    if (sym != null) {
                        if (IsIrrelevant_(sym, prevType)) {
                            builder.Accept(new HtmlSymbol(sym.Start, String.Empty, sym.Type, sym.Errors));
                        }
                        else {
                            builder.Accept(new HtmlSymbol(sym.Start, MinifyContent_(sym.Content), sym.Type, sym.Errors));
                        }
                        prevType = sym.Type;
                    }
                    else {
                        builder.Accept(item);
                    }
                }
                span.ReplaceWith(builder);
            }

            base.VisitSpan(span);
        }

        static bool IsIrrelevant_(HtmlSymbol sym, HtmlSymbolType prevType)
        {
            return sym.Type == HtmlSymbolType.NewLine
                || (sym.Type == HtmlSymbolType.WhiteSpace && prevType == HtmlSymbolType.NewLine);
        }

        static string MinifyContent_(string literal)
        {
            return MutipleWhitespacesRegex_.Replace(literal, " ");
        }

        void SetBaseType_(string modelTypeName)
        {
            var baseType = new CodeTypeReference(Context.Host.DefaultBaseClass + "<" + modelTypeName + ">");
            Context.GeneratedClass.BaseTypes.Clear();
            Context.GeneratedClass.BaseTypes.Add(baseType);
        }
    }
}
