namespace Narvalo.Web
{
    using System.Web;
    using Narvalo;

    public abstract class HttpHandlerBase<TQuery, TBinder> : HttpHandlerBase
        where TBinder : IQueryBinder<TQuery>, new()
    {
        protected abstract void ProcessRequestCore(HttpContext context, TQuery query);

        protected abstract void HandleBindingFailure(HttpResponse response, string errorMessage);

        protected override void ProcessRequestCore(HttpContext context)
        {
            DebugCheck.NotNull(context);

            // Liaison du modèle.
            var binder = new TBinder();

            var query = binder.Bind(context.Request);

            if (binder.Validate()) {
                HandleBindingFailure(context.Response, "FIXME");
                return;
            }

            ProcessRequestCore(context, query);
        }
    }

}
