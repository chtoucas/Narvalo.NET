namespace Narvalo.Web.Optimization
{
    // Cette classe est inspirée de MvcCSharpRazorCodeGenerator.
    // Il faut donc synchroniser celle-ci à chaque nouvelle version de ASP.NET MVC.

    using System.CodeDom;
    using System.Web.Mvc.Razor;
    using System.Web.Razor;
    using System.Web.Razor.Generator;
    using System.Web.Razor.Parser.SyntaxTree;

    public class WhiteSpaceBusterMvcCSharpRazorCodeGenerator : CSharpRazorCodeGenerator
    {
        readonly RazorOptimizer _optimizer;

        public WhiteSpaceBusterMvcCSharpRazorCodeGenerator(
            string className,
            string rootNamespaceName,
            string sourceFileName,
            RazorEngineHost host,
            RazorOptimizer optimizer)
            : base(className, rootNamespaceName, sourceFileName, host)
        {
            _optimizer = optimizer;

            var mvcHost = host as MvcWebPageRazorHost;

            // À la différence de MvcCSharpRazorCodeGenerator, on n'a pas besoin de vérifier
            // host.IsSpecialPage car on exclut ce type de hôte en amont au niveau de 
            // WhiteSpaceBusterMvcWebRazorHostFactory.
            if (mvcHost != null) {
                SetBaseType_("dynamic");
            }
        }

        public override void VisitSpan(Span span)
        {
            Requires.NotNull(span, "span");

            if (span.Kind == SpanKind.Markup) {
                _optimizer.OptimizeSpan(span);
            }

            base.VisitSpan(span);
        }

        void SetBaseType_(string modelTypeName)
        {
            var baseType = new CodeTypeReference(Context.Host.DefaultBaseClass + "<" + modelTypeName + ">");
            Context.GeneratedClass.BaseTypes.Clear();
            Context.GeneratedClass.BaseTypes.Add(baseType);
        }
    }
}
