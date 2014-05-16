namespace Playground.Views
{
    using System;
    using Narvalo.Mvp;

    public interface ITestView : IView
    {
        event EventHandler Completed;
    }
}
