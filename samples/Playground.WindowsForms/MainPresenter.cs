// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.WindowsForms
{
    using System;
    using System.Windows.Forms;
    using Narvalo.Mvp;

    public sealed class TestMessage
    {
        public string Text { get; set; }
    }

    public interface IMainView : IView
    {
        event EventHandler TextBoxTextChanged;
    }

    public class SamplePresenter : Presenter<IView>
    {
        public SamplePresenter(IView view)
            : base(view)
        {
            View.Load += Load;
        }

        public void Load(object sender, EventArgs e)
        {
            Messages.Subscribe<TestMessage>(_ => MessageBox.Show(_.Text));
        }
    }

    public sealed class MainPresenter : Presenter<IMainView>
    {
        public MainPresenter(IMainView view)
            : base(view)
        {
            View.TextBoxTextChanged += TextChanged;
        }

        public void TextChanged(object sender, EventArgs e)
        {
            Messages.Publish(new TestMessage { Text = (sender as TextBox).Text });
        }
    }
}
