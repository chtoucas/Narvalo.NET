namespace Playground.WindowsForms
{
    using Narvalo.Mvp;
    using Narvalo.Mvp.Windows;

    [PresenterBinding(typeof(SamplePresenter),
         ViewType = typeof(IView), 
         BindingMode = PresenterBindingMode.SharedPresenter)]
    public partial class SampleUserControl : MvpUserControl
    {
        public SampleUserControl()
        {
            InitializeComponent();
        }
    }
}
