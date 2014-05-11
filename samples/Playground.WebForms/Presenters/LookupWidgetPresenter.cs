namespace Playground.WebForms.Presenters
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Domain;
    using Playground.WebForms.Views;
    using Playground.WebForms.Views.Models;

    public class LookupWidgetPresenter : HttpPresenter<ILookupWidgetView, LookupWidgetModel>
    {
        readonly IWidgetRepository widgetRepository;

        // NB: Prefer IOC if available.
        public LookupWidgetPresenter(ILookupWidgetView view)
            : this(view, new WidgetRepository()) { }

        public LookupWidgetPresenter(ILookupWidgetView view, IWidgetRepository widgetRepository)
            : base(view)
        {
            this.widgetRepository = widgetRepository;

            View.Finding += Finding;
            View.Model.Widgets = new List<Widget>();
        }

        void Finding(object sender, FindingWidgetEventArgs e)
        {
            if ((!e.Id.HasValue || e.Id <= 0) && String.IsNullOrEmpty(e.Name))
                return;

            if (e.Id.HasValue && e.Id > 0) {
                AsyncManager.RegisterAsyncTask(
                    beginHandler: (asyncSender, ea, callback, state) =>
                        widgetRepository.BeginFind(e.Id.Value, callback, state),
                    endHandler: result =>
                    {
                        var widget = widgetRepository.EndFind(result);
                        if (widget != null) {
                            View.Model.Widgets.Add(widget);
                        }
                    },
                    timeoutHandler: result => { },
                    state: null,
                    executeInParallel: false);
            }
            else {
                AsyncManager.RegisterAsyncTask(
                    beginHandler: (asyncSender, ea, callback, state) =>
                        widgetRepository.BeginFindByName(e.Name, callback, state),
                        endHandler: result =>
                    {
                        var widget = widgetRepository.EndFindByName(result);
                        if (widget != null) {
                            View.Model.Widgets.Add(widget);
                        }
                    },
                    timeoutHandler: result => { },
                    state: null,
                    executeInParallel: false);
            }

            AsyncManager.ExecuteRegisteredAsyncTasks();

            View.Model.ShowResults = true;
        }
    }
}