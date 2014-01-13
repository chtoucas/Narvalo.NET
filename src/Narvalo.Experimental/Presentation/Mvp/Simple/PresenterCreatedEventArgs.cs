namespace Narvalo.Presentation.Mvp.Simple
{
    using System;

    /// <summary>
    /// Provides data for the <see cref="WebFormsMvp.Binder.PresenterBinder.PresenterCreated"/> event.
    /// </summary>
    public class PresenterCreatedEventArgs : EventArgs
    {
        readonly IPresenter _presenter;

        /// <summary />
        /// <param name="presenter">The presenter that was just created.</param>
        public PresenterCreatedEventArgs(IPresenter presenter)
        {
            _presenter = presenter;
        }

        /// <summary>
        /// Gets the presenter that was just created.
        /// </summary>
        public IPresenter Presenter
        {
            get { return _presenter; }
        }
    }
}