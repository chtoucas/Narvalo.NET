namespace Playground.WebForms.Presenters
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Mvp;
    using Playground.WebForms.Domain;
    using Playground.WebForms.Views;
    using Playground.WebForms.Views.Models;

    public class LookupWidgetPresenter : PresenterOf<ILookupWidgetView, LookupWidgetModel>
    {
        readonly IWidgetRepository widgetRepository;

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
                //AsyncManager.RegisterAsyncTask(
                //    (asyncSender, ea, callback, state) => // Begin
                //        widgetRepository.BeginFind(e.Id.Value, callback, state),
                //    result => // End
                //    {
                //        var widget = widgetRepository.EndFind(result);
                //        if (widget != null)
                //        {
                //            View.Model.Widgets.Add(widget);
                //        }
                //    },
                //    result => { }, // Timeout
                //    null, false);
            }
            else {
                //AsyncManager.RegisterAsyncTask(
                //    (asyncSender, ea, callback, state) => // Begin
                //        widgetRepository.BeginFindByName(e.Name, callback, state),
                //    result => // End
                //    {
                //        var widget = widgetRepository.EndFindByName(result);
                //        if (widget != null)
                //        {
                //            View.Model.Widgets.Add(widget);
                //        }
                //    },
                //    result => { }, // Timeout
                //    null, false);
            }
            //AsyncManager.ExecuteRegisteredAsyncTasks();
            View.Model.ShowResults = true;
        }
    }
}