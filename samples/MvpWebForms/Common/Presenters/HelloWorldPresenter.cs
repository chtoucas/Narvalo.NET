namespace MvpWebForms.Presenters
{
    using Narvalo.Mvp;
    using MvpWebForms.Views;

    public sealed class HelloWorldPresenter : PresenterOf<StringModel>
    {
        public HelloWorldPresenter(IView<StringModel> view)
            : base(view)
        {
            View.Load += (sender, e) => View.Model.Message = "Hello World from Presenter!";
        }
    }
}