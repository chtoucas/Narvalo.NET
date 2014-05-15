namespace Playground.WebForms.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Services;
    using Playground.WebForms.Views;

    public class LookupWidgetPresenter : HttpPresenter<ILookupWidgetView, LookupWidgetModel>
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
                BeginEventHandler beginAsync
                    = (source, ea, cb, state) => _widgetRepository.BeginFind(e.Id.Value, cb, state);

                AsyncManager.RegisterAsyncTask(beginAsync, EndFindByIdAsync, null, null, false);
            }
            else if (!String.IsNullOrEmpty(e.Name)) {
                BeginEventHandler beginAsync
                    = (source, ea, cb, state) => _widgetRepository.BeginFindByName(e.Name, cb, state);

                AsyncManager.RegisterAsyncTask(beginAsync, EndFindByNameAsync, null, null, false);
            }
            else {
                return;
            }

            AsyncManager.ExecuteRegisteredAsyncTasks();

            View.Model.ShowResults = true;
        }

        void EndFindByIdAsync(IAsyncResult ar)
        {
            var widget = _widgetRepository.EndFind(ar);
            if (widget != null) {
                View.Model.Widgets.Add(widget);
            }
        }

        void EndFindByNameAsync(IAsyncResult ar)
        {
            var widget = _widgetRepository.EndFindByName(ar);
            if (widget != null) {
                View.Model.Widgets.Add(widget);
            }
        }
    }
}