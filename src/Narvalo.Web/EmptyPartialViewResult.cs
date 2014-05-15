// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Web.Mvc;

    public sealed class EmptyPartialViewResult : PartialViewResult
    {
        public override void ExecuteResult(ControllerContext context) { }
    }
}