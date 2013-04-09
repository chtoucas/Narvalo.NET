namespace Narvalo.Web.UI
{
    using System;
    using System.Web.UI;

    // See: http://haacked.com/archive/2009/05/05/page-view-lockdown.aspx
    public class NoScriptTagParserFilter : UnrestrictedParserFilterBase
    {
        public override bool ProcessCodeConstruct(CodeConstructType codeType, string code)
        {
            if (codeType == CodeConstructType.ScriptTag) {
                throw new InvalidOperationException("Say NO to server script blocks!");
            }
            return base.ProcessCodeConstruct(codeType, code);
        }
    }
}
