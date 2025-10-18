namespace PrintJobInterceptor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvPrintJobs = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvPrintJobs).BeginInit();
            SuspendLayout();
            // 
            // dgvPrintJobs
            // 
            dgvPrintJobs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPrintJobs.Dock = DockStyle.Fill;
            dgvPrintJobs.Location = new Point(0, 0);
            dgvPrintJobs.Name = "dgvPrintJobs";
            dgvPrintJobs.RowHeadersWidth = 51;
            dgvPrintJobs.Size = new Size(983, 600);
            dgvPrintJobs.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(983, 600);
            Controls.Add(dgvPrintJobs);
            Name = "MainForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dgvPrintJobs).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvPrintJobs;
    }
}
