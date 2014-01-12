namespace Narvalo.Presentation.Mvp
{
    public interface IPresenter
    {
        /// <summary>
        /// Gets or sets the message bus used for cross presenter messaging.
        /// </summary>
        IMessageBus Messages { get; set; }
    }
}
