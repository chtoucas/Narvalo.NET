using Autofac.Integration.Mvc;

namespace Narvalo.Web
{
    using System;
    using System.Security.Principal;
    using System.Web.Mvc;

    [ModelBinderType(typeof(IPrincipal))]
    public class PrincipalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            if (controllerContext == null) {
                throw new ArgumentNullException("controllerContext");
            }

            if (bindingContext == null) {
                throw new ArgumentNullException("bindingContext");
            }

            IPrincipal p = controllerContext.HttpContext.User;

            return p;
        }
    }
}
