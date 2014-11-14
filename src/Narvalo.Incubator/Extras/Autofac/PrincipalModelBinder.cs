using Autofac.Integration.Mvc;

namespace Narvalo.Extras.Autofac
{
    using System.Security.Principal;
    using System.Web.Mvc;
    using Narvalo;

    [ModelBinderType(typeof(IPrincipal))]
    public class PrincipalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            Require.NotNull(controllerContext, "controllerContext");

            return controllerContext.HttpContext.User;
        }
    }
}
