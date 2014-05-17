// Uncomment ASYNC directive to test async database query.
// For task based query with EF, uncomment EF directive and your web.config:
//  <appSettings>
//      <add key="aspnet:UseTaskFriendlySynchronizationContext" value="true" />
//  </appSettings>
// See the comments in AsyncTplPresenter.
#define ASYNC
#define EF

namespace Playground.Presenters
{
    using Playground.Entities;
    using Playground.Views;

#if ASYNC
#if EF
    using System.Threading.Tasks;
    using Narvalo.Mvp.Web;
    using Playground.Data;
#else
    using System;
    using System.Data.SqlClient;
    using System.Web;
    using System.Web.Configuration;
    using Narvalo.Mvp.Web;
#endif
#else
    using Narvalo.Mvp;
    using Playground.Data;
#endif

    public class LookupWidgetPresenter
#if ASYNC
 : HttpPresenter<ILookupWidgetView, LookupWidgetModel>
#else
 : Presenter<ILookupWidgetView, LookupWidgetModel>
#endif
    {
#if !ASYNC || EF
        readonly PlaygroundContext _dbContext = new PlaygroundContext();
#endif

        public LookupWidgetPresenter(ILookupWidgetView view)
            : base(view)
        {
            View.Finding += Finding;
        }

        void UpdateView(Widget widget)
        {
            if (widget != null) {
                View.Model.Widgets.Add(widget);
                View.Model.ShowResults = true;
            }
        }

#if ASYNC
#if EF

        void Finding(object sender, WidgetIdEventArgs e)
        {
            AsyncManager.RegisterAsyncTask(() => FindAsync(e.Id));
        }

        async Task FindAsync(int id)
        {
            var widget = await _dbContext.Widgets.FindAsync(id);

            UpdateView(widget);
        }

#else

        void Finding(object sender, WidgetIdEventArgs e)
        {
            BeginEventHandler beginAsync = (sa, ea, cb, state) => BeginAsync(e.Id, sa, ea, cb, state);

            AsyncManager.RegisterAsyncTask(beginAsync, EndAsync, null, null, false);
        }

        IAsyncResult BeginAsync(int id, object sender, EventArgs e, AsyncCallback cb, object state)
        {
            var connectionString = WebConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
            var conn = new SqlConnection(connectionString);
            conn.Open();

            var sql = String.Format("select Id, Name, Description from dbo.Widget where Id={0}", id);

            var cmd = new SqlCommand(sql, conn);

            return cmd.BeginExecuteReader(cb, cmd);
        }

        void EndAsync(IAsyncResult ar)
        {
            Widget widget = null;

            using (var cmd = (SqlCommand)ar.AsyncState) {
                using (cmd.Connection) {
                    using (var rdr = cmd.EndExecuteReader(ar)) {
                        if (rdr.Read()) {
                            widget = new Widget {
                                Id = rdr.GetInt32(0),
                                Name = rdr.GetString(1),
                                Description = rdr.GetString(2),
                            };
                        }
                    }
                }
            }

            UpdateView(widget);
        }

#endif
#else

        void Finding(object sender, WidgetIdEventArgs e)
        {
            var widget = _dbContext.Widgets.Find(e.Id);

            UpdateView(widget);
        }

#endif
    }
}