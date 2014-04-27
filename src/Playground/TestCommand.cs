// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground
{
    using System;
    using Narvalo;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Binder;

    public interface ITestView : IView
    {
        void DisplayText();
    }

    public sealed class TestPresenter : IPresenter<ITestView>
    {
        readonly ITestView _view;

        public TestPresenter(ITestView view)
        {
            Require.NotNull(view, "view");

            _view = view;
        }

        public ITestView View { get { return _view; } }

        public IMessageBus Messages { get; set; }
    }

    [PresenterBinding(typeof(TestPresenter), ViewType = typeof(ITestView))]
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
