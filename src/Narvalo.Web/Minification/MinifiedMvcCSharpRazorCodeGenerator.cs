namespace Narvalo.Web.Minification
{
    using System.CodeDom;
    using System.Web.Mvc.Razor;
    using System.Web.Razor;
    using System.Web.Razor.Generator;
    using System.Web.Razor.Parser.SyntaxTree;
    using System.Web.Razor.Tokenizer.Symbols;

    internal class MinifiedMvcCSharpRazorCodeGenerator : CSharpRazorCodeGenerator
    {
        const string DefaultModelTypeName_ = "dynamic";

        // NB: Cf. MvcCSharpRazorCodeGenerator
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
                foreach (ISymbol item in span.Symbols) {
                    var sym = item as HtmlSymbol;
                    if (sym != null) {
                        builder.Accept(new HtmlSymbol(sym.Start, AdvancedMinify_(sym.Content), sym.Type, sym.Errors));
                    }
                    else {
                        builder.Accept(item);
                    }
                }
                span.ReplaceWith(builder);
            }

            base.VisitSpan(span);
        }

        static string AdvancedMinify_(string literal)
        {
            return Minifier.RemoveWhiteSpaces(literal, MinifyLevel.Advanced);
        }

        void SetBaseType_(string modelTypeName)
        {
            var baseType = new CodeTypeReference(Context.Host.DefaultBaseClass + "<" + modelTypeName + ">");
            Context.GeneratedClass.BaseTypes.Clear();
            Context.GeneratedClass.BaseTypes.Add(baseType);
        }
    }

    //internal class MinifiedMvcCSharpRazorCodeGenerator : MvcCSharpRazorCodeGenerator
    //{
    //    public MinifiedMvcCSharpRazorCodeGenerator(string className, string rootNamespaceName, string sourceFileName, RazorEngineHost host)
    //        : base(className, rootNamespaceName, sourceFileName, host)
    //    {
    //    }

    //    public override void VisitSpan(Span span)
    //    {
    //        Requires.NotNull(span, "span");

    //        if (span.Kind == SpanKind.Markup) {
    //            span.Content = Minifier.RemoveWhiteSpaces(span.Content, MinifyLevel.Advanced);
    //        }

    //        base.VisitSpan(span);
    //    }

    //    //private bool IsDebuggingEnabled() {
    //    //    if (HttpContext.Current != null) {
    //    //        return HttpContext.Current.IsDebuggingEnabled;
    //    //    }

    //    //    string virtualPath = ((WebPageRazorHost)Host).VirtualPath;
    //    //    return ((CompilationSection)WebConfigurationManager.GetSection("system.web/compilation", virtualPath)).Debug;
    //    //}
    //}
}
