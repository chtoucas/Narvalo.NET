namespace Playground.WebForms.Presenters
{
    using System;
    using System.Linq;
    using Narvalo.Mvp;
    using Playground.WebForms.Services;
    using Playground.WebForms.Views;
    using Playground.WebForms.Views.Models;

    public class EditWidgetPresenter : Presenter<IEditWidgetView, EditWidgetModel>
    {
        readonly IWidgetRepository _widgetRepository;

        // NB: Prefer IOC if available.
        public EditWidgetPresenter(IEditWidgetView view)
            : this(view, new WidgetRepository()) { }

        public EditWidgetPresenter(IEditWidgetView view, IWidgetRepository widgetRepository)
            : base(view)
        {
            _widgetRepository = widgetRepository;

            View.GettingWidgets += GettingWidgets;
            View.GettingWidgetsTotalCount += GettingWidgetsTotalCount;
            View.UpdatingWidget += UpdatingWidget;
            View.InsertingWidget += InsertingWidget;
            View.DeletingWidget += DeletingWidget;
        }

        void GettingWidgets(object sender, GettingWidgetEventArgs e)
        {
            View.Model.Widgets = _widgetRepository.FindAll()
                .Skip(e.StartRowIndex * e.MaximumRows)
                .Take(e.MaximumRows);
        }

        void GettingWidgetsTotalCount(object sender, EventArgs e)
        {
            View.Model.TotalCount = _widgetRepository.FindAll().Count();
        }

        void UpdatingWidget(object sender, UpdateWidgetEventArgs e)
        {
            _widgetRepository.Save(e.Widget, e.OriginalWidget);
        }

        void InsertingWidget(object sender, EditWidgetEventArgs e)
        {
            _widgetRepository.Save(e.Widget, null);
        }

        static void DeletingWidget(object sender, EditWidgetEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}