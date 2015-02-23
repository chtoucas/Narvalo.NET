// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWindowsForms
{
    using System;

    using Narvalo.Mvp.Windows.Forms;

    public partial class MainForm : MvpForm, IMainView
    {
        public MainForm()
        {
            InitializeComponent();

            Text = Strings.MainForm_Text;
        }

        public event EventHandler TextBoxTextChanged;

        private void OnTextChanged(object sender, EventArgs e)
        {
            var localHandler = TextBoxTextChanged;

            if (localHandler != null)
            {
                localHandler(sender, e);
            }
        }
    }
}
