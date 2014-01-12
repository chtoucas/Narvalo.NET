namespace Narvalo.Presentation.Mvp
{
    /// <summary>
    /// Represents a class that is a presenter with a strongly typed view in a Web Forms 
    /// MVP application.
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    public interface IPresenter<TView> : IPresenter
        where TView : class, IView
    {
        /// <summary>
        /// Gets the view instance that this presenter is bound to.
        /// </summary>
        TView View { get; }
    }
}
