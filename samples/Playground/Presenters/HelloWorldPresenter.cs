namespace Playground.Presenters
{
    using Narvalo.Mvp;
    using Playground.Views;

    public sealed class HelloWorldPresenter : PresenterOf<StringModel>
    {
        public HelloWorldPresenter(IView<StringModel> view)
            : base(view)
        {
            View.Load += (sender, e) => View.Model.Message = "Hello World from Presenter!";
        }
    }
}