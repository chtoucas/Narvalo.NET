using Autofac;

namespace Narvalo.Autofac
{
    using Narvalo.Presentation.Mvp.Simple;

    public abstract class AutofacProgram
    {
        protected AutofacProgram() { }

        protected abstract IContainer CreateContainer();

        public void Run()
        {
            using (var container = CreateContainer()) {
                PresenterBinder.Factory = new AutofacPresenterFactory(container);

                //var presenter = container.Resolve<IAppPresenter>();

                //presenter.Run();
            }
        }
    }
}
