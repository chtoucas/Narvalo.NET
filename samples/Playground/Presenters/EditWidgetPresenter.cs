namespace Playground.Presenters
{
    using System;
    using System.Linq;
    using Narvalo.Mvp;
    using Playground.Data;
    using Playground.Views;

    public sealed class EditWidgetPresenter : Presenter<IEditWidgetView, EditWidgetModel>
    {
        // NB: This is quick and dirty...
        readonly PlaygroundDataContext _dataContext = new PlaygroundDataContext();

        public EditWidgetPresenter(IEditWidgetView view)
            : base(view)
        {
            View.GettingWidgets += GettingWidgets;
            View.CountingWidgets += CountingWidgets;
            View.UpdatingWidget += UpdatingWidget;
            View.InsertingWidget += InsertingWidget;
            View.DeletingWidget += DeletingWidget;
        }

        void CountingWidgets(object sender, EventArgs e)
        {
            View.Model.WidgetCount = _dataContext.Widget.Count();
        }

        void GettingWidgets(object sender, GettingWidgetsEventArgs e)
        {
            View.Model.Widgets = _dataContext.Widget
                .Skip(e.StartRowIndex * e.MaximumRows)
                .Take(e.MaximumRows);
        }

        void InsertingWidget(object sender, InsertingWidgettEventArgs e)
        {
            _dataContext.Widget.InsertOnSubmit(e.Widget);
            _dataContext.SubmitChanges();
        }

        void UpdatingWidget(object sender, UpdatingWidgetEventArgs e)
        {
            _dataContext.Widget.Attach(e.Widget, e.OriginalWidget);
            _dataContext.SubmitChanges();
        }

        void DeletingWidget(object sender, WidgetIdEventArgs e)
        {
            var q = from _ in _dataContext.Widget where _.Id == e.Id select _;
            var widget = q.SingleOrDefault();

            if (widget != null) {
                _dataContext.Widget.DeleteOnSubmit(widget);
                _dataContext.SubmitChanges();
            }
        }
    }
}