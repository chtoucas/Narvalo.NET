// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Presenters
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using MvpWebForms.Data;
    using MvpWebForms.Views;
    using Narvalo.Mvp;

    // NB: Just to make thinks simpler I removed all the async stuff found in the original code.
    // You can still see async at work width EntityFramework in LookupWidgetPresenter.
    public sealed class WidgetsReadWritePresenter
        : Presenter<IWidgetsReadWriteView, WidgetsReadWriteModel>, IDisposable
    {
        MvpWebFormsContext _dbContext = new MvpWebFormsContext();

        bool _disposed = false;

        public WidgetsReadWritePresenter(IWidgetsReadWriteView view)
            : base(view)
        {
            View.GettingWidgets += GettingWidgets;
            View.CountingWidgets += CountingWidgets;
            View.UpdatingWidget += UpdatingWidget;
            View.InsertingWidget += InsertingWidget;
            View.DeletingWidget += DeletingWidget;
        }

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        void CountingWidgets(object sender, EventArgs e)
        {
            View.Model.Count = _dbContext.Widgets.Count();
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

        void Dispose(bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    if (_dbContext != null) {
                        _dbContext.Dispose();
                        _dbContext = null;
                    }
                }

                _disposed = true;
            }
        }
    }
}
