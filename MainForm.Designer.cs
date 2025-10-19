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
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            btnCancel = new FontAwesome.Sharp.IconButton();
            btnResume = new FontAwesome.Sharp.IconButton();
            btnPause = new FontAwesome.Sharp.IconButton();
            siticoneActivityButton1 = new SiticoneNetCoreUI.SiticoneActivityButton();
            ((System.ComponentModel.ISupportInitialize)dgvPrintJobs).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvPrintJobs
            // 
            dgvPrintJobs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPrintJobs.Dock = DockStyle.Fill;
            dgvPrintJobs.Location = new Point(3, 3);
            dgvPrintJobs.Name = "dgvPrintJobs";
            dgvPrintJobs.RowHeadersWidth = 51;
            dgvPrintJobs.Size = new Size(977, 475);
            dgvPrintJobs.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(dgvPrintJobs, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 119F));
            tableLayoutPanel1.Size = new Size(983, 600);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(siticoneActivityButton1);
            panel1.Controls.Add(btnCancel);
            panel1.Controls.Add(btnResume);
            panel1.Controls.Add(btnPause);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 484);
            panel1.Name = "panel1";
            panel1.Size = new Size(977, 113);
            panel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            btnCancel.IconChar = FontAwesome.Sharp.IconChar.None;
            btnCancel.IconColor = Color.Black;
            btnCancel.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnCancel.Location = new Point(553, 15);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(195, 76);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnResume
            // 
            btnResume.IconChar = FontAwesome.Sharp.IconChar.None;
            btnResume.IconColor = Color.Black;
            btnResume.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnResume.Location = new Point(345, 15);
            btnResume.Name = "btnResume";
            btnResume.Size = new Size(174, 76);
            btnResume.TabIndex = 1;
            btnResume.Text = "Resume";
            btnResume.UseVisualStyleBackColor = true;
            // 
            // btnPause
            // 
            btnPause.IconChar = FontAwesome.Sharp.IconChar.None;
            btnPause.IconColor = Color.Black;
            btnPause.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnPause.Location = new Point(53, 18);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(196, 76);
            btnPause.TabIndex = 0;
            btnPause.Text = "Pause";
            btnPause.UseVisualStyleBackColor = true;
            // 
            // siticoneActivityButton1
            // 
            siticoneActivityButton1.ActivityDuration = 2000;
            siticoneActivityButton1.ActivityIndicatorColor = Color.White;
            siticoneActivityButton1.ActivityIndicatorSize = 4;
            siticoneActivityButton1.ActivityIndicatorSpeed = 100;
            siticoneActivityButton1.ActivityText = "Processing...";
            siticoneActivityButton1.AnimationEasing = SiticoneNetCoreUI.SiticoneActivityButton.AnimationEasingType.EaseOutQuad;
            siticoneActivityButton1.BackColor = Color.Transparent;
            siticoneActivityButton1.BorderColor = Color.FromArgb(10, 10, 10, 50);
            siticoneActivityButton1.BorderWidth = 2;
            siticoneActivityButton1.CornerRadiusBottomLeft = 5;
            siticoneActivityButton1.CornerRadiusBottomRight = 5;
            siticoneActivityButton1.CornerRadiusTopLeft = 5;
            siticoneActivityButton1.CornerRadiusTopRight = 5;
            siticoneActivityButton1.DisabledColor = Color.FromArgb(160, 160, 160);
            siticoneActivityButton1.Elevation = 2F;
            siticoneActivityButton1.Font = new Font("Segoe UI", 9F);
            siticoneActivityButton1.HoverAnimationDuration = 200;
            siticoneActivityButton1.HoverColor = Color.FromArgb(66, 165, 245);
            siticoneActivityButton1.Location = new Point(778, 18);
            siticoneActivityButton1.Name = "siticoneActivityButton1";
            siticoneActivityButton1.PressAnimationDuration = 150;
            siticoneActivityButton1.PressedColor = Color.FromArgb(21, 101, 192);
            siticoneActivityButton1.PressedElevation = 1F;
            siticoneActivityButton1.RippleColor = Color.FromArgb(128, 255, 255, 255);
            siticoneActivityButton1.RippleDuration = 1800;
            siticoneActivityButton1.RippleSize = 5;
            siticoneActivityButton1.ShowActivityText = true;
            siticoneActivityButton1.Size = new Size(156, 55);
            siticoneActivityButton1.TabIndex = 3;
            siticoneActivityButton1.Text = "siticoneActivityButton1";
            siticoneActivityButton1.TextColor = Color.White;
            siticoneActivityButton1.UseAnimation = true;
            siticoneActivityButton1.UseElevation = false;
            siticoneActivityButton1.UseRippleEffect = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(983, 600);
            Controls.Add(tableLayoutPanel1);
            Name = "MainForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dgvPrintJobs).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvPrintJobs;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private FontAwesome.Sharp.IconButton btnCancel;
        private FontAwesome.Sharp.IconButton btnResume;
        private FontAwesome.Sharp.IconButton btnPause;
        private SiticoneNetCoreUI.SiticoneActivityButton siticoneActivityButton1;
    }
}
