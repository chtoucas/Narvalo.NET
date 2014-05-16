namespace Playground.Presenters
{
    using System.Linq;
    using Narvalo.Mvp;
    using Playground.Data;
    using Playground.Views;

    public class LookupWidgetPresenter : Presenter<ILookupWidgetView, LookupWidgetModel>
    {
        // NB: This is quick and dirty...
        readonly PlaygroundDataContext _dataContext = new PlaygroundDataContext();

        public LookupWidgetPresenter(ILookupWidgetView view)
            : base(view)
        {
            View.Finding += Finding;
        }

        void Finding(object sender, WidgetIdEventArgs e)
        {
            var q = from _ in _dataContext.Widget where _.Id == e.Id select _;
            var widget = q.SingleOrDefault();

            if (widget != null) {
                View.Model.Widgets.Add(widget);
                View.Model.ShowResults = true;
            }
        }
    }
}