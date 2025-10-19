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
            tabControlContent = new SiticoneNetCoreUI.SiticoneTabControl();
            activeTab = new TabPage();
            dgvPrintJobs = new DataGridView();
            historyTab = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            dgvHistoryJobs = new DataGridView();
            btnRefresh = new FontAwesome.Sharp.IconButton();
            detailsSplitContainer = new SplitContainer();
            panelDetails = new SiticoneNetCoreUI.SiticonePanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            lblDetailsGlobalStatus = new Label();
            iconStatus = new FontAwesome.Sharp.IconPictureBox();
            lblIndividualJobNames = new Label();
            lblDetailsJobCount = new Label();
            lblDetailsGroupingStatus = new Label();
            lblDetailsHeader = new Label();
            pnlButtons = new Panel();
            buttonCancel = new SiticoneNetCoreUI.SiticoneButton();
            buttonPause = new SiticoneNetCoreUI.SiticoneButton();
            buttonResume = new SiticoneNetCoreUI.SiticoneButton();
            label2 = new Label();
            tableLayoutPanelMain.SuspendLayout();
            panelTitleBar.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MainSplitContainer).BeginInit();
            MainSplitContainer.Panel1.SuspendLayout();
            MainSplitContainer.Panel2.SuspendLayout();
            MainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)contentSplitContainer1).BeginInit();
            contentSplitContainer1.Panel1.SuspendLayout();
            contentSplitContainer1.Panel2.SuspendLayout();
            contentSplitContainer1.SuspendLayout();
            panelPrinters.SuspendLayout();
            tabControlContent.SuspendLayout();
            activeTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPrintJobs).BeginInit();
            historyTab.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistoryJobs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)detailsSplitContainer).BeginInit();
            detailsSplitContainer.Panel1.SuspendLayout();
            detailsSplitContainer.Panel2.SuspendLayout();
            detailsSplitContainer.SuspendLayout();
            panelDetails.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconStatus).BeginInit();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 1;
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Controls.Add(panelTitleBar, 0, 0);
            tableLayoutPanelMain.Controls.Add(panelContent, 0, 1);
            tableLayoutPanelMain.Controls.Add(label2, 0, 2);
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
            // 
            // MainSplitContainer.Panel2
            // 
            MainSplitContainer.Panel2.Controls.Add(detailsSplitContainer);
            MainSplitContainer.Panel2MinSize = 200;
            MainSplitContainer.Size = new Size(1184, 683);
            MainSplitContainer.SplitterDistance = 973;
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
            // 
            // contentSplitContainer1.Panel2
            // 
            contentSplitContainer1.Panel2.Controls.Add(tabControlContent);
            contentSplitContainer1.Size = new Size(973, 683);
            contentSplitContainer1.SplitterDistance = 233;
            contentSplitContainer1.TabIndex = 0;
            // 
            // panelPrinters
            // 
            panelPrinters.BackColor = Color.FromArgb(34, 34, 34);
            panelPrinters.Controls.Add(dropDownPrinters);
            panelPrinters.Dock = DockStyle.Fill;
            panelPrinters.Location = new Point(0, 0);
            panelPrinters.Name = "panelPrinters";
            panelPrinters.Size = new Size(233, 683);
            panelPrinters.TabIndex = 0;
            // 
            // dropDownPrinters
            // 
            dropDownPrinters.AllowMultipleSelection = true;
            dropDownPrinters.BackColor = Color.FromArgb(36, 36, 36);
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
            dropDownPrinters.Size = new Size(233, 67);
            dropDownPrinters.TabIndex = 0;
            dropDownPrinters.Text = "siticoneDropdown1";
            dropDownPrinters.UltraFastPerformance = true;
            dropDownPrinters.UnselectedItemTextColor = Color.FromArgb(240, 240, 240);
            dropDownPrinters.ValueMember = null;
            // 
            // tabControlContent
            // 
            tabControlContent.BorderColor = Color.FromArgb(36, 36, 36);
            tabControlContent.BorderWidth = 0;
            tabControlContent.CloseButtonColor = Color.Gray;
            tabControlContent.CloseButtonHoverColor = Color.Red;
            tabControlContent.CloseButtonSymbolPadding = 0.25F;
            tabControlContent.CloseButtonThickness = 1.8F;
            tabControlContent.ContextMenuFont = new Font("Segoe UI Variable Display", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            tabControlContent.Controls.Add(activeTab);
            tabControlContent.Controls.Add(historyTab);
            tabControlContent.Dock = DockStyle.Fill;
            tabControlContent.DragIndicatorColor = Color.FromArgb(25, 118, 210);
            tabControlContent.EnableHoverEffects = false;
            tabControlContent.EnablePulseEffects = false;
            tabControlContent.EnableRippleEffects = false;
            tabControlContent.Font = new Font("Segoe UI", 10F);
            tabControlContent.GhostBackColor = Color.FromArgb(20, 34, 30, 65);
            tabControlContent.GhostForeColor = Color.FromArgb(180, 0, 0, 0);
            tabControlContent.ItemSize = new Size(160, 40);
            tabControlContent.Location = new Point(0, 0);
            tabControlContent.Margin = new Padding(0);
            tabControlContent.Name = "tabControlContent";
            tabControlContent.Padding = new Point(0, 0);
            tabControlContent.PinIconHoverColor = Color.DarkGray;
            tabControlContent.PinnedIconColor = Color.FromArgb(30, 136, 229);
            tabControlContent.RippleColor = Color.Transparent;
            tabControlContent.SelectedIndex = 0;
            tabControlContent.SelectedTabBackColor = Color.FromArgb(64, 64, 64);
            tabControlContent.SelectedTabFont = new Font("Segoe UI Variable Display", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 238);
            tabControlContent.SelectedTabIndicatorColor = Color.Gray;
            tabControlContent.SelectedTabIndicatorHeight = 3;
            tabControlContent.SelectedTextColor = Color.White;
            tabControlContent.SeparatorLineColor = Color.FromArgb(0, 0, 0);
            tabControlContent.SeparatorLineOpacity = 0.4F;
            tabControlContent.Size = new Size(736, 683);
            tabControlContent.SizeMode = TabSizeMode.Fixed;
            tabControlContent.TabImageSize = 22;
            tabControlContent.TabImageTextGap = 6;
            tabControlContent.TabIndex = 0;
            tabControlContent.UnpinnedIconColor = Color.Gray;
            tabControlContent.UnselectedTabColor = Color.Transparent;
            tabControlContent.UnselectedTextColor = Color.Gray;
            // 
            // activeTab
            // 
            activeTab.Controls.Add(dgvPrintJobs);
            activeTab.Location = new Point(4, 44);
            activeTab.Margin = new Padding(0);
            activeTab.Name = "activeTab";
            activeTab.Size = new Size(728, 635);
            tabControlContent.SetTabImage(activeTab, null);
            activeTab.TabIndex = 0;
            activeTab.Text = "Active";
            activeTab.UseVisualStyleBackColor = true;
            // 
            // dgvPrintJobs
            // 
            dgvPrintJobs.BackgroundColor = SystemColors.ControlDarkDark;
            dgvPrintJobs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPrintJobs.Dock = DockStyle.Fill;
            dgvPrintJobs.Location = new Point(0, 0);
            dgvPrintJobs.Margin = new Padding(0);
            dgvPrintJobs.Name = "dgvPrintJobs";
            dgvPrintJobs.RowHeadersWidth = 51;
            dgvPrintJobs.Size = new Size(728, 635);
            dgvPrintJobs.TabIndex = 0;
            // 
            // historyTab
            // 
            historyTab.Controls.Add(tableLayoutPanel2);
            historyTab.Location = new Point(4, 44);
            historyTab.Name = "historyTab";
            historyTab.Padding = new Padding(3);
            historyTab.Size = new Size(728, 635);
            tabControlContent.SetTabImage(historyTab, null);
            historyTab.TabIndex = 1;
            historyTab.Text = "History";
            historyTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.FromArgb(32, 32, 32);
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(dgvHistoryJobs, 0, 1);
            tableLayoutPanel2.Controls.Add(btnRefresh, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(722, 629);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // dgvHistoryJobs
            // 
            dgvHistoryJobs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHistoryJobs.Dock = DockStyle.Fill;
            dgvHistoryJobs.Location = new Point(0, 37);
            dgvHistoryJobs.Margin = new Padding(0);
            dgvHistoryJobs.Name = "dgvHistoryJobs";
            dgvHistoryJobs.Size = new Size(722, 592);
            dgvHistoryJobs.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.Dock = DockStyle.Right;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.IconChar = FontAwesome.Sharp.IconChar.ArrowRotateLeft;
            btnRefresh.IconColor = Color.White;
            btnRefresh.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnRefresh.IconSize = 30;
            btnRefresh.Location = new Point(674, 3);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(45, 31);
            btnRefresh.TabIndex = 1;
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // detailsSplitContainer
            // 
            detailsSplitContainer.Dock = DockStyle.Fill;
            detailsSplitContainer.Location = new Point(0, 0);
            detailsSplitContainer.Name = "detailsSplitContainer";
            detailsSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // detailsSplitContainer.Panel1
            // 
            detailsSplitContainer.Panel1.Controls.Add(panelDetails);
            // 
            // detailsSplitContainer.Panel2
            // 
            detailsSplitContainer.Panel2.Controls.Add(pnlButtons);
            detailsSplitContainer.Size = new Size(207, 683);
            detailsSplitContainer.SplitterDistance = 468;
            detailsSplitContainer.TabIndex = 0;
            // 
            // panelDetails
            // 
            panelDetails.AcrylicTintColor = Color.FromArgb(128, 255, 255, 255);
            panelDetails.BackColor = Color.FromArgb(36, 36, 36);
            panelDetails.BorderAlignment = System.Drawing.Drawing2D.PenAlignment.Center;
            panelDetails.BorderDashPattern = null;
            panelDetails.BorderGradientEndColor = Color.Purple;
            panelDetails.BorderGradientStartColor = Color.Blue;
            panelDetails.BorderThickness = 2F;
            panelDetails.Controls.Add(tableLayoutPanel3);
            panelDetails.Controls.Add(lblIndividualJobNames);
            panelDetails.Controls.Add(lblDetailsJobCount);
            panelDetails.Controls.Add(lblDetailsGroupingStatus);
            panelDetails.Controls.Add(lblDetailsHeader);
            panelDetails.CornerRadiusBottomLeft = 10F;
            panelDetails.CornerRadiusBottomRight = 10F;
            panelDetails.CornerRadiusTopLeft = 10F;
            panelDetails.CornerRadiusTopRight = 10F;
            panelDetails.EnableAcrylicEffect = false;
            panelDetails.EnableMicaEffect = true;
            panelDetails.EnableRippleEffect = false;
            panelDetails.FillColor = Color.FromArgb(36, 36, 36);
            panelDetails.GradientColors = new Color[]
    {
    Color.White,
    Color.LightGray,
    Color.Gray
    };
            panelDetails.GradientPositions = new float[]
    {
    0F,
    0.5F,
    1F
    };
            panelDetails.Location = new Point(6, 31);
            panelDetails.Name = "panelDetails";
            panelDetails.PatternStyle = System.Drawing.Drawing2D.HatchStyle.Max;
            panelDetails.RippleAlpha = 50;
            panelDetails.RippleAlphaDecrement = 3;
            panelDetails.RippleColor = Color.FromArgb(50, 255, 255, 255);
            panelDetails.RippleMaxSize = 600F;
            panelDetails.RippleSpeed = 15F;
            panelDetails.ShowBorder = true;
            panelDetails.Size = new Size(189, 388);
            panelDetails.TabIndex = 1;
            panelDetails.TabStop = true;
            panelDetails.TrackSystemTheme = false;
            panelDetails.UseBorderGradient = false;
            panelDetails.UseMultiGradient = false;
            panelDetails.UsePatternTexture = false;
            panelDetails.UseRadialGradient = false;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 23F));
            tableLayoutPanel3.Controls.Add(lblDetailsGlobalStatus, 0, 0);
            tableLayoutPanel3.Controls.Add(iconStatus, 1, 0);
            tableLayoutPanel3.Location = new Point(14, 81);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(139, 24);
            tableLayoutPanel3.TabIndex = 6;
            // 
            // lblDetailsGlobalStatus
            // 
            lblDetailsGlobalStatus.AutoSize = true;
            lblDetailsGlobalStatus.Dock = DockStyle.Fill;
            lblDetailsGlobalStatus.Font = new Font("Segoe UI Variable Display Semib", 9.75F, FontStyle.Bold);
            lblDetailsGlobalStatus.ForeColor = Color.White;
            lblDetailsGlobalStatus.Location = new Point(3, 0);
            lblDetailsGlobalStatus.Name = "lblDetailsGlobalStatus";
            lblDetailsGlobalStatus.Size = new Size(110, 24);
            lblDetailsGlobalStatus.TabIndex = 1;
            // 
            // iconStatus
            // 
            iconStatus.BackColor = Color.FromArgb(36, 36, 36);
            iconStatus.Dock = DockStyle.Fill;
            iconStatus.ForeColor = Color.Transparent;
            iconStatus.IconChar = FontAwesome.Sharp.IconChar.Circle;
            iconStatus.IconColor = Color.Transparent;
            iconStatus.IconFont = FontAwesome.Sharp.IconFont.Solid;
            iconStatus.IconSize = 17;
            iconStatus.Location = new Point(119, 3);
            iconStatus.Name = "iconStatus";
            iconStatus.Size = new Size(17, 18);
            iconStatus.TabIndex = 2;
            iconStatus.TabStop = false;
            // 
            // lblIndividualJobNames
            // 
            lblIndividualJobNames.AutoSize = true;
            lblIndividualJobNames.Font = new Font("Segoe UI Variable Display Semib", 9.75F, FontStyle.Bold);
            lblIndividualJobNames.ForeColor = Color.White;
            lblIndividualJobNames.Location = new Point(14, 224);
            lblIndividualJobNames.Name = "lblIndividualJobNames";
            lblIndividualJobNames.Size = new Size(0, 17);
            lblIndividualJobNames.TabIndex = 5;
            // 
            // lblDetailsJobCount
            // 
            lblDetailsJobCount.AutoSize = true;
            lblDetailsJobCount.Font = new Font("Segoe UI Variable Display Semib", 9.75F, FontStyle.Bold);
            lblDetailsJobCount.ForeColor = Color.White;
            lblDetailsJobCount.Location = new Point(14, 179);
            lblDetailsJobCount.Name = "lblDetailsJobCount";
            lblDetailsJobCount.Size = new Size(0, 17);
            lblDetailsJobCount.TabIndex = 3;
            // 
            // lblDetailsGroupingStatus
            // 
            lblDetailsGroupingStatus.AutoSize = true;
            lblDetailsGroupingStatus.Font = new Font("Segoe UI Variable Display Semib", 9.75F, FontStyle.Bold);
            lblDetailsGroupingStatus.ForeColor = Color.White;
            lblDetailsGroupingStatus.Location = new Point(14, 130);
            lblDetailsGroupingStatus.Name = "lblDetailsGroupingStatus";
            lblDetailsGroupingStatus.Size = new Size(0, 17);
            lblDetailsGroupingStatus.TabIndex = 2;
            // 
            // lblDetailsHeader
            // 
            lblDetailsHeader.AutoSize = true;
            lblDetailsHeader.Font = new Font("Segoe UI Variable Display Semib", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 238);
            lblDetailsHeader.ForeColor = Color.White;
            lblDetailsHeader.Location = new Point(14, 36);
            lblDetailsHeader.Name = "lblDetailsHeader";
            lblDetailsHeader.Size = new Size(0, 17);
            lblDetailsHeader.TabIndex = 0;
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(buttonCancel);
            pnlButtons.Controls.Add(buttonPause);
            pnlButtons.Controls.Add(buttonResume);
            pnlButtons.Dock = DockStyle.Fill;
            pnlButtons.Location = new Point(0, 0);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(207, 211);
            pnlButtons.TabIndex = 0;
            // 
            // buttonCancel
            // 
            buttonCancel.AccessibleDescription = "The default button control that accept input though the mouse, touch and keyboard";
            buttonCancel.AccessibleName = "Cancel";
            buttonCancel.AutoSizeBasedOnText = false;
            buttonCancel.BackColor = Color.Transparent;
            buttonCancel.BadgeBackColor = Color.Black;
            buttonCancel.BadgeFont = new Font("Segoe UI", 8F, FontStyle.Bold);
            buttonCancel.BadgeValue = 0;
            buttonCancel.BadgeValueForeColor = Color.White;
            buttonCancel.BorderColor = Color.FromArgb(45, 45, 45);
            buttonCancel.BorderWidth = 1;
            buttonCancel.ButtonBackColor = Color.FromArgb(45, 45, 45);
            buttonCancel.ButtonImage = null;
            buttonCancel.ButtonTextLeftPadding = 0;
            buttonCancel.CanBeep = true;
            buttonCancel.CanGlow = false;
            buttonCancel.CanShake = true;
            buttonCancel.ContextMenuStripEx = null;
            buttonCancel.CornerRadiusBottomLeft = 6;
            buttonCancel.CornerRadiusBottomRight = 6;
            buttonCancel.CornerRadiusTopLeft = 6;
            buttonCancel.CornerRadiusTopRight = 6;
            buttonCancel.CustomCursor = Cursors.Default;
            buttonCancel.DisabledTextColor = Color.FromArgb(150, 150, 150);
            buttonCancel.EnableLongPress = false;
            buttonCancel.EnableRippleEffect = true;
            buttonCancel.EnableShadow = false;
            buttonCancel.EnableTextWrapping = false;
            buttonCancel.Font = new Font("Segoe UI Variable Display Semib", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            buttonCancel.ForeColor = Color.White;
            buttonCancel.GlowColor = Color.FromArgb(100, 255, 255, 255);
            buttonCancel.GlowIntensity = 100;
            buttonCancel.GlowRadius = 20F;
            buttonCancel.GradientBackground = false;
            buttonCancel.GradientColor = Color.FromArgb(0, 227, 64);
            buttonCancel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            buttonCancel.HintText = null;
            buttonCancel.HoverBackColor = Color.FromArgb(240, 240, 240);
            buttonCancel.HoverFontStyle = FontStyle.Regular;
            buttonCancel.HoverTextColor = Color.FromArgb(0, 0, 0);
            buttonCancel.HoverTransitionDuration = 250;
            buttonCancel.ImageAlign = ContentAlignment.MiddleLeft;
            buttonCancel.ImagePadding = 5;
            buttonCancel.ImageSize = new Size(16, 16);
            buttonCancel.IsRadial = false;
            buttonCancel.IsReadOnly = false;
            buttonCancel.IsToggleButton = false;
            buttonCancel.IsToggled = false;
            buttonCancel.Location = new Point(3, 129);
            buttonCancel.LongPressDurationMS = 1000;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.NormalFontStyle = FontStyle.Regular;
            buttonCancel.ParticleColor = Color.FromArgb(200, 200, 200);
            buttonCancel.ParticleCount = 15;
            buttonCancel.PressAnimationScale = 0.97F;
            buttonCancel.PressedBackColor = Color.FromArgb(225, 227, 230);
            buttonCancel.PressedFontStyle = FontStyle.Regular;
            buttonCancel.PressTransitionDuration = 150;
            buttonCancel.ReadOnlyTextColor = Color.FromArgb(100, 100, 100);
            buttonCancel.RippleColor = Color.FromArgb(0, 0, 0);
            buttonCancel.RippleRadiusMultiplier = 0.6F;
            buttonCancel.ShadowBlur = 5;
            buttonCancel.ShadowColor = Color.FromArgb(30, 0, 0, 0);
            buttonCancel.ShadowOffset = new Point(0, 2);
            buttonCancel.ShakeDuration = 500;
            buttonCancel.ShakeIntensity = 5;
            buttonCancel.Size = new Size(171, 57);
            buttonCancel.TabIndex = 1;
            buttonCancel.Text = "Cancel";
            buttonCancel.TextAlign = ContentAlignment.MiddleCenter;
            buttonCancel.TextColor = Color.White;
            buttonCancel.TooltipText = null;
            buttonCancel.UseAdvancedRendering = true;
            buttonCancel.UseParticles = false;
            // 
            // buttonPause
            // 
            buttonPause.AccessibleDescription = "The default button control that accept input though the mouse, touch and keyboard";
            buttonPause.AccessibleName = "Pause";
            buttonPause.AutoSizeBasedOnText = false;
            buttonPause.BackColor = Color.Transparent;
            buttonPause.BadgeBackColor = Color.Black;
            buttonPause.BadgeFont = new Font("Segoe UI", 8F, FontStyle.Bold);
            buttonPause.BadgeValue = 0;
            buttonPause.BadgeValueForeColor = Color.White;
            buttonPause.BorderColor = Color.FromArgb(45, 45, 45);
            buttonPause.BorderWidth = 1;
            buttonPause.ButtonBackColor = Color.FromArgb(45, 45, 45);
            buttonPause.ButtonImage = null;
            buttonPause.ButtonTextLeftPadding = 0;
            buttonPause.CanBeep = true;
            buttonPause.CanGlow = false;
            buttonPause.CanShake = true;
            buttonPause.ContextMenuStripEx = null;
            buttonPause.CornerRadiusBottomLeft = 6;
            buttonPause.CornerRadiusBottomRight = 6;
            buttonPause.CornerRadiusTopLeft = 6;
            buttonPause.CornerRadiusTopRight = 6;
            buttonPause.CustomCursor = Cursors.Default;
            buttonPause.DisabledTextColor = Color.FromArgb(150, 150, 150);
            buttonPause.EnableLongPress = false;
            buttonPause.EnableRippleEffect = true;
            buttonPause.EnableShadow = false;
            buttonPause.EnableTextWrapping = false;
            buttonPause.Font = new Font("Segoe UI Variable Display Semib", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            buttonPause.ForeColor = Color.White;
            buttonPause.GlowColor = Color.FromArgb(100, 255, 255, 255);
            buttonPause.GlowIntensity = 100;
            buttonPause.GlowRadius = 20F;
            buttonPause.GradientBackground = false;
            buttonPause.GradientColor = Color.FromArgb(0, 227, 64);
            buttonPause.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            buttonPause.HintText = null;
            buttonPause.HoverBackColor = Color.FromArgb(240, 240, 240);
            buttonPause.HoverFontStyle = FontStyle.Regular;
            buttonPause.HoverTextColor = Color.FromArgb(0, 0, 0);
            buttonPause.HoverTransitionDuration = 250;
            buttonPause.ImageAlign = ContentAlignment.MiddleLeft;
            buttonPause.ImagePadding = 5;
            buttonPause.ImageSize = new Size(16, 16);
            buttonPause.IsRadial = false;
            buttonPause.IsReadOnly = false;
            buttonPause.IsToggleButton = false;
            buttonPause.IsToggled = false;
            buttonPause.Location = new Point(3, 3);
            buttonPause.LongPressDurationMS = 1000;
            buttonPause.Name = "buttonPause";
            buttonPause.NormalFontStyle = FontStyle.Regular;
            buttonPause.ParticleColor = Color.FromArgb(200, 200, 200);
            buttonPause.ParticleCount = 15;
            buttonPause.PressAnimationScale = 0.97F;
            buttonPause.PressedBackColor = Color.FromArgb(225, 227, 230);
            buttonPause.PressedFontStyle = FontStyle.Regular;
            buttonPause.PressTransitionDuration = 150;
            buttonPause.ReadOnlyTextColor = Color.FromArgb(100, 100, 100);
            buttonPause.RippleColor = Color.FromArgb(0, 0, 0);
            buttonPause.RippleRadiusMultiplier = 0.6F;
            buttonPause.ShadowBlur = 5;
            buttonPause.ShadowColor = Color.FromArgb(30, 0, 0, 0);
            buttonPause.ShadowOffset = new Point(0, 2);
            buttonPause.ShakeDuration = 500;
            buttonPause.ShakeIntensity = 5;
            buttonPause.Size = new Size(171, 57);
            buttonPause.TabIndex = 2;
            buttonPause.Text = "Pause";
            buttonPause.TextAlign = ContentAlignment.MiddleCenter;
            buttonPause.TextColor = Color.White;
            buttonPause.TooltipText = null;
            buttonPause.UseAdvancedRendering = true;
            buttonPause.UseParticles = false;
            // 
            // buttonResume
            // 
            buttonResume.AccessibleDescription = "The default button control that accept input though the mouse, touch and keyboard";
            buttonResume.AccessibleName = "Resume";
            buttonResume.AutoSizeBasedOnText = false;
            buttonResume.BackColor = Color.Transparent;
            buttonResume.BadgeBackColor = Color.Black;
            buttonResume.BadgeFont = new Font("Segoe UI", 8F, FontStyle.Bold);
            buttonResume.BadgeValue = 0;
            buttonResume.BadgeValueForeColor = Color.White;
            buttonResume.BorderColor = Color.FromArgb(45, 45, 45);
            buttonResume.BorderWidth = 1;
            buttonResume.ButtonBackColor = Color.FromArgb(45, 45, 45);
            buttonResume.ButtonImage = null;
            buttonResume.ButtonTextLeftPadding = 0;
            buttonResume.CanBeep = true;
            buttonResume.CanGlow = false;
            buttonResume.CanShake = true;
            buttonResume.ContextMenuStripEx = null;
            buttonResume.CornerRadiusBottomLeft = 6;
            buttonResume.CornerRadiusBottomRight = 6;
            buttonResume.CornerRadiusTopLeft = 6;
            buttonResume.CornerRadiusTopRight = 6;
            buttonResume.CustomCursor = Cursors.Default;
            buttonResume.DisabledTextColor = Color.FromArgb(150, 150, 150);
            buttonResume.EnableLongPress = false;
            buttonResume.EnableRippleEffect = true;
            buttonResume.EnableShadow = false;
            buttonResume.EnableTextWrapping = false;
            buttonResume.Font = new Font("Segoe UI Variable Display Semib", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            buttonResume.ForeColor = Color.White;
            buttonResume.GlowColor = Color.FromArgb(100, 255, 255, 255);
            buttonResume.GlowIntensity = 100;
            buttonResume.GlowRadius = 20F;
            buttonResume.GradientBackground = false;
            buttonResume.GradientColor = Color.FromArgb(0, 227, 64);
            buttonResume.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            buttonResume.HintText = null;
            buttonResume.HoverBackColor = Color.FromArgb(240, 240, 240);
            buttonResume.HoverFontStyle = FontStyle.Regular;
            buttonResume.HoverTextColor = Color.FromArgb(0, 0, 0);
            buttonResume.HoverTransitionDuration = 250;
            buttonResume.ImageAlign = ContentAlignment.MiddleLeft;
            buttonResume.ImagePadding = 5;
            buttonResume.ImageSize = new Size(16, 16);
            buttonResume.IsRadial = false;
            buttonResume.IsReadOnly = false;
            buttonResume.IsToggleButton = false;
            buttonResume.IsToggled = false;
            buttonResume.Location = new Point(3, 66);
            buttonResume.LongPressDurationMS = 1000;
            buttonResume.Name = "buttonResume";
            buttonResume.NormalFontStyle = FontStyle.Regular;
            buttonResume.ParticleColor = Color.FromArgb(200, 200, 200);
            buttonResume.ParticleCount = 15;
            buttonResume.PressAnimationScale = 0.97F;
            buttonResume.PressedBackColor = Color.FromArgb(225, 227, 230);
            buttonResume.PressedFontStyle = FontStyle.Regular;
            buttonResume.PressTransitionDuration = 150;
            buttonResume.ReadOnlyTextColor = Color.FromArgb(100, 100, 100);
            buttonResume.RippleColor = Color.FromArgb(0, 0, 0);
            buttonResume.RippleRadiusMultiplier = 0.6F;
            buttonResume.ShadowBlur = 5;
            buttonResume.ShadowColor = Color.FromArgb(30, 0, 0, 0);
            buttonResume.ShadowOffset = new Point(0, 2);
            buttonResume.ShakeDuration = 500;
            buttonResume.ShakeIntensity = 5;
            buttonResume.Size = new Size(171, 57);
            buttonResume.TabIndex = 0;
            buttonResume.Text = "Resume";
            buttonResume.TextAlign = ContentAlignment.MiddleCenter;
            buttonResume.TextColor = Color.White;
            buttonResume.TooltipText = null;
            buttonResume.UseAdvancedRendering = true;
            buttonResume.UseParticles = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Right;
            label2.ForeColor = Color.White;
            label2.Location = new Point(1118, 731);
            label2.Name = "label2";
            label2.Size = new Size(63, 30);
            label2.TabIndex = 4;
            label2.Text = "Version 1.0";
            label2.TextAlign = ContentAlignment.MiddleCenter;
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
            tableLayoutPanelMain.PerformLayout();
            panelTitleBar.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panelContent.ResumeLayout(false);
            MainSplitContainer.Panel1.ResumeLayout(false);
            MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MainSplitContainer).EndInit();
            MainSplitContainer.ResumeLayout(false);
            contentSplitContainer1.Panel1.ResumeLayout(false);
            contentSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)contentSplitContainer1).EndInit();
            contentSplitContainer1.ResumeLayout(false);
            panelPrinters.ResumeLayout(false);
            tabControlContent.ResumeLayout(false);
            activeTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPrintJobs).EndInit();
            historyTab.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistoryJobs).EndInit();
            detailsSplitContainer.Panel1.ResumeLayout(false);
            detailsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)detailsSplitContainer).EndInit();
            detailsSplitContainer.ResumeLayout(false);
            panelDetails.ResumeLayout(false);
            panelDetails.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconStatus).EndInit();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanelMain;
        private DataGridView dgvPrintJobs;
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
        private SiticoneNetCoreUI.SiticoneTabControl tabControlContent;
        private TabPage activeTab;
        private TabPage historyTab;
        private SplitContainer detailsSplitContainer;
        private SiticoneNetCoreUI.SiticoneButton buttonPause;
        private SiticoneNetCoreUI.SiticoneButton buttonCancel;
        private SiticoneNetCoreUI.SiticoneButton buttonResume;
        private DataGridView dgvHistoryJobs;
        private TableLayoutPanel tableLayoutPanel2;
        private FontAwesome.Sharp.IconButton btnRefresh;
        private Panel pnlButtons;
        private Label label5;
        private Label label4;
        private Label label3;
        private SiticoneNetCoreUI.SiticonePanel panelDetails;
        private Label lblDetailsJobCount;
        private Label lblDetailsGroupingStatus;
        private Label lblDetailsHeader;
        private Label lblDetailsGlobalStatus;
        private Label lblIndividualJobNames;
        private TableLayoutPanel tableLayoutPanel3;
        private FontAwesome.Sharp.IconPictureBox iconStatus;
        private Label label2;
    }
}
