﻿namespace Narvalo.Web
{
    using System.Web.Mvc;

    public class EmptyPartialViewResult : PartialViewResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            ;
        }
    }
}