// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Controls
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using MvpWebForms.Presenters;
    using MvpWebForms.Views;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;

    [PresenterBinding(typeof(WidgetsReadOnlyPresenter))]
    [PresenterBinding(typeof(WidgetsReadOnlyAsyncPresenter))]
    public partial class WidgetsReadOnlyControl
        : MvpUserControl<WidgetsReadOnlyModel>, IWidgetsReadOnlyView
    {
        public event EventHandler<WidgetIdEventArgs> Finding;

        public event EventHandler<WidgetIdEventArgs> FindingApm;

        public event EventHandler<WidgetIdEventArgs> FindingTap;

        protected void Find_Click(object sender, EventArgs e)
        {
            var id = ParseId();

            if (id.HasValue)
            {
                OnFinding(id.Value);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Apm")]
        protected void FindApm_Click(object sender, EventArgs e)
        {
            var id = ParseId();

            if (id.HasValue)
            {
                OnFindingApm(id.Value);
            }
        }

        protected void FindTap_Click(object sender, EventArgs e)
        {
            var id = ParseId();

            if (id.HasValue)
            {
                OnFindingTap(id.Value);
            }
        }

        private int? ParseId()
        {
            return String.IsNullOrEmpty(WidgetId.Text)
                ? (int?)null
                : Convert.ToInt32(WidgetId.Text, CultureInfo.InvariantCulture);
        }

        private void OnFinding(int id)
        {
            if (Finding != null)
            {
                Finding(this, new WidgetIdEventArgs(id));
            }
        }

        private void OnFindingApm(int id)
        {
            if (FindingApm != null)
            {
                FindingApm(this, new WidgetIdEventArgs(id));
            }
        }

        private void OnFindingTap(int id)
        {
            if (FindingTap != null)
            {
                FindingTap(this, new WidgetIdEventArgs(id));
            }
        }
    }
}
