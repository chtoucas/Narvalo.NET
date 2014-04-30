// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.WindowsForms
{
    using System;
    using System.Windows.Forms;
    using Narvalo;
    using Narvalo.Mvp;

    public sealed class TestMessage
    {
        public string Text { get; set; }
    }

    public interface IMainView : IView
    {
        event EventHandler TextBoxTextChanged;
    }

    public sealed class SamplePresenter : IPresenter<IView>, IDisposable
    {
        readonly IView _view;

        public SamplePresenter(IView view)
        {
            Require.NotNull(view, "view");

            _view = view;

            View.Load += Load;
        }

        public IView View { get { return _view; } }

        public IMessageBus Messages { get; set; }

        public void Load(object sender, EventArgs e)
        {
            Messages.Subscribe<TestMessage>(_ => MessageBox.Show(_.Text));
        }

        public void Dispose()
        {
            View.Load -= Load;
        }
    }

    public sealed class MainPresenter : IPresenter<IMainView>, IDisposable
    {
        readonly IMainView _view;

        public MainPresenter(IMainView view)
        {
            Require.NotNull(view, "view");

            _view = view;

            View.TextBoxTextChanged += TextChanged;
        }

        public IMainView View { get { return _view; } }

        public IMessageBus Messages { get; set; }

        public void TextChanged(object sender, EventArgs e)
        {
            Messages.Publish(new TestMessage { Text = (sender as TextBox).Text });
        }

        public void Dispose()
        {
            View.TextBoxTextChanged -= TextChanged;
        }
    }
}
