// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWindowsForms
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Forms;
    using Narvalo.Mvp;

    public interface IMainView : IView
    {
        event EventHandler TextBoxTextChanged;
    }

    public sealed class TestMessage
    {
        public string Text { get; set; }
    }

    public class SamplePresenter : Presenter<IView>
    {
        public SamplePresenter(IView view)
            : base(view)
        {
            View.Load += Load;
        }

        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions",
            Justification = "This library is not localized for a culture that uses a right-to-left reading order.")]
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
