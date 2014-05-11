namespace Playground.WebForms.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Linq;
    using System.Data.SqlClient;
    using System.Linq;

    // Temporary fake class.
    public class SiteDbDataContext : DataContext
    {
        public SiteDbDataContext() : base(String.Empty) { }

        public IQueryable<Widget> Widgets { get { throw new NotImplementedException(); } }
    }

    public class WidgetRepository : IWidgetRepository
    {
        SqlCommand _beginFindCmd = null;

        SiteDbDataContext DataContext { get { throw new NotImplementedException(); } }

        public Widget Find(int id)
        {
            Widget widget = (from w in DataContext.Widgets where w.Id == id select w).SingleOrDefault();

            return widget;
        }

        public IAsyncResult BeginFind(int id, AsyncCallback callback, Object asyncState)
        {
            var query = from w in DataContext.Widgets where w.Id == id select w;
            _beginFindCmd = DataContext.GetCommand(query) as SqlCommand;
            DataContext.Connection.Open();

            return _beginFindCmd.BeginExecuteReader(callback, asyncState, CommandBehavior.CloseConnection);
        }

        public Widget EndFind(IAsyncResult result)
        {
            var rdr = _beginFindCmd.EndExecuteReader(result);
            var widget = (from w in DataContext.Translate<Widget>(rdr) select w).SingleOrDefault();
            rdr.Close();

            return widget;
        }

        public IEnumerable<Widget> FindAll()
        {
            return from w in DataContext.Widgets select w;
        }

        SqlCommand _beginFindAllCmd = null;

        public IAsyncResult BeginFindAll(AsyncCallback callback, Object asyncState)
        {
            var query = from w in DataContext.Widgets select w;
            _beginFindAllCmd = DataContext.GetCommand(query) as SqlCommand;
            DataContext.Connection.Open();

            return _beginFindAllCmd.BeginExecuteReader(callback, asyncState, CommandBehavior.CloseConnection);
        }

        public IEnumerable<Widget> EndFindAll(IAsyncResult result)
        {
            var rdr = _beginFindAllCmd.EndExecuteReader(result);
            var widgets = (from w in DataContext.Translate<Widget>(rdr) select w).ToList();
            rdr.Close();
            return widgets;
        }

        public Widget FindByName(string name)
        {
            Widget widget = (from w in DataContext.Widgets where w.Name == name select w).SingleOrDefault();

            return widget;
        }

        SqlCommand _beginFindByNameCmd = null;

        public IAsyncResult BeginFindByName(string name, AsyncCallback callback, Object asyncState)
        {
            var query = from w in DataContext.Widgets where w.Name == name select w;
            _beginFindByNameCmd = DataContext.GetCommand(query) as SqlCommand;
            DataContext.Connection.Open();

            return _beginFindByNameCmd.BeginExecuteReader(callback, asyncState, CommandBehavior.CloseConnection);
        }

        public Widget EndFindByName(IAsyncResult result)
        {
            var rdr = _beginFindByNameCmd.EndExecuteReader(result);
            var widget = (from w in DataContext.Translate<Widget>(rdr) select w).SingleOrDefault();
            rdr.Close();

            return widget;
        }

        public void Save(Widget widget, Widget originalWidget)
        {
            throw new NotImplementedException();

            //if (widget.Id > 0) {
            //    // Update
            //    DataContext.Widgets.Attach(widget, originalWidget);
            //}
            //else {
            //    // Create
            //    DataContext.Widgets.InsertOnSubmit(widget);
            //}

            //DataContext.SubmitChanges();
        }
    }
}