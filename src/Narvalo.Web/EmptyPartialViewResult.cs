namespace Narvalo.Web
{
    using System.Web.Mvc;

    public sealed class EmptyPartialViewResult : PartialViewResult
    {
        public override void ExecuteResult(ControllerContext context) { }
    }
}