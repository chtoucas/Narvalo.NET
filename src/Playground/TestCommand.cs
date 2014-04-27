// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground
{
    using System;
    using Narvalo;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Binder;

    public interface ITestView : IView
    {
        event EventHandler Completed;
    }

    public sealed class TestPresenter : IPresenter<ITestView>, IDisposable
    {
        readonly ITestView _view;

        public TestPresenter(ITestView view)
        {
            Require.NotNull(view, "view");

            _view = view;

            View.Completed += Completed;
            View.Load += Load;
        }

        public ITestView View { get { return _view; } }

        public IMessageBus Messages { get; set; }

        public void Completed(object sender, EventArgs e)
        {
            Console.WriteLine("Completed.");
        }

        public void Load(object sender, EventArgs e)
        {
            Console.WriteLine("Load.");
        }

        public void Dispose()
        {
            View.Completed -= Completed;
            View.Load -= Load;
        }
    }

    [PresenterBinding(typeof(TestPresenter),
        ViewType = typeof(ITestView),
        BindingMode = PresenterBindingMode.SharedPresenter)]
    public sealed class TestCommand : MvpCommand, ITestView
    {
        protected override void ExecuteCore()
        {
            DisplayText();
        }

        public void DisplayText()
        {
            Console.WriteLine();
            Console.WriteLine("Help");
            Console.WriteLine();
        }
    }
}
