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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tableLayoutPanelMain = new TableLayoutPanel();
            panelTitleBar = new Panel();
            btnMinimize = new FontAwesome.Sharp.IconButton();
            tableLayoutPanel1 = new TableLayoutPanel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            btnClose = new FontAwesome.Sharp.IconButton();
            panelContent = new Panel();
            MainSplitContainer = new SplitContainer();
            contentSplitContainer1 = new SplitContainer();
            panelPrinters = new Panel();
            dropDownPrinters = new SiticoneNetCoreUI.SiticoneDropdown();
            panel1 = new Panel();
            dgvPrintJobs = new DataGridView();
            siticoneActivityButton1 = new SiticoneNetCoreUI.SiticoneActivityButton();
            btnCancel = new FontAwesome.Sharp.IconButton();
            btnResume = new FontAwesome.Sharp.IconButton();
            btnPause = new FontAwesome.Sharp.IconButton();
            tableLayoutPanelMain.SuspendLayout();
            panelTitleBar.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainSplitContainer).BeginInit();
            MainSplitContainer.Panel1.SuspendLayout();
            MainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)contentSplitContainer1).BeginInit();
            contentSplitContainer1.Panel1.SuspendLayout();
            contentSplitContainer1.SuspendLayout();
            panelPrinters.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPrintJobs).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 1;
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Controls.Add(panelTitleBar, 0, 0);
            tableLayoutPanelMain.Controls.Add(panelContent, 0, 1);
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelMain.Location = new Point(0, 0);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 3;
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanelMain.Size = new Size(1184, 761);
            tableLayoutPanelMain.TabIndex = 2;
            // 
            // panelTitleBar
            // 
            panelTitleBar.BackColor = Color.FromArgb(34, 34, 34);
            panelTitleBar.Controls.Add(btnMinimize);
            panelTitleBar.Controls.Add(tableLayoutPanel1);
            panelTitleBar.Controls.Add(btnClose);
            panelTitleBar.Dock = DockStyle.Fill;
            panelTitleBar.Location = new Point(0, 0);
            panelTitleBar.Margin = new Padding(0);
            panelTitleBar.Name = "panelTitleBar";
            panelTitleBar.Size = new Size(1184, 48);
            panelTitleBar.TabIndex = 2;
            // 
            // btnMinimize
            // 
            btnMinimize.Dock = DockStyle.Right;
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            btnMinimize.IconChar = FontAwesome.Sharp.IconChar.WindowMinimize;
            btnMinimize.IconColor = Color.White;
            btnMinimize.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnMinimize.IconSize = 25;
            btnMinimize.Location = new Point(1088, 0);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(48, 48);
            btnMinimize.TabIndex = 5;
            btnMinimize.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(pictureBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(label1, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Left;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(200, 48);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(40, 48);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI Variable Display Semib", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label1.ForeColor = Color.White;
            label1.Location = new Point(43, 0);
            label1.Name = "label1";
            label1.Size = new Size(154, 48);
            label1.TabIndex = 1;
            label1.Text = "PrintJob Interceptor";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            btnClose.Dock = DockStyle.Right;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            btnClose.ForeColor = Color.White;
            btnClose.IconChar = FontAwesome.Sharp.IconChar.None;
            btnClose.IconColor = Color.FromArgb(32, 32, 32);
            btnClose.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnClose.Location = new Point(1136, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(48, 48);
            btnClose.TabIndex = 0;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // panelContent
            // 
            panelContent.Controls.Add(MainSplitContainer);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 48);
            panelContent.Margin = new Padding(0);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(1184, 683);
            panelContent.TabIndex = 3;
            // 
            // MainSplitContainer
            // 
            MainSplitContainer.Dock = DockStyle.Fill;
            MainSplitContainer.Location = new Point(0, 0);
            MainSplitContainer.Margin = new Padding(0);
            MainSplitContainer.Name = "MainSplitContainer";
            // 
            // MainSplitContainer.Panel1
            // 
            MainSplitContainer.Panel1.Controls.Add(contentSplitContainer1);
            MainSplitContainer.Size = new Size(1184, 683);
            MainSplitContainer.SplitterDistance = 962;
            MainSplitContainer.TabIndex = 0;
            // 
            // contentSplitContainer1
            // 
            contentSplitContainer1.Dock = DockStyle.Fill;
            contentSplitContainer1.Location = new Point(0, 0);
            contentSplitContainer1.Name = "contentSplitContainer1";
            // 
            // contentSplitContainer1.Panel1
            // 
            contentSplitContainer1.Panel1.Controls.Add(panelPrinters);
            contentSplitContainer1.Size = new Size(962, 683);
            contentSplitContainer1.SplitterDistance = 232;
            contentSplitContainer1.TabIndex = 0;
            // 
            // panelPrinters
            // 
            panelPrinters.BackColor = Color.FromArgb(34, 34, 34);
            panelPrinters.Controls.Add(panel1);
            panelPrinters.Controls.Add(dropDownPrinters);
            panelPrinters.Dock = DockStyle.Fill;
            panelPrinters.Location = new Point(0, 0);
            panelPrinters.Name = "panelPrinters";
            panelPrinters.Size = new Size(232, 683);
            panelPrinters.TabIndex = 0;
            // 
            // dropDownPrinters
            // 
            dropDownPrinters.AllowMultipleSelection = true;
            dropDownPrinters.BackColor = Color.FromArgb(45, 45, 48);
            dropDownPrinters.BorderColor = Color.FromArgb(80, 80, 80);
            dropDownPrinters.CanBeep = false;
            dropDownPrinters.CanShake = true;
            dropDownPrinters.DataSource = null;
            dropDownPrinters.DisplayMember = null;
            dropDownPrinters.Dock = DockStyle.Top;
            dropDownPrinters.DropdownBackColor = Color.FromArgb(45, 45, 48);
            dropDownPrinters.DropdownWidth = 0;
            dropDownPrinters.DropShadowEnabled = false;
            dropDownPrinters.Font = new Font("Segoe UI", 10F);
            dropDownPrinters.ForeColor = Color.FromArgb(240, 240, 240);
            dropDownPrinters.HoveredItemBackColor = Color.FromArgb(60, 60, 65);
            dropDownPrinters.HoveredItemTextColor = Color.White;
            dropDownPrinters.IsReadonly = false;
            dropDownPrinters.ItemHeight = 30;
            dropDownPrinters.Location = new Point(0, 0);
            dropDownPrinters.MaxDropDownItems = 8;
            dropDownPrinters.Name = "dropDownPrinters";
            dropDownPrinters.PlaceholderColor = Color.Gainsboro;
            dropDownPrinters.PlaceholderDisappearsOnFocus = false;
            dropDownPrinters.PlaceholderText = "Printers";
            dropDownPrinters.SelectedIndex = -1;
            dropDownPrinters.SelectedItem = null;
            dropDownPrinters.SelectedItemBackColor = Color.FromArgb(0, 120, 215);
            dropDownPrinters.SelectedItemTextColor = Color.White;
            dropDownPrinters.SelectedValue = null;
            dropDownPrinters.Size = new Size(232, 67);
            dropDownPrinters.TabIndex = 0;
            dropDownPrinters.Text = "siticoneDropdown1";
            dropDownPrinters.UltraFastPerformance = true;
            dropDownPrinters.UnselectedItemTextColor = Color.FromArgb(240, 240, 240);
            dropDownPrinters.ValueMember = null;
            // 
            // panel1
            // 
            panel1.Controls.Add(dgvPrintJobs);
            panel1.Controls.Add(siticoneActivityButton1);
            panel1.Controls.Add(btnCancel);
            panel1.Controls.Add(btnResume);
            panel1.Controls.Add(btnPause);
            panel1.Location = new Point(22, 204);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(191, 244);
            panel1.TabIndex = 1;
            // 
            // dgvPrintJobs
            // 
            dgvPrintJobs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPrintJobs.Dock = DockStyle.Right;
            dgvPrintJobs.Location = new Point(39, 0);
            dgvPrintJobs.Margin = new Padding(3, 2, 3, 2);
            dgvPrintJobs.Name = "dgvPrintJobs";
            dgvPrintJobs.RowHeadersWidth = 51;
            dgvPrintJobs.Size = new Size(152, 244);
            dgvPrintJobs.TabIndex = 0;
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
            siticoneActivityButton1.Location = new Point(661, 14);
            siticoneActivityButton1.Margin = new Padding(3, 2, 3, 2);
            siticoneActivityButton1.Name = "siticoneActivityButton1";
            siticoneActivityButton1.PressAnimationDuration = 150;
            siticoneActivityButton1.PressedColor = Color.FromArgb(21, 101, 192);
            siticoneActivityButton1.PressedElevation = 1F;
            siticoneActivityButton1.RippleColor = Color.FromArgb(128, 255, 255, 255);
            siticoneActivityButton1.RippleDuration = 1800;
            siticoneActivityButton1.RippleSize = 5;
            siticoneActivityButton1.ShowActivityText = true;
            siticoneActivityButton1.Size = new Size(63, 41);
            siticoneActivityButton1.TabIndex = 3;
            siticoneActivityButton1.Text = "siticoneActivityButton1";
            siticoneActivityButton1.TextColor = Color.White;
            siticoneActivityButton1.UseAnimation = true;
            siticoneActivityButton1.UseElevation = false;
            siticoneActivityButton1.UseRippleEffect = true;
            // 
            // btnCancel
            // 
            btnCancel.IconChar = FontAwesome.Sharp.IconChar.None;
            btnCancel.IconColor = Color.Black;
            btnCancel.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnCancel.Location = new Point(484, 11);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(171, 57);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnResume
            // 
            btnResume.IconChar = FontAwesome.Sharp.IconChar.None;
            btnResume.IconColor = Color.Black;
            btnResume.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnResume.Location = new Point(302, 11);
            btnResume.Margin = new Padding(3, 2, 3, 2);
            btnResume.Name = "btnResume";
            btnResume.Size = new Size(152, 57);
            btnResume.TabIndex = 1;
            btnResume.Text = "Resume";
            btnResume.UseVisualStyleBackColor = true;
            // 
            // btnPause
            // 
            btnPause.IconChar = FontAwesome.Sharp.IconChar.None;
            btnPause.IconColor = Color.Black;
            btnPause.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnPause.Location = new Point(46, 14);
            btnPause.Margin = new Padding(3, 2, 3, 2);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(172, 57);
            btnPause.TabIndex = 0;
            btnPause.Text = "Pause";
            btnPause.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(1184, 761);
            Controls.Add(tableLayoutPanelMain);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MinimumSize = new Size(1100, 700);
            Name = "MainForm";
            Text = "Form1";
            tableLayoutPanelMain.ResumeLayout(false);
            panelTitleBar.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panelContent.ResumeLayout(false);
            MainSplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MainSplitContainer).EndInit();
            MainSplitContainer.ResumeLayout(false);
            contentSplitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)contentSplitContainer1).EndInit();
            contentSplitContainer1.ResumeLayout(false);
            panelPrinters.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPrintJobs).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanelMain;
        private Panel panel1;
        private DataGridView dgvPrintJobs;
        private SiticoneNetCoreUI.SiticoneActivityButton siticoneActivityButton1;
        private FontAwesome.Sharp.IconButton btnCancel;
        private FontAwesome.Sharp.IconButton btnResume;
        private FontAwesome.Sharp.IconButton btnPause;
        private Panel panelTitleBar;
        private FontAwesome.Sharp.IconButton btnClose;
        private FontAwesome.Sharp.IconButton btnMinimize;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pictureBox1;
        private Label label1;
        private Panel panelContent;
        private SplitContainer MainSplitContainer;
        private SplitContainer contentSplitContainer1;
        private Panel panelPrinters;
        private SiticoneNetCoreUI.SiticoneDropdown dropDownPrinters;
    }
}
