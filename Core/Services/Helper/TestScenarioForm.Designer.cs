namespace PrintJobInterceptor.Core.Services.Helper
{
    partial class TestScenarioForm
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
            if (disposing && (components != null))
            {
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
            comboScenarios = new ComboBox();
            btnRunTest = new FontAwesome.Sharp.IconButton();
            SuspendLayout();
            // 
            // comboScenarios
            // 
            comboScenarios.FormattingEnabled = true;
            comboScenarios.Location = new Point(65, 107);
            comboScenarios.Name = "comboScenarios";
            comboScenarios.Size = new Size(317, 28);
            comboScenarios.TabIndex = 0;
            // 
            // btnRunTest
            // 
            btnRunTest.IconChar = FontAwesome.Sharp.IconChar.None;
            btnRunTest.IconColor = Color.Black;
            btnRunTest.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnRunTest.Location = new Point(458, 80);
            btnRunTest.Name = "btnRunTest";
            btnRunTest.Size = new Size(133, 81);
            btnRunTest.TabIndex = 1;
            btnRunTest.Text = "Run Test";
            btnRunTest.UseVisualStyleBackColor = true;
            btnRunTest.Click += btnRunTest_Click;
            // 
            // TestScenarioForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnRunTest);
            Controls.Add(comboScenarios);
            Name = "TestScenarioForm";
            Text = "TestScenarioForm";
            Load += TestScenarioForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private ComboBox comboScenarios;
        private FontAwesome.Sharp.IconButton btnRunTest;
    }
}