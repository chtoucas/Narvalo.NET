namespace Narvalo.Presentation.Mvp
{
    using System;

    public interface ITermView : IView
    {
        event EventHandler Ending;
    }
}
