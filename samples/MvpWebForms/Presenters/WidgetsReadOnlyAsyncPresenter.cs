// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Presenters
{
    using System;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Configuration;
    using MvpWebForms.Data;
    using MvpWebForms.Entities;
    using MvpWebForms.Views;
    using Narvalo.Mvp.Web;

    public sealed class WidgetsReadOnlyAsyncPresenter
        : HttpPresenter<IWidgetsReadOnlyView, WidgetsReadOnlyModel>
    {
        public WidgetsReadOnlyAsyncPresenter(IWidgetsReadOnlyView view)
            : base(view)
        {
            View.FindingTap += FindingTap;
            View.FindingApm += FindingApm;
        }

        void FindingTap(object sender, WidgetIdEventArgs e)
        {
            AsyncManager.RegisterAsyncTask(() => FindingAsync(e.Id));
            AsyncManager.ExecuteRegisteredAsyncTasks();
        }

        void FindingApm(object sender, WidgetIdEventArgs e)
        {
            BeginEventHandler beginAsync = (sa, ea, cb, state) => BeginFind(e.Id, sa, ea, cb, state);

            AsyncManager.RegisterAsyncTask(beginAsync, EndFind, null);
            AsyncManager.ExecuteRegisteredAsyncTasks();
        }

        async Task FindingAsync(int id)
        {
            // TODO: Add CancellationToken
            using (var context = new MvpWebFormsContext()) {
                var widget = await context.Widgets.FindAsync(id);

                if (widget != null) {
                    View.Model.Widgets.Add(widget);
                    View.Model.ShowResult = true;
                }
            }
        }

        IAsyncResult BeginFind(int id, object sender, EventArgs e, AsyncCallback cb, object state)
        {
            var connectionString = WebConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
            var conn = new SqlConnection(connectionString);
            conn.Open();

            var sql = String.Format(CultureInfo.InvariantCulture, "select Id, Name, Description from dbo.Widget where Id={0}", id);

            var cmd = new SqlCommand(sql, conn);

            return cmd.BeginExecuteReader(cb, cmd);
        }

        void EndFind(IAsyncResult ar)
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

            if (widget != null) {
                View.Model.Widgets.Add(widget);
                View.Model.ShowResult = true;
            }
        }
    }
}
