namespace Narvalo.Web.Minification
{
    using System.Web.Mvc.Razor;
    using System.Web.Razor;
    using System.Web.Razor.Parser.SyntaxTree;

    internal class MinifiedMvcVBRazorCodeGenerator : MvcVBRazorCodeGenerator
    {
        public MinifiedMvcVBRazorCodeGenerator(string className, string rootNamespaceName, string sourceFileName, RazorEngineHost host)
            : base(className, rootNamespaceName, sourceFileName, host)
        {
        }

        public override void VisitSpan(Span span)
        {
            Requires.NotNull(span, "span");

            if (span.Kind == SpanKind.Markup) {
                span.Content = Minifier.RemoveWhiteSpaces(span.Content, MinifyLevel.Advanced);
            }

            base.VisitSpan(span);
        }

        //private bool IsDebuggingEnabled() {
        //    if (HttpContext.Current != null) {
        //        return HttpContext.Current.IsDebuggingEnabled;
        //    }

        //    string virtualPath = ((WebPageRazorHost)Host).VirtualPath;
        //    return ((CompilationSection)WebConfigurationManager.GetSection("system.web/compilation", virtualPath)).Debug;
        //}
    }
}
