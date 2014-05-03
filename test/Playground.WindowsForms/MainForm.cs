namespace Playground.WindowsForms
{
    using System;
    using Narvalo.Mvp.Windows.Forms;

    public partial class MainForm : MvpForm, IMainView
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public event EventHandler TextBoxTextChanged;

        private void OnTextChanged(object sender, EventArgs e)
        {
            var localHandler = TextBoxTextChanged;

            if (localHandler != null) {
                localHandler(sender, e);
            }
        }
    }
}
