namespace MvpWebForms
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.UI;

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Apm")]
    public partial class AsyncApmPage : Page
    {
        public AsyncApmPage()
            : base()
        {
            PreRenderComplete += Page_PreRenderComplete;
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            AsyncApmControl.Model.RecordPagePreRenderComplete();
        }
    }
}