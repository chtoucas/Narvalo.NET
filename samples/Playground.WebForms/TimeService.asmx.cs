namespace Playground.WebForms
{
    using System;
    using System.ComponentModel;
    using System.Web.Services;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Presenters;
    using Playground.WebForms.Views;

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [PresenterBinding(typeof(TimeServicePresenter), ViewType = typeof(ITimeView))]
    public class TimeService : MvpWebService, ITimeView
    {
        public event EventHandler<GetCurrentTimeCalledEventArgs> GetCurrentTimeCalled;

        protected void OnGetCurrentTimeCalled(GetCurrentTimeCalledEventArgs args)
        {
            if (GetCurrentTimeCalled != null)
            {
                GetCurrentTimeCalled(this, args);
            }
        }

        [WebMethod]
        public DateTime GetCurrentTime(bool localTime)
        {
            var args = new GetCurrentTimeCalledEventArgs(localTime);
            OnGetCurrentTimeCalled(args);
            return args.Result;
        }
    }
}