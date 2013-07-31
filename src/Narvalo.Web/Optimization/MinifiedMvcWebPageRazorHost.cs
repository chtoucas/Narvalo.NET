namespace Narvalo.Web.Optimization
{
    using System.Web.Mvc.Razor;
    using System.Web.Razor.Generator;

    internal class MinifiedMvcWebPageRazorHost : MvcWebPageRazorHost
    {
        public MinifiedMvcWebPageRazorHost(string virtualPath, string physicalPath)
            : base(virtualPath, physicalPath)
        {
        }

        public override RazorCodeGenerator DecorateCodeGenerator(RazorCodeGenerator incomingCodeGenerator)
        {
            if (incomingCodeGenerator is CSharpRazorCodeGenerator) {
                return new MinifiedMvcCSharpRazorCodeGenerator(
                            incomingCodeGenerator.ClassName,
                            incomingCodeGenerator.RootNamespaceName,
                            incomingCodeGenerator.SourceFileName,
                            incomingCodeGenerator.Host);
            }
            //else if (incomingCodeGenerator is VBRazorCodeGenerator) {
            //    return new MinifiedMvcVBRazorCodeGenerator(
            //                incomingCodeGenerator.ClassName,
            //                incomingCodeGenerator.RootNamespaceName,
            //                incomingCodeGenerator.SourceFileName,
            //                incomingCodeGenerator.Host);
            //}
            else {
                return base.DecorateCodeGenerator(incomingCodeGenerator);
            }
        }
    }
}
