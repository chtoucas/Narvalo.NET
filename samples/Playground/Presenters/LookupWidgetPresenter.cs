namespace Playground.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.Data;
    using Playground.Views;

    public class LookupWidgetPresenter : Presenter<ILookupWidgetView, LookupWidgetModel>
    {
        readonly IWidgetRepository _widgetRepository;

        // NB: Prefer IOC if available.
        public LookupWidgetPresenter(ILookupWidgetView view)
            : this(view, new WidgetRepository()) { }

        public LookupWidgetPresenter(ILookupWidgetView view, IWidgetRepository widgetRepository)
            : base(view)
        {
            _widgetRepository = widgetRepository;

            View.Finding += Finding;
        }

        void Finding(object sender, FindingWidgetEventArgs e)
        {
            if (e.Id.HasValue && e.Id > 0) {
                Find(e.Id.Value);
            }
            else if (!String.IsNullOrEmpty(e.Name)) {
                FindByName(e.Name);
            }
        }

        void Find(int id)
        {
            var widget = _widgetRepository.Find(id);
            if (widget != null) {
                View.Model.Widgets.Add(widget);
                View.Model.ShowResults = true;
            }
        }

        void FindByName(string name)
        {
            var widget = _widgetRepository.FindByName(name);
            if (widget != null) {
                View.Model.Widgets.Add(widget);
                View.Model.ShowResults = true;
            }
        }
    }
}