namespace Playground.WindowsForms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TextBox = new System.Windows.Forms.TextBox();
            this.sampleUserControl1 = new Playground.WindowsForms.SampleUserControl();
            this.SuspendLayout();
            // 
            // TextBox
            // 
            this.TextBox.Location = new System.Drawing.Point(88, 55);
            this.TextBox.Name = "TextBox";
            this.TextBox.Size = new System.Drawing.Size(203, 20);
            this.TextBox.TabIndex = 0;
            this.TextBox.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // sampleUserControl1
            // 
            this.sampleUserControl1.Location = new System.Drawing.Point(41, 111);
            this.sampleUserControl1.Name = "sampleUserControl1";
            this.sampleUserControl1.Size = new System.Drawing.Size(298, 161);
            this.sampleUserControl1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 300);
            this.Controls.Add(this.sampleUserControl1);
            this.Controls.Add(this.TextBox);
            this.Name = "MainForm";
            this.Text = "MvpForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBox;
        private SampleUserControl sampleUserControl1;
    }
}

