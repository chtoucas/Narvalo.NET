// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    // Cette classe est adaptée de :
    //  http://haacked.com/archive/2009/05/05/page-view-lockdown.aspx

    using System;
    using System.Web.UI;

    public class NoScriptTagPageParserFilter : UnrestrictedPageParserFilter
    {
        public NoScriptTagPageParserFilter() : base() { }

        public override bool ProcessCodeConstruct(CodeConstructType codeType, string code)
        {
            if (codeType == CodeConstructType.ScriptTag) {
                throw new InvalidOperationException("Say NO to server script blocks!");
            }
            return base.ProcessCodeConstruct(codeType, code);
        }
    }
}
