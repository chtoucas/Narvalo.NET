namespace Playground.Presenters
{
    using System;
    using System.Linq;
    using Narvalo.Mvp;
    using Playground.Services;
    using Playground.Views;

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
            View.CountingWidgets += CountingWidgets;
            View.UpdatingWidget += UpdatingWidget;
            View.InsertingWidget += InsertingWidget;
            View.DeletingWidget += DeletingWidget;
        }

        void CountingWidgets(object sender, EventArgs e)
        {
            View.Model.WidgetCount = _widgetRepository.FindAll().Count();
        }

        void GettingWidgets(object sender, GettingWidgetEventArgs e)
        {
            View.Model.Widgets = _widgetRepository.FindAll()
                .Skip(e.StartRowIndex * e.MaximumRows)
                .Take(e.MaximumRows);
        }

        void InsertingWidget(object sender, EditingWidgetEventArgs e)
        {
            _widgetRepository.Save(e.Widget, null);
        }

        void UpdatingWidget(object sender, UpdatingWidgetEventArgs e)
        {
            _widgetRepository.Save(e.Widget, e.OriginalWidget);
        }

        static void DeletingWidget(object sender, EditingWidgetEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}