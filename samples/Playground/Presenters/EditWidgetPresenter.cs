namespace Playground.Presenters
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Narvalo.Mvp;
    using Playground.Data;
    using Playground.Views;

    // NB: Just to make thinks simpler I removed all the async stuff found in the original code.
    // You can still see async at work width EntityFramework in LookupWidgetPresenter.
    public sealed class EditWidgetPresenter : Presenter<IEditWidgetView, EditWidgetModel>
    {
        readonly PlaygroundContext _dbContext = new PlaygroundContext();

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
            View.Model.WidgetCount = _dbContext.Widgets.Count();
        }

        void GettingWidgets(object sender, GettingWidgetsEventArgs e)
        {
            View.Model.Widgets = _dbContext.Widgets
                .OrderBy(_ => _.Id)
                .Skip(e.StartRowIndex * e.MaximumRows)
                .Take(e.MaximumRows);
        }

        void InsertingWidget(object sender, WidgetEventArgs e)
        {
            _dbContext.Widgets.Add(e.Widget);
            _dbContext.SaveChanges();
        }

        void UpdatingWidget(object sender, WidgetEventArgs e)
        {
            _dbContext.Entry(e.Widget).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        void DeletingWidget(object sender, WidgetIdEventArgs e)
        {
            var widget = _dbContext.Widgets.Find(e.Id);
            if (widget != null) {
                _dbContext.Widgets.Remove(widget);
                _dbContext.SaveChanges();
            }
        }
    }
}