namespace MvpWebForms.Presenters
{
    using MvpWebForms.Views;
    using Narvalo.Mvp;

    public sealed class HelloWorldPresenter : PresenterOf<StringModel>
    {
        public HelloWorldPresenter(IView<StringModel> view)
            : base(view)
        {
            View.Load += (sender, e) => View.Model.Message = "Hello World from Presenter!";
        }
    }
}