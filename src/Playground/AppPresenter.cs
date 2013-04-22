namespace Narvalo.Playground
{
    using System;
    using Narvalo.Playground.Properties;
    using Narvalo.Presentation.Mvp;

    public class AppPresenter : TermPresenter
    {
        readonly IAppService _service;

        public AppPresenter(ITermView view, IAppService service)
            : base(view)
        {
            Requires.NotNull(service, "service");

            _service = service;

            View.Loaded += Load;
            View.Ending += Exit;
        }

        void Load(object sender, EventArgs e)
        {
        }

        public void Exit(object sender, EventArgs e)
        {
            Output.WriteLine(Resources.PressAnyKeyToExit);
        }
    }
}
